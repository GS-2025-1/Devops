namespace Alagamenos.Dto;

using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

[SwaggerSchema("DTO usado para criar um novo alerta")]
public class AlertaDto
{
    [Required]
    [SwaggerSchema("Mensagem do Alerta emitido")]
    public string Mensagem { get; set; }

    [Required]
    [SwaggerSchema("Data e hora em que o alerta foi criado")]
    public DateTime DataCriacao { get; set; }

    [Required]
    [SwaggerSchema("Identificador da rua onde o alerta será registrado")]
    public int RuaId { get; set; }
}