using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Dto;

[SwaggerSchema("DTO usado para criar um novo bairro")]
public class BairroDto
{
    [Required]
    [SwaggerSchema("Nome do bairro")]
    public string NomeBairro { get; set; }
    
    [Required]
    [SwaggerSchema("Identificador único da cidade em que se encontra o bairro")]
    public int CidadeId { get; set; }
}