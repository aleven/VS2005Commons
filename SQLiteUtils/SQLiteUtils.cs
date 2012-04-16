using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Data.SQLite;
using NLog;

namespace VS2005Commons
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLiteUtils : IDisposable, VS2005Commons.IDBUtils<System.Data.SqlClient.SqlConnection, System.Data.SqlClient.SqlTransaction, System.Data.SqlClient.SqlCommand, System.Data.SqlClient.SqlDataReader>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private String connectionString;
        private System.Data.SqlClient.SqlConnection conn;
        private System.Data.SqlClient.SqlTransaction transaction;
        private bool passedConnection = false;
        private bool transactionOpen = false;

        public static int contatoreConnessioniAperte;

        [DebuggerStepThroughAttribute]
        public SQLiteUtils(String connectionString)
        {
            this.connectionString = connectionString;
        }

        [DebuggerStepThrough]
        public SQLiteUtils(System.Data.SqlClient.SqlConnection conn)
        {
            this.conn = conn;
            passedConnection = true;
        }

        [DebuggerStepThroughAttribute]
        public SQLiteUtils(System.Data.SqlClient.SqlConnection conn, System.Data.SqlClient.SqlTransaction transaction)
            : this(conn)
        {
            // this.conn = conn;
            this.transaction = transaction;
            // passedConnection = true;
        }

        public static SQLiteUtils getIstance(String connectionString)
        {
            return new SQLiteUtils(connectionString);
        }

        public static SQLiteUtils getIstance(System.Data.SqlClient.SqlConnection conn)
        {
            return new SQLiteUtils(conn);
        }

        public static SQLiteUtils getIstance(System.Data.SqlClient.SqlConnection conn, System.Data.SqlClient.SqlTransaction transaction)
        {
            return new SQLiteUtils(conn, transaction);
        }

        public static System.Data.SqlClient.SqlConnection createConnection(String connectionString)
        {
            return new System.Data.SqlClient.SqlConnection(connectionString);
        }

        /// <summary>
        /// Returns a new System.Data.SqlClient.SqlConnection instance
        /// </summary>
        // [DebuggerStepThroughAttribute]
        public System.Data.SqlClient.SqlConnection GetConnection()
        {
            // TODO: RETRIVE CONNECTION STRING
            // System.Data.SqlClient.SqlConnection retVal = new System.Data.SqlClient.SqlConnection(this.connectionString);
            //System.Data.SqlClient.SqlConnection retVal = new System.Data.SqlClient.SqlConnection();
            if (conn == null)
            {
                // this.conn = new System.Data.SqlClient.SqlConnection(this.connectionString);
                this.conn = createConnection(this.connectionString);

                //logger.Debug(connectionString);
                //logger.Debug("Opening connection ... ");
                //this.conn.Open();

            }
            return this.conn;
        }

        /// <summary>
        /// Returns a new System.Data.SqlClient.SqlCommand instance
        /// </summary>
        /// <param name="query">The text of the query</param>
        public System.Data.SqlClient.SqlCommand CreateCommand(String query)
        {
            // return (GetTransaction() != null) ? new System.Data.SqlClient.SqlCommand(query, GetConnection(), GetTransaction()) : new System.Data.SqlClient.SqlCommand(query, GetConnection());
            return new System.Data.SqlClient.SqlCommand(query, GetConnection(), GetTransaction());
        }

        /// <summary>
        /// Run a SQL Command INSERT, UPDATE, DELETE
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        public int ExecuteCommand(System.Data.SqlClient.SqlCommand cmd)
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
        private int ExecuteCommand(System.Data.SqlClient.SqlCommand cmd, bool keepOpenConnection)
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
            catch (SQLiteException ex)
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
        public System.Data.SqlClient.SqlDataReader ExecuteReader(System.Data.SqlClient.SqlCommand cmd, bool keepOpenConnection)
        {
            System.Data.SqlClient.SqlDataReader res = null;

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
            catch (SQLiteException ex)
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
        public object ExecuteScalar(System.Data.SqlClient.SqlCommand cmd)
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
        private object ExecuteScalar(System.Data.SqlClient.SqlCommand cmd, bool keepOpenConnection)
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
            catch (SQLiteException ex)
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
        /// Execute more SQLiteCommands into a System.Data.SqlClient.SqlTransaction
        /// </summary>
        /// <param name="commands"></param>
        public void ExecuteTransactedCommand(params System.Data.SqlClient.SqlCommand[] commands)
        {

            System.Data.SqlClient.SqlConnection conn = GetConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
            }

            System.Data.SqlClient.SqlTransaction trans = conn.BeginTransaction();

            try
            {
                foreach (System.Data.SqlClient.SqlCommand cmd in commands)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = trans;
                    ExecuteCommand(cmd, true);
                }
                trans.Commit();
            }
            catch (SQLiteException ex)
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

        public bool ExecuteExist(System.Data.SqlClient.SqlCommand cmd)
        {
            bool res;

            cmd.Connection = GetConnection();
            System.Data.SqlClient.SqlDataReader dr = null;

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

        public static System.Data.SqlClient.SqlParameter CreateParameter(string parameterName, object value)
        {
            System.Data.SqlClient.SqlParameter ret = new System.Data.SqlClient.SqlParameter();

            // ret = new System.Data.SqlClient.SqlParameter("Test", SqlDbType.DateTime);

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
            System.Data.SqlClient.SqlCommand cmd = this.CreateCommand(SQL);
            return this.ExecuteCommand(cmd);
        }

        public System.Data.SqlClient.SqlDataReader SelectAllFromTable(string tableName, bool keepOpenConnection)
        {
            String SQL = "SELECT * FROM " + tableName;
            System.Data.SqlClient.SqlCommand cmd = this.CreateCommand(SQL);
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public System.Data.SqlClient.SqlDataReader SelectAllFromTable(string tableName, string where, string sort, bool keepOpenConnection)
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

            System.Data.SqlClient.SqlCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public System.Data.SqlClient.SqlDataReader SelectTop1FromTable(string tableName, string where, string sort, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.AppendFormat("SELECT * FROM {0} ", tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            if (StringUtils.isNotEmpty(sort))
            {
                SQL.Append(" ORDER BY " + sort);
            }
            SQL.AppendFormat(" LIMIT {0}", 1);

            System.Data.SqlClient.SqlCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public System.Data.SqlClient.SqlDataReader SelectTopNFromTable(string tableName, string where, string sort, int topN, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.AppendFormat("SELECT * FROM {0} ", tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            if (StringUtils.isNotEmpty(sort))
            {
                SQL.Append(" ORDER BY " + sort);
            }
            SQL.AppendFormat(" LIMIT {0}", topN);

            System.Data.SqlClient.SqlCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        /// <summary>
        /// Per compatibilta con la versione SQLServer teniamo campoCount anche se non viene usato
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="campoCount"></param>
        /// <param name="where"></param>
        /// <param name="keepOpenConnection"></param>
        /// <returns></returns>
        public long SelectCountFromTable(string tableName, string campoCount, string where, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT COUNT(*) FROM " + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }

            System.Data.SqlClient.SqlCommand cmd = this.CreateCommand(SQL.ToString());
            return Convert.ToInt64(this.ExecuteScalar(cmd, keepOpenConnection));
        }

        //public void EmptyTable(string tableName,
        //    System.Data.SqlClient.SqlConnection conn)
        //{
        //    String SQL = "DELETE FROM " + tableName;
        //    System.Data.SqlClient.SqlCommand cmd = this.CreateCommand(SQL);
        //    cmd.Connection = conn;
        //    this.ExecuteCommand(cmd, true);
        //}

        /// <summary>
        /// Attenzione questo forza la Chiusura anche se c'e' una Transazione Aperta
        /// </summary>
        // [DebuggerStepThroughAttribute]
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

        //public static System.Data.SqlClient.SqlParameter CreateParameter(String name, Object value)
        //{
        //    System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter();
        //    param.ParameterName = name;
        //    param.Value = value;

        //    return param;
        //}

        // [DebuggerStepThroughAttribute]
        public void BeginTransaction()
        {
            //if (transaction == null)
            //{
            if (GetConnection().State != ConnectionState.Open)
            {
                GetConnection().Open(); // Devo Aprire prima di Iniziare la Transazione
                contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
            }
            transaction = GetConnection().BeginTransaction();
            transactionOpen = true;
            //}
        }

        // [DebuggerStepThroughAttribute]
        public void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
                /*
                 * Ripristino da Mirco il 27/3 dopo un commit non posso fare read con stesso SQLLiteHelper
                 */
                transaction = null;
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
                transaction = null;
                transactionOpen = false;
            }
        }

        [DebuggerStepThroughAttribute]
        public System.Data.SqlClient.SqlTransaction GetTransaction()
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