using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;

namespace 測試用專案
{
    public partial class Form1 : Form
    {
        string[] Axle = new string[10];
        //string c1_Axle_Center, c2_Axle_Center, d1_Axle_Center, d2_Axle_Center, b1_Axle_Center, b2_Axle_Center, f1_Axle_Center, f2_Axle_Center ;
        //string c1_Axle_r, c2_Axle_r, d1_Axle_r, d2_Axle_r, b1_Axle_r, b2_Axle_r, f1_Axle_r, f2_Axle_r;
        string bicycle_num = "A1302"; 
        string RxString ;

        string[] RxStringArray ={"0",
        "1,711.74,318.02,-320.2 ,711.74,307.61,-320.2 ,711.74,312.7 ,-315.2 ",
        "2,703.07,254.21,-118.61,703.07,238.28,-118.61,703.07,246.28,-110.79",
        "3,705.16,240.8 ,-305.24,705.16,231.8 ,-305.24,705.16,235.8 ,-301.24",
        "4,723.77,-210.14,177.77,723.77,-250.27,188.72,744.77,-231.14,183.47",
        "5,724.71,-236.57,66.8 ,724.71,-286.75,80.41,749.75,-262.54,73.76"};

         string[] xyzStringArray = new string[10];


         //byte[] buffer = new byte[1024];
        
         float Ax; float Ay; float Az; 

         float Bx; float By; float Bz;

         float Cx; float Cy; float Cz; 


         Coordinate_calculation bicycle;

        //Series series1 = new Series("temp", 500); //初始畫線條(名稱，最大值).
        //string[] a = new string[8] ;
        
        DataSet ds = new DataSet();

        Point[] red = {
            new Point(0,0),
            new Point(945, 376),
            new Point(875, 223) ,
            new Point(883, 372),
            new Point(467, 36),
            new Point(451, 107),
            new Point(16,1),
            new Point(6,57),
            new Point(363,312)
        };

        Rectangle CutArea;//切圖區域
        Point StartPiont;//切圖起始位置

        Mysql mysql;

        public Form1()
        {   
            InitializeComponent();
            point1_center.Text = "";
            point2_center.Text = "";
            point3_center.Text = "";
            point4_center.Text = "";
            point5_center.Text = "";

            point1_r.Text = "";
            point2_r.Text = "";
            point3_r.Text = "";
            point4_r.Text = "";
            point5_r.Text = "";


            //MessageBox.Show(testst);

            this.redPoint.Parent = this.bicycle_img;
            this.redPoint.BackColor = Color.Transparent;

            this.cutImage1.Parent = this.bicycle_img;
            this.cutImage1.BackColor = Color.Transparent;
            this.cutImage2.Parent = this.bicycle_img;
            this.cutImage2.BackColor = Color.Transparent;
            this.cutImage3.Parent = this.bicycle_img;
            this.cutImage3.BackColor = Color.Transparent;
            this.cutImage4.Parent = this.bicycle_img;
            this.cutImage4.BackColor = Color.Transparent;
            this.cutImage5.Parent = this.bicycle_img;
            this.cutImage5.BackColor = Color.Transparent;

            this.pictureBox1.Parent = this.bicycle_img;
            this.pictureBox1.BackColor = Color.Transparent;

        }

