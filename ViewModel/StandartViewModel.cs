using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using Model;
using CommunityToolkit.Common;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Windows.Data;
using System.Windows;


namespace ViewModel
{
    public class StandartViewModel
    {
        public string login { get; set; }
        public string password { get; set; }
        public int Id { get; set; }
        public string NewDisciplineName { get; set; }
        public bool NewDisciplineStatus { get; set; }

        public ObservableCollection<Discipline> Disciplines { set; get; } = new ObservableCollection<Discipline>();
        public ObservableCollection<Discipline> FullDisciplines { set; get; } = new ObservableCollection<Discipline>();
        public Discipline SelectedDiscipline { get; set; }
        public (string[], int) AuthInfo { set; get; }
        
        SQLBaseControl control { get; set; } = new SQLBaseControl();
        private bool allButtonIsChecked = true;

        public bool AllButtonIsChecked
        {
            get { return allButtonIsChecked; }
            set
            {
                allButtonIsChecked = value;
                
            }
        }
        private bool passedButtonIsChecked;

        public bool PassedButtonIsChecked
        {
            get { return passedButtonIsChecked; }
            set
            {
                passedButtonIsChecked = value;
                
            }
        }
        private bool notPassedButtonIsChecked;

        public bool NotPassedButtonIsChecked
        {
            get { return notPassedButtonIsChecked; }
            set
            {
                notPassedButtonIsChecked = value;
                
            }
        }
        public event PropertyChangedEventHandler CollectionChanged;
        public void OnCollectionChanged([CallerMemberName] string prop = "")
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new PropertyChangedEventArgs(prop));
        }

        public StandartViewModel()
        {

            LoginCommand = new AsyncRelayCommand(LoginUserCommand);
            SQLLoadCommand = new AsyncRelayCommand(LoadCommand);
            FilterCommand = new AsyncRelayCommand(FilterDisciplinesCommand);
            ChangeCommand = new AsyncRelayCommand(ChangeStatusCommand);
            DeleteCommand = new AsyncRelayCommand(DeleteDisciplineCommand);
            AddDiscCommand = new AsyncRelayCommand(AddDisciplineCommand);
            SaveDiscCommand = new AsyncRelayCommand(SaveDisciplineCommand);

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

        public IAsyncRelayCommand SQLLoadCommand { get; }

        async private Task LoadCommand()
        {
            FullDisciplines = await control.ReadDisciplines(AuthInfo);
            //Disciplines = await control.ReadDisciplines(AuthInfo);
            Id = AuthInfo.Item2;
            foreach (var x in FullDisciplines)
            {
                Disciplines.Add(x);
            }


        }

        public IAsyncRelayCommand ChangeCommand { get; }

        async private Task ChangeStatusCommand()
        {
            if(SelectedDiscipline.Status)
            {
                SelectedDiscipline.Status=false;
            }
            else
            {
                SelectedDiscipline.Status = true;
            }

        }
        public IAsyncRelayCommand DeleteCommand { get; }

        async private Task DeleteDisciplineCommand()
        {
            FullDisciplines.Remove(SelectedDiscipline);
            FilterCommand.Execute(new object());

        }

        public IAsyncRelayCommand AddDiscCommand { get; }

        async private Task AddDisciplineCommand()
        {
            FullDisciplines.Add(new Discipline(NewDisciplineName, NewDisciplineStatus));
            FilterCommand.Execute(new object());

        }
        public IAsyncRelayCommand SaveDiscCommand { get; }

        async private Task SaveDisciplineCommand()
        {
           await control.SaveDisciplines(Id, FullDisciplines, login, CreateMD5(this.password));

        }

        public IAsyncRelayCommand FilterCommand { get; }

         private async Task FilterDisciplinesCommand()
        {
            Disciplines.Clear();
            if (AllButtonIsChecked)
            {
                
                foreach (Discipline item in FullDisciplines)
                {
                    
                        Disciplines.Add(item);
                    
                }
            }
            else if(PassedButtonIsChecked)
            {

                foreach (Discipline item in FullDisciplines)
                {
                    if (item.Status)
                    {
                        Disciplines.Add(item);
                    }
                }
            }
            else
            {
                
                foreach (Discipline item in FullDisciplines)
                {
                    if (!item.Status)
                    {
                        Disciplines.Add(item);
                    }
                }
            }
        }


    }

}
