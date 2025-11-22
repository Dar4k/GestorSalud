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
    /// Lógica de interacción para RegistroComidaView.xaml
    /// </summary>
    public partial class RegistroComidaView : Window
    {
        private readonly int _usuarioId;
        private readonly RegistroComidaController _controller;
        private RegistroComidaModel _comidaEnEdicion = null;
        private RegistroComidaModel _comidaSeleccionada = null;

        
        public RegistroComidaView(int usuarioId)
        {
            InitializeComponent();
            _usuarioId = usuarioId;
            _controller = new RegistroComidaController();
            CargarComidasHoy();
        }

        private void CargarComidasHoy()
        {
            try
            {
                var comidas = _controller.ObtenerComidasHoy(_usuarioId);
                lstComidasHoy.ItemsSource = comidas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar comidas: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validación básica
                if (string.IsNullOrWhiteSpace(txtAlimentos.Text))
                {
                    MessageBox.Show("Por favor, ingresa qué comiste.", "Advertencia",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtCalorias.Text, out int calorias) || calorias < 0)
                {
                    MessageBox.Show("Ingresa calorías válidas (número entero ≥ 0).", "Advertencia",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var comida = new RegistroComidaModel
                {
                    UsuarioId = _usuarioId,
                    TipoComida = cmbTipoComida.SelectedItem?.ToString() ?? "Desayuno",
                    Alimentos = txtAlimentos.Text.Trim(),
                    Calorias = calorias,
                    Notas = txtNotas.Text.Trim()
                };

                bool exito;

                if (_comidaEnEdicion != null)
                {
                    // Modo edición
                    comida.Id = _comidaEnEdicion.Id;
                    comida.FechaComida = _comidaEnEdicion.FechaComida;
                    exito = _controller.ActualizarComida(comida);
                }
                else
                {
                    // Modo creación
                    exito = _controller.GuardarComida(comida);
                }

                if (exito)
                {
                    MessageBox.Show(_comidaEnEdicion != null
                        ? "✅ Comida actualizada correctamente."
                        : "✅ Comida registrada correctamente.",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    LimpiarFormulario();
                    CargarComidasHoy();
                }
                else
                {
                    MessageBox.Show("❌ Error al guardar. Inténtalo de nuevo.", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_comidaSeleccionada == null) return;

            // Cargar en formulario
            for (int i = 0; i < cmbTipoComida.Items.Count; i++)
            {
                if (cmbTipoComida.Items[i] is ComboBoxItem item &&
                    item.Content.ToString() == _comidaSeleccionada.TipoComida)
                {
                    cmbTipoComida.SelectedIndex = i;
                    break;
                }
            }

            txtAlimentos.Text = _comidaSeleccionada.Alimentos;
            txtCalorias.Text = _comidaSeleccionada.Calorias?.ToString() ?? "0";
            txtNotas.Text = _comidaSeleccionada.Notas;

            _comidaEnEdicion = _comidaSeleccionada;
            btnGuardar.Content = "✅ Actualizar";
        }



        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (_comidaEnEdicion == null) return;

            var resultado = MessageBox.Show(
                $"¿Seguro que deseas eliminar el registro de {_comidaEnEdicion.TipoComida}?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                if (_controller.EliminarComida(_comidaEnEdicion.Id, _usuarioId))
                {
                    MessageBox.Show("🗑️ Registro eliminado.", "Éxito",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarFormulario();
                    CargarComidasHoy();
                }
                else
                {
                    MessageBox.Show("❌ No se pudo eliminar el registro.", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LstComidasHoy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstComidasHoy.SelectedItem is RegistroComidaModel comida)
            {
                _comidaSeleccionada = comida;
                btnEditar.IsEnabled = true;
                btnEliminar.IsEnabled = true;
            }
            else
            {
                _comidaSeleccionada = null;
                btnEditar.IsEnabled = false;
                btnEliminar.IsEnabled = false;
            }
        }

        private void LimpiarFormulario()
        {
            _comidaEnEdicion = null;
            _comidaSeleccionada = null;
            cmbTipoComida.SelectedIndex = 0;
            txtAlimentos.Clear();
            txtCalorias.Text = "0";
            txtNotas.Clear();
            btnGuardar.Content = "💾 Guardar";
            lstComidasHoy.SelectedItem = null;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


