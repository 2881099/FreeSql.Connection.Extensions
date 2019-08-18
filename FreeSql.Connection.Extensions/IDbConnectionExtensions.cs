using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

public static class IDbConnectionExtensions
{

    public static ISelect<T1> Select<T1>(this IDbConnection that) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Select<T1>().WithConnection(that as DbConnection);
    public static ISelect<T1> Select<T1>(this IDbConnection that, object dywhere) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Select<T1>(dywhere).WithConnection(that as DbConnection);
    public static IInsert<T1> Insert<T1>(this IDbConnection that) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Insert<T1>().WithConnection(that as DbConnection);
    public static IInsert<T1> Insert<T1>(this IDbConnection that, T1 source) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Insert<T1>(source).WithConnection(that as DbConnection);
    public static IInsert<T1> Insert<T1>(this IDbConnection that, T1[] source) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Insert<T1>(source).WithConnection(that as DbConnection);
    public static IInsert<T1> Insert<T1>(this IDbConnection that, IEnumerable<T1> source) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Insert<T1>(source).WithConnection(that as DbConnection);
    public static IUpdate<T1> Update<T1>(this IDbConnection that) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Update<T1>().WithConnection(that as DbConnection);
    public static IUpdate<T1> Update<T1>(this IDbConnection that, object dywhere) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Update<T1>(dywhere).WithConnection(that as DbConnection);
    public static IDelete<T1> Delete<T1>(this IDbConnection that) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Delete<T1>().WithConnection(that as DbConnection);
    public static IDelete<T1> Delete<T1>(this IDbConnection that, object dywhere) where T1 : class => DbConnectionExtensions.GetCurd(that?.GetType()).Delete<T1>(dywhere).WithConnection(that as DbConnection);

    public static List<T> Query<T>(this IDbConnection that, string cmdText, object parms = null) => DbConnectionExtensions.GetCurd(that?.GetType()).Ado.Query<T>(that as DbConnection, null, cmdText, parms);
    public static List<T> Query<T>(this IDbConnection that, CommandType cmdType, string cmdText, params DbParameter[] cmdParms) => DbConnectionExtensions.GetCurd(that?.GetType()).Ado.Query<T>(that as DbConnection, null, cmdType, cmdText, cmdParms);
}
