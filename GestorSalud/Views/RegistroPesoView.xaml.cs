using System;
using System.Windows;
using GestorSalud.Controllers;
using GestorSalud.Models;
using GestorSalud.Services;

namespace GestorSalud.Views
{
    public partial class RegistroPesoView : Window
    {
        private int usuarioId;
        private RegistroPesoController registroController = new RegistroPesoController();

        public RegistroPesoView(int usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
            dpFechaRegistro.SelectedDate = DateTime.Today;
        }

        private void CalcularIMC_Click(object sender, RoutedEventArgs e)
        {
            CalcularYMostrarIMC();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (CalcularYMostrarIMC())
            {
                GuardarRegistro();
            }
        }

        private bool CalcularYMostrarIMC()
        {
            try
            {
                // Validar campos vacíos
                if (string.IsNullOrWhiteSpace(txtPeso.Text) || string.IsNullOrWhiteSpace(txtAltura.Text))
                {
                    MessageBox.Show("Por favor ingresa tanto el peso como la altura.", "Datos incompletos",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Convertir valores
                if (double.TryParse(txtPeso.Text.Replace(".", ","), out double peso) &&
                    double.TryParse(txtAltura.Text.Replace(".", ","), out double altura))
                {
                    if (peso <= 0 || altura <= 0)
                    {
                        MessageBox.Show("El peso y la altura deben ser valores positivos.", "Valores inválidos",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    if (altura > 3) // Validar altura razonable
                    {
                        MessageBox.Show("Por favor ingresa la altura en metros (ej: 1.75 no 175).", "Altura inválida",
                                      MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }

                    // Calcular IMC usando el controller
                    double imc = registroController.CalcularIMC(peso, altura);
                    string clasificacion = registroController.ClasificarIMC(imc);

                    // Mostrar resultados
                    txtIMCResultado.Text = $"IMC Calculado: {imc:F2}";
                    txtClasificacion.Text = clasificacion;
                    borderIMC.Visibility = Visibility.Visible;

                    return true;
                }
                else
                {
                    MessageBox.Show("Por favor ingresa valores numéricos válidos para peso y altura.", "Datos inválidos",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular IMC: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void GuardarRegistro()
        {
            try
            {
                if (!double.TryParse(txtPeso.Text.Replace(".", ","), out double peso) || peso <= 0)
                {
                    MessageBox.Show("Ingresa un peso válido.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtAltura.Text.Replace(".", ","), out double altura) || altura <= 0)
                {
                    MessageBox.Show("Ingresa una altura válida.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (dpFechaRegistro.SelectedDate == null)
                {
                    MessageBox.Show("Selecciona una fecha válida.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Usar el controller para guardar
                bool guardado = registroController.GuardarRegistroPeso(
                    usuarioId,
                    peso,
                    altura,
                    dpFechaRegistro.SelectedDate.Value,
                    txtNotas.Text ?? ""
                );

                if (guardado)
                {
                    MessageBox.Show($"✅ Peso registrado correctamente!\n\n" +
                                  $"Peso: {peso} kg\n" +
                                  $"Altura: {altura} m\n" +
                                  $"IMC: {registroController.CalcularIMC(peso, altura):F2}",
                                  "Registro Exitoso",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);

                    // Cerrar la ventana retornando TRUE (indicando que se guardó exitosamente)
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Error al guardar el registro en la base de datos.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}