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
using GestorSalud.Controllers;
using GestorSalud.Models;


namespace GestorSalud.Views
{
    /// <summary>
    /// Lógica de interacción para InformeNutricionalView.xaml
    /// </summary>
    public partial class InformeNutricionalView : Window
    {
        public InformeNutricionalView(int usuarioId)
        {
            InitializeComponent();
            CargarInforme(usuarioId);
        }

        private void CargarInforme(int usuarioId)
        {
            try
            {
                var controller = new InformeNutricionalController();
                var informe = controller.GenerarInforme(usuarioId);

                txtCalorias.Text = $"{informe.TotalCaloriasHoy} / {informe.MetaCalorica} kcal";
                txtEstadoCalorias.Text = informe.EstadoCalorias;
                txtIMC.Text = informe.UltimoIMC?.ToString("F1") ?? "--";
                txtClasificacion.Text = informe.ClasificacionIMC;
                txtRecomendacion.Text = informe.Recomendacion;
            }
            catch (System.Exception ex)
            {
                txtRecomendacion.Text = $"Error al cargar: {ex.Message}";
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
