using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using nwind;

#if ORACLE
using xint = System.Int32;
#elif POSTGRES
using xint = System.Int32;
using XSqlConnection = Npgsql.NpgsqlConnection;
using XSqlCommand = Npgsql.NpgsqlCommand;
#else
using XSqlConnection = MySql.Data.MySqlClient.MySqlConnection;
using XSqlCommand = MySql.Data.MySqlClient.MySqlCommand;
using xint = System.UInt32;
#endif

namespace Test_NUnit
{
    [TestFixture]
    public class StoredProcTest : TestBase
    {


        [Test]
        public void SP1_CallHello0()
        {
            Northwind db = base.CreateDB();
            string result = db.hello0();
            Assert.IsNotNull(result);
        }

        [Test]
        public void SP2_CallHello1()
        {
            Northwind db = base.CreateDB();
            string result = db.hello1("xx");
            Assert.IsTrue(result!=null && result.Contains("xx"));
        }

        [Test]
        public void SP3_GetOrderCount_SelField()
        {
            Northwind db = base.CreateDB();
            var q = from c in db.Customers select new {c, OrderCount=db.getOrderCount(c.CustomerID)};

            int count = 0;
            foreach (var v in q)
            {
                Assert.Greater(v.c.CustomerID, 0);
                Assert.Greater(v.OrderCount, -1);
                count++;
            }
            Assert.Greater(count, 0);
        }

        [Test]
        public void SP4_GetOrderCount_Having()
        {
            Northwind db = base.CreateDB();
            var q = from c in db.Customers where db.getOrderCount(c.CustomerID) > 1 select c;

            int count = 0;
            foreach (var c in q)
            {
                Assert.Greater(c.CustomerID, 0);
                count++;
            }
            Assert.Greater(count, 0);
        }
    }
}
