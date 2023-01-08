using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RapidPay.Helpers
{
    public class AuthenticationHelper
    {
        private readonly IConfiguration _config;
        private readonly List<User> AppUsers = new List<User>
        {
            new User { FullName = "Super Admin", UserName = "admin", Password = "adminpass", UserRole = "Admin" },
            new User { FullName = "User", UserName ="user", Password = "userpass", UserRole = "User" }
        };

        public AuthenticationHelper(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Users should be stored in a database for professional purposes.
        /// </summary>
        /// <param name="loginCredentials"></param>
        /// <returns></returns>
        public User AuthenticateUser(User loginCredentials)
        {
            User user = AppUsers.SingleOrDefault(x => x.UserName == loginCredentials.UserName && x.Password == loginCredentials.Password);
            return user;
        }

        public string GenerateJWTToken(User userInfo)
        {
            var issuer = _config.GetSection("Jwt").GetSection("Issuer").Value;
            var audience = _config.GetSection("Jwt").GetSection("Audience").Value;
            var secretKey = _config.GetSection("Jwt").GetSection("SecretKey").Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim("fullName", userInfo.FullName.ToString()),
                new Claim("role",userInfo.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}