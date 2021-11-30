using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    class DataB
    {
        MySqlConnection connection = new MySqlConnection("server = mysql-59996-0.cloudclusters.net; port = 19041; Username = admin; Password = 8mqkiYqQ; database = InternetShop; charset = utf8");

        public void openConn()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }
        public void closeConn()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
        public MySqlConnection getConn()
        {
            return connection;
        }
    }
}
