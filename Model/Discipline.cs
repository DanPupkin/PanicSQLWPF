using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public class Discipline : INotifyPropertyChanged
    {
        public string Name { get; set; }
        private bool status;
        public bool Status { get { return status; } set { status = value; OnPropertyChanged("status"); } } //stackoverflow
        public Discipline(string name, bool status)

        {
            Name = name;
            Status = status;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
