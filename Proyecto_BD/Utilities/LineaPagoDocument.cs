using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using Proyecto_BD.Models;

namespace Proyecto_BD.Utilities
{
    public class LineaPagoDocument : IDocument
    {
        private readonly LineaPagoData data;

        public LineaPagoDocument(LineaPagoData data)
        {
            this.data = data;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content().Column(col =>
                {
                    col.Item().Text("Clínica XYZ").Bold().FontSize(20).AlignCenter();
                    col.Item().Text("Línea de Pago - Cita Médica").Bold().FontSize(14).AlignCenter();
                    col.Item().PaddingVertical(10).LineHorizontal(1);

                    col.Item().Text($"Paciente: {data.Paciente}");
                    col.Item().Text($"CURP: {data.CURP}");
                    col.Item().Text($"Fecha: {data.FechaCita:dd/MM/yyyy}");
                    col.Item().Text($"Hora: {data.HoraCita}");
                    col.Item().Text($"Médico: Dr. {data.Medico}");
                    col.Item().Text($"Especialidad: {data.Especialidad}");
                    col.Item().Text($"Consultorio: {data.Consultorio}");

                    col.Item().PaddingVertical(10).LineHorizontal(1);

                    col.Item().Text($"Costo de la cita: ${data.Precio:F2}").Bold();
                    col.Item().Text("Estado del pago: PENDIENTE").FontColor(Colors.Red.Medium).Bold();
                    col.Item().Text("Cuenta con 8hr para realizar el pago").FontColor(Colors.Red.Medium).Bold();
                    col.Item().Text("Cancelacion:").FontColor(Colors.Red.Medium).Bold();
                    col.Item().Text("Mas de 48 hr -> 100% reembolso:").FontColor(Colors.Red.Medium).Bold();
                    col.Item().Text("Mas de 24 hr -> 50% reembolso:").FontColor(Colors.Red.Medium).Bold();
                    col.Item().Text("Menos de 24 hr -> 0% reembolso:").FontColor(Colors.Red.Medium).Bold();

                    col.Item().PaddingTop(20).Text($"Realiza tu pago desde la pagina con el folio: {data.LinkPago}").Italic();
                });
            });
        }
    }
}
