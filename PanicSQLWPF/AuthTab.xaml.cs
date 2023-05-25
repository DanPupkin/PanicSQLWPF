using System;
using System.Windows;
using System.Windows.Controls;
using ViewModel;

namespace PanicSQLWPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthTab.xaml
    /// </summary>
    public partial class AuthTab : Window
    {
        public AuthTab(StandartViewModel Vm)
        {
            InitializeComponent();
            

            this.DataContext = Vm;
        }
        
        private void PassText_TextChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).student.MD5 = StandartViewModel.CreateMD5(((PasswordBox)sender).Password);

        }
    }
}
