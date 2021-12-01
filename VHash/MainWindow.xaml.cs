using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

namespace VHash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Minimize_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }


        private string _key;
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                NotifyPropertyChanged(nameof(Key));
            }
        }

        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                NotifyPropertyChanged(nameof(Input));
            }
        }

        private string _output;
        public string Output
        {
            get { return _output; }
            set
            {
                _output = value;
                NotifyPropertyChanged(nameof(Output));
            }
        }

        //private const int SALT_SIZE = 24; // size in bytes
        private const int HASH_SIZE = 24; // size in bytes
        private const int ITERATIONS = 10000; // number of pbkdf2 iterations
        private void Hash(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Key) && Key.Length >= 8 && !string.IsNullOrEmpty(Input))
            {
                try
                {
                    byte[] salt = Encoding.ASCII.GetBytes(Key);
                    // Generate the hash
                    Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Input, salt, ITERATIONS);
                    byte[] output = pbkdf2.GetBytes(HASH_SIZE);

                    //int outputLength = ((inputLength+2)/3)*4
                    Output = Convert.ToBase64String(output);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
