using System;
using System.Data;
using System.Data.SqlClient;

namespace API.DAO.Util
{
    public class SqlHelp
    {
        private string mstr_StringConexao;
        private SqlConnection mobj_ConexaoSQL;
        private SqlCommand mobj_ComandoSQL;
        private int mint_TimeoutComando = 60000000;
        private string dbConexao;

        public SqlHelp()
        {
            try
            {
                mstr_StringConexao = System.Configuration.ConfigurationManager.AppSettings["connectionstring"].ToString();

                mobj_ConexaoSQL = new SqlConnection(mstr_StringConexao);
                mobj_ComandoSQL = new SqlCommand();
                mobj_ComandoSQL.CommandTimeout = mint_TimeoutComando;
                mobj_ComandoSQL.Connection = mobj_ConexaoSQL;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inicializar conexão." + Environment.NewLine + ex.Message);
            }
        }

        public SqlHelp(string dbConexao)
        {
            mobj_ConexaoSQL = new SqlConnection(dbConexao);
            mobj_ComandoSQL = new SqlCommand();
            mobj_ComandoSQL.CommandTimeout = mint_TimeoutComando;
            mobj_ComandoSQL.Connection = mobj_ConexaoSQL;

            // TODO: Complete member initialization
            this.dbConexao = dbConexao;
        }

        public void Dispose()
        {
            try
            {
                //Limpando a conexao
                if (mobj_ConexaoSQL != null)
                {
                    if (mobj_ConexaoSQL.State != ConnectionState.Closed)
                        mobj_ConexaoSQL.Close();
                    mobj_ConexaoSQL.Dispose();
                    mobj_ConexaoSQL = null;
                }

                //Limpando o command
                if (mobj_ComandoSQL != null)
                {
                    mobj_ComandoSQL.Dispose();
                    mobj_ComandoSQL = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no dispose." + Environment.NewLine + ex.Message);
            }
        }

        public void FecharConexao()
        {
            if (mobj_ConexaoSQL != null)
                if (mobj_ConexaoSQL.State != ConnectionState.Closed) mobj_ConexaoSQL.Close();
        }

        public void GetExecuteNonQueryByCommand(string Command)
        {
            try
            {
                mobj_ComandoSQL.CommandText = Command;
                mobj_ComandoSQL.CommandTimeout = mint_TimeoutComando;
                mobj_ComandoSQL.CommandType = CommandType.StoredProcedure;

                mobj_ConexaoSQL.Open();

                mobj_ComandoSQL.Connection = mobj_ConexaoSQL;
                mobj_ComandoSQL.ExecuteNonQuery();

                FecharConexao();
            }
            catch (Exception ex)
            {
                FecharConexao();
                throw ex;
            }
        }

        public DataSet GetDatasetByCommand(string Command)
        {
            try
            {
                mobj_ComandoSQL.CommandText = Command;
                mobj_ComandoSQL.CommandTimeout = mint_TimeoutComando;
                mobj_ComandoSQL.CommandType = CommandType.StoredProcedure;

                mobj_ConexaoSQL.Open();

                SqlDataAdapter adpt = new SqlDataAdapter(mobj_ComandoSQL);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FecharConexao();
            }
        }

        public SqlDataReader GetReaderBySQL(string strSQL)
        {
            mobj_ConexaoSQL.Open();
            try
            {
                SqlCommand myCommand = new SqlCommand(strSQL, mobj_ConexaoSQL);
                return myCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                FecharConexao();
                throw ex;
            }
        }

        public SqlDataReader GetReaderByCmd(string Command)
        {
            SqlDataReader objSqlDataReader = null;
            try
            {
                mobj_ComandoSQL.CommandText = Command;
                mobj_ComandoSQL.CommandType = CommandType.StoredProcedure;
                mobj_ComandoSQL.CommandTimeout = mint_TimeoutComando;

                mobj_ConexaoSQL.Open();
                mobj_ComandoSQL.Connection = mobj_ConexaoSQL;

                objSqlDataReader = mobj_ComandoSQL.ExecuteReader();
                return objSqlDataReader;
            }
            catch (Exception ex)
            {
                FecharConexao();
                throw ex;
            }

        }

        public void AddParameterToSQLCommand(string NomeParametro, SqlDbType TipoParametro)
        {
            try
            {
                mobj_ComandoSQL.Parameters.Add(new SqlParameter(NomeParametro, TipoParametro));
                mobj_ComandoSQL.Parameters[NomeParametro].Value = System.DBNull.Value;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddParameterToSQLCommand(string NomeParametro, SqlDbType TipoParametro, object Valor)
        {
            try
            {
                mobj_ComandoSQL.Parameters.Add(new SqlParameter(NomeParametro, TipoParametro));
                mobj_ComandoSQL.Parameters[NomeParametro].Value = Valor;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetExecuteScalarByCommandString(string Command)
        {
            object retorno = string.Empty;
            try
            {
                mobj_ComandoSQL.CommandText = Command;
                mobj_ComandoSQL.CommandTimeout = mint_TimeoutComando;
                mobj_ComandoSQL.CommandType = CommandType.StoredProcedure;

                mobj_ConexaoSQL.Open();

                mobj_ComandoSQL.Connection = mobj_ConexaoSQL;
                retorno = mobj_ComandoSQL.ExecuteScalar().ToString();
                mobj_ComandoSQL.Dispose();
                mobj_ComandoSQL = null;
                FecharConexao();
            }
            catch (Exception ex)
            {
                FecharConexao();
                throw ex;
            }
            return Convert.ToString(retorno);
        }

        public int GetExecuteScalarByCommand(string Command)
        {
            object identity = 0;
            try
            {
                mobj_ComandoSQL.CommandText = Command;
                mobj_ComandoSQL.CommandTimeout = mint_TimeoutComando;
                mobj_ComandoSQL.CommandType = CommandType.StoredProcedure;

                mobj_ConexaoSQL.Open();

                mobj_ComandoSQL.Connection = mobj_ConexaoSQL;
                identity = mobj_ComandoSQL.ExecuteScalar();
                mobj_ComandoSQL.Dispose();
                mobj_ComandoSQL = null;
                FecharConexao();
            }
            catch (Exception ex)
            {
                FecharConexao();
                throw ex;
            }
            return Convert.ToInt32(identity);
        }
    }
}