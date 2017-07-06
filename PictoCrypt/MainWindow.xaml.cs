using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PictoCrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String Keyphrase = "";
        Crypter c;
        Bitmap i;
        public MainWindow()
        {
            InitializeComponent();
             c = new Crypter(Keyphrase);
        }

        private void SelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();

            CommonFileDialogResult result = dialog.ShowDialog();
            try
            {
                ((Button)sender).Content = dialog.FileName;
                //i = new BitmapImage(new Uri(@"" + dialog.FileName));
                i = new Bitmap(dialog.FileName);
                Image.Source = new BitmapImage(new Uri(@"" + dialog.FileName));
                c.Encrypt(i, dialog.FileName + "encrypted");
            }
            catch
            {
                ((Button)sender).Content = "Select Photo";
            }
        }

        private void Key_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = Keyphrase;
        }

        private void grid1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            grid1.Focus();
        }

        private void Key_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text.Equals(""))
            {
                ((TextBox)sender).Text = "Enter an encryption key";
                Keyphrase = "";
            }
            else
            {
                Keyphrase = text;
            }
        }

        private void Key_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
