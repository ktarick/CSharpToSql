using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToSql
{
    public class User
    {
        private static string CONN_STRING = @"server=STUDENT05\SQLEXPRESS; database=PrsDb; trusted_connection=true;";
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }

        private static SqlConnection CreateAndCheckConnection()
        {
            var Connection = new SqlConnection(CONN_STRING);
            Connection.Open();
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connection did not open");
                return null;
            }
            return Connection;
        }
        public static bool UpdateUser(User user)
        {
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return false;
            }
            var isReviewer = user.IsReviewer ? 1 : 0;
            var isAdmin = user.IsAdmin ? 1 : 0;
            var sql = new StringBuilder("UPDATE Users set ");
            sql.Append($"Username = '{user.Username}',"); //added single to surround input username
            sql.Append($"Password = '{user.Password}',");
            sql.Append($"Firstname = '{user.Firstname}',");
            sql.Append($"Lastname = '{user.Lastname}',");
            sql.Append($"Phone = '{user.Phone}',");
            sql.Append($"Email = '{user.Email}',");
            sql.AppendFormat("IsReviewer = {0} ,", (user.IsReviewer ? 1 : 0)); //using ternary operator
            sql.AppendFormat("IsAdmin = {0}", (user.IsAdmin ? 1 : 0));
            sql.Append($" Where Id = {user.Id}");
            var cmd = new SqlCommand(sql.ToString(), Connection);
            var recsAffected = cmd.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;
        }
        public static bool DeleteUser(int Id)
        {
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return false;
            }
            var sql = $"DELETE from Users Where Id = {Id}";
            var cmd = new SqlCommand(sql, Connection);
            var recsAffected = cmd.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;
        }
        public static bool InsertUser(User user)
        {
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return false;
            }
            var isReviewer = user.IsReviewer ? 1 : 0;
            var isAdmin = user.IsAdmin ? 1 : 0;
            var sql = $"INSERT into users (Username, Password, Firstname, Lastname, Phone, Email, IsReviewer, IsAdmin)" + $"values ('{user.Username}', '{user.Password}', '{user.Firstname}', '{user.Lastname}', '{user.Phone}', '{user.Email}', {isReviewer}, {isAdmin})";
            var cmd = new SqlCommand(sql, Connection);
            var recsAffected = cmd.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;
        }
        private static SqlDataReader CheckSqlReaderAndCheck(string sql, SqlConnection Connection)
        {
            var cmd = new SqlCommand(sql, Connection);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Result set has no row.");
                Connection.Close();
                return null;
            }
            return reader;
        }
        public static User GetUserByPrimaryKey(int Id)
        {
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return null;
            }
            var sql = $"SELECT * from Users WHERE Id = {Id};";
            var reader = CheckSqlReaderAndCheck(sql, Connection);
            reader.Read();
            var user = new User();
            user.Id = (int)reader["Id"];
            user.Username = (string)reader["Username"];
            user.Firstname = (string)reader["Firstname"];
            user.Lastname = (string)reader["Lastname"];
            user.Phone = reader["Phone"] == DBNull.Value ? null : (string)reader["Phone"];
            user.Email = reader["Email"] == DBNull.Value ? null : (string)reader["Email"];
            user.IsReviewer = (bool)reader["IsReviewer"];
            user.IsAdmin = (bool)reader["IsAdmin"];
            Connection.Close();
            return user;
        }
        public static User[] GetAllUsers()
        {
            //connection string (server,database, authentication)
            //refactored even more
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return null;
            }            
            var sql = "SELECT * from Users;"; //sqlcommand          
            var reader = CheckSqlReaderAndCheck(sql, Connection); //sqldatareader object
            var users = new List<User>();
            var index = 0;
            while (reader.Read()) //moves pointer to the next vaild row
            {
                var user = new User();
                user.Id = (int)reader["Id"]; //should always reurn the column name
                user.Username = (string)reader["Username"];
                user.Password = (string)reader["Password"];
                user.Firstname = (string)reader["Firstname"];
                user.Lastname = (string)reader["Lastname"];
                user.Phone = reader["Phone"] == DBNull.Value ? null : (string)reader["Phone"];
                user.Email = reader["Email"] == DBNull.Value ? null : (string)reader["Email"];
                user.IsReviewer = (bool)reader["IsReviewer"];
                user.IsAdmin = (bool)reader["IsAdmin"];
                users.Add(user);                
            }
            //Console.ReadKey();
            //statement to close
            Connection.Close();
            return users.ToArray();
        }
        //const
        public User()
        { }
        public User(int id, string username, string password, string firstname, string lastname,
                    string phone, string email, bool isReviewewr, bool isAdmin)
        {
            Id = id;
            Username = username;
            Password = password;
            Firstname = firstname;
            Lastname = lastname;
            Phone = phone;
            Email = email;
            IsReviewer = isReviewewr;
            IsAdmin = isAdmin;
        }
        public override string ToString()
        {
            return $"[ToString()] Id={Id}, Username={Username}, Name={Firstname} {Lastname}";
        }
    }
}