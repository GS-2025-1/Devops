using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Dto;

[SwaggerSchema("DTO usado para criar um novo usuário")]
public class UsuarioDto
{
    [Required]
    [SwaggerSchema("Nome completo do Usuário")]
    public string Nome { get; set; }

    [Required]
    [SwaggerSchema("Data de nascimento do usuário")]
    public DateTime DataNascimento { get; set; }

    [Required]
    [SwaggerSchema("Telefone de contato do usuário")]
    public string Telefone { get; set; }

    [Required]
    [SwaggerSchema("Email do usuário")]
    public string Email { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema("Senha do usuário")]
    public string Senha { get; set; }
}