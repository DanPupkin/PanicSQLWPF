using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XamlAnimatedGif;
using System.IO;
using System.Globalization;
using ViewModel;
using System.Diagnostics;

namespace PanicSQLWPF_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AuthTab authTab;
        
        public MainWindow()
        {
            InitializeComponent();

            StandartViewModel VM = new StandartViewModel();
            authTab = new AuthTab();
            authTab.Show();
            DataContext = VM;
            authTab.DataContext = VM;
            
            this.Closing += Window_Closing;


            if (VM.CloseAction == null)
            {
                VM.CloseAction = new Action(authTab.Close);
            }

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //authTab.Close();
            Application.Current.Shutdown();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                Trace.WriteLine(value.ToString());
                return "Green";
            }
            return "Red";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }


}
