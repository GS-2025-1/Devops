using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Alagamenos.Model;

[Table("RUA")]
[SwaggerSchema("Tabela que representa as ruas de um bairro")]
public class Rua : IBindableFromHttpContext<Rua>
{
    public static async ValueTask<Rua?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Rua));
            return (Rua?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Rua>();
    }
    
    [Column("ID")]
    [Key]
    [SwaggerSchema("Identificador único da rua", ReadOnly = true)]
    public int Id { get; set; }
    
    [Column("NOME_RUA")]
    [SwaggerSchema("Nome da rua", ReadOnly = true)]
    public string NomeRua { get; set; }
    
    [Column("OBSERVACAO")]
    [SwaggerSchema("Observação referente a rua", ReadOnly = true)]
    public string? Observacao { get; set; }
    
    [Column("BAIRRO_ID")]
    [SwaggerSchema("Identificador único do bairro em que se encontra a rua", ReadOnly = true)]
    public int BairroId { get; set; }
    
    [ForeignKey("BairroId")]
    [SwaggerSchema("Bairro associado a rua", ReadOnly = true)]
    public Bairro Bairro { get; set; }
}