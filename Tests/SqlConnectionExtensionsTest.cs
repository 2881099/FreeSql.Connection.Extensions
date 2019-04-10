using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;

namespace Tests.SqlConnectionExtensions {
	public class Methods {

		string _connectString = "Data Source=.;Integrated Security=True;Initial Catalog=freesqlTest;Pooling=true;Max Pool Size=5";

		public Methods() {
			g.sqlserver.CodeFirst.SyncStructure<TestConnectionExt>();
		}

		[Fact]
		public void Insert() {
			var affrows = 0;
			using (var conn = new SqlConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testinsert" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				conn.Close();
			}
			Assert.Equal(1, affrows);
		}
		[Fact]
		public void Update() {
			var affrows = 0;
			using (var conn = new SqlConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testupdate" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				Assert.Equal(1, affrows);
				item = conn.Select<TestConnectionExt>().First();
				affrows = conn.Update<TestConnectionExt>().SetSource(item).Set(a => a.title, "testupdated").ExecuteAffrows();
				conn.Close();
			}
			Assert.Equal(1, affrows);
		}
		[Fact]
		public void Delete() {
			var affrows = 0;
			using (var conn = new SqlConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testdelete" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				Assert.Equal(1, affrows);
				affrows = conn.Delete<TestConnectionExt>().Where(item).ExecuteAffrows();
				conn.Close();
			}
			Assert.Equal(1, affrows);
		}
		[Fact]
		public void Select() {
			var list = new List<TestConnectionExt>();
			var affrows = 0;
			using (var conn = new SqlConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testselect" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				Assert.Equal(1, affrows);
				list = conn.Select<TestConnectionExt>().Where(a => a.id == item.id).ToList();
				conn.Close();
			}
			Assert.Single(list);
		}

		[Fact]
		public void Query() {
			var list = new List<TestConnectionExt>();
			var affrows = 0;
			using (var conn = new SqlConnection(_connectString)) {
				var item = new TestConnectionExt { title = "testquery" };
				affrows = conn.Insert<TestConnectionExt>().AppendData(item).ExecuteAffrows();
				Assert.Equal(1, affrows);
				list = conn.Query<TestConnectionExt>("select * from TestConnectionExt where id = @id", new { id = item.id });
				conn.Close();
			}
			Assert.Single(list);
		}

		class TestConnectionExt {
			public Guid id { get; set; }
			public string title { get; set; }
			public DateTime createTime { get; set; } = DateTime.Now;
		}
	}
}
