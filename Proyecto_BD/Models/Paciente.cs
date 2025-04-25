using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Paciente
    {
        [Key]
        public int ID_Paciente { get; set; }

        public int ID_Usuario { get; set; }
        [ForeignKey("ID_Usuario")]
        public Usuario Usuario { get; set; }

        [Display(Name = "Tipo de Sangre")]
        public int ID_Tipo_Sangre { get; set; }
        [ForeignKey("ID_Tipo_Sangre")]
        public TipoSangre Tipo_Sangres { get; set; }

        [Required]
        public float Peso {  get; set; }

        [Required]
        [Display(Name = "Alergias")]
        public string Alergia { get; set; }

        [Required]
        public float Estatura { get; set; }

        //Llaves
        public ICollection<Cita> Cita { get; set; }
    }
}
