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
                
                dpFechaNacimiento.SelectedDate = perfil.FechaNacimiento;
                cbGenero.Text = perfil.Genero;
                txtAltura.Text = perfil.Altura.ToString("0.00");
                txtObjetivoPeso.Text = perfil.ObjetivoPeso?.ToString("0.00") ?? "";
                cbNivelActividad.Text = perfil.NivelActividad;
                txtCondicionesMedicas.Text = perfil.CondicionesMedicas;
            }
            else
            {
               
                dpFechaNacimiento.SelectedDate = DateTime.Now.AddYears(-30); 
                cbGenero.SelectedIndex = 0;
                cbNivelActividad.SelectedIndex = 0;
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (dpFechaNacimiento.SelectedDate == null)
                {
                    MessageBox.Show("Por favor selecciona tu fecha de nacimiento.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
                if (string.IsNullOrWhiteSpace(txtAltura.Text) ||
                    !decimal.TryParse(txtAltura.Text, out decimal altura) ||
                    altura <= 0 || altura >= 10)
                {
                    MessageBox.Show("Por favor ingresa una altura válida entre 0.50 y 2.50 metros.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
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