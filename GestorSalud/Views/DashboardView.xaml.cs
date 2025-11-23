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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestorSalud.Views
{
    public partial class DashboardView : Window
    {
        private UsuariosModel usuario;
        private RegistroPesoController registroController = new RegistroPesoController();

        public DashboardView(UsuariosModel usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            CargarDatosUsuario();
            CargarUltimoRegistroIMC();
        }

        private void CargarDatosUsuario()
        {
            txtUsuario.Text = $"Hola, {usuario.Nombre}";
        }


        
        private void CargarUltimoRegistroIMC()
        {
            try
            {
                var ultimoRegistro = registroController.ObtenerUltimoRegistro(usuario.Id);

                if (ultimoRegistro != null)
                {
                    
                    txtUltimoPeso.Text = $"{ultimoRegistro.Peso} kg";
                    txtIMC.Text = $"{ultimoRegistro.IMCCalculado:F1}";
                }
                else
                {
                    
                    txtUltimoPeso.Text = "-- kg";
                    txtIMC.Text = "--";
                }
            }
            catch (Exception ex)
            {
                
                txtUltimoPeso.Text = "-- kg";
                txtIMC.Text = "--";
            }
        }


        
        private void AbrirPerfilSalud_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("👤 Perfil de Salud - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        
        private void AbrirRegistroPeso_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("⚖️ Registro de Peso - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        
        private void AbrirRegistroComidas_Click(object sender, RoutedEventArgs e)
        {
            var comidasView = new RegistroComidaView(usuario.Id);
            comidasView.ShowDialog();
        }

        
        private void AbrirCalculadoraIMC_Click(object sender, RoutedEventArgs e)
        {
            CalculadoraIMCView imcView = new CalculadoraIMCView(usuario.Id);
            imcView.ShowDialog();
            CargarUltimoRegistroIMC();
        }

        
        private void AbrirHidratacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("💧 Hidratación - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        
        private void AbrirActividadFisica_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🏃 Actividad Física - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AbrirInformeNutricional_Click(object sender, RoutedEventArgs e)
        {
            var informeView = new InformeNutricionalView(usuario.Id);
            informeView.ShowDialog();
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}
