using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Receta
    {
        [Key]
        public int ID_Receta { get; set; }

        public int ID_Cita { get; set; }
        [ForeignKey("ID_Cita")]
        [Required]
        public Cita Cita { get; set; }

        [Required]
        public string Diagnostico { get; set; }
    }
}
