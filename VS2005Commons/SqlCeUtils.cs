using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NLog;
using System.Diagnostics;
using System.Data;

namespace VS2005Commons
{
    /// <summary>
    /// 
    /// </summary>
    // public class SqlUtils : IDisposable, VS2005Commons.IDBUtils<System.Data.SqlServerCe.SqlCeConnection, System.Data.SqlServerCe.SqlCeTransaction, System.Data.SqlServerCe.SqlCeCommand, System.Data.SqlServerCe.SqlCeDataReader>
    public class SqlUtils : IDisposable, VS2005Commons.IDBUtils<System.Data.SqlServerCe.SqlCeConnection, System.Data.SqlServerCe.SqlCeTransaction, System.Data.SqlServerCe.SqlCeCommand, System.Data.SqlServerCe.SqlCeDataReader>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private String connectionString;
        private System.Data.SqlServerCe.SqlCeConnection conn;
        private System.Data.SqlServerCe.SqlCeTransaction transaction;
        private bool passedConnection = false;
        private bool transactionOpen = false;

        public static int contatoreConnessioniAperte;

        [DebuggerStepThroughAttribute]
        private SqlUtils(String connectionString)
        {
            this.connectionString = connectionString;
        }

        [DebuggerStepThrough]
        private SqlUtils(System.Data.SqlServerCe.SqlCeConnection conn)
        {
            this.conn = conn;
            passedConnection = true;
        }

        [DebuggerStepThroughAttribute]
        private SqlUtils(System.Data.SqlServerCe.SqlCeConnection conn, System.Data.SqlServerCe.SqlCeTransaction transaction)
            : this(conn)
        {
            // this.conn = conn;
            this.transaction = transaction;
            // passedConnection = true;
        }

        public static SqlUtils getIstance(String connectionString)
        {
            return new SqlUtils(connectionString);
        }

        public static SqlUtils getIstance(System.Data.SqlServerCe.SqlCeConnection conn)
        {
            return new SqlUtils(conn);
        }

        public static SqlUtils getIstance(System.Data.SqlServerCe.SqlCeConnection conn, System.Data.SqlServerCe.SqlCeTransaction transaction)
        {
            return new SqlUtils(conn, transaction);
        }

        public static System.Data.SqlServerCe.SqlCeConnection createConnection(String connectionString)
        {
            return new System.Data.SqlServerCe.SqlCeConnection(connectionString);
        }

        /// <summary>
        /// Returns a new System.Data.SqlServerCe.SqlCeConnection instance
        /// </summary>
        [DebuggerStepThroughAttribute]
        public System.Data.SqlServerCe.SqlCeConnection GetConnection()
        {
            // TODO: RETRIVE CONNECTION STRING
            // System.Data.SqlServerCe.SqlCeConnection retVal = new System.Data.SqlServerCe.SqlCeConnection(this.connectionString);
            //System.Data.SqlServerCe.SqlCeConnection retVal = new System.Data.SqlServerCe.SqlCeConnection();
            if (conn == null)
            {
                // this.conn = new System.Data.SqlServerCe.SqlCeConnection(this.connectionString);
                this.conn = createConnection(this.connectionString);
                //logger.Debug(connectionString);
                //logger.Debug("Opening connection ... ");
                //this.conn.Open();

            }
            return this.conn;
        }

        /// <summary>
        /// Returns a new System.Data.SqlServerCe.SqlCeCommand instance
        /// </summary>
        /// <param name="query">The text of the query</param>
        public System.Data.SqlServerCe.SqlCeCommand CreateCommand(String query)
        {
            // return (GetTransaction() != null) ? new System.Data.SqlServerCe.SqlCeCommand(query, GetConnection(), GetTransaction()) : new System.Data.SqlServerCe.SqlCeCommand(query, GetConnection());
            return new System.Data.SqlServerCe.SqlCeCommand(query, GetConnection(), GetTransaction());
        }

