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
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using System.IO;
namespace GrafikUppgiftBankomat2023
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        private readonly string accountsFilePath = "accounts.txt";
        public CreateAccount()
        {
            InitializeComponent();
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

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;

            if (int.TryParse(txtPin.Password, out int pin))
            {
                Account accountInfo = new Account { Name = name, Pin = pin };

                
                SaveNewAccount(accountInfo);

                
                MessageBox.Show("Account created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close(); 
            }
            else
            {
                MessageBox.Show("Invalid PIN. Please enter a valid integer PIN.");
            }
        }

        private void SaveNewAccount(Account newAccount)
        {
            List<Account> accounts = new List<Account>();

            if (File.Exists(accountsFilePath))
            {
                accounts = LoadAccountsFromFile();
            }

            accounts.Add(newAccount);
            SaveAccountsToFile(accounts);
        }

        private List<Account> LoadAccountsFromFile()
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

        private void SaveAccountsToFile(List<Account> accounts)
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







    }
}

