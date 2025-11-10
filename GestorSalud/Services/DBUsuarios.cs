using GestorSalud.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace GestorSalud.Services
{
    public class DBUsuarios
    {
        string conexion = "server=localhost; user=root; database=salud; password= ; port=3306; ";
        

        public Boolean insertarUsuario(UsuariosModel usuarios) {

            string validacion = "SELECT COUNT(*) FROM usuarios WHERE Correo = @correo OR Identificacion = @identificacion";
            string procedimiento = "INSERT into usuarios (Identificacion, Nombre, Correo, Contraseña) VALUES (@identificacion, @nombre, @correo, @contraseña)";

            using (MySqlConnection mySqlConnection = new MySqlConnection(conexion))
            {
                using (MySqlCommand command = new MySqlCommand(validacion, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@correo", usuarios.Correo);
                    command.Parameters.AddWithValue("@identificacion", usuarios.Identificacion);
                    mySqlConnection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        return false;
                    }
                }
            }

            using (MySqlConnection mySqlConnection = new MySqlConnection(conexion))
            {
                using (MySqlCommand command = new MySqlCommand(procedimiento, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@identificacion", usuarios.Identificacion );
                    command.Parameters.AddWithValue("@nombre", usuarios.Nombre );
                    command.Parameters.AddWithValue("@correo", usuarios.Correo );
                    command.Parameters.AddWithValue("@contraseña", usuarios.Contraseña );

                    mySqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean actualizarUsuario(UsuariosModel usuarios) {

            string procedimiento = "UPDATE usuarios set Identificacion=@identificacion, Nombre=@nombre, Correo=@correo, Contraseña=@contraseña) WHERE Identificacion= " + usuarios.Identificacion;
            using (MySqlConnection mySqlConnection = new MySqlConnection(conexion))
            {
                using (MySqlCommand command = new MySqlCommand(procedimiento, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@identificacion", usuarios.Identificacion);
                    command.Parameters.AddWithValue("@nombre", usuarios.Nombre);
                    command.Parameters.AddWithValue("@correo", usuarios.Correo);
                    command.Parameters.AddWithValue("@contraseña", usuarios.Contraseña);

                    mySqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public UsuariosModel seleccionarUsuario(UsuariosModel usuarios)
        {
            UsuariosModel usuariosModel = new UsuariosModel();
            string procedimiento = "SELECT * from usuarios WHERE Identificacion= " + usuarios.Identificacion;
            using (MySqlConnection mySqlConnection = new MySqlConnection(conexion))
            {
                using (MySqlCommand command = new MySqlCommand(procedimiento, mySqlConnection))
                {
                    mySqlConnection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuariosModel.Id = reader.GetInt32("Id");
                            usuariosModel.Identificacion = reader.GetInt32("Identificacion");
                            usuariosModel.Nombre = reader.GetString("Nombre");
                            usuariosModel.Correo = reader.GetString("Correo");
                            usuariosModel.Contraseña = reader.GetString("Contraseña");
                        }
                    }
                }
            }
            return usuariosModel;
        }

        public UsuariosModel ValidarUsuario(string correo, string contraseñaEncriptada)
        {
            string sql = "SELECT id, Identificacion, Nombre, Correo, Contraseña FROM usuarios WHERE Correo = @correo AND Contraseña = @contraseña";

            using (MySqlConnection conn = new MySqlConnection(conexion))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@contraseña", contraseñaEncriptada);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var usuario = new UsuariosModel
                        {
                            Id = reader.GetInt32("id"),
                            Identificacion = reader.IsDBNull(reader.GetOrdinal("Identificacion")) ? 0 : reader.GetInt32("Identificacion"),
                            Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? "" : reader.GetString("Nombre"),
                            Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? "" : reader.GetString("Correo"),
                            Contraseña = reader.IsDBNull(reader.GetOrdinal("Contraseña")) ? "" : reader.GetString("Contraseña")
                        };

                        return usuario;
                    }
                }
            }

            return null;
        }


        public string EncriptarContraseña(string contraseña)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(contraseña);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
