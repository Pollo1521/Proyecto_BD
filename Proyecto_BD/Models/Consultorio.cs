using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Consultorio
    {
        [Key]
        public int ID_Consultorio { get; set; }

        [Required]
        public string Piso { get; set; }

        [Required]
        [Display(Name = "Numero de Consultorio")]
        public string Numero_Consultorio { get; set; }

        //Llave
        ICollection<Medico> Medico { get; set; }
    }
}
