using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FESIFS;
using System.Windows.Forms;

namespace RoBotArm_test
{
    class YRC
    {
        string IP = "192.168.10.10";
        int Port = 10040;
        short[] err_code = new short[2];
        private FesIF FESIF = new FesIF();
        string job1 = "START_CENTER";
        string job2 = "START_RADIUS";

        float[] SLURBT = new float[] {
        0.000859163f,
        0.000693696426f,
        0.0008791046088f,
        0.001173440713f,
        0.001375978474f,
        0.002150023889f };


        public YRC()
        {

        }
        public string GetErr(short[] err_code)
        {
            string err_log = "\r\n《err_code》= " + err_code[0] + " , " + err_code[1];
            return err_log;
        }
        public void YRC_On()
        {
            try
            {
                FESIF.Open(IP, Port);
                FESIF.ServoSwitch(1, err_code);

                MessageBox.Show("Robot Connect Success !" + GetErr(err_code));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        public void YRC_Off()
        {
            try
            {
                FESIF.Open(IP, Port);
                FESIF.ServoSwitch(2, err_code);
                MessageBox.Show("Robot Disconnect !" + GetErr(err_code));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void select_job1()
        {
            SelectJob select_job = new SelectJob();
            select_job.name = job1;
            select_job.line = 0;
            FESIF.JobSelect(1, select_job, err_code);
            MessageBox.Show("call:job1" + GetErr(err_code));
            FESIF.JobStart(err_code);
        }
        public void select_job2()
        {
            SelectJob select_job = new SelectJob();
            select_job.name = job2;
            select_job.line =0;
            FESIF.JobSelect(1, select_job, err_code);
            MessageBox.Show("call:job2" + GetErr(err_code));
            FESIF.JobStart(err_code);
        }

        public float[] get_Axis_Angle()
        {

            float[] Axis_Angle = new float[6];
            PosData nowpos = new PosData();
            FESIF.RobPosSnglR(1, ref nowpos, err_code);
            //Console.WriteLine(nowpos.axis[0]);

            for (int i = 0; i < 5; i++)
            {
                Axis_Angle[i] = nowpos.axis[i] * SLURBT[i];

            }

            return Axis_Angle;
            Array.Clear(Axis_Angle, 0, Axis_Angle.Length);

        }

        int status;
        public int checkQueryFinish() {
            PosData nowpos = new PosData();
           
            nowpos.type = 0;
            nowpos.pattern = 0;
            nowpos.tool_no = 0;
            nowpos.user_coord_no = 0;
            nowpos.ex_pattern = 0;
            FESIF.PosSnglR(100, ref nowpos, err_code);
            status = nowpos.axis[0] / 1000;
            
            return status;
        }
        public float[,] getposbase1() {
            PosData nowpos = new PosData();
            nowpos.type = 0;
            nowpos.pattern = 0;
            nowpos.tool_no = 0;
            nowpos.user_coord_no = 0;
            nowpos.ex_pattern = 0;

            float[,] Base_Position2D = new float[127, 127];
            for (short i = 70; i < 94; i++)
            {
                FESIF.PosSnglR(i, ref nowpos, err_code);
                for (int j = 0; j < 3; j++)
                {
                    Base_Position2D[i, j] = Convert.ToSingle(nowpos.axis[j]) / 1000;//為了取mm
                }

            }
            return Base_Position2D;
        }
        //機器手臂目前位置讀出(基座)
        public float[,] getposbase2()
        {
            //string[] R  = new string[4] {"","","",""}; 
            //float[] Base_Position = new float[48];
            PosData nowpos = new PosData();
            nowpos.type = 0;
            nowpos.pattern = 0;
            nowpos.tool_no = 0;
            nowpos.user_coord_no = 0;
            nowpos.ex_pattern = 0;
           
            float[,] Base_Position2D = new float[127, 127];
            for (short i = 101; i < 125; i++)
            {   
                FESIF.PosSnglR(i, ref nowpos, err_code);
                for (int j = 0; j <3 ; j++)
                {
                    Base_Position2D[i,j]  = Convert.ToSingle(nowpos.axis[j]) / 1000;//為了取mm
                }
           
            }
            return Base_Position2D;
            
        }
    }
}
