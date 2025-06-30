using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Consultorio
    {
        [Key]
        [Display(Name = "Consultorio")]
        public int ID_Consultorio { get; set; }

        [Required]
        [Display(Name = "Piso")]
        public string Piso { get; set; }

        [Required]
        [Display(Name = "Numero de Consultorio")]
        public string Numero_Consultorio { get; set; }

        [Display(Name = "Disponibilidad")]
        public bool Disponible { get; set; }

        //Llave
        ICollection<Medico> Medico { get; set; }
    }
}
