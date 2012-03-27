using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using NLog;
using System.Diagnostics;

namespace VS2005Commons
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlUtils : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private String connectionString;
        private SqlConnection conn;
        private SqlTransaction transaction;
        private bool passedConnection = false;
        private bool transactionOpen = false;

        public static int contatoreConnessioniAperte;

        [DebuggerStepThroughAttribute]
        public SqlUtils(String connectionString)
        {
            this.connectionString = connectionString;
        }

        [DebuggerStepThrough]
        public SqlUtils(SqlConnection conn)
        {
            this.conn = conn;
            passedConnection = true;
        }

        [DebuggerStepThroughAttribute]
        public SqlUtils(SqlConnection conn, SqlTransaction transaction)
            : this(conn)
        {
            // this.conn = conn;
            this.transaction = transaction;
            // passedConnection = true;
        }

        /// <summary>
        /// Returns a new SqlConnection instance
        /// </summary>
        [DebuggerStepThroughAttribute]
        public SqlConnection GetConnection()
        {
            // TODO: RETRIVE CONNECTION STRING
            // SqlConnection retVal = new SqlConnection(this.connectionString);
            //SqlConnection retVal = new SqlConnection();
            if (conn == null)
            {
                this.conn = new SqlConnection(this.connectionString);
                //logger.Debug(connectionString);
                //logger.Debug("Opening connection ... ");
                //this.conn.Open();

            }
            return this.conn;
        }

        /// <summary>
        /// Returns a new SqlCommand instance
        /// </summary>
        /// <param name="query">The text of the query</param>
        public SqlCommand CreateCommand(String query)
        {
            // return (GetTransaction() != null) ? new SqlCommand(query, GetConnection(), GetTransaction()) : new SqlCommand(query, GetConnection());
            return new SqlCommand(query, GetConnection(), GetTransaction());
        }

        /// <summary>
        /// Run a SQL Command INSERT, UPDATE, DELETE
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        public int ExecuteCommand(SqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            cmd.Transaction = GetTransaction();
            return ExecuteCommand(cmd, false);
        }

        /// <summary>
        /// Run a SQL Command INSERT, UPDATE, DELETE
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        /// <param name="keepOpenConnection">If false the connection will close</param>
        private int ExecuteCommand(SqlCommand cmd, bool keepOpenConnection)
        {
            int res = -1;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }

                logger.Debug("Executing");
                logger.Debug(cmd.CommandText);

                res = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if ((cmd.Connection.State == ConnectionState.Open) && (!keepOpenConnection) && (!passedConnection) && (!transactionOpen))
                {
                    cmd.Connection.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }

            return res;
        }

        /// <summary>
        /// Aggiunto da Mirco per leggere Dati
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(SqlCommand cmd, bool keepOpenConnection)
        {
            SqlDataReader res = null;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }

                logger.Debug(cmd.CommandText);

                // cmd.Transaction = GetTransaction();

                // res = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                res = cmd.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if ((cmd.Connection.State == ConnectionState.Open) && (!keepOpenConnection) && (!passedConnection) && (!transactionOpen))
                {
                    cmd.Connection.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }

            return res;
        }

        /// <summary>
        /// Run a SELECT comand which return a single value
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        public object ExecuteScalar(SqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            cmd.Transaction = GetTransaction();
            return ExecuteScalar(cmd, false);
        }

        /// <summary>
        /// Run a SELECT comand which return a single value
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        /// <param name="keepOpenConnection">If false the connection will close</param>
        private object ExecuteScalar(SqlCommand cmd, bool keepOpenConnection)
        {
            object ret = null;
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }
                ret = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if ((cmd.Connection.State == ConnectionState.Open) && (!keepOpenConnection) && (!passedConnection) && (!transactionOpen))
                {
                    cmd.Connection.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }
            return ret;
        }


        /// <summary>
        /// Execute more SqlCommands into a SqlTransaction
        /// </summary>
        /// <param name="commands"></param>
        public void ExecuteTransactedCommand(params SqlCommand[] commands)
        {

            SqlConnection conn = GetConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
            }

            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                foreach (SqlCommand cmd in commands)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = trans;
                    ExecuteCommand(cmd, true);
                }
                trans.Commit();
            }
            catch (SqlException ex)
            {
                try
                {
                    trans.Rollback();
                }
                finally
                {
                    throw ex;
                }
            }
            finally
            {
                conn.Close();
                contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                conn.Dispose();
            }
        }

        public bool ExecuteExist(SqlCommand cmd)
        {
            bool res;

            cmd.Connection = GetConnection();
            SqlDataReader dr = null;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }

                // dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dr = cmd.ExecuteReader();
                res = dr.Read();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                // TODO: MAKE LOG
                // Utils.ManageException("SqlCeUtils.ExecuteExist()", ex);
                res = false;
            }
            finally
            {
                if ((dr != null) && (!dr.IsClosed))
                {
                    dr.Close();
                }
                if (cmd.Connection.State == ConnectionState.Open && (!passedConnection) && (!transactionOpen))
                {
                    cmd.Connection.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }

            return res;
        }

        public static SqlParameter CreateParameter(string parameterName, object value)
        {
            SqlParameter ret = new SqlParameter();

            // ret = new SqlParameter("Test", SqlDbType.DateTime);

            ret.ParameterName = parameterName;
            if (value != null)
            {
                /*
                 * Gestisco le DateTime di C# per SQLServer
                 * In SQL non possono essere valorizzate come DateTime.MinDate quindi le considero un null
                 * 
                 */
                if (value is DateTime)
                {
                    // ret.SqlDbType = SqlDbType.DateTime;
                    DateTime valueDate = (DateTime)value;
                    if (valueDate == DateTime.MinValue)
                    {
                        ret.Value = DBNull.Value;
                    }
                    else
                    {
                        ret.Value = value;
                    }
                }
                else
                {
                    ret.Value = value;
                }
            }
            else
            {
                ret.Value = DBNull.Value;
            }
            return ret;
        }

        public int EmptyTable(string tableName)
        {
            String SQL = "DELETE FROM " + tableName;
            SqlCommand cmd = this.CreateCommand(SQL);
            return this.ExecuteCommand(cmd);
        }

        public SqlDataReader SelectAllFromTable(string tableName, bool keepOpenConnection)
        {
            String SQL = "SELECT * FROM " + tableName;
            SqlCommand cmd = this.CreateCommand(SQL);
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public SqlDataReader SelectAllFromTable(string tableName, string where, string sort, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT * FROM " + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            if (StringUtils.isNotEmpty(sort))
            {
                SQL.Append(" ORDER BY " + sort);
            }

            SqlCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public SqlDataReader SelectTop1FromTable(string tableName, string where, string sort, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT TOP(1) * FROM " + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            if (StringUtils.isNotEmpty(sort))
            {
                SQL.Append(" ORDER BY " + sort);
            }

            SqlCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public SqlDataReader SelectTopNFromTable(string tableName, string where, string sort, int topN, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append(String.Format("SELECT TOP({0}) * FROM ", topN) + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            if (StringUtils.isNotEmpty(sort))
            {
                SQL.Append(" ORDER BY " + sort);
            }

            SqlCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public long SelectCountFromTable(string tableName, string campoCount, string where, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT COUNT(" + campoCount + ") FROM " + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }

            SqlCommand cmd = this.CreateCommand(SQL.ToString());
            return Convert.ToInt64(this.ExecuteScalar(cmd, keepOpenConnection));
        }

        //public void EmptyTable(string tableName,
        //    SqlConnection conn)
        //{
        //    String SQL = "DELETE FROM " + tableName;
        //    SqlCommand cmd = this.CreateCommand(SQL);
        //    cmd.Connection = conn;
        //    this.ExecuteCommand(cmd, true);
        //}

        /// <summary>
        /// Attenzione questo forza la Chiusura anche se c'e' una Transazione Aperta
        /// </summary>
        [DebuggerStepThroughAttribute]
        public void CloseConnection()
        {
            // If Not conn Is Nothing AndAlso conn.State = ConnectionState.Open Then
            if (this.conn != null && this.conn.State == ConnectionState.Open)
            {
                if (!passedConnection)
                {
                    conn.Close();
                    contatoreConnessioniAperte = contatoreConnessioniAperte - 1;
                }
            }
        }

        //public static SqlParameter CreateParameter(String name, Object value)
        //{
        //    SqlParameter param = new SqlParameter();
        //    param.ParameterName = name;
        //    param.Value = value;

        //    return param;
        //}

        [DebuggerStepThroughAttribute]
        public void BeginTransaction()
        {
            /*
             * Ripristinato da Mirco 27/03
             */
            if (transaction == null) // era commentato
            {
                if (GetConnection().State != ConnectionState.Open)
                {
                    GetConnection().Open(); // Devo Aprire prima di Iniziare la Transazione
                    contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
                }
                transaction = GetConnection().BeginTransaction();
                transactionOpen = true;
            }
        }

        [DebuggerStepThroughAttribute]
        public void Commit()
        {

            if (transaction != null)
            {
                transaction.Commit();
                /*
                 * Ripristinato da Mirco 27/03
                 */
                transaction = null; // era commentato
                transactionOpen = false;
            }
        }

        [DebuggerStepThroughAttribute]
        public void RollBack()
        {
            /* 
             * Aggiunto transactionOpen perche' così posso richiamarlo sempre questo metodo
             * anche se dovessi fare il commit e poi succede una eccezione per qualche altro motivo
             * questo non solleva piu' l'eccezione che la transazione e' gia stata chiusa
             */

            if (transaction != null && transactionOpen)
            {
                transaction.Rollback();
                /*
                 * Ripristinato da Mirco 27/03
                 */
                transaction = null; // era commentato
                transactionOpen = false;
            }
        }

        [DebuggerStepThroughAttribute]
        public SqlTransaction GetTransaction()
        {
            return transaction;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.CloseConnection();
        }

        #endregion
    }
}