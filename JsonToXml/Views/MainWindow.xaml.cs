using JsonToXml.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonToXml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel;

        public MainWindow()
        {
            MainViewModel = new MainViewModel();

            DataContext = MainViewModel;
            MainViewModel.Logs = new List<string>();
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LV_Logs.ItemsSource = MainViewModel.Logs;
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Multiselect = true;
            dlg.Filter = "JSON Files (*.json)|*.json";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                this.MainViewModel.JsonPaths = dlg.FileNames.ToList();
                string filename = dlg.FileName;

                if (await Task.Run(() => this.MainViewModel.ReadFiles()))
                {
                    this.MainViewModel.ConvertToXmlFiles();
                }
                
            }
        }
    }
}
