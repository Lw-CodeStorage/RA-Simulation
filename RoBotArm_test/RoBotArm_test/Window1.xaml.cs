using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RoBotArm_test
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Window1 : Window
    {
        string TT1textBoxCorrect, TT1textBoxError, TTS1textBoxCorrect, TTS1textBoxError,ST1textBoxCorrect, ST1textBoxError;
        string TT2textBoxCorrect, TT2textBoxError, TTS2textBoxCorrect, TTS2textBoxError, ST2textBoxCorrect, ST2textBoxError;
        string TTtexBoxtWidth, TTtexBoxtError, TTStexBoxtWidth, TTStexBoxtError, STtextBoxWidth, STtextBoxError;
        string CenterPlanetexBoxError, HoleplaneparalleltexBoxError;
        Mysql mysql = new Mysql();
        MySqlDataReader mysqlread;
        DataTable dt = new DataTable();
        public Window1()
        {
            InitializeComponent();
           
            dt = mysql.mysql_Select_ForSetting("Error_Setting");
           // mysqlread.Read();
         
            TT1_textBoxCorrect.Text = dt.Rows[0][1].ToString();
            TT1_textBoxError.Text = dt.Rows[0][2].ToString();

            TT2_textBoxCorrect.Text = dt.Rows[0][3].ToString();
            TT2_textBoxError.Text = dt.Rows[0][4].ToString();

            TT_Width.Text = dt.Rows[0][5].ToString();
            TT_Error.Text = dt.Rows[0][6].ToString();
           
            ST1_textBoxCorrect.Text = dt.Rows[0][7].ToString();
            ST1_textBoxError.Text = dt.Rows[0][8].ToString();

            ST2_textBoxCorrect.Text = dt.Rows[0][9].ToString();
            ST2_textBoxError.Text = dt.Rows[0][10].ToString();

            ST_Width.Text = dt.Rows[0][11].ToString();
            ST_Error.Text = dt.Rows[0][12].ToString();

            TTS1_textBoxCorrect.Text = dt.Rows[0][13].ToString();
            TTS1_textBoxError.Text = dt.Rows[0][14].ToString();

            TTS2_textBoxCorrect.Text = dt.Rows[0][15].ToString();
            TTS2_textBoxError.Text = dt.Rows[0][16].ToString();


            TTS_Width.Text = dt.Rows[0][17].ToString();
            TTS_Error.Text = dt.Rows[0][18].ToString();

            Center_Plane_Error.Text = dt.Rows[0][19].ToString();
            Holeplaneparallel_Error.Text = dt.Rows[0][20].ToString();

            /*
             TT1_textBoxCorrect.Text =  RoBotArm_test.Properties.Settings.Default.TT1_textBoxCorrect.ToString();
             TT1_textBoxError.Text = RoBotArm_test.Properties.Settings.Default.TT1_textBoxError.ToString();

             TTS1_textBoxCorrect.Text = RoBotArm_test.Properties.Settings.Default.TTS1_textBoxCorrect.ToString();
             TTS1_textBoxError.Text = RoBotArm_test.Properties.Settings.Default.TTS1_textBoxError.ToString();

             ST1_textBoxCorrect.Text = RoBotArm_test.Properties.Settings.Default.ST1_textBoxCorrect.ToString();
             ST1_textBoxError.Text = RoBotArm_test.Properties.Settings.Default.ST1_textBoxError.ToString();

             TT2_textBoxCorrect.Text = RoBotArm_test.Properties.Settings.Default.TT2_textBoxCorrect.ToString();
             TT2_textBoxError.Text = RoBotArm_test.Properties.Settings.Default.TT2_textBoxError.ToString();

             TTS2_textBoxCorrect.Text = RoBotArm_test.Properties.Settings.Default.TTS2_textBoxCorrect.ToString();
             TTS2_textBoxError.Text = RoBotArm_test.Properties.Settings.Default.TTS2_textBoxError.ToString();

             ST2_textBoxCorrect.Text = RoBotArm_test.Properties.Settings.Default.ST2_textBoxCorrect.ToString();
             ST2_textBoxError.Text = RoBotArm_test.Properties.Settings.Default.ST2_textBoxError.ToString();

             TT_Width.Text = RoBotArm_test.Properties.Settings.Default.TT_Width.ToString();
             TT_Error.Text = RoBotArm_test.Properties.Settings.Default.TT_Error.ToString();

             TTS_Width.Text = RoBotArm_test.Properties.Settings.Default.TTS_Width.ToString();
             TTS_Error.Text = RoBotArm_test.Properties.Settings.Default.TTS_Error.ToString();

             ST_Width.Text = RoBotArm_test.Properties.Settings.Default.ST_Width.ToString();
             ST_Error.Text = RoBotArm_test.Properties.Settings.Default.ST_Error.ToString();

             Center_Plane_Error.Text = RoBotArm_test.Properties.Settings.Default.Center_Plane_Error.ToString();
             */

        }
        
        private void settingButton(object sender, RoutedEventArgs e)
        {
            TT1textBoxCorrect = TT1_textBoxCorrect.Text;
            TT1textBoxError = TT1_textBoxError.Text;

            TT2textBoxCorrect = TT2_textBoxCorrect.Text;
            TT2textBoxError= TT2_textBoxError.Text;

            TTtexBoxtWidth = TT_Width.Text;
            TTtexBoxtError = TT_Error.Text;

            ST1textBoxCorrect = ST1_textBoxCorrect.Text;
            ST1textBoxError = ST1_textBoxError.Text;

            ST2textBoxCorrect = ST2_textBoxCorrect.Text;
            ST2textBoxError = ST2_textBoxError.Text;


            STtextBoxWidth = ST_Width.Text;
            STtextBoxError = ST_Error.Text;

            TTS1textBoxCorrect = TTS1_textBoxCorrect.Text;
            TTS1textBoxError = TTS1_textBoxError.Text;

            TTS2textBoxCorrect = TTS2_textBoxCorrect.Text;
            TTS2textBoxError = TTS2_textBoxError.Text;

            TTStexBoxtWidth = TTS_Width.Text;
            TTStexBoxtError = TTS_Error.Text;

            CenterPlanetexBoxError = Center_Plane_Error.Text;
            HoleplaneparalleltexBoxError = Holeplaneparallel_Error.Text;

            mysql.mysql_Update_ForError_Setting(
                TT1textBoxCorrect, 
                TT1textBoxError, 
                TT2textBoxCorrect, 
                TT2textBoxError, 
                TTtexBoxtWidth,
                TTtexBoxtError, 
                ST1textBoxCorrect, 
                ST1textBoxError, 
                ST2textBoxCorrect, 
                ST2textBoxError, 
                STtextBoxWidth,
                STtextBoxError,
                TTS1textBoxCorrect, 
                TTS1textBoxError,
                TTS2textBoxCorrect, 
                TTS2textBoxError, 
                TTStexBoxtWidth, 
                TTStexBoxtError, 
                CenterPlanetexBoxError,
                HoleplaneparalleltexBoxError
                );
            /*
            TT1textBoxCorrect = Convert.ToSingle(TT1_textBoxCorrect.Text);
            TT1textBoxError = Convert.ToSingle(TT1_textBoxError.Text);
            RoBotArm_test.Properties.Settings.Default.TT1_textBoxCorrect = TT1textBoxCorrect;
            RoBotArm_test.Properties.Settings.Default.TT1_textBoxError = TT1textBoxError;

            TT2textBoxCorrect = Convert.ToSingle(TT2_textBoxCorrect.Text);
            TT2textBoxError = Convert.ToSingle(TT2_textBoxError.Text);
            RoBotArm_test.Properties.Settings.Default.TT2_textBoxCorrect = TT2textBoxCorrect;
            RoBotArm_test.Properties.Settings.Default.TT2_textBoxError = TT2textBoxError;

            TTS1textBoxCorrect = Convert.ToSingle(TTS1_textBoxCorrect.Text);
            TTS1textBoxError = Convert.ToSingle(TTS1_textBoxError.Text);
            RoBotArm_test.Properties.Settings.Default.TTS1_textBoxCorrect = TTS1textBoxCorrect;
            RoBotArm_test.Properties.Settings.Default.TTS1_textBoxError = TTS1textBoxError;

            TTS2textBoxCorrect = Convert.ToSingle(TTS2_textBoxCorrect.Text);
            TTS2textBoxError = Convert.ToSingle(TTS2_textBoxError.Text);
            RoBotArm_test.Properties.Settings.Default.TTS2_textBoxCorrect = TTS2textBoxCorrect;
            RoBotArm_test.Properties.Settings.Default.TTS2_textBoxError = TTS2textBoxError;

            ST1textBoxCorrect = Convert.ToSingle(ST1_textBoxCorrect.Text);
            ST1textBoxError = Convert.ToSingle(ST1_textBoxError.Text);
            RoBotArm_test.Properties.Settings.Default.ST1_textBoxCorrect = ST1textBoxCorrect;
            RoBotArm_test.Properties.Settings.Default.ST1_textBoxError = ST1textBoxError;

            ST2textBoxCorrect = Convert.ToSingle(ST2_textBoxCorrect.Text);
            ST2textBoxError = Convert.ToSingle(ST2_textBoxError.Text);
            RoBotArm_test.Properties.Settings.Default.ST2_textBoxCorrect = ST2textBoxCorrect;
            RoBotArm_test.Properties.Settings.Default.ST2_textBoxError = ST2textBoxError;

            TTtexBoxtWidth = Convert.ToSingle(TT_Width.Text);
            TTtexBoxtError = Convert.ToSingle(TT_Error.Text);
            RoBotArm_test.Properties.Settings.Default.TT_Width = TTtexBoxtWidth;
            RoBotArm_test.Properties.Settings.Default.TT_Error = TTtexBoxtError;

            TTStexBoxtWidth = Convert.ToSingle(TTS_Width.Text);
            TTStexBoxtError = Convert.ToSingle(TTS_Error.Text);
            RoBotArm_test.Properties.Settings.Default.TTS_Width = TTStexBoxtWidth;
            RoBotArm_test.Properties.Settings.Default.TTS_Error = TTStexBoxtError;

            STtextBoxWidth = Convert.ToSingle(ST_Width.Text);
            STtextBoxError = Convert.ToSingle(ST_Error.Text);
            RoBotArm_test.Properties.Settings.Default.ST_Width = STtextBoxWidth;
            RoBotArm_test.Properties.Settings.Default.ST_Error = STtextBoxError;

            CenterPlanetexBoxError = Convert.ToSingle(Center_Plane_Error.Text);
            RoBotArm_test.Properties.Settings.Default.Center_Plane_Error = CenterPlanetexBoxError;

            RoBotArm_test.Properties.Settings.Default.Save();
            Close();
            */
        }
    }
}
