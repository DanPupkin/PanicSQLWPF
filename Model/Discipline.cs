using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Discipline: INotifyPropertyChanged
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
