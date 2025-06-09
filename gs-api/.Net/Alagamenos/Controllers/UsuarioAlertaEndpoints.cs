using Alagamenos.DbConfig;
using Alagamenos.Model;
using Alagamenos.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alagamenos.Controllers;

public class UsuarioAlertaEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/usuario-alertas").WithTags("UsuarioAlerta");

        // GET all
        group.MapGet("/", async (AlagamenosDbContext db) =>
            await db.UsuarioAlertas
                    .Include(ua => ua.Usuario)
                    .Include(ua => ua.Alerta)
                    .ToListAsync())
            .WithSummary("Retorna todos os vínculos entre Usuários e Alertas")
            .WithDescription("Lista todos os registros da relação N:N entre Usuário e Alerta");

        // GET all paginado
        group.MapGet("/paginadas", async (int? page, AlagamenosDbContext db) =>
            {
                var pageSize = 5;
                var currentPage = page ?? 1;
                var skipItems = (currentPage - 1) * pageSize;

                var totalItems = await db.UsuarioAlertas.CountAsync();
                var data = await db.UsuarioAlertas
                    .Include(ua => ua.Usuario)
                    .Include(ua => ua.Alerta)
                    .Skip(skipItems)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new SearchDto<UsuarioAlerta>(null, currentPage, totalItems, data));
            })
            .WithSummary("Retorna alertas de usuários paginados")
            .WithDescription("Retorna todos os registros de endereços paginados. " +
                             "Cada página retorna um número fixo de aletas de usuários (10 por página neste exemplo).");
        
        // GET com chave composta
        group.MapGet("/usuario/{usuarioId:int}/alerta/{alertaId:int}", async (int usuarioId, int alertaId, AlagamenosDbContext db) =>
        {
            var vinculo = await db.UsuarioAlertas
                .Include(ua => ua.Usuario)
                .Include(ua => ua.Alerta)
                .FirstOrDefaultAsync(ua => ua.UsuarioId == usuarioId && ua.AlertaId == alertaId);

            return vinculo is not null ? Results.Ok(vinculo) : Results.NotFound();
        })
        .WithSummary("Busca vínculo específico entre usuário e alerta")
        .WithDescription("Retorna um vínculo da tabela de junção com base nos IDs do usuário e do alerta");

        // POST - criar vínculo
        group.MapPost("/inserir", async ([FromBody] UsuarioAlertaDto usuarioAlertaDto, AlagamenosDbContext db) =>
            {
                var usuarioAlerta = new UsuarioAlerta
                {
                    UsuarioId = usuarioAlertaDto.UsuarioId,
                    AlertaId = usuarioAlertaDto.AlertaId
                };
            
            var existe = await db.UsuarioAlertas.AnyAsync(ua => 
                ua.UsuarioId == usuarioAlerta.UsuarioId && ua.AlertaId == usuarioAlerta.AlertaId);

            if (existe)
                return Results.Conflict("Este vínculo já existe.");

            db.UsuarioAlertas.Add(usuarioAlerta);
            await db.SaveChangesAsync();
            return Results.Created($"/usuario-alertas/{usuarioAlerta.UsuarioId}/{usuarioAlerta.AlertaId}", usuarioAlerta);
        })
        .WithSummary("Cria um novo vínculo entre usuário e alerta")
        .WithDescription("Adiciona um novo registro na tabela de junção UsuarioAlerta");

        // DELETE - remover vínculo
        group.MapDelete("/deletar/{usuarioId:int}/{alertaId:int}", async (int usuarioId, int alertaId, AlagamenosDbContext db) =>
        {
            var vinculo = await db.UsuarioAlertas
                .FirstOrDefaultAsync(ua => ua.UsuarioId == usuarioId && ua.AlertaId == alertaId);

            if (vinculo is null) return Results.NotFound();

            db.UsuarioAlertas.Remove(vinculo);
            await db.SaveChangesAsync();

            return Results.Ok($"Vínculo entre usuário {usuarioId} e alerta {alertaId} removido com sucesso.");
        })
        .WithSummary("Remove um vínculo específico entre usuário e alerta")
        .WithDescription("Deleta o registro da tabela UsuarioAlerta com base na chave composta.");
    }
}
