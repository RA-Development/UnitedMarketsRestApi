using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.Core.ApplicationServices.HelperServices
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly byte[] _secretBytes;

        public AuthenticationHelper(byte[] secret)
        {
            _secretBytes = secret;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (var i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != storedHash[i])
                    return false;

            return true;
        }

        public string GenerateToken(User user)
        {
            var claims = SetUpClaims(user);

            //Create a token with a header (token type JWT,private key and algorithm used for signing token) and payload (set of claims).
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(_secretBytes),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(null, // issuer - not needed (ValidateIssuer = false)
                    null, // audience - not needed (ValidateAudience = false)
                    claims.ToArray(),
                    DateTime.Now, // notBefore
                    DateTime.Now.AddMinutes(10))); // expiration time

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        ///     Create a list of claims containing username and permission.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<Claim> SetUpClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            if (user.IsAdmin) claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            else claims.Add(new Claim(ClaimTypes.Role, "User"));

            return claims;
        }
    }
}