        /// <summary>
        /// Run a SQL Command INSERT, UPDATE, DELETE
        /// </summary>
        /// <param name="cmd">Command to Execute</param>
        public int ExecuteCommand(System.Data.SqlServerCe.SqlCeCommand cmd)
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
        private int ExecuteCommand(System.Data.SqlServerCe.SqlCeCommand cmd, bool keepOpenConnection)
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
            catch (System.Data.SqlServerCe.SqlCeException ex)
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
        public System.Data.SqlServerCe.SqlCeDataReader ExecuteReader(System.Data.SqlServerCe.SqlCeCommand cmd, bool keepOpenConnection)
        {
            System.Data.SqlServerCe.SqlCeDataReader res = null;

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
            catch (System.Data.SqlServerCe.SqlCeException ex)
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
        public object ExecuteScalar(System.Data.SqlServerCe.SqlCeCommand cmd)
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
        private object ExecuteScalar(System.Data.SqlServerCe.SqlCeCommand cmd, bool keepOpenConnection)
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
            catch (System.Data.SqlServerCe.SqlCeException ex)
            {
                logger.Error(ex);
                if (ex.InnerException != null)
                {
                    logger.Error(ex.InnerException);
                }
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
        /// Execute more SQLiteCommands into a System.Data.SqlServerCe.SqlCeTransaction
        /// </summary>
        /// <param name="commands"></param>
        public void ExecuteTransactedCommand(params System.Data.SqlServerCe.SqlCeCommand[] commands)
        {

            System.Data.SqlServerCe.SqlCeConnection conn = GetConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                contatoreConnessioniAperte = contatoreConnessioniAperte + 1;
            }

            System.Data.SqlServerCe.SqlCeTransaction trans = conn.BeginTransaction();

            try
            {
                foreach (System.Data.SqlServerCe.SqlCeCommand cmd in commands)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = trans;
                    ExecuteCommand(cmd, true);
                }
                trans.Commit();
            }
            catch (System.Data.SqlServerCe.SqlCeException ex)
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

        public bool ExecuteExist(System.Data.SqlServerCe.SqlCeCommand cmd)
        {
            bool res;

            cmd.Connection = GetConnection();
            System.Data.SqlServerCe.SqlCeDataReader dr = null;

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

        // public static SqlParameter CreateParameter(string parameterName, object value)
        public static System.Data.SqlServerCe.SqlCeParameter CreateParameter(string parameterName, object value)
        {
            System.Data.SqlServerCe.SqlCeParameter newParam = new System.Data.SqlServerCe.SqlCeParameter();

            // ret = new SqlParameter("Test", SqlDbType.DateTime);

            newParam.ParameterName = parameterName;
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
                        newParam.Value = DBNull.Value;
                    }
                    else
                    {
                        newParam.Value = value;
                    }
                }
                else
                {
                    newParam.Value = value;
                }
            }
            else
            {
                newParam.Value = DBNull.Value;
            }

            return newParam;
        }

        public int EmptyTable(string tableName)
        {
            String SQL = "DELETE FROM " + tableName;
            System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL);
            return this.ExecuteCommand(cmd);
        }

        public int DeleteFromTable(string tableName, string where)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("DELETE FROM " + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }
            System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteCommand(cmd);
        }

        public System.Data.SqlServerCe.SqlCeDataReader SelectAllFromTable(string tableName, bool keepOpenConnection)
        {
            String SQL = "SELECT * FROM " + tableName;
            System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL);
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public System.Data.SqlServerCe.SqlCeDataReader SelectAllFromTable(string tableName, string where, string sort, bool keepOpenConnection)
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

            System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public System.Data.SqlServerCe.SqlCeDataReader SelectTop1FromTable(string tableName, string where, string sort, bool keepOpenConnection)
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

            System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        public System.Data.SqlServerCe.SqlCeDataReader SelectTopNFromTable(string tableName, string where, string sort, int topN, bool keepOpenConnection)
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

            System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL.ToString());
            return this.ExecuteReader(cmd, keepOpenConnection);
        }

        //public System.Data.SqlServerCe.SqlCeDataReader SelectTop1FromTable(string tableName, string where, string sort, bool keepOpenConnection)
        //{
        //    StringBuilder SQL = new StringBuilder();
        //    SQL.AppendFormat("SELECT * FROM {0} ", tableName);
        //    if (StringUtils.isNotEmpty(where))
        //    {
        //        SQL.Append(" WHERE " + where);
        //    }
        //    if (StringUtils.isNotEmpty(sort))
        //    {
        //        SQL.Append(" ORDER BY " + sort);
        //    }
        //    SQL.AppendFormat(" LIMIT {0}", 1);

        //    System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL.ToString());
        //    return this.ExecuteReader(cmd, keepOpenConnection);
        //}

        //public System.Data.SqlServerCe.SqlCeDataReader SelectTopNFromTable(string tableName, string where, string sort, int topN, bool keepOpenConnection)
        //{
        //    StringBuilder SQL = new StringBuilder();
        //    SQL.AppendFormat("SELECT * FROM {0} ", tableName);
        //    if (StringUtils.isNotEmpty(where))
        //    {
        //        SQL.Append(" WHERE " + where);
        //    }
        //    if (StringUtils.isNotEmpty(sort))
        //    {
        //        SQL.Append(" ORDER BY " + sort);
        //    }
        //    SQL.AppendFormat(" LIMIT {0}", topN);

        //    System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL.ToString());
        //    return this.ExecuteReader(cmd, keepOpenConnection);
        //}

        public long SelectCountFromTable(string tableName, string campoCount, string where, bool keepOpenConnection)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT COUNT(" + campoCount + ") FROM " + tableName);
            if (StringUtils.isNotEmpty(where))
            {
                SQL.Append(" WHERE " + where);
            }

            System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL.ToString());
            return Convert.ToInt64(this.ExecuteScalar(cmd, keepOpenConnection));
        }

        //public void EmptyTable(string tableName,
        //    System.Data.SqlServerCe.SqlCeConnection conn)
        //{
        //    String SQL = "DELETE FROM " + tableName;
        //    System.Data.SqlServerCe.SqlCeCommand cmd = this.CreateCommand(SQL);
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
        public System.Data.SqlServerCe.SqlCeTransaction GetTransaction()
        {
            return transaction;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.CloseConnection();
        }

        #endregion

        /// <summary>
        /// In SQLCe non posso fare DataReader.HasRows
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static bool HasRows(System.Data.SqlServerCe.SqlCeDataReader dr)
        {
            // return dr.Read();
            // return dr.HasRows;
            return true;
        }
    }
}