using System;
using System.Data;
namespace VS2005Commons
{
    public interface IDBUtils<C, T, S, R>
        where C : IDbConnection
        where T : IDbTransaction
        where S : IDbCommand
        where R : IDataReader
    {
        void BeginTransaction();
        void CloseConnection();
        void Commit();
        S CreateCommand(string query);
        void Dispose();
        int EmptyTable(string tableName);
        int ExecuteCommand(S cmd);
        bool ExecuteExist(S cmd);
        R ExecuteReader(S cmd, bool keepOpenConnection);
        object ExecuteScalar(S cmd);
        void ExecuteTransactedCommand(params S[] commands);
        C GetConnection();
        T GetTransaction();
        void RollBack();
        R SelectAllFromTable(string tableName, bool keepOpenConnection);
        R SelectAllFromTable(string tableName, string where, string sort, bool keepOpenConnection);
        long SelectCountFromTable(string tableName, string campoCount, string where, bool keepOpenConnection);
        R SelectTop1FromTable(string tableName, string where, string sort, bool keepOpenConnection);
        R SelectTopNFromTable(string tableName, string where, string sort, int topN, bool keepOpenConnection);

        // static IDBUtils<C, T, S, R> getIstance(string connectionString);
        // static IDBUtils<C, T, S, R> getIstance(C conn, T transaction);
    }
}
