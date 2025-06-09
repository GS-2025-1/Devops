using Alagamenos.Dto;
using Alagamenos.Model;
using Microsoft.EntityFrameworkCore;

namespace Alagamenos.DbConfig;

public class AlagamenosDbContext : DbContext
{
    public AlagamenosDbContext(DbContextOptions<AlagamenosDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   

        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UsuarioAlerta>()
            .HasKey(ua => new { ua.UsuarioId, ua.AlertaId });
        
        modelBuilder.Entity<UsuarioAlerta>()
            .HasOne(ua => ua.Alerta)
            .WithMany() 
            .HasForeignKey(ua => ua.AlertaId);

        modelBuilder.Entity<UsuarioAlerta>()
            .HasOne(ua => ua.Usuario)
            .WithMany() 
            .HasForeignKey(ua => ua.UsuarioId);
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<Rua> Ruas { get; set; }
    public DbSet<Bairro> Bairros { get; set; }
    public DbSet<Cidade> Cidades { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<UsuarioAlerta> UsuarioAlertas { get; set; }
}
