using GestorSalud.Controllers;
using GestorSalud.Models;
using GestorSalud.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestorSalud.Views
{
    public partial class EjerciciosView : Window
    {
        private int usuarioId;
        private ObtenerEjercicioController ejercicioController = new ObtenerEjercicioController();
        private DescargarRutina descargarRutina = new DescargarRutina();
        public EjerciciosView(int usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            CargarEjercicios(usuarioId);
        }

        private void CargarEjercicios(int usuarioId)
        {
            var listaEjercicios = ejercicioController.ObtenerEjerciciosPorCategoria(usuarioId);

            if (listaEjercicios != null && listaEjercicios.Count > 0)
            {
                lvEjercicios.ItemsSource = listaEjercicios;
            }
            else
            {
                MessageBox.Show("No hay ejercicios recomendados aún. Registra tu peso o IMC primero.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            DashboardView dashboardView = new DashboardView(new UsuariosModel { Id = usuarioId });
            dashboardView.Show();
            this.Close();
        }

        private void DescargarPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ListaEjercicios = lvEjercicios.ItemsSource as List<ActividadFisicaModel>;
                if (ListaEjercicios == null || ListaEjercicios.Count == 0)
                {
                    MessageBox.Show("No hay ejercicios para exportar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    descargarRutina.DescargarRutinaPDF(ListaEjercicios);
                    MessageBox.Show("PDF generado con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
