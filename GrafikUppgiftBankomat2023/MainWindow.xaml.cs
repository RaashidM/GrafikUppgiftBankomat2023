using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using System.IO;

namespace GrafikUppgiftBankomat2023
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Account> accounts;
        private Account loggedInAccount;
        private readonly string accountsFilePath = "accounts.txt";
        public MainWindow()
        {
            InitializeComponent();
            accounts = LoadAccounts();
        }

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);

            }

            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);

        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            this.Visibility = Visibility.Hidden;
            createAccount.Show();

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtPin.Password, out int enteredPin))
            {
                Account foundAccount = FindAccountByPin(enteredPin);

                if (foundAccount != null)
                {
                    
                    loggedInAccount = foundAccount;

                    
                    Atm atmWindow = new Atm(loggedInAccount, accounts, accountsFilePath); 
                    atmWindow.Show();

                   
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid PIN. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Invalid PIN. Please enter a valid integer PIN.");
            }
        }

        private Account FindAccountByPin(int pin)
        {
            foreach (Account account in accounts)
            {
                if (account.Pin == pin)
                {
                    return account;
                }
            }
            return null; 
        }

        private void SaveAccounts(List<Account> accounts)
        {
            try
            {
                List<string> lines = new List<string>();

                foreach (Account account in accounts)
                {
                    lines.Add($"{account.Name},{account.Pin},{account.Balance}");
                }

                File.WriteAllLines(accountsFilePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving accounts: {ex.Message}");
            }
        }

        private List<Account> LoadAccounts()
        {
            List<Account> loadedAccounts = new List<Account>();

            if (File.Exists(accountsFilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(accountsFilePath);

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3 && int.TryParse(parts[1], out int pin) && decimal.TryParse(parts[2], out decimal balance))
                        {
                            loadedAccounts.Add(new Account { Name = parts[0], Pin = pin, Balance = balance });
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading accounts: {ex.Message}");
                }
            }

            return loadedAccounts;
        

    }









        
    }
}
