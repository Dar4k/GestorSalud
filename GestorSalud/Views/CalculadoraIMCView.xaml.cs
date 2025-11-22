using System;
using System.Windows;
using GestorSalud.Controllers;
using MySql.Data.MySqlClient;

namespace GestorSalud.Views
{
    public partial class CalculadoraIMCView : Window
    {
        private int usuarioId;
        private RegistroPesoController registroController = new RegistroPesoController();

        // CONSTRUCTOR ACTUALIZADO QUE RECIBE EL USUARIO_ID
        public CalculadoraIMCView(int usuarioId)
        {
            InitializeComponent();
            this.usuarioId = usuarioId;
        }

        private void Calcular_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // VERIFICAR QUE LOS CAMPOS NO ESTÉN VACÍOS
                if (string.IsNullOrEmpty(txtPeso.Text) || string.IsNullOrEmpty(txtAltura.Text))
                {
                    MessageBox.Show("Por favor ingresa peso y altura.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // INTENTAR CONVERTIR LOS VALORES
                if (double.TryParse(txtPeso.Text.Replace(".", ","), out double peso) &&
                    double.TryParse(txtAltura.Text.Replace(".", ","), out double altura))
                {
                    if (peso > 0 && altura > 0 && altura < 3)
                    {
                        double imc = Math.Round(peso / (altura * altura), 2);
                        string clasificacion = ClasificarIMC(imc);

                        // USAR EL CONTROLLER PARA GUARDAR EN BD
                        bool guardado = registroController.GuardarRegistroIMC(usuarioId, peso, altura, imc, clasificacion);

                        if (guardado)
                        {
                            MessageBox.Show($"✅ IMC guardado en tu historial: {imc:F2}\n{clasificacion}",
                                          "Resultado IMC",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show($"❌ Error al guardar en base de datos\nIMC: {imc:F2}\n{clasificacion}",
                                          "Resultado IMC",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingresa valores válidos:\n- Peso mayor a 0\n- Altura entre 0.5 y 2.5 metros",
                                      "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ingresa valores numéricos válidos.\nEjemplo: 70.5 y 1.75",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string ClasificarIMC(double imc)
        {
            if (imc < 18.5) return "BAJO PESO\nConsulta un nutricionista";
            else if (imc < 25) return "PESO NORMAL\n¡Excelente!";
            else if (imc < 30) return "SOBREPESO\nMás ejercicio y dieta balanceada";
            else return "OBESIDAD\nConsulta con un profesional";
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}