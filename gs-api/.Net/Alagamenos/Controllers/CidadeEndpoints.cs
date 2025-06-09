using Alagamenos.DbConfig;
using Alagamenos.Dto;
using Alagamenos.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alagamenos.Controllers;

public class CidadeEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/cidades").WithTags("Cidade");
        
        //Get all
        group.MapGet("/", async (AlagamenosDbContext db) =>
            await db.Cidades
                .Include(c => c.Estado)
                .ToListAsync())
            .WithSummary("Retorna todos as cidades")
            .WithDescription("Retorna todos as cidades cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado uma cidade, ele ainda vai retornar uma lista");

        // GET all paginado
        group.MapGet("/paginadas", async (int? page, AlagamenosDbContext db) =>
            {
                var pageSize = 5;
                var currentPage = page ?? 1;
                var skipItems = (currentPage - 1) * pageSize;

                var totalItems = await db.Cidades.CountAsync();
                var data = await db.Cidades
                    .Include(c => c.Estado)
                    .Skip(skipItems)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new SearchDto<Cidade>(null, currentPage, totalItems, data));
            })
            .WithSummary("Retorna alertas de cidades paginados")
            .WithDescription("Retorna todos os registros de cidades paginados. " +
                             "Cada página retorna um número fixo de cidades (5 por página neste exemplo).");
        
        //GetById
        group.MapGet("/{id}", async (int id, AlagamenosDbContext db) =>
        {
            var cidade = await db.Cidades
                .Include(c => c.Estado)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            return cidade is not null ? Results.Ok(cidade) : Results.NotFound();
        })
        .WithSummary("Busca uma cidade pelo ID")
        .WithDescription("Retorna os dados de uma cidade específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/inserir", async ( [FromBody] CidadeDto cidadeDto, [FromServices] AlagamenosDbContext db) =>
            {
                var cidade = new Cidade
                {
                    NomeCidade = cidadeDto.NomeCidade,
                    EstadoId = cidadeDto.EstadoId
                };
                
                db.Cidades.Add(cidade);
                await db.SaveChangesAsync();
                return Results.Created($"/Cidades/{cidade.Id}", cidade);
            })
            .WithSummary("Insere uma nova cidade")
            .WithDescription("Adiciona uma nova cidade ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/atualizar/{id}", async (int id, [FromBody] CidadeDto cidadeDto, [FromServices] AlagamenosDbContext db) =>
        {
            var existing = await db.Cidades.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.NomeCidade = cidadeDto.NomeCidade;
            await db.SaveChangesAsync();

            return Results.Ok($"Cidade com ID {id} atualizado com sucesso.");
        })
        .WithSummary("Atualiza uma cidade existente")
        .WithDescription("Atualiza os dados de uma cidade já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, AlagamenosDbContext db) =>
            {
                var cidade = await db.Cidades.FindAsync(id);
                if (cidade is null) return Results.NotFound();

                db.Cidades.Remove(cidade);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Cidade com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove uma cidade")
        .WithDescription("Remove uma cidade do banco de dados com base no ID informado. " +
                         "Caso a cidade não seja encontrado, retorna 404 Not Found.");
    }
}