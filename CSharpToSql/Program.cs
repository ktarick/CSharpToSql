using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CSharpToSql
{
    class Program
    { // STUDENT05\SQLEXPRESS
        static void Main(string[] args)
        {
            //var user = new User();
            var user = new User(0, "xxx5", "xxx5", "First", "Last", "555-555-5555", "user@user.com", true, true);
            var returnCode = User.InsertUser(user);
            User[] users = User.GetAllUsers();
            foreach(var u in users)
            {
                if(u == null)
                {
                    continue;
                }
                Console.WriteLine(u.ToPrint());
            }
            User userpk = User.GetUserByPrimaryKey(1);
            Console.WriteLine(userpk.ToPrint());
            Console.ReadKey();
        }
    }
}