using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleMemorySupplier;
using MemorizeIt.CredentialsStorage;
using NUnit.Framework;

namespace GoogleMemorySupplierTests
{
    [TestFixture]
    public class GoogleCredentialsTests
    {
        [Test]
        public void LogIn_Login_and_password_invalid_CredentialsException()
        {

            //arrange

            GoogleCredentials target = CreateGoogleCredentials();

            //act and throw

            Assert.Throws<CredentialsException>(() => target.LogIn("fake", "invalid password"),"Invalid credentials");

        }

        [Test]
        public void LogIn_Login_and_password_are_valid_doc_is_absent_CredentialsException()
        {

            //arrange

            GoogleCredentials target = CreateGoogleCredentials("aaaaaaaaaaaaaa");

            //act and throw

            Assert.Throws<CredentialsException>(() => target.LogIn("memorize.it.test@gmail.com", "MemorizeIt"), "spreadsheet is absent");

        }

        [Test]
        public void LogIn_Login_and_password_valid_doc_presented_User_is_logged_id_password_is_not_stored()
        {

            //arrange

            GoogleCredentials target = CreateGoogleCredentials();

            //act

            string userName = "memorize.it.test@gmail.com";
            target.LogIn(userName, "MemorizeIt");

            //assert
            Assert.That(target.IsLoggedIn, Is.EqualTo(true));
            var user = target.GetCurrentUser();
            Assert.That(user.Login,Is.EqualTo(userName));
            Assert.That(user.Password, Is.EqualTo(null));
        }

        private GoogleCredentials CreateGoogleCredentials(string docName=null)
        {
            return string.IsNullOrEmpty(docName) ? new GoogleCredentials() : new GoogleCredentials(docName);
        }
    }
}
