using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Dto;

[SwaggerSchema("DTO usado para criar um novo estado")]
public class EstadoDto
{
    [Required]
    [SwaggerSchema("Nome do estado")]
    public string NomeEstado { get; set; }
}