using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MD5 { get; set; }
        public ObservableCollection<Discipline> Disciplines { get; set; }
        public string[] BaseDisp;
    }
}
