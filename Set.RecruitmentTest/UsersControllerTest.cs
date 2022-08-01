using AutoMapper;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.DTO;
using Sat.Recruitment.Api.Utils;

namespace Set.Recruitment.Test
{

    [TestClass]
    public class UsersControllerTest
    {
        protected IMapper MapperConfiguration()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfiles());
            });
            return config.CreateMapper();
        }

        [TestMethod]
        public void CreateUserTest()
        {
            var mapper = MapperConfiguration();
            var normalizer = new EmailNormalizer();

            var testUser = new UserDTO()
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };
            var userController = new UsersController(mapper, normalizer);
            var result = userController.CreateUser(testUser).Result;
            Assert.AreEqual(true, result.IsSuccess);
            Assert.AreEqual("User Created", result.Errors);
        }

        [TestMethod]
        public void DuplicatedUserTestAsync()
        {
            var mapper = MapperConfiguration();
            var normalizer = new EmailNormalizer();

            var testUser = new UserDTO()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };

            var userController = new UsersController(mapper, normalizer);
            var result = userController.CreateUser(testUser).Result;
            Assert.AreEqual(false, result.IsSuccess);
            Assert.AreEqual("The user is duplicated", result.Errors);
        }
    }
}