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
    /// <summary>
    /// Lógica de interacción para PerfilView.xaml
    /// </summary>
    public partial class PerfilView : Window
    {
        private UsuariosModel usuario;
        public PerfilView(UsuariosModel usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            cargarUsuario();
        }

        private void cargarUsuario()
        {
            txtNombre.Text = usuario.Nombre;
            txtCorreo.Text = usuario.Correo;
            txtContraseña.Password = "***********";
        }

        private void cerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }


    }
}
