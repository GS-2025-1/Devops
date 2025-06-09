using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Dto;

[SwaggerSchema("DTO usado para criar uma nova cidade")]
public class CidadeDto
{
    [Required]
    [SwaggerSchema("Nome da cidade")]
    public string NomeCidade { get; set; }
    
    [Required]
    [SwaggerSchema("Identificador único do estado em que se encontra a cidade")]
    public int EstadoId { get; set; }
}