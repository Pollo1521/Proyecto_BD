using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Recepcionista
    {
        [Key]
        public int ID_Recepcionista { get; set; }

        [Required]
        public int ID_Usuario { get; set; }
        [ForeignKey("ID_Usuario")]
        [Required]
        public Usuario Usuario { get; set; }

        [Required]
        public int ID_Jornada { get; set; }
        [ForeignKey("ID_Jornada")]
        [Required]
        public Jornada Jornada { get; set; }

        //Llave

        ICollection<Ventas> Ventas { get; set; }
    }
}
