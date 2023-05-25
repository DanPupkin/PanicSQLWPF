using Npgsql;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Model
{
    public class SQLBaseControl
    {
        NpgsqlConnection con;

        public SQLBaseControl()
        {
            var cs = "Host=localhost;Username=postgres;Password=Duster2019;Database=Panic";


            this.con = new NpgsqlConnection(cs);
            con.Open();

        }
        ~SQLBaseControl()

        {
            con.Close();

        }
        async public Task<(string[], int)> Login(string username, string MD5)
        {
            string sql = $"SELECT student_disciplines, student_id FROM students WHERE student_name = '{username}' AND password_md5 = '{MD5.ToLower()}'";
            (string[], int) result = (null, -1);


            using var cmd = new NpgsqlCommand(sql, con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                result = ((string[])rdr.GetValue(0), rdr.GetInt32(1));


            }
            return result;
        }

        async public Task<ObservableCollection<Discipline>> ReadDisciplines((string[], int) auth)
        {

            int id = auth.Item2;
            Trace.WriteLine($"{auth.Item1}   {auth.Item2}");
            string[] disciplinesList = auth.Item1 ?? throw new LoadException("Данные не успели загрузиться");

            //            SELECT matan.discipline_status AS matan, algebra.discipline_status, proga.discipline_status
            //            FROM students
            //              JOIN matan ON students.student_id = matan.student_id JOIN algebra ON students.student_id = algebra.student_id
            //              JOIN proga ON students.student_id = proga.student_id
            //              WHERE student_name = 'pavel';


            string sql = "SELECT ";
            string join_sql = "";
            ObservableCollection<Discipline> result = new ObservableCollection<Discipline>();
            foreach (var item in disciplinesList[0].Split(", "))
            {

                sql += $"{item}.discipline_status AS {item}, ";
                join_sql += $"JOIN {item} ON students.student_id = {item}.student_id ";
            }
            sql = sql.TrimEnd().TrimEnd(',');
            sql += $" FROM students {join_sql} WHERE students.student_id = {id}";
            Trace.WriteLine(sql);
            using var cmd = new NpgsqlCommand(sql, con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()) //столбы здесь находтся в том же порядке, в котором считывались дисциплины
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    result.Add(new Discipline(rdr.GetName(i), rdr.GetBoolean(i)));
                }

            }

            return result;
        }
        async public Task SaveDisciplines(int id, ObservableCollection<Discipline> disciplines, string name, string MD5)
        {
            string sql = $"";

            string[] disp_list = new string[disciplines.Count];

            int i = 0;
            foreach (Discipline discipline in disciplines)
            {
                sql = $"CREATE TABLE IF NOT EXISTS {discipline.Name} (student_id int REFERENCES students(student_id) ON DELETE CASCADE, discipline_status bool DEFAULT false)";
                using var cmd = new NpgsqlCommand(sql, con);
                disp_list[i++] = discipline.Name;
                cmd.ExecuteNonQuery();
            }
            foreach (Discipline discipline in disciplines)
            {
                sql = $"INSERT INTO {discipline.Name} (student_id, discipline_status) SELECT {id}, {discipline.Status} WHERE NOT EXISTS (SELECT student_id FROM {discipline.Name} WHERE student_id = {id})";
                using var cmd2 = new NpgsqlCommand(sql, con);

                cmd2.ExecuteNonQuery();
            }
            foreach (Discipline discipline in disciplines)
            {
                sql = $"UPDATE {discipline.Name} SET discipline_status = {discipline.Status} WHERE student_id = {id}";
                using var cmd4 = new NpgsqlCommand(sql, con);

                cmd4.ExecuteNonQuery();
            }
            string sub_str = "{\"";
            foreach (string str in disp_list)
            {
                sub_str += $"{str}, ";
            }
            sub_str = sub_str.TrimEnd().TrimEnd(',') + "\"}";
            //{"matan, algebra, proga"}
            sql = $"UPDATE students SET student_disciplines = '{sub_str}' WHERE student_id = {id}";
            using var cmd3 = new NpgsqlCommand(sql, con);
            Trace.WriteLine(sub_str);
            cmd3.ExecuteNonQuery();
            //con.Close();

        }
    }
    class LoadException : Exception
    {
        public LoadException(string message)
            : base(message) { }
    }
}
