using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.DTO;
using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Api.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{    
    [ApiController]
    [Route("users")]
    public partial class UsersController : ControllerBase
    {

        private readonly List<User> _users = new List<User>();
        private readonly IMapper mapper;
        private readonly EmailNormalizer normalizer;

        public UsersController(IMapper mapper, EmailNormalizer normalizer)
        {
            this.mapper = mapper;
            this.normalizer = normalizer;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            List<User> users = new List<User>();
            users = GetUsersFromFile();
            if(users.Count == 0) return NotFound();
            return Ok(users);
        }

        [HttpPost]
        public async Task<Result> CreateUser([FromBody] UserDTO userDTO)
        {
            var newUser = mapper.Map<User>(userDTO);

            normalizer.NormalizeEmail(ref newUser);

            if (isDuplicated(newUser, GetUsersFromFile())){
                return new Result
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                };
            }              
           
            GetNewUserGift(ref newUser);
            _users.Add(newUser);
            return new Result { IsSuccess = true, Errors="User Created"};                      
        }

        private static void GetNewUserGift(ref User user)
        {            
            switch (user.UserType)
            {
                case "Normal":
                    var percentage = user.Money > 100 ? Convert.ToDecimal(0.12) :
                        user.Money > 10 ? Convert.ToDecimal(0.8) : Convert.ToDecimal(0);
                    user.Money += (user.Money * percentage);
                    break;

                case "SuperUser":
                    if (user.Money > 100)
                        user.Money += user.Money * Convert.ToDecimal(0.20);
                    
                    break;
                
                case "Premium":
                    if (user.Money > 100)
                        user.Money += (user.Money * 2);
                    
                    break;
                
                default: throw new Exception("UserType not definded");

                
            }
        }       
        
        private List<User> GetUsersFromFile()
        {
            var reader = ReadUsersFromFile();
                       
            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                _users.Add(user);
            }
            reader.Close();
            return _users;
        }
        
        private bool isDuplicated(User newUser, List<User> _users)
        {
            return _users.Contains(
            _users.Find(x => (x.Email == newUser.Email || x.Phone == newUser.Phone
                            || x.Name == newUser.Name || x.Address == newUser.Address)));
        }
    }    
}
