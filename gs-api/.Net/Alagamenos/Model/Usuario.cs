using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using Swashbuckle.AspNetCore.Annotations;


namespace Alagamenos.Model;

[Table("USUARIO")]
[SwaggerSchema("Tabela que representa os usuarios")]
public class Usuario : IBindableFromHttpContext<Usuario>
{
    public static async ValueTask<Usuario?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Estado));
            return (Usuario?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Usuario>();
    }
    [Column("ID")]
    [Key]
    [SwaggerSchema("Identificador único do usuário", ReadOnly = true)]
    public int Id { get; set; }

    [Column("NOME")]
    [SwaggerSchema("Nome completo do Usuário", ReadOnly = true)]
    public string Nome { get; set; }

    [Column("DATA_NASCIMENTO")]
    [SwaggerSchema("Data de nascimento do usuário", ReadOnly = true)]
    public DateTime DataNascimento { get; set; }

    [Column("TELEFONE")]
    [SwaggerSchema("Telefone de contato do usuário", ReadOnly = true)]
    public string Telefone { get; set; }

    [Column("EMAIL")]
    [SwaggerSchema("Email do usuário", ReadOnly = true)]
    public string Email { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema("Senha do usuário", ReadOnly = true)]
    [Column("SENHA")]
    public string Senha { get; set; }
}