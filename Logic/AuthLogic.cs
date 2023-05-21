using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using Model;
using System.Windows;
using System.Diagnostics;

using CommunityToolkit.Common;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace LogicLibrary
{
    public class AuthLogic
    {
        public string login { get; set; }
        public string password { get; set; }

        //public string[] LoginInfo { get; set; }
        public ObservableCollection<Discipline> Disciplines { set; get; }
        public (string[], int) AuthInfo { set; get; }
        public Student student { get; set; }
        SQLBaseControl control { get; set; } = new SQLBaseControl();

        public AuthLogic() 
        {
            
            LoginCommand = new AsyncRelayCommand(LoginUserCommand);
            SQLLoadCommand = new AsyncRelayCommand(LoadCommand);
            //var aboba = CommunityToolkit.Common.TaskExtensions.GetResultOrDefault(LoginUserCommand());

        }




        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +

            }
        }

        //private ICommand loginCommand;
        //public ICommand LoginCommand => loginCommand ?? (loginCommand = new RelayCommand(LoginButton));
        public Action HideAction { get; set; }
        public IAsyncRelayCommand LoginCommand { get; }

        async private Task LoginUserCommand()
        {
            HideAction();
            //LoginInfo = await SQLBaseControl.Login(this.login, CreateMD5(this.password));
            //Trace.WriteLine($"+++++++++++++++{LoginInfo}+++++++++++++");
            AuthInfo = await SQLBaseControl.Login(this.login, CreateMD5(this.password));
            //return new string[0]; // I d'nt know how to collect this result
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        public IAsyncRelayCommand SQLLoadCommand { get; }

        async private Task LoadCommand()
        {
            await control.ReadDisciplines(AuthInfo);
            
        }

    }
}
    