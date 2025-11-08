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
    /// <summary>
    /// Lógica de interacción para CalculadoraIMCView.xaml
    /// </summary>
    public partial class CalculadoraIMCView : Window
    {
        public CalculadoraIMCView()
        {
            InitializeComponent();
        }

        private void Calcular_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtPeso.Text, out double peso) &&
                double.TryParse(txtAltura.Text, out double altura))
            {
                if (peso > 0 && altura > 0)
                {
                    CalcularIMC(peso, altura);
                }
                else
                {
                    MessageBox.Show("Por favor ingresa valores válidos mayores a 0.",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor ingresa valores numéricos válidos.",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcularIMC(double peso, double altura)
        {
            double imc = peso / (altura * altura);
            string clasificacion = "";
            string recomendacion = "";
            Color colorBarra = Colors.Gray;

            // Clasificación IMC
            if (imc < 18.5)
            {
                clasificacion = "BAJO PESO";
                recomendacion = "💡 Recomendación: Consulta con un nutricionista para ganar peso saludablemente.";
                colorBarra = Color.FromRgb(241, 196, 15);
            }
            else if (imc < 25)
            {
                clasificacion = "PESO NORMAL";
                recomendacion = "✅ ¡Excelente! Mantén tus hábitos saludables.";
                colorBarra = Color.FromRgb(46, 204, 113);
            }
            else if (imc < 30)
            {
                clasificacion = "SOBREPESO";
                recomendacion = "💪 Recomendación: Más actividad física y balance en tu alimentación.";
                colorBarra = Color.FromRgb(230, 126, 34);
            }
            else
            {
                clasificacion = "OBESIDAD";
                recomendacion = "🏥 Recomendación: Consulta con un profesional de la salud.";
                colorBarra = Color.FromRgb(231, 76, 60);
            }

            txtResultado.Text = $"IMC: {imc:F2}";
            txtClasificacion.Text = clasificacion;
            txtRecomendacion.Text = recomendacion;

            double porcentaje = Math.Min((imc - 15) / (40 - 15) * 100, 100);
            barraProgreso.Width = porcentaje * 2.5;
            barraProgreso.Background = new SolidColorBrush(colorBarra);

            borderResultado.Visibility = Visibility.Visible;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
