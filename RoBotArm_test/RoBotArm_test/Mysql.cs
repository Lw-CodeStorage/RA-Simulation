using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RoBotArm_test
{
     class Mysql
    {
        static string connstr = "Server = localhost; Database = Measuring_System; Userid = root; Password = 123456789; SslMode = none;";
        public static string static_bicycle_id;

        public string get_Static_Bicycle_Id() {
            return static_bicycle_id;
        }
        //給設定介面用
        MySqlDataReader mysqlread;
        DataTable dt0 = new DataTable();
        public DataTable mysql_Select_ForSetting(string tabelName)
        {
            MySqlConnection connection = new MySqlConnection(connstr);
            connection.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + tabelName , connection);
            //mysqlread = cmd.ExecuteReader();
            dt0.Load(cmd.ExecuteReader());
            connection.Close();
            return dt0;
           
        }

        public void mysql_Update_ForError_Setting(string TT1textBoxCorrect, string TT1textBoxError, 
            string TT2textBoxCorrect, string TT2textBoxError, string TTtexBoxtWidth ,string TTtexBoxtError , 
            string ST1textBoxCorrect, string ST1textBoxError, string ST2textBoxCorrect,string ST2textBoxError, 
            string STtextBoxWidth, string STtextBoxError, string TTS1textBoxCorrect, string TTS1textBoxError, string TTS2textBoxCorrect, string TTS2textBoxError,
            string TTStexBoxtWidth, string TTStexBoxtError,  string CenterPlanetexBoxError ,string HoleplaneparalleltexBoxError) {
            MySqlConnection connection = new MySqlConnection(connstr);
 
            connection.Open();
            MySqlCommand cmd = 
            new MySqlCommand("UPDATE Error_Setting SET TT1='"+TT1textBoxCorrect+"',TT1_Error='"+TT1textBoxError+"',TT2='"+TT2textBoxCorrect+
            "',TT2_Error='"+TT2textBoxError+"',TTWidth='"+TTtexBoxtWidth+"',TTWidth_Error='"+TTtexBoxtError+"',ST1='"+ST1textBoxCorrect+"',ST1_Error='"+ST1textBoxError+
            "',ST2='"+ST2textBoxCorrect+"',ST2_Error='"+ST2textBoxError+"',STWidth='"+STtextBoxWidth+"',STWidth_Error='"+STtextBoxError+"',TTS1='"+TTS1textBoxCorrect+
            "',TTS1_Error='"+TTS1textBoxError+"',TTS2='"+TTS2textBoxCorrect+"',TTS2_Error='"+TTS2textBoxError+"',TTSWidth='"+TTStexBoxtWidth+"',TTSWidth_Error='"+TTStexBoxtError+
            "',CenterPlane_Error='"+CenterPlanetexBoxError+ "',	Holeplaneparallel_Error='"+ HoleplaneparalleltexBoxError + "'", connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }


        //給呈現 resultPage用
        DataTable dt = new DataTable();
        public DataTable mysql_Select_SearchResult(string bicycle_id) {
            MySqlConnection connection = new MySqlConnection(connstr);

            static_bicycle_id = bicycle_id;
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM bicycle_result where bicycle_id = "+ bicycle_id, connection);
            dt.Load(cmd.ExecuteReader());
           
            connection.Close();
            return dt;
            
        }

        public void mysql_Insert(string bicycle_id, string item,string value,string gonogo) {
            MySqlConnection connection = new MySqlConnection(connstr);
            string insertstr = "INSERT INTO Bicycle_Result (`bicycle_id`,`item`,`value`,`GoNogo`) VALUES('" + bicycle_id + "','" + item + "','" + value + "','" + gonogo + "')";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(insertstr,connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

    }
}
