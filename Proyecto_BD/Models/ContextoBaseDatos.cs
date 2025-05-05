using Microsoft.EntityFrameworkCore;
using Proyecto_BD.Controllers;

namespace Proyecto_BD.Models
{
    public class ContextoBaseDatos:DbContext
    {
        //Constructor Inyeccion de Dependencias

        public ContextoBaseDatos(DbContextOptions<ContextoBaseDatos> opt)
            : base(opt) { }

        //Entidades

        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<Cita> Cita { get; set; }
        public DbSet<Consultorio> Consultorio { get; set; }
        public DbSet<Especialidad> Especialidad { get; set; }
        public DbSet<EstatusCita> EstatusCita { get; set; }
        public DbSet<Jornada> Jornada { get; set; }
        public DbSet<Medicina> Medicina { get; set; }
        public DbSet<Medico> Medico { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Pago> Pago { get; set; }
        public DbSet<Recepcionista> Recepcionista { get; set; }
        public DbSet<Receta> Receta { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<TipoSangre> TipoSangre { get; set; }
        public DbSet<Tratamiento> Tratamiento { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Ventas> Venta { get; set; }
        public DbSet<Ticket> Ticket { get; set; }

        //Holi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach(var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
