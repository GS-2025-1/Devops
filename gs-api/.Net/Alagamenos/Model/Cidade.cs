using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Model;

[Table("CIDADE")]
[SwaggerSchema("Tabela que representa as cidades de um estado")]
public class Cidade : IBindableFromHttpContext<Cidade>
{
    public static async ValueTask<Cidade?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Estado));
            return (Cidade?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Cidade>();
    }
    
    [Column("ID")]
    [Key]
    [SwaggerSchema("Identificador único da cidade", ReadOnly = true)]
    public int Id { get; set; }
    
    [Column("NOME_CIDADE")]
    [SwaggerSchema("Nome da cidade", ReadOnly = true)]
    public string NomeCidade { get; set; }
    
    [Column("ESTADO_ID")]
    [SwaggerSchema("Identificador único do estado em que se encontra a cidade", ReadOnly = true)]
    public int EstadoId { get; set; }
    
    [ForeignKey("EstadoId")]
    [SwaggerSchema("Estado associado à cidade", ReadOnly = true)]
    public Estado Estado { get; set; }
}