        private void serial1_Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPORT1.IsOpen) {

                    serialPORT1.Open();

                    serialPORT1.Write("connect_test"+"\r");
                    //timer1.Enabled = true;
                }
            }
            catch (UnauthorizedAccessException ex) {
                MessageBox.Show(ex.Message);
            }
        }


        /************************************************************************************************/

        //serialPortRead,DataReceived事件
        //接收資料
        private void serial1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {
                //serialPORT1.DiscardInBuffer();清空緩衝區
                Thread.Sleep(2000);
                   
                //讀取 SerialPort 物件的資料流
                Array.Clear(xyzStringArray, 0, xyzStringArray.Length);
                
                //int length = serialPORT1.Read(buffer,0, buffer.Length);
                //Array.Resize(ref buffer, length);

                //MessageBox.Show(buffer.Length.ToString());
                
                RxString = serialPORT1.ReadExisting();
                
                if (RxString=="connect_success"+"\r")
                {
                    MessageBox.Show("連接機械手臂成功!");
                }
                //MessageBox.Show(RxString);
                xyzStringArray = RxString.Split(',');

                this.Invoke(new EventHandler(s1_DataReceived));
                //serialPORT1.Write("DataReceived,GoTo Thread");
                //Thread.Sleep(250);
            }
            catch
            {
               
            }
        }
        private void s1_DataReceived(object s, EventArgs e)
        {

            //redPoint指Form1紅點照片物件
            //bicycle_img指Form1車架照片

             try
             {    
                 switch (xyzStringArray[0])
                 {
                     case "1":
                       
                        redPoint.Location = red[1];
                        redPoint.Image = Properties.Resources.紅點;
                        redPoint.Size = new Size(37, 37);
                         // StretchImage 值會將影像放大或縮小為PictureBox Size大小
                         redPoint.SizeMode = PictureBoxSizeMode.StretchImage;
                        
                         Ax = Convert.ToSingle(xyzStringArray[1]);
                         Ay = Convert.ToSingle(xyzStringArray[2]);
                         Az = Convert.ToSingle(xyzStringArray[3]);
                         Bx = Convert.ToSingle(xyzStringArray[4]);
                         By = Convert.ToSingle(xyzStringArray[5]);
                         Bz = Convert.ToSingle(xyzStringArray[6]);
                         Cx = Convert.ToSingle(xyzStringArray[7]);
                         Cy = Convert.ToSingle(xyzStringArray[8]);
                         Cz = Convert.ToSingle(xyzStringArray[9]);
                         

                        bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");

                        point1_r.Text = "半徑:"+bicycle.get_radius().ToString();
                        point1_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        //serialPORT1.DiscardInBuffer();
                        break;
                     case "2":
                         redPoint.Location = red[2];
                        redPoint.Image = Properties.Resources.紅點;
                         redPoint.Size = new Size(37, 37);
                         redPoint.SizeMode = PictureBoxSizeMode.StretchImage;
                        
                         Ax = Convert.ToSingle(xyzStringArray[1]);
                         Ay = Convert.ToSingle(xyzStringArray[2]);
                         Az = Convert.ToSingle(xyzStringArray[3]);
                         Bx = Convert.ToSingle(xyzStringArray[4]);
                         By = Convert.ToSingle(xyzStringArray[5]);
                         Bz = Convert.ToSingle(xyzStringArray[6]);
                         Cx = Convert.ToSingle(xyzStringArray[7]);
                         Cy = Convert.ToSingle(xyzStringArray[8]);
                         Cz = Convert.ToSingle(xyzStringArray[9]);
                         
                         bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                         readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                         readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                         readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");

                        point2_r.Text = bicycle.get_radius().ToString();
                        point2_center.Text = "(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        //serialPORT1.DiscardInBuffer();
                        break;
                     case "3":
                         redPoint.Location = red[3];
                        redPoint.Image = Properties.Resources.紅點;
                         redPoint.Size = new Size(37, 37);
                         redPoint.SizeMode = PictureBoxSizeMode.StretchImage;
                        
                         Ax = Convert.ToSingle(xyzStringArray[1]);
                         Ay = Convert.ToSingle(xyzStringArray[2]);
                         Az = Convert.ToSingle(xyzStringArray[3]);
                         Bx = Convert.ToSingle(xyzStringArray[4]);
                         By = Convert.ToSingle(xyzStringArray[5]);
                         Bz = Convert.ToSingle(xyzStringArray[6]);
                         Cx = Convert.ToSingle(xyzStringArray[7]);
                         Cy = Convert.ToSingle(xyzStringArray[8]);
                         Cz = Convert.ToSingle(xyzStringArray[9]);
                         
                         bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                         readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                         readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                         readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");

                        point3_r.Text = bicycle.get_radius().ToString();
                        point3_center.Text = "(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        //serialPORT1.DiscardInBuffer();
                        break;

                     case "4":
                         redPoint.Location = red[4];
                       redPoint.Image = Properties.Resources.紅點;
                         redPoint.Size = new Size(37, 37);
                         redPoint.SizeMode = PictureBoxSizeMode.StretchImage;
                        
                         Ax = Convert.ToSingle(xyzStringArray[1]);
                         Ay = Convert.ToSingle(xyzStringArray[2]);
                         Az = Convert.ToSingle(xyzStringArray[3]);
                         Bx = Convert.ToSingle(xyzStringArray[4]);
                         By = Convert.ToSingle(xyzStringArray[5]);
                         Bz = Convert.ToSingle(xyzStringArray[6]);
                         Cx = Convert.ToSingle(xyzStringArray[7]);
                         Cy = Convert.ToSingle(xyzStringArray[8]);
                         Cz = Convert.ToSingle(xyzStringArray[9]);
                         
                         bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                         readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                         readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                         readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");

                        point4_r.Text = bicycle.get_radius().ToString();
                        point4_center.Text = "(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        //serialPORT1.DiscardInBuffer();
                        break;
                     case "5":
                         redPoint.Location = red[5];
                      redPoint.Image = Properties.Resources.紅點;
                         redPoint.Size = new Size(37, 37);
                         redPoint.SizeMode = PictureBoxSizeMode.StretchImage;
                        
                         Ax = Convert.ToSingle(xyzStringArray[1]);
                         Ay = Convert.ToSingle(xyzStringArray[2]);
                         Az = Convert.ToSingle(xyzStringArray[3]);
                         Bx = Convert.ToSingle(xyzStringArray[4]);
                         By = Convert.ToSingle(xyzStringArray[5]);
                         Bz = Convert.ToSingle(xyzStringArray[6]);
                         Cx = Convert.ToSingle(xyzStringArray[7]);
                         Cy = Convert.ToSingle(xyzStringArray[8]);
                         Cz = Convert.ToSingle(xyzStringArray[9]);
                         
                         bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                         readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                         readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                         readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");

                        point5_r.Text = bicycle.get_radius().ToString();
                        point5_center.Text = "(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        //serialPORT1.DiscardInBuffer();
                        break;
                     case "6":
                         redPoint.Location = red[6];
                        redPoint.Image = Properties.Resources.紅點;
                         redPoint.Size = new Size(37, 37);
                         redPoint.SizeMode = PictureBoxSizeMode.StretchImage;

                         Ax = Convert.ToSingle(xyzStringArray[1]);
                         Ay = Convert.ToSingle(xyzStringArray[2]);
                         Az = Convert.ToSingle(xyzStringArray[3]);
                         Bx = Convert.ToSingle(xyzStringArray[4]);
                         By = Convert.ToSingle(xyzStringArray[5]);
                         Bz = Convert.ToSingle(xyzStringArray[6]);
                         Cx = Convert.ToSingle(xyzStringArray[7]);
                         Cy = Convert.ToSingle(xyzStringArray[8]);
                         Cz = Convert.ToSingle(xyzStringArray[9]);

                         bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                         readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                         readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                         readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");

                        //serialPORT1.DiscardInBuffer();
                        break;
                     case "7":
                         redPoint.Location = red[7];
                         redPoint.Image = Properties.Resources.紅點;
                         redPoint.Size = new Size(37, 37);
                         redPoint.SizeMode = PictureBoxSizeMode.StretchImage;

                         Ax = Convert.ToSingle(xyzStringArray[1]);
                         Ay = Convert.ToSingle(xyzStringArray[2]);
                         Az = Convert.ToSingle(xyzStringArray[3]);
                         Bx = Convert.ToSingle(xyzStringArray[4]);
                         By = Convert.ToSingle(xyzStringArray[5]);
                         Bz = Convert.ToSingle(xyzStringArray[6]);
                         Cx = Convert.ToSingle(xyzStringArray[7]);
                         Cy = Convert.ToSingle(xyzStringArray[8]);
                         Cz = Convert.ToSingle(xyzStringArray[9]);

                         bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                         readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                         readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                         readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");

                        //serialPORT1.DiscardInBuffer();
                        break;
                     case "8":

                        redPoint.Location = red[8];
                        redPoint.Image = Properties.Resources.紅點;
                        redPoint.Size = new Size(37, 37);
                        redPoint.SizeMode = PictureBoxSizeMode.StretchImage;

                        Ax = Convert.ToSingle(xyzStringArray[1]);
                        Ay = Convert.ToSingle(xyzStringArray[2]);
                        Az = Convert.ToSingle(xyzStringArray[3]);
                        Bx = Convert.ToSingle(xyzStringArray[4]);
                        By = Convert.ToSingle(xyzStringArray[5]);
                        Bz = Convert.ToSingle(xyzStringArray[6]);
                        Cx = Convert.ToSingle(xyzStringArray[7]);
                        Cy = Convert.ToSingle(xyzStringArray[8]);
                        Cz = Convert.ToSingle(xyzStringArray[9]);

                         bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                         readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                         readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                         readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");

                        //serialPORT1.DiscardInBuffer();
                        break;
                 }
             }
             catch (Exception epp)
             {
                 MessageBox.Show(epp.ToString());
             }

             
        }

        private void button5_Click(object sender, EventArgs e)//發送按鈕
        {
            if (serialPORT1.IsOpen)
            {
                serialPORT1.Write(writeTextbox.Text);
            }
        }
        private void button4_Click(object sender, EventArgs e)//清除按鈕
        {
            readTextBox.Text = "";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPORT1.Close();
            timer1.Enabled = false;
        }
        
        private void serialPortClose_Click(object sender, EventArgs e)
        {
            serialPORT1.Write("close"+"\r");
            serialPORT1.Close();
            timer1.Enabled = false;
            MessageBox.Show("手臂已離線");
        }

        private void survey_result_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i,j;
            i = 0;
            MySqlDataAdapter data;
            ds.Clear();
            data = mysql.mysql_Select();
            data.Fill(ds,"車架資料");
            j = ds.Tables[0].Rows.Count;
            if (i != j)
            {
                dataGridView1.DataSource = ds.Tables["車架資料"];
                i = j;

            }
        }
        private void timer_stop_btn_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }


        /********************************************************/
      
        // Unity3D序列控制
        //use key_down

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.S)
            {
                serialPORT1.Write("1");
            }
            else if (e.KeyCode == Keys.W)
            {
                serialPORT1.Write("2");
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
                serialPORT1.Write("3");
            }
            else if (e.KeyCode == Keys.A && e.Control)
            {
                serialPORT1.Write("4");
            }
            */
        }
        //區域螢幕截圖
        private Image CutImage(Image SourceImage, Point StartPoint, Rectangle CutArea)
        {
            Bitmap NewBitmap = new Bitmap(CutArea.Width, CutArea.Height);
            Graphics tmpGraph = Graphics.FromImage(NewBitmap);
            tmpGraph.DrawImage(SourceImage, CutArea, StartPoint.X, StartPoint.Y, CutArea.Width, CutArea.Height, GraphicsUnit.Pixel);
            tmpGraph.Dispose();
            return NewBitmap;
        }

        private void simulation_Click(object sender, EventArgs e)
        {
            mysql = new Mysql();
            this.redPoint.Parent = this.bicycle_img;
            this.redPoint.BackColor = Color.Transparent;
            for (int i = 1; i < RxStringArray.Length; i++)
            {
                xyzStringArray = RxStringArray[i].Split(',');
                redPoint.Location = red[i];
                redPoint.Image = Properties.Resources.紅點;
                redPoint.Size = new Size(37, 37);
                redPoint.SizeMode = PictureBoxSizeMode.StretchImage;
                Ax = Convert.ToSingle(xyzStringArray[1]);
                Ay = Convert.ToSingle(xyzStringArray[2]);
                Az = Convert.ToSingle(xyzStringArray[3]);
                Bx = Convert.ToSingle(xyzStringArray[4]);
                By = Convert.ToSingle(xyzStringArray[5]);
                Bz = Convert.ToSingle(xyzStringArray[6]);
                Cx = Convert.ToSingle(xyzStringArray[7]);
                Cy = Convert.ToSingle(xyzStringArray[8]);
                Cz = Convert.ToSingle(xyzStringArray[9]);

                bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
            
                
                switch (xyzStringArray[0])
                {
                    case "1":
                        point1_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point1_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //c1_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //c1_Axle_r = bicycle.get_radius().ToString();
                        Axle[0] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[1] = bicycle.get_radius().ToString();
                   
                        break;

                    case "2":
                        point2_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point2_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //d1_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //d1_Axle_r = bicycle.get_radius().ToString();
                        Axle[2] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[3] = bicycle.get_radius().ToString();
                        
                        break;

                    case "3":
                        point3_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point3_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //b1_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //b1_Axle_r = bicycle.get_radius().ToString();
                        Axle[4] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[5] = bicycle.get_radius().ToString();
    
                        break;

                    case "4":
                        point4_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point4_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //f1_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //f1_Axle_r = bicycle.get_radius().ToString();
                        Axle[6] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[7] = bicycle.get_radius().ToString();
                       
                        break;

                    case "5":
                        point5_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point5_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //f2_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //f2_Axle_r = bicycle.get_radius().ToString();
                        Axle[8] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[9] = bicycle.get_radius().ToString();
 
                        break;
                }

            }
            /*
            string q = "INSERT INTO a( `c1_Axle_Center`, `c1_Axle_r`, `d1_Axle_Center`, `d1_Axle_r`, `b1_Axle_Center`, `b1_Axle_r`, `f1_Axle_Center`, `f1_Axle_r`, `f2_Axle_Center`, `f2_Axle_r`, `c2_Axle_Center`, `c2_Axle_r`, `d2_Axle_Center`, `d2_Axle_r`, `b2_Axle_Center`, `b2_Axle_r`, `流水號`) " +
                "VALUES("+ c1_Axle_Center + c2_Axle_Center+ c1_Axle_r + c2_Axle_r + d1_Axle_Center + d2_Axle_Center + d1_Axle_r + d2_Axle_r + b1_Axle_Center + b2_Axle_Center + b1_Axle_r + b2_Axle_r + f1_Axle_Center+ f2_Axle_Center +f1_Axle_r +f2_Axle_r + bicycle_num+")";
            mysql.mysql_Insert(q);
            */
            
            mysql.mysql_Insert(Axle, bicycle_num);
           
            //清空Array
            Array.Clear(Axle, 0, Axle.Length);
        }

        private void cutImage1_MouseDown(object sender, MouseEventArgs e)
        {
            int x1 = 10;
            int y1 = 10;
            int x2 = 210;
            int y2 = 110;
            string text2 = "Use TextFormatFlags and Rectangle objects to" + " center text in a rectangle.";
            Graphics g = pictureBox1.CreateGraphics();
            //g.Clear(Color.White);//清除
            using (Pen p = new Pen(Brushes.DeepSkyBlue, 4))
            {
                g.DrawRectangle(p, x1, y1, x2, y2);

                using (Font font2 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect2 = new Rectangle(10, 10, 210, 110);

                    // Create a TextFormatFlags with word wrapping, horizontal center and
                    // vertical center specified.
                    TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;

                    // Draw the text and the surrounding rectangle.
                    TextRenderer.DrawText(g, text2, font2, rect2, Color.Blue, flags);

                }
            }
        }

        private void start_Click(object sender, EventArgs e)
        {    
            bicycle_num = this.bicycle_num_input.Text;
            mysql = new Mysql();
            this.redPoint.Parent = this.bicycle_img;
            this.redPoint.BackColor = Color.Transparent;
            for (int i = 1; i < RxStringArray.Length; i++)
            {
                xyzStringArray = RxStringArray[i].Split(',');
                redPoint.Location = red[i];
                redPoint.Image = Properties.Resources.紅點;
                redPoint.Size = new Size(37, 37);
                redPoint.SizeMode = PictureBoxSizeMode.StretchImage;
                Ax = Convert.ToSingle(xyzStringArray[1]);
                Ay = Convert.ToSingle(xyzStringArray[2]);
                Az = Convert.ToSingle(xyzStringArray[3]);
                Bx = Convert.ToSingle(xyzStringArray[4]);
                By = Convert.ToSingle(xyzStringArray[5]);
                Bz = Convert.ToSingle(xyzStringArray[6]);
                Cx = Convert.ToSingle(xyzStringArray[7]);
                Cy = Convert.ToSingle(xyzStringArray[8]);
                Cz = Convert.ToSingle(xyzStringArray[9]);

                bicycle = new Coordinate_calculation(Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz);
                Vector3 a = new Vector3(0.0f, -41.5f, 0.0f);
                Vector3 b = new Vector3(489.7286f, -5f, 468.5091f);
                double bbbb;
                bbbb = Math.Acos(bicycle.get_Vectical(a, b)) * 180 / Math.PI;//向量垂直度運算
                MessageBox.Show(bbbb.ToString());

                switch (xyzStringArray[0])
                {
                    case "1":
                        point1_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point1_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //c1_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //c1_Axle_r = bicycle.get_radius().ToString();
                        Axle[0] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[1] = bicycle.get_radius().ToString();

                        break;

                    case "2":
                        point2_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point2_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //d1_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //d1_Axle_r = bicycle.get_radius().ToString();
                        Axle[2] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[3] = bicycle.get_radius().ToString();

                        break;

                    case "3":
                        point3_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point3_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //b1_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //b1_Axle_r = bicycle.get_radius().ToString();
                        Axle[4] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[5] = bicycle.get_radius().ToString();

                        break;

                    case "4":
                        point4_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point4_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //f1_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //f1_Axle_r = bicycle.get_radius().ToString();
                        Axle[6] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[7] = bicycle.get_radius().ToString();

                        break;

                    case "5":
                        point5_r.Text = "半徑:" + bicycle.get_radius().ToString();
                        point5_center.Text = "圓心:(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")";
                        readTextBox.AppendText("量測點 :(" + xyzStringArray[0].ToString() + ")" + "\n");
                        readTextBox.AppendText("中心點座標 :(" + bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString() + ")" + "\n");
                        readTextBox.AppendText("半徑r : " + bicycle.get_radius().ToString() + "\n");
                        //f2_Axle_Center = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        //f2_Axle_r = bicycle.get_radius().ToString();
                        Axle[8] = bicycle.get_center().X.ToString() + "," + bicycle.get_center().Y.ToString() + "," + bicycle.get_center().Z.ToString();
                        Axle[9] = bicycle.get_radius().ToString();

                        break;
                }

            }
            /*
            string q = "INSERT INTO a( `c1_Axle_Center`, `c1_Axle_r`, `d1_Axle_Center`, `d1_Axle_r`, `b1_Axle_Center`, `b1_Axle_r`, `f1_Axle_Center`, `f1_Axle_r`, `f2_Axle_Center`, `f2_Axle_r`, `c2_Axle_Center`, `c2_Axle_r`, `d2_Axle_Center`, `d2_Axle_r`, `b2_Axle_Center`, `b2_Axle_r`, `流水號`) " +
                "VALUES("+ c1_Axle_Center + c2_Axle_Center+ c1_Axle_r + c2_Axle_r + d1_Axle_Center + d2_Axle_Center + d1_Axle_r + d2_Axle_r + b1_Axle_Center + b2_Axle_Center + b1_Axle_r + b2_Axle_r + f1_Axle_Center+ f2_Axle_Center +f1_Axle_r +f2_Axle_r + bicycle_num+")";
            mysql.mysql_Insert(q);
            */

            mysql.mysql_Insert(Axle, bicycle_num);

            //清空Array
            Array.Clear(Axle, 0, Axle.Length);
           
        }
    }
}
