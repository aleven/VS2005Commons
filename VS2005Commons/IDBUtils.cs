using System;
namespace VS2005Commons
{
    public interface IDBUtils
    {
        void BeginTransaction();
        void CloseConnection();
        void Commit();
        System.Data.SqlClient.SqlCommand CreateCommand(string query);
        void Dispose();
        int EmptyTable(string tableName);
        int ExecuteCommand(System.Data.SqlClient.SqlCommand cmd);
        bool ExecuteExist(System.Data.SqlClient.SqlCommand cmd);
        System.Data.SqlClient.SqlDataReader ExecuteReader(System.Data.SqlClient.SqlCommand cmd, bool keepOpenConnection);
        object ExecuteScalar(System.Data.SqlClient.SqlCommand cmd);
        void ExecuteTransactedCommand(params System.Data.SqlClient.SqlCommand[] commands);
        System.Data.SqlClient.SqlConnection GetConnection();
        System.Data.SqlClient.SqlTransaction GetTransaction();
        void RollBack();
        System.Data.SqlClient.SqlDataReader SelectAllFromTable(string tableName, bool keepOpenConnection);
        System.Data.SqlClient.SqlDataReader SelectAllFromTable(string tableName, string where, string sort, bool keepOpenConnection);
        long SelectCountFromTable(string tableName, string campoCount, string where, bool keepOpenConnection);
        System.Data.SqlClient.SqlDataReader SelectTop1FromTable(string tableName, string where, string sort, bool keepOpenConnection);
        System.Data.SqlClient.SqlDataReader SelectTopNFromTable(string tableName, string where, string sort, int topN, bool keepOpenConnection);
   }
}
