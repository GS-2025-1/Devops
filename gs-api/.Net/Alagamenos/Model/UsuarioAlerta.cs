using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Model;

[Table("USUARIO_ALERTA")]
public class UsuarioAlerta
{
    [Key, Column("USUARIO_ID", Order = 0)]
    [SwaggerSchema("FK para o Usuário que recebeu o alerta", ReadOnly = true)]
    public int UsuarioId { get; set; }

    [Key, Column("ALERTA_ID", Order = 1)]
    [SwaggerSchema("FK para o Alerta que foi recebido", ReadOnly = true)]
    public int AlertaId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }

    [ForeignKey("AlertaId")]
    public Alerta Alerta { get; set; }
}