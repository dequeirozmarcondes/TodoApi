using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoAppIdentity.Model;

namespace TodoAppIdentity.Services;

// Serviço responsável por criar e assinar o JWT
public class TokenService(SymmetricSecurityKey signingKey, IConfiguration configuration)
{
    private readonly SymmetricSecurityKey _signingKey = signingKey;
    private readonly IConfiguration _configuration = configuration;

    public string CreateToken(ApplicationUser user, IEnumerable<string> roles)
    {
        // 1. OBTÉM O EMISSOR
        // Tenta pegar a URL dinâmica do Aspire, se não conseguir, usa o valor estático do appsettings.json
        var issuer = _configuration["Services:todoappidentity:https__default"]
                     ?? _configuration["Jwt:Issuer"]
                     ?? throw new InvalidOperationException("Issuer JWT não configurado (Verifique Aspire e Jwt:Issuer).");


        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id), // Subject (ID do usuário)
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID do token
            new(ClaimTypes.Name, user.UserName!), // Nome do usuário
            new(JwtRegisteredClaimNames.Iss, issuer)
        };

        // Adiciona as roles
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // Token expira em 7 dias

            // Opcional, mas mantém a clareza de onde o token está sendo emitido
            Issuer = issuer,

            SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
