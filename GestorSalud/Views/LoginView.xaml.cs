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
using System.Windows.Shapes;

namespace GestorSalud.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        UsuariosController usuariosController = new UsuariosController();

        public void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            string correo = txtCorreo.Text;
            string contraseña = txtPassword.Password;

            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                MessageBox.Show("Por favor ingresa tu correo y contraseña.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UsuariosModel usuario = usuariosController.ValidarUsuario(correo, contraseña);

            if (usuario != null)
            {
                DashboardView dashboardView = new DashboardView(usuario);
                dashboardView.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Correo o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void Registrar_Click(object sender, RoutedEventArgs e)
        {
            RegistroView registroView = new RegistroView();
            registroView.Show();
            this.Close();
        }   
    }
}
