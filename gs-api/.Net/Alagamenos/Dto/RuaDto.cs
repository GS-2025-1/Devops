using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Dto;

[SwaggerSchema("DTO usado para criar um novo estado")]
public class RuaDto
{
    [Required]
    [SwaggerSchema("Nome da rua")]
    public string NomeRua { get; set; }
    
    [Required]
    [SwaggerSchema("Observação referente a rua")]
    public string? Observacao { get; set; }
    
    [Required]
    [SwaggerSchema("Identificador único do bairro em que se encontra a rua")]
    public int BairroId { get; set; }
}