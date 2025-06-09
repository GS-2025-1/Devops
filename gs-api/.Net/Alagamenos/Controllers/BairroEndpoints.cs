using Alagamenos.DbConfig;
using Alagamenos.Dto;
using Alagamenos.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alagamenos.Controllers;

public class BairroEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/bairros").WithTags("Bairro");
        
        //Get all
        group.MapGet("/", async (AlagamenosDbContext db) =>
            await db.Bairros
                .Include(b => b.Cidade)
                .ThenInclude(c => c.Estado)
                .ToListAsync())
            .WithSummary("Retorna todos os bairros")
            .WithDescription("Retorna todos os bairros cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um bairro, ele ainda vai retornar uma lista");

        // GET all paginado
        group.MapGet("/paginadas", async (int? page, AlagamenosDbContext db) =>
            {
                var pageSize = 5;
                var currentPage = page ?? 1;
                var skipItems = (currentPage - 1) * pageSize;

                var totalItems = await db.Bairros.CountAsync();
                var data = await db.Bairros
                    .Include(b => b.Cidade)
                    .ThenInclude(c => c.Estado)
                    .Skip(skipItems)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new SearchDto<Bairro>(null, currentPage, totalItems, data));
            })
            .WithSummary("Retorna alertas de bairros paginados")
            .WithDescription("Retorna todos os registros de bairros paginados. " +
                             "Cada página retorna um número fixo de bairros (5 por página neste exemplo).");
        
        //GetById
        group.MapGet("/{id}", async (int id, AlagamenosDbContext db) =>
        {
            var bairro = await db.Bairros
                .Include(b => b.Cidade)
                .ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(b => b.Id == id);
            return bairro is not null ? Results.Ok(bairro) : Results.NotFound();
        })
        .WithSummary("Busca um bairro pelo ID")
        .WithDescription("Retorna os dados de um bairro específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/inserir", async ([FromBody] BairroDto bairroDto,[FromServices] AlagamenosDbContext db) =>
            {
                var bairro = new Bairro
                {
                    NomeBairro = bairroDto.NomeBairro,
                    CidadeId = bairroDto.CidadeId
                };
                
                db.Bairros.Add(bairro);
                await db.SaveChangesAsync();
                return Results.Created($"/Bairros/{bairro.Id}", bairro);
            })
            .WithSummary("Insere um novo bairro")
            .WithDescription("Adiciona um novo bairro ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/atualizar/{id}", async (int id, [FromBody] BairroDto bairroDto, [FromServices] AlagamenosDbContext db) =>
        {
            var existing = await db.Bairros.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.NomeBairro = bairroDto.NomeBairro;
            await db.SaveChangesAsync();

            return Results.Ok($"Bairro com ID {id} atualizado com sucesso.");
        })
        .WithSummary("Atualiza um bairro existente")
        .WithDescription("Atualiza os dados de um bairro já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, AlagamenosDbContext db) =>
            {
                var bairro = await db.Bairros.FindAsync(id);
                if (bairro is null) return Results.NotFound();

                db.Bairros.Remove(bairro);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Bairro com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove um bairro")
        .WithDescription("Remove um bairro do banco de dados com base no ID informado. " +
                         "Caso o bairro não seja encontrado, retorna 404 Not Found.");
    }
}