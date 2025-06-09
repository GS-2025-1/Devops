using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Model;

[Table("ENDERECO")]
[SwaggerSchema("Tabela que representa os endereços associados a um usuário em uma rua")]
public class Endereco : IBindableFromHttpContext<Endereco>
{
    public static async ValueTask<Endereco?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Endereco));
            return (Endereco?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Endereco>();
    }
    
    [Column("ID")]
    [Key]
    [SwaggerSchema("Identificador único do endereco", ReadOnly = true)]
    public int Id { get; set; }
    
    [Column("NUMERO_ENDERECO")]
    [SwaggerSchema("Número do endereço", ReadOnly = true)]
    public string NumeroEndereco { get; set; }
    
    [Column("COMPLEMENTO")]
    [SwaggerSchema("Complemento do endereço", ReadOnly = true)]
    public string? Complemento { get; set; }

    [Column("RUA_ID")]
    [SwaggerSchema("Identificador único da Rua em que se encontra o endereço", ReadOnly = true)]
    public int RuaId { get; set; }
    
    [ForeignKey("RuaId")]
    [SwaggerSchema("Rua associada ao endereço", ReadOnly = true)]
    public Rua Rua { get; set; }
    
    [Column("USUARIO_ID")]
    [SwaggerSchema("Identificador único do Usuário associado ao endereço", ReadOnly = true)]
    public int UsuarioId { get; set; }
    
    [ForeignKey("UsuarioId")]
    [SwaggerSchema("Rua associada ao endereço", ReadOnly = true)]
    public Usuario Usuario { get; set; }
}