﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ~SQLBaseControl() 
        
        { 
        con.Close();
        
        }
        async public  Task<(string[], int)> Login(string username, string MD5)
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
            while(rdr.Read()) //столбы здесь находтся в том же порядке, в котором считывались дисциплины
            {
                for (int i = 0; i< rdr.FieldCount; i++)
                {
                    result.Add(new Discipline(rdr.GetName(i), rdr.GetBoolean(i)));
                }
                
            }

            return result;
        }
    }
    class LoadException : Exception
    {
        public LoadException(string message)
            : base(message) { }
    }
}
