using GestorSalud.Controllers;
using GestorSalud.Models;
using System.Windows;

namespace GestorSalud.Views
{
    public partial class DashboardView : Window
    {
        private UsuariosModel usuario;
        private RegistroPesoController registroController = new RegistroPesoController();
        private HidratacionController hidratacionController = new HidratacionController();

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
                var hidratacion = hidratacionController.ObtenerHidratacionPorUsuario(usuario.Id);
                var agua = hidratacion.OrderByDescending(x => x.VasosAgua).FirstOrDefault();

                if (ultimoRegistro != null)
                {
                    txtUltimoPeso.Text = $"{ultimoRegistro.Peso} kg";
                    txtIMC.Text = $"{ultimoRegistro.IMCCalculado:F1}";
                    txtAguaHoy.Text = agua != null ? $"{agua.VasosAgua} vasos" : "0 vasos";
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
            MessageBox.Show("🍎 Registro de Comidas - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AbrirCalculadoraIMC_Click(object sender, RoutedEventArgs e)
        {
            CalculadoraIMCView imcView = new CalculadoraIMCView(usuario.Id);
            imcView.ShowDialog();
            CargarUltimoRegistroIMC();
        }

        private void AbrirHidratacion_Click(object sender, RoutedEventArgs e)
        {
            HidratacionView hidratacionView = new HidratacionView(usuario.Id);
            hidratacionView.Show();
            this.Close();
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void AbrirActividadFisica_Click(object sender, RoutedEventArgs e)
        {
            EjerciciosView ejerciciosView = new EjerciciosView(usuario.Id);
            ejerciciosView.Show();
            this.Close();
        }
    }
}
