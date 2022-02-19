using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Helpers
{
    public class JwtFacade
    {
        private readonly IOptions<JwtSettings> _jwtSettings;
        private string _issuer;
        private string _audience;
        private string _secretKey;
        private string _jwtAlg;

        public JwtFacade(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings;
            _issuer = _jwtSettings.Value.Issuer ?? throw new NullReferenceException();
            _audience = _jwtSettings.Value.Audience ?? throw new NullReferenceException();
            _secretKey = _jwtSettings.Value.SecretKey ?? throw new NullReferenceException();
            _jwtAlg = _jwtSettings.Value.Algorithm ?? throw new NullReferenceException();
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var signingCredentials = new SigningCredentials(securityKey, _jwtAlg);
            var securityToken = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(2),
                signingCredentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}