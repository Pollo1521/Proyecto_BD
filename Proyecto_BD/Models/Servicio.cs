using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using Proyecto_BD.Models;

namespace Proyecto_BD.Controllers
{
    public class Servicio
    {
        [Key]
        public int ID_Servicio { get; set; }

        [Display(Name = "Servicio")]
        [Required]
        public required string Descripcion { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public float Precio_Servicio { get; set; }

        //Llaves

        ICollection<Ticket> Ticket { get; set; }
    }
}
