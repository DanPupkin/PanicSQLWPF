using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ViewModel;

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
            authTab = new AuthTab(VM);
            authTab.Show();
            DataContext = VM;
            

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
