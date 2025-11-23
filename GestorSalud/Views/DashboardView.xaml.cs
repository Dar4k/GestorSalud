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
            // Abrir la vista de perfil de salud pasando el Id del usuario
            var perfilView = new PerfilSaludView(usuario.Id);
            perfilView.Owner = this;
            perfilView.ShowDialog();
        }

        private void AbrirRegistroPeso_Click(object sender, RoutedEventArgs e)
        {
            var registroPesoView = new RegistroPesoView(usuario.Id);
            registroPesoView.Owner = this;

            // Mostrar la ventana y esperar a que se cierre
            bool? result = registroPesoView.ShowDialog();

            // Si se guardó exitosamente (ventana se cerró después de guardar)
            if (result == true) 
            {
                // Actualizar inmediatamente las estadísticas
                CargarUltimoRegistroIMC();
                MessageBox.Show("✅ Peso registrado exitosamente. Estadísticas actualizadas.",
                               "Éxito",
                               MessageBoxButton.OK,
                               MessageBoxImage.Information);
            }
            else
            {
                // Si el usuario cerró sin guardar, también actualizar por si acaso
                CargarUltimoRegistroIMC();
            }
        }

        private void AbrirRegistroComidas_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🍎 Registro de Comidas - Próximamente!", "En desarrollo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AbrirCalculadoraIMC_Click(object sender, RoutedEventArgs e)
        {
            CalculadoraIMCView imcView = new CalculadoraIMCView(usuario.Id);
            imcView.Owner = this;
            bool? result = imcView.ShowDialog();

            // Actualizar estadísticas después de usar la calculadora
            if (result == true)
            {
                CargarUltimoRegistroIMC();
            }
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
