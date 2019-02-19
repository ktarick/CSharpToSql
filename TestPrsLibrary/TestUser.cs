using System;
using CSharpToSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPrsLibrary
{
    [TestClass]
    public class TestUser
    {
        [TestMethod]
        public void GetAllUsers()
        {
            try
            {
                var users = User.GetAllUsers();
                Assert.IsInstanceOfType(users, typeof(User[]), "users is Not a User[]");
                Assert.AreEqual(10, users.Length, "There should be 10 users in DB");
                var firstUser = users[0];
                Assert.AreEqual(1, firstUser.Id);
                Assert.AreEqual("karick", firstUser.Username);
                Assert.AreEqual("123", firstUser.Password);
                Assert.IsNull(firstUser.Phone);
                Assert.IsNull(firstUser.Email);
                Assert.IsTrue(firstUser.IsReviewer);
                Assert.IsTrue(firstUser.IsAdmin);
                
                var lastUser = users[users.Length -1];
                Assert.AreEqual(21, lastUser.Id);
                Assert.AreEqual("xxx9", lastUser.Username);
                Assert.AreEqual("xxx8", lastUser.Password);
                Assert.AreEqual("5135551234", lastUser.Phone);
                Assert.AreEqual("info@user.com", lastUser.Email);
                Assert.IsTrue(lastUser.IsReviewer);
                Assert.IsTrue(lastUser.IsAdmin);
            }
            catch (Exception)
            {
                Assert.Fail("May have Open connection problem.");
            }            
        }
    }
}
