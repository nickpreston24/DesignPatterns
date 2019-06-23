using Contract.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MappingTests
{
    [TestClass]
    public class MappingTests
    {
        readonly Data.Shared.User _dataUser = new Data.Shared.User
        {
            Age = 18,
            FirstName = "Michael",
            LastName = "Preston"
        };

        readonly User _user = new User
        {
            Age = 18,
            FullName = "Preston, Michael",
        };

        [TestMethod]
        public void CanMapUsers()
        {
            //Func<Data.Shared.User, User> toFunc = user => new User
            //{
            //    Age = user.Age,
            //    FullName = $"{user.LastName}, {user.FirstName}"
            //};

            //Func<User, Data.Shared.User> fromFunc = user => new Data.Shared.User
            //{
            //    FirstName = user.FullName.Split()[1],
            //    LastName = user.FullName.Split()[0],
            //    Age = user.Age,
            //};

            //var map = new Maps<Data.Shared.User, User>();
            //map.Register(typeof(Data.Shared.User), toFunc);
            //map.Register(typeof(User), fromFunc);

            //User userContract = map[typeof(Data.Shared.User)](_dataUser);
            //User dataResult = map[typeof(User)](_user);

            //Assert.IsNotInstanceOfType(userContract, typeof(Data.Shared.User));
            //Assert.IsNotInstanceOfType(dataResult, typeof(User));
        }
    }
}
