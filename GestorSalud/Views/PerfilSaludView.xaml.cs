using System;
using System.Windows;
using GestorSalud.Controllers;
using GestorSalud.Models;

namespace GestorSalud.Views
{
    public partial class PerfilSaludView : Window
    {
        private int usuarioId;
        private PerfilSaludController perfilController = new PerfilSaludController();

        public PerfilSaludView(int usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            CargarPerfilExistente();
        }

        private void CargarPerfilExistente()
        {
            var perfil = perfilController.ObtenerPerfilSalud(usuarioId);
            if (perfil != null)
            {
                // Cargar datos existentes
                dpFechaNacimiento.SelectedDate = perfil.FechaNacimiento;
                cbGenero.Text = perfil.Genero;
                txtAltura.Text = perfil.Altura.ToString("0.00");
                txtObjetivoPeso.Text = perfil.ObjetivoPeso?.ToString("0.00") ?? "";
                cbNivelActividad.Text = perfil.NivelActividad;
                txtCondicionesMedicas.Text = perfil.CondicionesMedicas;
            }
            else
            {
                // Valores por defecto para nuevo perfil
                dpFechaNacimiento.SelectedDate = DateTime.Now.AddYears(-30); // 30 años por defecto
                cbGenero.SelectedIndex = 0;
                cbNivelActividad.SelectedIndex = 0;
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones
                if (dpFechaNacimiento.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona tu fecha de nacimiento.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validar altura (decimal(3,2) = máximo 9.99)
                if (string.IsNullOrWhiteSpace(txtAltura.Text) ||
                    !decimal.TryParse(txtAltura.Text, out decimal altura) ||
                    altura <= 0 || altura >= 10)
                {
                    MessageBox.Show("Por favor ingresa una altura válida entre 0.50 y 2.50 metros.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validar objetivo peso si se ingresó
                decimal? objetivoPeso = null;
                if (!string.IsNullOrWhiteSpace(txtObjetivoPeso.Text))
                {
                    if (!decimal.TryParse(txtObjetivoPeso.Text, out decimal objetivo) || objetivo <= 0)
                    {
                        MessageBox.Show("Por favor ingresa un objetivo de peso válido o déjalo vacío.", "Error",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    objetivoPeso = objetivo;
                }

                // Crear modelo
                var perfil = new PerfilSaludModel
                {
                    UsuarioId = usuarioId,
                    FechaNacimiento = dpFechaNacimiento.SelectedDate.Value,
                    Genero = cbGenero.Text ?? "",
                    Altura = altura,
                    ObjetivoPeso = objetivoPeso,
                    NivelActividad = cbNivelActividad.Text ?? "",
                    CondicionesMedicas = txtCondicionesMedicas.Text ?? ""
                };

                // Guardar
                bool guardado = perfilController.GuardarPerfilSalud(perfil);

                if (guardado)
                {
                    MessageBox.Show("✅ Perfil de salud guardado correctamente.", "Éxito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Error al guardar el perfil.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HistorialPeso_Click(object sender, RoutedEventArgs e)
        {
            var historialView = new HistorialPesoView(usuarioId);
            historialView.Owner = this;
            historialView.ShowDialog();
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}