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
using LogicLibrary;
using XamlAnimatedGif;
using System.IO;


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
            //this.Hide();
            
            AuthLogic auth = new AuthLogic();
            authTab = new AuthTab();
            //studentsSystem = new StudentsSystem();
            //studentsSystem.DataContext= vm;
            //studentsSystem.Show();
            authTab.Show();
            DataContext = auth;
            authTab.DataContext = auth;

            

            this.Closing += Window_Closing;


            if (auth.HideAction == null)
            {
                auth.HideAction = new Action(authTab.Hide);
            }

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //authTab.Close();
            Application.Current.Shutdown();
        }

        
    }
}
