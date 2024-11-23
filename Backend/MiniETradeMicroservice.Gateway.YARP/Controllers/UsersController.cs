using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniETradeMicroservice.Gateway.YARP.Context;
using MiniETradeMicroservice.Gateway.YARP.Dtos;
using MiniETradeMicroservice.Gateway.YARP.Entities;
using MiniETradeMicroservice.Gateway.YARP.Models;
using MiniETradeMicroservice.Gateway.YARP.Services;

namespace MiniETradeMicroservice.Gateway.YARP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        public UsersController(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto userRegisterDto, CancellationToken ct)
        {
            using ApplicationDbContext context = new ApplicationDbContext();
            DbSet<User> dbSet = context.Set<User>();

            if (await dbSet.AnyAsync(p => p.Username == userRegisterDto.Username, ct))
            {
                return BadRequest(new Result(false, "this user already exist."));
            }
            dbSet.Add(userRegisterDto);
            return await context.SaveChangesAsync(ct) > 0 ? Ok(new DataResult<User>(status: true, message: "Error", data: userRegisterDto)) : BadRequest(new DataResult<User>(status: false, message: "Error", data: null));
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto userLoginDto, CancellationToken ct)
        {
            using ApplicationDbContext context = new ApplicationDbContext();
            DbSet<User> dbSet = context.Set<User>();

            if (!await dbSet.AnyAsync(p => p.Username == userLoginDto.Username && p.Password == userLoginDto.Password, ct))
            {
                return BadRequest(new Result(false, "this user not found."));
            }

            return Ok(new DataResult<object>(status: true, message: "Login Succesfully.", data: _jwtService.CreateToken(userLoginDto)));
        }
    }
}
