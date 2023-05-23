using System;
using System.Collections.Generic;
using System.Data;
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
using ViewModel;

namespace PanicSQLWPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthTab.xaml
    /// </summary>
    public partial class AuthTab : Window
    {
        public AuthTab()
        {
            InitializeComponent();
            StandartViewModel vm = new StandartViewModel();
            
            DataContext = vm;
        }
        private void LoginClick(object sender, EventArgs e)
        {
            string login = LoginBox.Text;
            string password = StandartViewModel.CreateMD5(PassBox.Password);


        }
        //private void LoginText_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    TextBox LoginText = (TextBox)sender;

        //}
        private void PassText_TextChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).password = ((PasswordBox)sender).Password;

        }
    }
}
