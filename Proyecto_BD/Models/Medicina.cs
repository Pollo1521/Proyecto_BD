using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Proyecto_BD.Models
{
    public class Medicina
    {
        [Key]
        public int ID_Medicina { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public float Precio_Medicina { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre_Medicina { get; set; }

        //Llave

        ICollection<Ticket> Ticket { get; set; }
    }
}
