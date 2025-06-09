using Alagamenos.DbConfig;
using Alagamenos.Dto;
using Alagamenos.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alagamenos.Controllers;

public class AlertaEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/alertas").WithTags("Alerta");
        
        //Get all
        group.MapGet("/", async (AlagamenosDbContext db) =>
            await db.Alertas
                .Include(a => a.Rua)
                .ThenInclude(r => r.Bairro)
                .ThenInclude(b => b.Cidade)
                .ThenInclude(c => c.Estado)
                .ToListAsync())
            .WithSummary("Retorna todos os alertas")
            .WithDescription("Retorna todos os alertas cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um alerta, ele ainda vai retornar uma lista");

        // GET all paginado
        group.MapGet("/paginadas", async (int? page, AlagamenosDbContext db) =>
            {
                var pageSize = 5;
                var currentPage = page ?? 1;
                var skipItems = (currentPage - 1) * pageSize;

                var totalItems = await db.Alertas.CountAsync();
                var data = await db.Alertas
                    .Include(a => a.Rua)
                    .ThenInclude(r => r.Bairro)
                    .ThenInclude(b => b.Cidade)
                    .ThenInclude(c => c.Estado)
                    .Skip(skipItems)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new SearchDto<Alerta>(null, currentPage, totalItems, data));
            })
            .WithSummary("Retorna alertas de usuários paginados")
            .WithDescription("Retorna todos os registros de endereços paginados. " +
                             "Cada página retorna um número fixo de aletas (5 por página neste exemplo).");

        
        //GetById
        group.MapGet("/{id}", async (int id, AlagamenosDbContext db) =>
        {
            var alerta = await db.Alertas
                .Include(a => a.Rua)
                .ThenInclude(r => r.Bairro)
                .ThenInclude(b => b.Cidade)
                .ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(a => a.Id == id);
            return alerta is not null ? Results.Ok(alerta) : Results.NotFound();
        })
        .WithSummary("Busca um alerta pelo ID")
        .WithDescription("Retorna os dados de um alerta específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/inserir", async ( [FromBody] AlertaDto alertaDto,[FromServices] AlagamenosDbContext db) =>
            {
                var alerta = new Alerta
                {
                    Mensagem = alertaDto.Mensagem,
                    DataCriacao = alertaDto.DataCriacao,
                    RuaId = alertaDto.RuaId
                };
                
                db.Alertas.Add(alerta);
                await db.SaveChangesAsync();
                return Results.Created($"/Alertas/{alerta.Id}", alerta);
            })
            .WithSummary("Insere um novo alerta")
            .WithDescription("Adiciona um novo alerta ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, AlagamenosDbContext db) =>
            {
                var alerta = await db.Alertas.FindAsync(id);
                if (alerta is null) return Results.NotFound();

                db.Alertas.Remove(alerta);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Alerta com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove um alerta")
        .WithDescription("Remove um alerta do banco de dados com base no ID informado. " +
                         "Caso o alerta não seja encontrado, retorna 404 Not Found.");
    }
}