using HLD_Vision_GUI.Model;
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

namespace HLD_Vision_GUI.View
{
    /// <summary>
    /// Interaction logic for LoginDlg.xaml
    /// </summary>
    public partial class LoginDlg : Window
    {
        string password;
        HLDSystem msystemData = new HLDSystem();

        public LoginDlg()
        {
            InitializeComponent();
            password = msystemData.GetPassword();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = pbPass.Password == password;
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.pbPass.Focus();
        }
    }
}
