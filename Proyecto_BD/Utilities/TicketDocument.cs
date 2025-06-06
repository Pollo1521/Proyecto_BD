using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Proyecto_BD.Models;

namespace Proyecto_BD.Utilities
{
    public class TicketDocument : IDocument
    {
        private readonly List<Ticket> _tickets;
        private readonly Ventas _venta;

        public TicketDocument(List<Ticket> tickets, Ventas venta)
        {
            _tickets = tickets;
            _venta = venta;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A6); // Ticket size
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("📋 TICKET DE VENTA")
                    .SemiBold().FontSize(16).AlignCenter();

                page.Content()
                    .Column(col =>
                    {
                        foreach (var ticket in _tickets)
                        {
                            string nombre = ticket.Tipo_item
                                ? ticket.Medicina?.Nombre_Medicina ?? "Medicamento desconocido"
                                : ticket.Servicio?.Descripcion ?? "Servicio desconocido";

                            col.Item().Text($"- {nombre} x{ticket.Cantidad} = ${ticket.Subtotal:0.00}");
                        }

                        col.Item().PaddingTop(10).Text($"Total: ${_tickets.Sum(t => t.Subtotal):0.00}").Bold();
                    });

                page.Footer().AlignCenter().Text("¡Gracias por su compra!").FontSize(10);
            });
        }
    }
}
