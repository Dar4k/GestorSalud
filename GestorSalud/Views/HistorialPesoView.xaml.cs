using GestorSalud.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace GestorSalud.Views
{
    public partial class HistorialPesoView : Window
    {
        private int _userId;
        private List<RegistroPesoModel> _registrosPeso;

        public HistorialPesoView(int usuarioId)
        {
            InitializeComponent();
            _userId = usuarioId;
            CargarTodoElHistorial();
        }

        private void CargarTodoElHistorial()
        {
            try
            {
                _registrosPeso = ObtenerRegistrosCompletos(_userId);

                if (_registrosPeso != null && _registrosPeso.Count > 0)
                {
                    dgHistorialPeso.ItemsSource = _registrosPeso.OrderByDescending(r => r.FechaRegistro);
                    CalcularTodasLasEstadisticas();
                    txtResumen.Text = $"Se encontraron {_registrosPeso.Count} registros de peso";
                }
                else
                {
                    txtResumen.Text = "No hay registros de peso disponibles";
                    LimpiarTodasLasEstadisticas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar historial: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<RegistroPesoModel> ObtenerRegistrosCompletos(int usuarioId)
        {
            try
            {
                var registros = new List<RegistroPesoModel>();
                string query = "SELECT * FROM registros_peso WHERE usuario_id = @usuario_id ORDER BY fecha_registro DESC";
                string conexion = "server=localhost; user=root; database=salud; password= ; port=3306; ";

                using (var conn = new MySqlConnection(conexion))
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario_id", usuarioId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            registros.Add(new RegistroPesoModel
                            {
                                Id = reader.GetInt32("id"),
                                UsuarioId = reader.GetInt32("usuario_id"),
                                FechaRegistro = reader.GetDateTime("fecha_registro"),
                                Peso = reader.GetDouble("peso"),
                                Altura = reader.GetDouble("altura"),
                                IMCCalculado = reader.GetDouble("imc_calculado"),
                                ClasificacionIMC = reader.GetString("clasificacion_imc"),
                                Notas = reader.IsDBNull(reader.GetOrdinal("notas")) ? "" : reader.GetString("notas")
                            });
                        }
                    }
                }
                return registros;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener registros: {ex.Message}", "Error BD",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<RegistroPesoModel>();
            }
        }

        private void CalcularTodasLasEstadisticas()
        {
            if (_registrosPeso == null || _registrosPeso.Count == 0) return;

            var ordenado = _registrosPeso.OrderBy(r => r.FechaRegistro).ToList();
            var pesoInicial = ordenado.First();
            var pesoActual = ordenado.Last();
            var diferencia = pesoActual.Peso - pesoInicial.Peso;

            txtPesoInicialHistorial.Text = $"{pesoInicial.Peso:F1} kg";
            txtPesoActualHistorial.Text = $"{pesoActual.Peso:F1} kg";
            txtDiferenciaHistorial.Text = $"{(diferencia >= 0 ? "+" : "")}{diferencia:F1} kg";
            txtTotalRegistrosHistorial.Text = _registrosPeso.Count.ToString();

            
            if (diferencia < 0)
                txtDiferenciaHistorial.Foreground = Brushes.Green;
            else if (diferencia > 0)
                txtDiferenciaHistorial.Foreground = Brushes.Red;
            else
                txtDiferenciaHistorial.Foreground = Brushes.Gray;
        }

        private void LimpiarTodasLasEstadisticas()
        {
            txtPesoInicialHistorial.Text = "--";
            txtPesoActualHistorial.Text = "--";
            txtDiferenciaHistorial.Text = "--";
            txtTotalRegistrosHistorial.Text = "--";
        }

        private void ActualizarHistorial_Click(object sender, RoutedEventArgs e)
        {
            CargarTodoElHistorial();
            MessageBox.Show("Historial actualizado", "Éxito",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportarPDFHistorial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_registrosPeso == null || _registrosPeso.Count == 0)
                {
                    MessageBox.Show("No hay datos para exportar", "Advertencia",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Historial_Peso_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    GenerarPDFCompleto(saveDialog.FileName);
                    MessageBox.Show($"✅ PDF exportado exitosamente:\n{saveDialog.FileName}",
                                  "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerarPDFCompleto(string filePath)
        {
            
            Document document = new Document(PageSize.A4.Rotate());

            try
            {
                
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                
                document.Open();

                
                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph title = new Paragraph("HISTORIAL DE PESO - MI GESTOR DE SALUD", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20;
                document.Add(title);

                
                Font dateFont = FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.GRAY);
                Paragraph date = new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}", dateFont);
                date.Alignment = Element.ALIGN_RIGHT;
                date.SpacingAfter = 15;
                document.Add(date);

                
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1.5f, 1f, 1f, 1f, 2f, 2f });

                
                Font headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);

                
                PdfPCell fechaHeader = new PdfPCell(new Phrase("FECHA", headerFont));
                fechaHeader.BackgroundColor = new BaseColor(41, 128, 185);
                fechaHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(fechaHeader);

                
                PdfPCell pesoHeader = new PdfPCell(new Phrase("PESO (kg)", headerFont));
                pesoHeader.BackgroundColor = new BaseColor(41, 128, 185);
                pesoHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(pesoHeader);

                
                PdfPCell alturaHeader = new PdfPCell(new Phrase("ALTURA (m)", headerFont));
                alturaHeader.BackgroundColor = new BaseColor(41, 128, 185);
                alturaHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(alturaHeader);

                
                PdfPCell imcHeader = new PdfPCell(new Phrase("IMC", headerFont));
                imcHeader.BackgroundColor = new BaseColor(41, 128, 185);
                imcHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(imcHeader);

                
                PdfPCell clasificacionHeader = new PdfPCell(new Phrase("CLASIFICACIÓN", headerFont));
                clasificacionHeader.BackgroundColor = new BaseColor(41, 128, 185);
                clasificacionHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(clasificacionHeader);

                
                PdfPCell notasHeader = new PdfPCell(new Phrase("NOTAS", headerFont));
                notasHeader.BackgroundColor = new BaseColor(41, 128, 185);
                notasHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(notasHeader);

                
                Font cellFont = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK);
                bool alternate = false;

                foreach (var registro in _registrosPeso.OrderByDescending(r => r.FechaRegistro))
                {
                    BaseColor rowColor = alternate ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                    alternate = !alternate;

                    
                    PdfPCell fechaCell = new PdfPCell(new Phrase(registro.FechaRegistro.ToString("dd/MM/yyyy"), cellFont));
                    fechaCell.BackgroundColor = rowColor;
                    table.AddCell(fechaCell);

                    
                    string pesoFormateado = string.Format("{0:F1}", registro.Peso);
                    PdfPCell pesoCell = new PdfPCell(new Phrase(pesoFormateado, cellFont));
                    pesoCell.BackgroundColor = rowColor;
                    table.AddCell(pesoCell);

                    
                    string alturaFormateada = string.Format("{0:F2}", registro.Altura);
                    PdfPCell alturaCell = new PdfPCell(new Phrase(alturaFormateada, cellFont));
                    alturaCell.BackgroundColor = rowColor;
                    table.AddCell(alturaCell);

                    
                    string imcFormateado = string.Format("{0:F1}", registro.IMCCalculado);
                    PdfPCell imcCell = new PdfPCell(new Phrase(imcFormateado, cellFont));
                    imcCell.BackgroundColor = rowColor;
                    table.AddCell(imcCell);

                    
                    string clasificacionLimpia = registro.ClasificacionIMC
                        .Replace("🔶", "")
                        .Replace("✅", "")
                        .Replace("🔴", "")
                        .Replace("\n", " ")
                        .Trim();
                    PdfPCell clasificacionCell = new PdfPCell(new Phrase(clasificacionLimpia, cellFont));
                    clasificacionCell.BackgroundColor = rowColor;
                    table.AddCell(clasificacionCell);

                    
                    PdfPCell notasCell = new PdfPCell(new Phrase(registro.Notas, cellFont));
                    notasCell.BackgroundColor = rowColor;
                    table.AddCell(notasCell);
                }

                document.Add(table);

                
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                Font statsFont = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.DARK_GRAY);
                Paragraph statsTitle = new Paragraph("ESTADÍSTICAS DEL HISTORIAL", statsFont);
                statsTitle.SpacingAfter = 10;
                document.Add(statsTitle);

                if (_registrosPeso.Count >= 2)
                {
                    var ordenado = _registrosPeso.OrderBy(r => r.FechaRegistro).ToList();
                    var pesoInicial = ordenado.First();
                    var pesoActual = ordenado.Last();
                    var diferencia = pesoActual.Peso - pesoInicial.Peso;

                    
                    var mejorIMC = _registrosPeso.OrderBy(r => Math.Abs((double)(r.IMCCalculado - 22.0))).First();

                    Font normalFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);

                    document.Add(new Paragraph($"• Peso inicial: {string.Format("{0:F1}", pesoInicial.Peso)} kg", normalFont));
                    document.Add(new Paragraph($"• Peso actual: {string.Format("{0:F1}", pesoActual.Peso)} kg", normalFont));
                    document.Add(new Paragraph($"• Diferencia: {(diferencia >= 0 ? "+" : "")}{string.Format("{0:F1}", diferencia)} kg", normalFont));
                    document.Add(new Paragraph($"• Mejor IMC registrado: {string.Format("{0:F1}", mejorIMC.IMCCalculado)}", normalFont));
                    document.Add(new Paragraph($"• Total de registros: {_registrosPeso.Count}", normalFont));
                    document.Add(new Paragraph($"• Período: {ordenado.First().FechaRegistro:dd/MM/yyyy} - {ordenado.Last().FechaRegistro:dd/MM/yyyy}", normalFont));
                }

                
                document.Add(new Paragraph(" "));
                Font footerFont = FontFactory.GetFont("Arial", 8, Font.ITALIC, BaseColor.GRAY);
                Paragraph footer = new Paragraph("Generado por Mi Gestor de Nutrición y Bienestar", footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                document.Add(footer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar PDF: {ex.Message}");
            }
            finally
            {
                document.Close();
            }
        }

        private void CerrarHistorial_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}