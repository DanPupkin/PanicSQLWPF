using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace LogicLibrary
{
    public class Logic : INotifyPropertyChanged
    {
        public ObservableCollection<Discipline> Disciplines { set; get; }
        public string DebugInfo { set; get; }
        SQLBaseControl control { get; set; }
        public Logic()
        {
            control = new SQLBaseControl();


            Disciplines = new ObservableCollection<Discipline>
            {
                 new Discipline("matan")

            };
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}