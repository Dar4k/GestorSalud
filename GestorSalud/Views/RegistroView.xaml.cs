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
    public partial class RegistroView : Window
    {
        //Services.DBUsuarios db = new Services.DBUsuarios();
        UsuariosController usuariosController = new UsuariosController();
        public RegistroView()
        {
            InitializeComponent();
        }

        public void Registrar_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(txtIdentificacion.Text) ||
                string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtPassword.Password) ||
                string.IsNullOrEmpty(txtPasswordSecond.Password))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPassword.Password != txtPasswordSecond.Password)
            {
                MessageBox.Show("Las contraseñas deben ser iguales.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UsuariosModel usuarios = new UsuariosModel
            {
                Identificacion = int.Parse(txtIdentificacion.Text),
                Nombre = txtNombre.Text,
                Correo = txtCorreo.Text,
                Contraseña = usuariosController.EncriptarContraseña(txtPassword.Password),
                FechaIngreso = DateTime.Now
            };

            bool resultado = usuariosController.InsertarUsuario(usuarios);

            if (resultado)
            {
                MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginView loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al registrar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Volver_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}
