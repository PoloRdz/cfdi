using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Models.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Data.DAO
{
    public class UsuarioDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public int getUsuariosCount()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USUARIOS_COUNT", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_TOTAL_REG", 0).Direction = ParameterDirection.InputOutput;
            int total;
            try
            {
                cmd.ExecuteNonQuery();
                total = (int)cmd.Parameters["@PP_TOTAL_REG"].Value;
                return total;
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public List<Usuario> getUsuarios(int page, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USUARIOS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", page);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader reader = null;
            var usuarios = new List<Usuario>();
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("No se han encontrado usuarios");
                while(reader.Read())
                    usuarios.Add(getUsuarioFromReader(reader));
                return usuarios;
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                reader.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public Usuario getUsuario(int id)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USUARIO_BY_ID", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", id);
            SqlDataReader reader = null;
            Usuario user;
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("No se ha encontrado el usuario");
                reader.Read();
                user = getUsuarioFromReader(reader);
                return user;
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                reader.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public InformacionFiscal getUsuarioFiscales(int id)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USUARIO_FISCALES", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", id);
            SqlDataReader reader = null;
            InformacionFiscal infoFis = null;
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("No se ha encontrado la información fiscal del usuario");
                reader.Read();
                infoFis = getInformacionFiscalFromReader(reader);
                return infoFis;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                reader.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public Usuario insertUsuario(Usuario usuario)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_USUARIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", 0).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_NOMBRE_USUARIO", usuario.usuario);
            cmd.Parameters.AddWithValue("@PP_CORREO", usuario.correo);
            cmd.Parameters.AddWithValue("@PP_CONTRASENA", usuario.password);
            cmd.Parameters.AddWithValue("@PP_NOMBRE", usuario.nombre);
            cmd.Parameters.AddWithValue("@PP_APELLIDO_P", usuario.apellidoP);
            cmd.Parameters.AddWithValue("@PP_APELLIDO_M", usuario.apellidoM);
            try
            {
                int id = (int)cmd.Parameters["@PP_ID_USUARIO"].Value;
                if (id <= 0)
                    throw new Exception("No fue posible insertar el usuario");
                usuario.id = id;
                return usuario;
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }

        }

        public void updateUsuario(Usuario usuario)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_USUARIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", usuario.id);
            cmd.Parameters.AddWithValue("@PP_NOMBRE_USUARIO", usuario.usuario);
            cmd.Parameters.AddWithValue("@PP_CORREO", usuario.correo);
            cmd.Parameters.AddWithValue("@PP_CONTRASENA", usuario.password);
            cmd.Parameters.AddWithValue("@PP_NOMBRE", usuario.nombre);
            cmd.Parameters.AddWithValue("@PP_APELLIDO_P", usuario.apellidoP);
            cmd.Parameters.AddWithValue("@PP_APELLIDO_M", usuario.apellidoM);
            try
            {
                int updatedRows = cmd.ExecuteNonQuery();
                if (updatedRows <= 0)
                    throw new Exception("No fue posible actualizar el usuario");
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public void deleteUsuario(int id)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_USUARIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", id);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows == 0)
                    throw new Exception("El usuario no ha sido eliminado");
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        private Usuario getUsuarioFromReader(SqlDataReader reader)
        {
            Usuario usuario = new Usuario();
            usuario.id = reader.GetInt32(0);
            usuario.usuario = reader.GetString(1);
            usuario.password = reader.GetString(2);
            usuario.nombre = reader.GetString(3);
            usuario.apellidoP = reader.GetString(4);
            usuario.apellidoM = reader.GetString(5);
            usuario.correo = reader.GetString(6);
            return usuario;
        }

        private InformacionFiscal getInformacionFiscalFromReader(SqlDataReader rdr)
        {
            InformacionFiscal ifu = new InformacionFiscal();
            ifu.id = rdr.GetInt32(0);
            ifu.rfc = rdr.GetString(1);
            ifu.razonSocial = rdr.GetString(2);
            ifu.direccionFiscal = rdr.GetString(3);
            ifu.codigoPostal = rdr.GetString(4);
            ifu.telefono = rdr.GetString(5);
            return ifu;
        }
    }
}
