using GestorSalud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSalud.Controllers
{
    public class UsuariosController
    {
        Services.DBUsuarios dbUsuarios = new Services.DBUsuarios();
        public bool InsertarUsuario(UsuariosModel usuariosModel)
        {
            return dbUsuarios.insertarUsuario(usuariosModel);
        }

        public bool ActualizarUsuario(UsuariosModel usuariosModel)
        {
            return dbUsuarios.actualizarUsuario(usuariosModel);
        }

        public string EncriptarContraseña(string contraseña)
        {
            return dbUsuarios.EncriptarContraseña(contraseña);
        }

        public UsuariosModel ValidarUsuario(string correo, string contraseña)
        {
            contraseña = EncriptarContraseña(contraseña);
            UsuariosModel usuario = dbUsuarios.ValidarUsuario(correo, contraseña);
            return usuario;
        }
    }
}
