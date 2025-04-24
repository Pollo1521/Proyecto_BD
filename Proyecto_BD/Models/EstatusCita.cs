using System.ComponentModel.DataAnnotations;

namespace Proyecto_BD.Models
{
    public class EstatusCita
    {
        [Key]
        public int ID_Estatus_Cita { get; set; }

        [Display(Name = "Estatus")]
        [Required]
        public required string Estatus_Cita { get; set; }

        // LLaves
        public ICollection<Cita> Cita { get; set; }
    }
}
