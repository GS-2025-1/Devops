using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Dto;

[SwaggerSchema("DTO usado para criar um novo endereço")]
public class EnderecoDto
{
    [Required]
    [SwaggerSchema("Número do endereço")]
    public string NumeroEndereco { get; set; }
    
    [Required]
    [SwaggerSchema("Complemento do endereço")]
    public string? Complemento { get; set; }

    [Required]
    [SwaggerSchema("Identificador único da Rua em que se encontra o endereço")]
    public int RuaId { get; set; }
    
    [Required]
    [SwaggerSchema("Identificador único do Usuário associado ao endereço")]
    public int UsuarioId { get; set; }
}