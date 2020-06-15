using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using KiancaAPI.DTOs;

namespace KiancaAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AutorizaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em: " + DateTime.Now.ToLongDateString();
        }

        [HttpGet("obtertoken")]
        public ActionResult ObterToken([FromQuery]string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Email inválido...");
                return BadRequest(ModelState);
            }
            return Ok(GerarToken(email));
        }

        private UsuarioTokenDTO GerarToken(string email)
        {
            var claims = new[]
            {
                  new Claim(JwtRegisteredClaimNames.UniqueName, email),
                  new Claim("qualquerchave", "qualquervalor"),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
              };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireHours = double.Parse(_config["TokenConfiguration:ExpireHours"]);
            var expiration = DateTime.UtcNow.AddHours(expireHours);

            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _config["TokenConfiguration:Issuer"],
              audience: _config["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credentials
            );

            return new UsuarioTokenDTO()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }
    }
}