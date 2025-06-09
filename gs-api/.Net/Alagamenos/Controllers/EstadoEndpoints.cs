using Alagamenos.DbConfig;
using Alagamenos.Dto;
using Alagamenos.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alagamenos.Controllers;

public class EstadoEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/estados").WithTags("Estado");
        
        //Get all
        group.MapGet("/", async (AlagamenosDbContext db) =>
            await db.Estados.ToListAsync())
            .WithSummary("Retorna todos os estados")
            .WithDescription("Retorna todos os estados cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um estado, ele ainda vai retornar uma lista");
        
        //Get all paginado
        group.MapGet("/paginadas", async (int? page, AlagamenosDbContext db) =>
            {
                var pageSize = 5;
                var currentPage = page ?? 1;
                var skipItems = (currentPage - 1) * pageSize;

                var totalItems = await db.Estados.CountAsync();
                var data = await db.Estados
                    .Skip(skipItems)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new SearchDto<Estado>(null, currentPage, totalItems, data));
            })
            .WithSummary("Retorna endereços paginadas")
            .WithDescription("Retorna todos os registros de estados paginados. " +
                             "Cada página retorna um número fixo de registros (5 por página neste exemplo).");

        //GetById
        group.MapGet("/{id}", async (int id, AlagamenosDbContext db) =>
        {
            var estado = await db.Estados.FindAsync(id);
            return estado is not null ? Results.Ok(estado) : Results.NotFound();
        })
        .WithSummary("Busca um estado pelo ID")
        .WithDescription("Retorna os dados de um estado específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/inserir", async ([FromBody] EstadoDto estadoDto, [FromServices] AlagamenosDbContext db) =>
            {
                var estado = new Estado
                {
                    NomeEstado = estadoDto.NomeEstado
                };
                
                db.Estados.Add(estado);
                await db.SaveChangesAsync();
                return Results.Created($"/Estados/{estado.Id}", estado);
            })
            .WithSummary("Insere um novo estado")
            .WithDescription("Adiciona um novo estado ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/atualizar/{id}", async (int id, [FromBody] EstadoDto estadoDto, [FromServices] AlagamenosDbContext db) =>
        {
            var existing = await db.Estados.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.NomeEstado = estadoDto.NomeEstado;
            await db.SaveChangesAsync();

            return Results.Ok($"Estado com ID {id} atualizado com sucesso.");
        })
        .WithSummary("Atualiza um estado existente")
        .WithDescription("Atualiza os dados de um estado já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, AlagamenosDbContext db) =>
            {
                var estado = await db.Estados.FindAsync(id);
                if (estado is null) return Results.NotFound();

                db.Estados.Remove(estado);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Estado com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove um estado")
        .WithDescription("Remove um estado do banco de dados com base no ID informado. " +
                         "Caso o estado não seja encontrado, retorna 404 Not Found.");
    }
}