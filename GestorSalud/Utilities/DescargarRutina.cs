using Microsoft.Win32;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GestorSalud.Utilities
{
    public class DescargarRutina
    {
        public void DescargarRutinaPDF(List<Models.ActividadFisicaModel> listaEjercicios)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "RutinaEjercicios.pdf",
                Filter = "Archivos PDF (*.pdf)|*.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Rutina de Ejercicios";
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont fontTitle = new XFont("Arial", 16, XFontStyle.Bold);
                XFont fontBody = new XFont("Arial", 12, XFontStyle.Regular);

                gfx.DrawString("Rutina de Ejercicios Recomendados", fontTitle, XBrushes.Black,
                    new XRect(0, 20, page.Width, 40), XStringFormats.TopCenter);

                int yPoint = 60;

                foreach (var ejercicio in listaEjercicios)
                {
                    gfx.DrawString($"Clasificación IMC: {ejercicio.ClasificacionImc}", fontBody, XBrushes.Black,
                        new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
                    yPoint += 20;

                    gfx.DrawString($"Ejercicio: {ejercicio.EjercicioRecomendado}", fontBody, XBrushes.Black,
                        new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
                    yPoint += 20;

                    gfx.DrawString($"Intensidad: {ejercicio.Intensidad}", fontBody, XBrushes.Black,
                        new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
                    yPoint += 20;

                    gfx.DrawString($"Duración: {ejercicio.DuracionSugerida} min", fontBody, XBrushes.Black,
                        new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
                    yPoint += 35;

                    if (yPoint > page.Height - 100)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPoint = 40;
                    }
                }
                document.Save(saveFileDialog.FileName);
            }
        }
    }
}
