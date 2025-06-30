using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Especialidad
    {
        [Key]
        [Display(Name = "Especialidad")]
        public int ID_Especialidad { get; set; }

        [Required]
        [Display(Name = "Especialidad")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Precio de Cita")]
        public float PrecioCita { get; set; }

        //Llave

        ICollection<Medico> Medico { get; set; }
    }
}
