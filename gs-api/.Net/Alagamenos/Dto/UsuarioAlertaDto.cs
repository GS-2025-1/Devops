using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Dto;

[SwaggerSchema("DTO usado para criar um novo alerta de usuario")]
public class UsuarioAlertaDto
{
    [Required]
    [SwaggerSchema("FK para o Usuário que recebeu o alerta")]
    public int UsuarioId { get; set; }

    [Required]
    [SwaggerSchema("FK para o Alerta que foi recebido")]
    public int AlertaId { get; set; }
}