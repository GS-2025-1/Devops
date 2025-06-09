using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Model;

[Table("BAIRRO")]
[SwaggerSchema("Tabela que representa os bairros de uma cidade")]
public class Bairro : IBindableFromHttpContext<Bairro>
{
    public static async ValueTask<Bairro?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Bairro));
            return (Bairro?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Bairro>();
    }
    
    [Column("ID")]
    [Key]
    [SwaggerSchema("Identificador único do bairro", ReadOnly = true)]
    public int Id { get; set; }
    
    [Column("NOME_BAIRRO")]
    [SwaggerSchema("Nome do bairro", ReadOnly = true)]
    public string NomeBairro { get; set; }
    
    [Column("CIDADE_ID")]
    [SwaggerSchema("Identificador único da cidade em que se encontra o bairro", ReadOnly = true)]
    public int CidadeId { get; set; }
    
    [ForeignKey("CidadeId")]
    [SwaggerSchema("Cidade associada ao bairro", ReadOnly = true)]
    public Cidade Cidade { get; set; }
}