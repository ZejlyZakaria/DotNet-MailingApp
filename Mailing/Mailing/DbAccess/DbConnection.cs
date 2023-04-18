using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Mailing.Models;

namespace Mailing.DbAccess
{
    public class DbConnection
    {
        private SqlConnection connection;
        public DbConnection()
        {
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hp\Desktop\Mailing\Mailing\App_Data\mailing.mdf;Integrated Security=True";
                connection.Open();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public SqlConnection Connection { get => connection; set => connection = value; }

        public int Insert(String tableName, params Object[] row)
        {
            string query = "INSERT INTO " + tableName + " VALUES ('" + row[0] + "'";
            for (int i = 1; i < row.Length; i++)
            {
                query += ", '" + row[i] + "'";

            }
            query += ")";
            SqlCommand cmd = new SqlCommand(query, connection);
            return cmd.ExecuteNonQuery();
        }

        public int Insert(String tableName, Object[] columns, params Object[] row)
        {
            string query = "INSERT INTO " + tableName + "(" + columns[0];
            for (int i = 1; i < columns.Length; i++)
            {
                query += ", " + columns[i];

            }
            query += ") VALUES ('" + row[0] + "'";
            for (int i = 1; i < row.Length; i++)
            {
                query += ", '" + row[i] + "'";

            }
            query += ")";
            SqlCommand cmd = new SqlCommand(query, connection);
            return cmd.ExecuteNonQuery();
        }

        public int Delete(string tableName, string column, Object condition)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + column + " = '" + condition + "'";
            SqlCommand cmd = new SqlCommand(query, connection);
            return cmd.ExecuteNonQuery();
        }

        public int Update(string tableName, string column, Object condition, Object[] columns, params Object[] row)
        {
            string query = "UPDATE " + tableName + " SET " + columns[0] + " = '" + row[0] + "'";
            for (int i = 1; i < row.Length; i++)
            {
                query += ", " + columns[i] + " = '" + row[i] + "'";

            }
            query += " WHERE " + column +  " = '" + condition + "'";
            SqlCommand cmd = new SqlCommand(query, connection);
            return cmd.ExecuteNonQuery();
        }

        public SqlDataReader SelectAll(string tableName, Object[] columns, params Object[] row)
        {
            string query = "SELECT * FROM " + tableName;
            if (columns.Length > 0)
            {
                query += " WHERE " + columns[0] + " = @0";
                for (int i = 1; i < row.Length; i++)
                {
                    query += " AND " + columns[i] + " = @" + i;

                }
                SqlCommand cmd = new SqlCommand(query, connection);
                for (int i = 0; i < row.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@" + i, row[i]);

                }
                return cmd.ExecuteReader();

            }
            else
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                return cmd.ExecuteReader();
            }

        }

        public SqlDataReader SelectAllOr(string tableName, Object[] columns, params Object[] row)
        {
            string query = "SELECT * FROM " + tableName;
            if (columns.Length > 0)
            {
                query += " WHERE " + columns[0] + " = @0";
                for (int i = 1; i < row.Length; i++)
                {
                    query += " OR " + columns[i] + " = @" + i;

                }
                SqlCommand cmd = new SqlCommand(query, connection);
                for (int i = 0; i < row.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@" + i, row[i]);

                }
                return cmd.ExecuteReader();

            }
            else
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                return cmd.ExecuteReader();
            }

        }
        public SqlDataReader SelectLastId(string tableName)
        {
            string query = "SELECT TOP 1 id FROM " +  tableName + " ORDER BY ID DESC;";
            SqlCommand cmd = new SqlCommand(query, connection);
            return cmd.ExecuteReader();
        }


    }
}