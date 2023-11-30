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
    /// Interaction logic for Atm.xaml
    /// </summary>
    public partial class Atm : Window
    {
        private Account loggedInAccount;
        private List<Account> accounts;
        private List<Account> allAccounts;
        private readonly string accountsFilePath;

        public Atm(Account account, List<Account> allAccounts, string filePath)
        {
            InitializeComponent();
            loggedInAccount = account;
            accounts = allAccounts;
            accountsFilePath = filePath;
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

        private void BalanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInAccount != null)
            {

                MessageBox.Show($"Your account balance is: {loggedInAccount.Balance:C}");
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            DepositInput depositInput = new DepositInput();

            if (depositInput.ShowDialog() == true)
            {
                if (decimal.TryParse(depositInput.DepositAmount, out decimal depositAmount))
                {
                    loggedInAccount.Balance += depositAmount;

                    
                    SaveUpdatedBalance(loggedInAccount);

                    MessageBox.Show($"Deposit successful! New balance: {loggedInAccount.Balance:C}");
                }
                else
                {
                    MessageBox.Show("Invalid deposit amount. Please enter a valid decimal value.");
                }
            }


        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            WithdrawalInput withdrawalInput = new WithdrawalInput();

            if (withdrawalInput.ShowDialog() == true)
            {
                if (decimal.TryParse(withdrawalInput.WithdrawalAmount, out decimal withdrawalAmount))
                {
                    if (withdrawalAmount <= loggedInAccount.Balance)
                    {
                        loggedInAccount.Balance -= withdrawalAmount;

                        
                        SaveUpdatedBalance(loggedInAccount);

                        MessageBox.Show($"Withdrawal successful! New balance: {loggedInAccount.Balance:C}");
                    }
                    else
                    {
                        MessageBox.Show("Insufficient funds. Please enter a valid withdrawal amount.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid withdrawal amount. Please enter a valid decimal value.");
                }
            }
        }



        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();


            loginWindow.Show();
            this.Close();
        }

        private void SaveUpdatedBalance(Account account)
        {
            try
            {
                List<string> lines = new List<string>();

                foreach (Account acc in accounts)
                {
                   
                    if (acc.Pin == account.Pin)
                    {
                        acc.Balance = account.Balance;
                    }

                    lines.Add($"{acc.Name},{acc.Pin},{acc.Balance}");
                }

                
                File.WriteAllLines(accountsFilePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving updated balance: {ex.Message}");
            }
        }
    }
}
