using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentManagementApplication.Contracts.Student;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementApplication.Middleware;

public class TokenProvider(IConfiguration configuration)
{
    public async Task<string> GenrateToken(LoginStudentDto loginStudent, string studentEmail)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretkey = configuration["JwT:Secrets"];
        var key = Encoding.UTF8.GetBytes(secretkey!);

        var authClaims = new List<Claim>()
        {
            new Claim("Username", loginStudent.MatricNos),
            new Claim("Email", studentEmail),
            new Claim("Id", Guid.NewGuid().ToString())
        };

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, studentEmail.ToString()),
            new("MatricNumber", loginStudent.MatricNos.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = configuration["JwT:Issuer"],
            Audience = configuration["JwT:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
