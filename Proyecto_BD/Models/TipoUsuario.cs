using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class TipoUsuario
    {
        [Key]
        public int ID_Tipo_Usuario { get; set; }

        [Display(Name = "Tipo de Usuario")]
        [Required]
        public required string Tipo_Usuario { get; set; }

        // LLaves
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
