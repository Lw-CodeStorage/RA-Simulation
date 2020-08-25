using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace 測試用專案
{
    class Mysql
    {
        string connstr = "Server = 120.108.10.131; Database = test; Userid = st831209; Password = 123456789; SslMode = none;";
        string query = "SELECT * FROM `bicycle_1` WHERE 1";
        string append_str="";
        string insrt_str = "INSERT INTO bicycle_1(`c1_Axle_Center`,`c1_Axle_r`,`d1_Axle_Center`,`d1_Axle_r`,`b1_Axle_Center`, " +
            "`b1_Axle_r`,`f1_Axle_Center`,`f1_Axle_r`,`f2_Axle_Center`,`f2_Axle_r`,`流水號`) VALUES(";
        string insrt_value="";


        MySqlDataAdapter data;


        public MySqlDataAdapter mysql_Select() {

            using (MySqlConnection conn_datagrid = new MySqlConnection())
            {
                conn_datagrid.ConnectionString = connstr;

                data = new MySqlDataAdapter(query, conn_datagrid);
                return data;

            }
        }

        public void mysql_Insert(string[] insrt_str_array,string bicycle_num) {

            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = connstr;
                append_str = "";
                for (int i = 0; i < insrt_str_array.Length; i++)
                {
                    append_str += "'"+insrt_str_array[i]+"',";
                    //append_str += insrt_str_array[i] + ",";
                }
                insrt_value = append_str + "'"+bicycle_num+"')";
                MessageBox.Show(insrt_value);
                insrt_str = insrt_str+ insrt_value;
                MessageBox.Show(insrt_str);
                MySqlCommand command = conn.CreateCommand();

                conn.Open();
                command.CommandText = insrt_str;
                command.ExecuteNonQuery();
                conn.Close();
                
               
            }
            insrt_str = "INSERT INTO bicycle_1(`c1_Axle_Center`,`c1_Axle_r`,`d1_Axle_Center`,`d1_Axle_r`,`b1_Axle_Center`, " +
               "`b1_Axle_r`,`f1_Axle_Center`,`f1_Axle_r`,`f2_Axle_Center`,`f2_Axle_r`,`流水號`) VALUES(";
        }
        /*
        ~Mysql()
        {
            string insrt_str = "INSERT INTO bicycle_1(`c1_Axle_Center`,`c1_Axle_r`,`d1_Axle_Center`,`d1_Axle_r`,`b1_Axle_Center`, " +
                "`b1_Axle_r`,`f1_Axle_Center`,`f1_Axle_r`,`f2_Axle_Center`,`f2_Axle_r`,`流水號`) VALUES(";
            string insrt_value = "";
        }*/
        
    }
}
