using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hisoka;
using HisokaExample.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HisokaExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserRepository userRepository,
            ILogger<UsersController> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(ResourceQueryFilter query) 
        {
            var users = await _userRepository.GetUsers(query);
            return Ok(users);
        }

        [HttpGet("teste")]
        public async Task<IActionResult> GetUsersRoles(ResourceQueryFilter query) 
        {
            var usrs = await _userRepository.GetUserRoles(query);
            return Ok(usrs);
        }
    }
}
