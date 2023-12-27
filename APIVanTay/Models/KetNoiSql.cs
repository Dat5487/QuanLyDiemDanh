using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace APIVanTay.Models
{
    public class KetNoiSql
    {
        protected SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-IS4BEN5\SQLEXPRESS;Initial Catalog=QLDD;Integrated Security=True");
    }
}
