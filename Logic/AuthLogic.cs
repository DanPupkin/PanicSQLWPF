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


namespace LogicLibrary
{
    public class AuthLogic
    {
        public string login { get; set; }
        public string password { get; set; }
        public AuthLogic() 
        {

            LoginCommand = new AsyncRelayCommand(LoginUserCommand);


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

        private Task<string[]> LoginUserCommand()
        {
            HideAction();
            return SQLBaseControl.Login(this.login, CreateMD5(this.password));
        }
        //public void OpenWindow(object mw)
        //{
        //    (Window)mw.Show();
        //}
        public void LoginButton()
        {

        }
    }
}
    