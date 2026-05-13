using FleetTrackAPI.Models;
using Microsoft.EntityFrameworkCore;



namespace FleetTrackAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options){}

        public DbSet<Usuario> Usuarios {get; set;}
        public DbSet<Cliente> Clientes {get; set;}
        public DbSet<Conductor> Conductores {get; set;}
        public DbSet<Vehiculo> Vehiculos {get; set;}
        public DbSet<Pedido> Pedidos {get; set;}
        public DbSet<Entrega> Entregas {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relaciones entre entidades
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Entrega)
                .WithOne(e => e.Pedido)
                .HasForeignKey<Entrega>(e => e.PedidoId);

            modelBuilder.Entity<Entrega>()
                .HasOne(e => e.Conductor)
                .WithMany(c => c.Entregas)
                .HasForeignKey(e => e.ConductorId);

            modelBuilder.Entity<Entrega>()
                .HasOne(e => e.Vehiculo)
                .WithMany(v => v.Entregas)
                .HasForeignKey(e => e.VehiculoId);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId);
        }
    }
}