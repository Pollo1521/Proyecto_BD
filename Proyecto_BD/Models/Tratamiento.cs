using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_BD.Models
{
    public class Tratamiento
    {
        [Key]
        public int ID_Tratamiento { get; set; }

        public int ID_Receta { get; set; }
        [ForeignKey("ID_Receta")]
        [Required]
        public Receta Receta { get; set; }

        [Required]
        public string Medicamento { get; set; }
        
        [Required]
        public string Indicaciones { get; set; }
    }
}
