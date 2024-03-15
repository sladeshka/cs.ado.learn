using Microsoft.VisualBasic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;
using System.Windows.Controls.Primitives;
using System.Collections;

namespace WpfApp1
{
    public class MyDB
    {
        public NpgsqlConnection connect;
        public string tableName;
        public List<string> columns;
        public MyDB(string login, string password) {
            string connectionString = ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;
            connectionString += $"" +
                $"Username={login};" +
                $"Password={password}";
            connect = new NpgsqlConnection(connectionString);           
        }
        public void setColumns()
        {
            connect.Open();
            string query = $"SELECT column_name FROM information_schema.columns WHERE table_name = '{tableName}';";
            using (NpgsqlCommand command = new NpgsqlCommand(query, connect))
            {
                columns = new List<string>();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader["column_name"].ToString());
                    }
                    connect.Close();
                }
            }
        }
        public void updateOrNewRow(DataRowView dataRow)
        {
            if (dataRow != null)
            {
                connect.Open();
                int columnN = 0;
                int columnCount = columns.Count;
                string queryInsertTable = "";
                string queryInsertValues = "";
                string queryUpdate = "";
                foreach (string column in columns)
                {
                    queryInsertTable += column;
                    queryInsertValues += "@CI" + columnN;
                    queryUpdate += column + "=EXCLUDED." + column;
                    if (columnCount != columnN + 1)
                    {
                        queryInsertTable += ", ";
                        queryInsertValues += ", ";
                        queryUpdate += ", ";
                    }

                    columnN++;
                }

                string query = $"INSERT INTO {tableName} ({queryInsertTable}) VALUES ({queryInsertValues}) ON CONFLICT({columns[0]}) DO UPDATE SET {queryUpdate};";
                try
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connect))
                    {
                        for (int i = 0; i < columnCount; i++)
                        {
                            command.Parameters.AddWithValue("@CI" + i, dataRow[i]);
                        }
                        command.ExecuteNonQuery();
                        MessageBox.Show("row updated");
                        connect.Close();
                    }
                }
                catch (Exception ex)
                {
                    connect.Close();
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void deleteRow(DataRowView dataRow)
        {
            if (dataRow != null)
            {
                connect.Open();
                string query = $"DELETE FROM {tableName} WHERE {columns[0]} = @C0";
                try
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connect))
                    {
                        command.Parameters.AddWithValue("@C0", dataRow[0]);
                        command.ExecuteNonQuery();
                        MessageBox.Show("row deleted");
                        connect.Close();
                    }
                }
                catch (Exception ex)
                {
                    connect.Close();
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
