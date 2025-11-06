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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestorSalud.Views
{
    public partial class DashboardView : Window
    {
        private UsuariosModel usuario;

        public DashboardView(UsuariosModel usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            CargarDatosUsuario();
        }

        private void CargarDatosUsuario()
        {
            txtUsuario.Text = $"Hola, {usuario.Nombre}";
        }

        // CRUD 1: Perfil Salud
        private void AbrirPerfilSalud_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("👤 Perfil de Salud - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // CRUD 2: Registro Peso
        private void AbrirRegistroPeso_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("⚖️ Registro de Peso - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // CRUD 3: Registro Comidas
        private void AbrirRegistroComidas_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🍎 Registro de Comidas - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // No-CRUD: Calculadora IMC
        private void AbrirCalculadoraIMC_Click(object sender, RoutedEventArgs e)
        {
            CalculadoraIMCView imcView = new CalculadoraIMCView();
            imcView.ShowDialog();
        }

        // CRUD 4: Hidratación
        private void AbrirHidratacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("💧 Hidratación - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // CRUD 5: Actividad Física
        private void AbrirActividadFisica_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🏃 Actividad Física - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}
