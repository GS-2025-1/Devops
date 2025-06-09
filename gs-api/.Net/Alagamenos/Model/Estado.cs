using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Model;

[Table("ESTADO")]
[SwaggerSchema("Tabela que representa os estados do país")]
public class Estado : IBindableFromHttpContext<Estado>
{
    public static async ValueTask<Estado?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Estado));
            return (Estado?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Estado>();
    }
    
    [Column("ID")]
    [Key]
    [SwaggerSchema("Identificador único de estado", ReadOnly = true)]
    public int Id { get; set; }
    
    [Column("NOME_ESTADO")]
    [SwaggerSchema("Nome do estado", ReadOnly = true)]
    public string NomeEstado { get; set; }
    
}