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

        public ObservableCollection<Discipline> Disciplines { set; get; } = new ObservableCollection<Discipline>();
       // public ObservableCollection<Discipline> Disciplines { set { OnPropertyChanged("disp"); disciplines = value; } get { return disciplines; } }
        public (string[], int) AuthInfo { set; get; }
        public Student student { get; set; }
        SQLBaseControl control { get; set; } = new SQLBaseControl();

        public AuthLogic() 
        {
            
            LoginCommand = new AsyncRelayCommand(LoginUserCommand);
            SQLLoadCommand = new AsyncRelayCommand(LoadCommand);
            control = new SQLBaseControl();
            //Disciplines = new ObservableCollection<Discipline> {new Discipline("Test", false) };

        }




        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); 

            }
        }

        
        public Action CloseAction { get; set; }
        public IAsyncRelayCommand LoginCommand { get; }

        async private Task LoginUserCommand()
        {
            
            CloseAction();
            AuthInfo = await control.Login(this.login, CreateMD5(this.password));
            try 
            { 
                SQLLoadCommand.Execute(new object());// спорно, может стоит переделать
            }
            catch (Exception ex)
            {
                //нужно придумать задержку, чтобы поток не встал; нужно погуглить, как дожидаться окончания выполнения потока
                //в данном проекте, проблем быть не должно, но с большой базой надо сглаживать этот угол
                //оптимально до окончания выполения потока делать кнопки во view non-clickable 
                //(я не уверен, что  LoadException вылетит здесь, надо будет дебажить)
                Task.Delay(300).Wait();
                SQLLoadCommand.Execute(new object());
            }


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
            ObservableCollection<Discipline> sub_Disciplines = await control.ReadDisciplines(AuthInfo);
            foreach(var x in sub_Disciplines)
            {
                Disciplines.Add(x);
            }



        }


    }
}
    