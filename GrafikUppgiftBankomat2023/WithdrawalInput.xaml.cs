using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GrafikUppgiftBankomat2023
{
    /// <summary>
    /// Interaction logic for WithdrawalInput.xaml
    /// </summary>
    public partial class WithdrawalInput : Window
    {
        public string WithdrawalAmount { get; private set; }
        public WithdrawalInput()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            WithdrawalAmount = txtWithdrawalAmount.Text;
            DialogResult = true; 
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
