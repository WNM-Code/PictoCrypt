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
        String location;
        String filename;
        String filetype;
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
                var tempfile = dialog.FileName;
                ((Button)sender).Content = tempfile;
                i = new Bitmap(tempfile);
                Image.Source = new BitmapImage(new Uri(@"" + tempfile));
                location = "";
                filename = "";
                filetype = "";
                int lasts = tempfile.LastIndexOf('\\');
                location = tempfile.Substring(0, lasts +1);
                int lastp = tempfile.LastIndexOf('.');
                int lastp2 = lastp - (lasts + 1);
                filename = tempfile.Substring(lasts + 1, lastp2);
                filetype = tempfile.Substring(lastp);

                Console.WriteLine(location);
                Console.WriteLine(filename);
                Console.WriteLine(filetype);
            }
            catch
            {
                ((Button)sender).Content = "Select Photo";
                Image.Visibility = Visibility.Hidden;
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
                c.setKey(text);
                Keyphrase = text;
            }
        }

        private void Key_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            c.encrypt(i, location, filename, filetype);
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            c.decrypt(i, location, filename, filetype);
        }
    }
}
