using Microsoft.EntityFrameworkCore;

namespace Proyecto_BD.Models
{
    public class ContextoBaseDatos:DbContext
    {
        //Constructor Inyeccion de Dependencias

        public ContextoBaseDatos(DbContextOptions<ContextoBaseDatos> opt)
            : base(opt) { }

        //Entidades

        public DbSet<TipoUsuario> TiposUsuario { get; set; } 
    }
}
