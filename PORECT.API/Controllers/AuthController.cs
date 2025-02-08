using Tes.Domain;
using PORECT.API.Services;
using Microsoft.AspNetCore.Mvc;
using PORECT.Helper;
using System.Security.Cryptography;

namespace PORECT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        //private readonly JwtKeyService _jwtKeyService;

        public AuthController(JwtTokenService jwtTokenService)//, JwtKeyService jwtKeyService)
        {
            this._jwtTokenService = jwtTokenService;
            //this._jwtKeyService = jwtKeyService;
        }

        [HttpGet("GenerateKey")]
        public IActionResult GenerateKey()
        {
            JwtKeyService keyService = new JwtKeyService();
            return Ok(keyService.GetKey());
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AppUserRequest model)
        {
            if (model.Username == AppConfig.Config.ConfigJwt.Username && model.Password == AppConfig.Config.ConfigJwt.Password)
            {
                var result = new ReturnToken
                {
                    Token = _jwtTokenService.GenerateToken(model.Username)
                };
                return Ok(result);
            }
            return Unauthorized();
        }
    }
}
