using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MTCG_SWEN1.Test
{
    public class Tests
    {
        private User _user1;
        private User _user2;
        private User _user3;        

        [SetUp]
        public void Setup()
        {
            _user1 = new(Guid.NewGuid(), "lukas", "varga", 20, 100, "if20b167", ":-)", 0, 0, 0);
            _user2 = new(Guid.NewGuid(), "sophie", "arcade", 20, 100, "angestellt", ";-)", 0, 0, 0);
            _user3 = new(Guid.NewGuid(), "kienboec", "daniel", 20, 100, "angestellt", ";-)", 0, 0, 0);
        }

        [Test]
        public void Test_InvalidCredentialsWillFailToLogin()
        {
            bool isValid = UserService.CheckIfCredentialsComplete(_user1.Username, "");
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Test_ValidCredentialsWillSucceedToLogin()
        {
            bool isValid = UserService.CheckIfCredentialsComplete(_user1.Username, _user1.Password);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Test_CreateValidUserTokenContainingUsername()
        {
            var token = UserService.CreateToken(_user1);
            Assert.AreEqual($"Basic {_user1.Username}-mtcgToken", token);
        }

        [Test]
        public void Test_PathParameterEqualToLoginUser()
        {
            string pathparameter = "lukas";
            bool isEqual = UserService.ValidateUserLoginWithPathParameter(pathparameter, _user1.Username);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void Test_SuccessfulRegistrationOfNewUser()
        {
            Dictionary<string, string> credentials = new();
            credentials.Add("Username", _user3.Username);
            credentials.Add("Password", _user3.Password);

            Assert.DoesNotThrow(() => UserService.RegisterService(credentials));
        }

        [Test]
        public void Test_GetUserSuccessfulByToken()
        {
            string token = UserService.CreateToken(_user3);
            User user = new();

            Assert.DoesNotThrow(() => UserService.GetUser(token));
        }
    }
}