using Alagamenos.DbConfig;
using Alagamenos.Dto;
using Alagamenos.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alagamenos.Controllers;

public class RuaEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/ruas").WithTags("Rua");
        
        //Get all
        group.MapGet("/", async (AlagamenosDbContext db) =>
            await db.Ruas
                .Include(r => r.Bairro)
                .ThenInclude(b => b.Cidade)
                .ThenInclude(c => c.Estado)
                .ToListAsync())
            .WithSummary("Retorna todos as ruas")
            .WithDescription("Retorna todos as ruas cadastradas no banco de dados, " +
                             "mesmo que só seja encontrado uma rua, ele ainda vai retornar uma lista");

        // GET all paginado
        group.MapGet("/paginadas", async (int? page, AlagamenosDbContext db) =>
            {
                var pageSize = 5;
                var currentPage = page ?? 1;
                var skipItems = (currentPage - 1) * pageSize;

                var totalItems = await db.Ruas.CountAsync();
                var data = await db.Ruas
                    .Include(r => r.Bairro)
                    .ThenInclude(b => b.Cidade)
                    .ThenInclude(c => c.Estado)
                    .Skip(skipItems)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new SearchDto<Rua>(null, currentPage, totalItems, data));
            })
            .WithSummary("Retorna alertas de ruas paginados")
            .WithDescription("Retorna todos os registros de ruas paginados. " +
                             "Cada página retorna um número fixo de ruas (5 por página neste exemplo).");
        
        //GetById
        group.MapGet("/{id}", async (int id, AlagamenosDbContext db) =>
        {
            var rua = await db.Ruas
                .Include(r => r.Bairro)
                .ThenInclude(b => b.Cidade)
                .ThenInclude(c => c.Estado)
                .FirstOrDefaultAsync(r => r.Id == id);
            return rua is not null ? Results.Ok(rua) : Results.NotFound();
        })
        .WithSummary("Busca uma rua pelo ID")
        .WithDescription("Retorna os dados de uma rua específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/inserir", async ([FromBody] RuaDto ruaDto, [FromServices] AlagamenosDbContext db) =>
            {
                var rua = new Rua
                {
                    NomeRua = ruaDto.NomeRua,
                    BairroId = ruaDto.BairroId,
                    Observacao = ruaDto.Observacao
                };
                
                db.Ruas.Add(rua);
                await db.SaveChangesAsync();
                return Results.Created($"/Ruas/{rua.Id}", rua);
            })
            .WithSummary("Insere uma nova rua")
            .WithDescription("Adiciona uma nova rua ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/atualizar/{id}", async (int id, [FromBody] RuaDto ruaDto, [FromServices] AlagamenosDbContext db) =>
        {
            var existing = await db.Ruas.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.NomeRua = ruaDto.NomeRua;
            await db.SaveChangesAsync();

            return Results.Ok($"Rua com ID {id} atualizado com sucesso.");
        })
        .WithSummary("Atualiza uma rua existente")
        .WithDescription("Atualiza os dados de uma rua já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, AlagamenosDbContext db) =>
            {
                var rua = await db.Ruas.FindAsync(id);
                if (rua is null) return Results.NotFound();

                db.Ruas.Remove(rua);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Rua com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove uma rua")
        .WithDescription("Remove uma rua do banco de dados com base no ID informado. " +
                         "Caso a rua não seja encontrado, retorna 404 Not Found.");
    }
}