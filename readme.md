
## 提示：该功能库不再维护，CRUD 扩展方法已转移至 FreeSql.dll v1.8.0 中，为了与 Dapper.Query 不冲突已移除 Query 扩展方法。

这是 [FreeSql](https://github.com/2881099/FreeSql) 衍生出来的扩展包，实现（Mysql/postgresql/sqlserver/Oracle/SQLite）数据库连接对象扩展方法，像 Dapper 一样的使用习惯（QQ群：4336577）。

> dotnet add package FreeSql.Connection.Extensions

## 更新日志

- 增加 数据库对象.Select 方法；
- 增加 数据库对象.Update 方法；
- 增加 数据库对象.Insert 方法；
- 增加 数据库对象.Delete 方法；
- 增加 数据库对象.Query 执行 SQL 语句的查询方法；

## 快速开始

### 测试实体类
```csharp
class TestConnectionExt {
    public Guid id { get; set; }
    public string title { get; set; }
    public DateTime createTime { get; set; } = DateTime.Now;

    public List<Detail> Details { get; set; }
}
class Detail {
    public Guid id { get; set; }

    public Guid ParentId { get; set; }
    public DateTime createTime { get; set; } = DateTime.Now;
}
```

### 查询
```csharp
using (var conn = new MySqlConnection(_connectString)) {
    var list = conn.Select<TestConnectionExt>()
        .Where(a => a.id == item.id)
        .IncludeMany(a => a.Details.Where(b => b.ParentId == a.id))
        .ToList();
}
```
更多前往Wiki：[《Select 查询数据文档》](https://github.com/2881099/FreeSql/wiki/%e6%9f%a5%e8%af%a2)

### 插入
```csharp
using (var conn = new MySqlConnection(_connectString)) {
    var item = new TestConnectionExt { title = "testinsert" };
    var affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
}
```
更多前往Wiki：[《Insert 插入数据文档》](https://github.com/2881099/FreeSql/wiki/%e6%b7%bb%e5%8a%a0)

### 更新
```csharp
using (var conn = new MySqlConnection(_connectString)) {
    var affrows = conn.Update<TestConnectionExt>()
        .Where(a => a.Id == xxx)
        .Set(a => a.title, "testupdated")
        .ExecuteAffrows();
}
```
更多前往Wiki：[《Update 更新数据文档》](https://github.com/2881099/FreeSql/wiki/%e4%bf%ae%e6%94%b9)

### 删除
```csharp
using (var conn = new MySqlConnection(_connectString)) {
    var affrows = conn.Delete<TestConnectionExt>()
        .Where(a => a.Id == xxx)
        .ExecuteAffrows();
}
```
更多前往Wiki：[《Delete 删除数据文档》](https://github.com/2881099/FreeSql/wiki/%e5%88%a0%e9%99%a4)

### 事务

就像 ado.net 那样使用即可。