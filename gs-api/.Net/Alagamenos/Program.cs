using Alagamenos.DbConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Faz a API ouvir em http://*:80 dentro do container
builder.WebHost.UseUrls("http://*:80");

// Configura o banco Oracle
builder.Services.AddDbContext<AlagamenosDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("FiapOracleDb")));

// Configura os controllers e serialização
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddXmlSerializerFormatters();

// Swagger sempre habilitado
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Alagamenos",
        Version = "v1",
        Description = "API para envio de alertas de alagamento para usuários."
    });
    opt.EnableAnnotations(); 
});

var app = builder.Build();

// 🔧 Habilita o Swagger mesmo fora de desenvolvimento
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Alagamenos v1");
    c.RoutePrefix = string.Empty; // Acessa direto por http://localhost:5000/
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); 

// Mapeia os endpoints declarados separadamente
Alagamenos.Controllers.AlertaEndpoints.Map(app);
Alagamenos.Controllers.BairroEndpoints.Map(app);
Alagamenos.Controllers.CidadeEndpoints.Map(app);
Alagamenos.Controllers.EnderecoEndpoints.Map(app);
Alagamenos.Controllers.EstadoEndpoints.Map(app);
Alagamenos.Controllers.RuaEndpoints.Map(app);
Alagamenos.Controllers.UsuarioEndpoints.Map(app);
Alagamenos.Controllers.UsuarioAlertaEndpoints.Map(app);

app.Run();
