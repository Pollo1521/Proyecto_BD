using System.ComponentModel.DataAnnotations;

namespace Proyecto_BD.Models
{
    public class TipoSangre
    {
        [Key]
        public int ID_Tipo_Sangre { get; set; }

        [Display(Name = "Tipo de Sangre")]
        [Required]
        public required string Tipo_Sangre { get; set; }

        // LLaves
       public ICollection<Paciente> Paciente { get; set; }
    }
}
