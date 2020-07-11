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
        public Usuario GetUsuario(string username, string correo)
        {
            Usuario usuario = new Usuario();
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USUARIO_INFO", cnn);
            SqlDataReader reader = null;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            //////////////////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_NOMBRE_USUARIO", username == null ? "" : username);
            cmd.Parameters.AddWithValue("@PP_CORREO", correo == null ? "" : correo);
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("El usuario no se encuentra registrado o esta desactivado");
                reader.Read();
                usuario.id = reader.GetInt32(0);
                usuario.usuario = reader.GetValue(1).ToString();
                usuario.correo = reader.GetValue(2).ToString();
                usuario.password = reader.GetValue(3).ToString();
                usuario.nombre = reader.GetValue(4).ToString();
                usuario.apellidoP = reader.GetValue(5).ToString();
                usuario.apellidoM = reader.GetValue(6).ToString();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                reader.Close();
                cnn.Dispose();
                cmd.Dispose();
            }
            return usuario;
        }

        public void actualizarContrasena(int idUsuario, string contrasena)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_USUARIO_CONTRASENA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", idUsuario);
            cmd.Parameters.AddWithValue("@PP_PASSWORD", contrasena);
            try
            {
                int ar = cmd.ExecuteNonQuery();
                if (ar < 1)
                    throw new Exception("No fue posible actualizar la contraseña");
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

        public bool buscarNombreUsuario(string  nombreUsuario) 
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_NOMBRE_USUARIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NOMBRE_USUARIO", nombreUsuario);
            SqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                return reader.HasRows;
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

        public bool buscarCorreo(string correo)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_CORREO_USUARIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_CORREO", correo);
            SqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                return reader.HasRows;
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
            SqlCommand cmd = new SqlCommand("PG_IN_USUARIO_V3", cnn);
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
                cmd.ExecuteNonQuery();
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
            SqlCommand cmd = new SqlCommand("PG_UP_USUARIO_V2", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", usuario.id);
            //cmd.Parameters.AddWithValue("@PP_NOMBRE_USUARIO", usuario.usuario);
            //cmd.Parameters.AddWithValue("@PP_CORREO", usuario.correo);
            //cmd.Parameters.AddWithValue("@PP_CONTRASENA", usuario.password);
            cmd.Parameters.AddWithValue("@PP_NOMBRE", usuario.nombre);
            cmd.Parameters.AddWithValue("@PP_APELLIDO_P", usuario.apellidoP);
            cmd.Parameters.AddWithValue("@PP_APELLIDO_M", usuario.apellidoM);
            try
            {
                int updatedRows = cmd.ExecuteNonQuery();
                if (updatedRows < 1)
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

        public InformacionFiscal insertInformacionFiscal(int idUsuario, InformacionFiscal infoFis)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_USUARIO_INFO_FISCAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_INFORMACION_FISCAL", infoFis.id).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", idUsuario);
            cmd.Parameters.AddWithValue("@PP_RFC", infoFis.rfc);
            cmd.Parameters.AddWithValue("@PP_RAZON_SOCIAL", infoFis.razonSocial);
            cmd.Parameters.AddWithValue("@PP_DIRECCION_FISCAL", infoFis.direccionFiscal);
            cmd.Parameters.AddWithValue("@PP_CODIGO_POSTAL", infoFis.codigoPostal);
            cmd.Parameters.AddWithValue("@PP_TELEFONO", infoFis.telefono);
            cmd.Parameters.AddWithValue("@PP_ID_REGIMEN_FISCAL", infoFis.regimenFiscal.idRegimenFiscal);
            try
            {
                cmd.ExecuteNonQuery();
                int id = (int)cmd.Parameters["@PP_ID_INFORMACION_FISCAL"].Value;
                if (id <= 0)
                    throw new Exception("No fue posible insertar la información fiscal");
                infoFis.id = id;
                return infoFis;
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

        public bool buscarRFC(string rfc)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_RFC_USUARIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_RFC", rfc);
            SqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                return reader.HasRows;
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
            InformacionFiscal ifu = new InformacionFiscal
            {
                id = rdr.GetInt32(0),
                rfc = rdr.GetString(1),
                razonSocial = rdr.GetString(2),
                direccionFiscal = rdr.GetString(3),
                codigoPostal = rdr.GetString(4),
                telefono = rdr.GetString(5),
                regimenFiscal = new RegimenFiscal
                {
                    idRegimenFiscal = rdr.GetInt32(6),
                    descripcion = rdr.GetString(7),
                    personaFisica = rdr.GetBoolean(8),
                    personaMoral = rdr.GetBoolean(9)
                }
            };
            return ifu;
        }
    }
}
