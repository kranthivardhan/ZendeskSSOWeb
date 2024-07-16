using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.IdentityModel.Tokens.Jwt;

namespace ZendeskSSOWeb
{
    public class ZendeskRedirector
    {
        public string RedirectToZendeskArticle(string subdomain, string userEmail, string Username, string sharedSecret, string articleId)
        {
            //ZendeskSSO zendeskSSO = new ZendeskSSO();
            string token = GenerateJwtToken(Username, userEmail, sharedSecret);
            //string redirectUrl = $"https://{subdomain}.zendesk.com/access/jwt?jwt={token}&return_to=https://{subdomain}.zendesk.com/hc/en-us/articles/{articleId}";
            string redirectUrl = $"https://{subdomain}.zendesk.com/access/jwt?jwt={HttpUtility.UrlEncode(token)}&return_to={HttpUtility.UrlEncode($"https://{subdomain}.zendesk.com/hc/en-us/articles/{articleId}")},_blank";

            // Log the generated token and redirection URL for debugging
            //Console.WriteLine("Generated JWT Token: " + token);
            //Console.WriteLine("Redirection URL: " + redirectUrl);

            //HttpContext.Current.Response.Redirect(redirectUrl);
            //Application.Navigate(redirectUrl, target: "_blank");
            
            return redirectUrl;
        }

        public string GenerateJwtToken(string Username, string userEmail, string sharedSecret)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sharedSecret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {

            new Claim("iat",DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim("jti", System.Guid.NewGuid().ToString()),
            new Claim("name", Username),
              new Claim("email", userEmail)
        };

                var token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                throw;
            }
        }
    }
}