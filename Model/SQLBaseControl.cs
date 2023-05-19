using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        async public static Task<string[]> Login(string username, string MD5)
        {
            string sql = $"SELECT student_name, password_md5, student_disciplines FROM students WHERE student_name = '{username}' AND password_md5 = '{MD5.ToLower()}'";
            string[] result = new string[0];
            var cs = "Host=localhost;Username=postgres;Password=Duster2019;Database=Panic";
            using var con = new NpgsqlConnection(cs);
            con.Open();
            using var cmd = new NpgsqlCommand(sql, con);
            Trace.WriteLine($"========TRACE {sql}==========");
            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                result = (string[])rdr.GetValue(2);
                Trace.WriteLine($"========TRACE {result[0]} ==========");

            }
            return result;
        }

        async public Task<(string, bool)> ReadDisciplines(string name)
        {
            string sql = "SELECT matan.discipline_status AS matan, algebra.discipline_status AS algebra, proga.discipline_status AS proga FROM students JOIN matan ON students.student_id = matan.student_id JOIN algebra ON students.student_id = algebra.student_id JOIN proga ON students.student_id = proga.student_id WHERE student_name = 'Брахислав';";
            using var cmd = new NpgsqlCommand(sql, con);

            await using NpgsqlDataReader rdr = cmd.ExecuteReader();

            var result = ("", true);
            while (rdr.Read())
            {
                //Trace.WriteLine($" Try to debug");
                result = (rdr.GetName(0), rdr.GetBoolean(0));
            }
            return result;
        }
    }
}
