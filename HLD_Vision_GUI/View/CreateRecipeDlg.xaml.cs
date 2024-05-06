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
    /// Interaction logic for CreateRecipeDlg.xaml
    /// </summary>
    public partial class CreateRecipeDlg : Window
    {
        public string NewRecipePath
        {
            get
            {
                string parent = App.RecipePath;
                string des = System.IO.Path.Combine(parent, tbNewRecipe.Text);
                return des;
            }
        }
        public string OldRecipePath
        {
            get
            {
                string parent = App.RecipePath;
                string src = System.IO.Path.Combine(parent, tbReference.Text);
                return src;
            }
        }

        HLDSystem msystemData = new HLDSystem();
        public MessageBoxResult DialogResult = MessageBoxResult.Cancel;
        public CreateRecipeDlg(string refname)
        {
            InitializeComponent();
            tbReference.Text = tbNewRecipe.Text = refname;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbNewRecipe.Text))
                {
                    MessageBox.Show("It's a invalid recipe name !!", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (tbNewRecipe.Text == tbReference.Text)
                {
                    MessageBox.Show("It's a invalid recipe name !!", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                if (System.IO.Directory.Exists(NewRecipePath))
                {
                    MessageBox.Show("There's same recipe !!", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //System.IO.Directory.CreateDirectory(des);

                //src = System.IO.Path.Combine(src, "RecipeData.dat");
                //des = System.IO.Path.Combine(des, "RecipeData.dat");

                //System.IO.File.Copy(src, des);
                
                DialogResult = MessageBoxResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Create_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbNewRecipe.Focus();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
