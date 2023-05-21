namespace Model
{
    public class Student
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Discipline> Disciplines = new List<Discipline>();


    }
}