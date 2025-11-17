using GestorSalud.Controllers;
using GestorSalud.Models;
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
    public partial class HidratacionView : Window
    {
        HidratacionController hidratacionController = new HidratacionController();
        private int usuarioId;
        private HidratacionModel registroSeleccionado = null;
        public HidratacionView(int usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            hidratacionController = new HidratacionController();
            caragarRegistros();
        }

        private void caragarRegistros()
        {
            dgHidratacion.ItemsSource = hidratacionController.ObtenerHidratacionPorUsuario(usuarioId);
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtVasos.Text) || string.IsNullOrEmpty(txtTotalMl.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int vasosAgua = int.Parse(txtVasos.Text);
                int totalMl = int.Parse(txtTotalMl.Text);
                string otrosLiquidos = txtOtrosLiquidos.Text;

                if (registroSeleccionado == null)
                {
                    var nuevo = new HidratacionModel
                    {
                        UsuarioId = usuarioId,
                        FechaRegistro = DateTime.Now,
                        VasosAgua = vasosAgua,
                        OtrosLiquidos = otrosLiquidos,
                        TotalMl = vasosAgua * totalMl
                    };

                    if (hidratacionController.RegistrarHidratacion(nuevo))
                    {
                        MessageBox.Show("Registro guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    registroSeleccionado.VasosAgua = vasosAgua;
                    registroSeleccionado.OtrosLiquidos = otrosLiquidos;
                    registroSeleccionado.TotalMl = registroSeleccionado.VasosAgua * int.Parse(txtTotalMl.Text);

                    if (hidratacionController.ActualizarHidratacion(registroSeleccionado))
                    {
                        MessageBox.Show("Registro actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    registroSeleccionado = null;
                }

                LimpiarCampos();
                caragarRegistros();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LimpiarCampos()
        {
            txtVasos.Clear();
            txtTotalMl.Clear();
        }

        private void dgHidratacion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgHidratacion.SelectedItem is HidratacionModel seleccionado)
            {
                registroSeleccionado = seleccionado;

                txtVasos.Text = seleccionado.VasosAgua.ToString();
                txtOtrosLiquidos.Text = seleccionado.OtrosLiquidos;
            }
        }

        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            DashboardView dashboardView = new DashboardView(new UsuariosModel { Id = usuarioId });
            dashboardView.Show();
            this.Close();
        }
    }
}
