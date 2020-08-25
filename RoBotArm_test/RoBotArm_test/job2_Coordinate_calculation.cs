using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace RoBotArm_test
{
    public  class job2_Coordinate_calculation
    {
        Vector3 AB, AC, N, Center;

        float Ax, Ay, Az, Ad = 1.0f;
        float Bx, By, Bz, Bd = 1.0f;
        float Cx, Cy, Cz, Cd = 1.0f;
        float Dx, Dy, Dz, Dd = 0f;

        float[] A = new float[6];
        float[] B = new float[6];
        float[] C = new float[6];
        float[] D = new float[6];

        float A_a, B_a, C_a, D_a;//等式答案
        float A_res, B_res, C_res, D_res;//聯立ABCD答案
        double A_val, B_val, C_val, D_val;//計算半徑、圓心所轉double

        double r;
        double distance;

        float delta, delta_1, delta_2, delta_3, delta_4;
        float deltaA, deltaA_1, deltaA_2, deltaA_3, deltaA_4;
        float deltaB, deltaB_1, deltaB_2, deltaB_3, deltaB_4;
        float deltaC, deltaC_1, deltaC_2, deltaC_3, deltaC_4;
        float deltaD, deltaD_1, deltaD_2, deltaD_3, deltaD_4;

        public job2_Coordinate_calculation(Vector3 a,Vector3 b,Vector3 c)
        {
            this.Ax = a.X;
            this.Ay = a.Y;
            this.Az = a.Z;

            this.Bx = b.X;
            this.By = b.Y;
            this.Bz = b.Z;

            this.Cx = c.X;
            this.Cy = c.Y;
            this.Cz = c.Z;

            AB = new Vector3(Bx - Ax, By - Ay, Bz - Az);
            AC = new Vector3(Cx - Ax, Cy - Ay, Cz - Az);
            N = Vector3.Cross(AB, AC);
            Dx = N.X; Dy = N.Y; Dz = N.Z;

            //補位
            A[0] = 0; B[0] = 0; C[0] = 0; D[0] = 0;

            //等式右邊答案
            this.A_a = -(Ax * Ax + Ay * Ay + Az * Az);
            this.B_a = -(Bx * Bx + By * By + Bz * Bz);
            this.C_a = -(Cx * Cx + Cy * Cy + Cz * Cz);
            this.D_a = -(Ax * N.X + Ay * N.Y + Az * N.Z);

            //公式
            A[1] = Ax; A[2] = Ay; A[3] = Az; A[4] = Ad; A[5] = A_a;
            B[1] = Bx; B[2] = By; B[3] = Bz; B[4] = Bd; B[5] = B_a;
            C[1] = Cx; C[2] = Cy; C[3] = Cz; C[4] = Cd; C[5] = C_a;
            D[1] = Dx / 2; D[2] = Dy / 2; D[3] = Dz / 2; D[4] = Dd; D[5] = D_a;

            //四階行列降階
            //+
            delta_1 = A[1] * (B[2] * C[3] * D[4] + B[3] * C[4] * D[2] + B[4] * C[2] * D[3] - B[4] * C[3] * D[2] - C[4] * D[3] * B[2] - B[3] * C[2] * D[4]);
            //-
            delta_2 = B[1] * (A[2] * C[3] * D[4] + A[3] * C[4] * D[2] + C[2] * D[3] * A[4] - A[4] * C[3] * D[2] - C[4] * D[3] * A[2] - A[3] * C[2] * D[4]);
            //+
            delta_3 = C[1] * (A[2] * B[3] * D[4] + A[3] * B[4] * D[2] + A[4] * B[2] * D[3] - D[2] * B[3] * A[4] - D[3] * B[4] * A[2] - D[4] * B[2] * A[3]);
            //-
            delta_4 = D[1] * (A[2] * B[3] * C[4] + A[3] * B[4] * C[2] + A[4] * B[2] * C[3] - C[2] * B[3] * A[4] - C[3] * B[4] * A[2] - C[4] * B[2] * A[3]);

            delta = delta_1 - delta_2 + delta_3 - delta_4;

            //+
            deltaA_1 = A[5] * (B[2] * C[3] * D[4] + B[3] * C[4] * D[2] + B[4] * C[2] * D[3] - D[2] * C[3] * B[4] - C[4] * D[3] * B[2] - C[2] * B[3] * D[4]);
            //-
            deltaA_2 = B[5] * (A[2] * C[3] * D[4] + A[3] * C[4] * D[2] + C[2] * D[3] * A[4] - A[4] * C[3] * D[2] - C[4] * D[3] * A[2] - A[3] * C[2] * D[4]);
            //+
            deltaA_3 = C[5] * (A[2] * B[3] * D[4] + A[3] * B[4] * D[2] + A[4] * B[2] * D[3] - D[2] * B[3] * A[4] - D[3] * B[4] * A[2] - D[4] * B[2] * A[3]);
            //-
            deltaA_4 = D[5] * (A[2] * B[3] * C[4] + A[3] * B[4] * C[2] + A[4] * B[2] * C[3] - C[2] * B[3] * A[4] - C[3] * B[4] * A[2] - C[4] * B[2] * A[3]);

            deltaA = deltaA_1 - deltaA_2 + deltaA_3 - deltaA_4;

            //+
            deltaB_1 = A[1] * (B[5] * C[3] * D[4] + B[3] * C[4] * D[5] + B[4] * C[5] * D[3] - D[5] * C[3] * B[4] - C[4] * D[3] * B[5] - C[5] * B[3] * D[4]);
            //-
            deltaB_2 = B[1] * (A[5] * C[3] * D[4] + A[3] * C[4] * D[5] + C[5] * D[3] * A[4] - A[4] * C[3] * D[5] - C[4] * D[3] * A[5] - A[3] * C[5] * D[4]);
            //+
            deltaB_3 = C[1] * (A[5] * B[3] * D[4] + A[3] * B[4] * D[5] + A[4] * B[5] * D[3] - D[5] * B[3] * A[4] - D[3] * B[4] * A[5] - D[4] * B[5] * A[3]);
            //-
            deltaB_4 = D[1] * (A[5] * B[3] * C[4] + A[3] * B[4] * C[5] + A[4] * B[5] * C[3] - C[5] * B[3] * A[4] - C[3] * B[4] * A[5] - C[4] * B[5] * A[3]);

            deltaB = deltaB_1 - deltaB_2 + deltaB_3 - deltaB_4;


            deltaC_1 = A[1] * (B[2] * C[5] * D[4] + B[5] * C[4] * D[2] + B[4] * C[2] * D[5] - D[2] * C[5] * B[4] - C[4] * D[5] * B[2] - C[2] * B[5] * D[4]);
            //-
            deltaC_2 = B[1] * (A[2] * C[5] * D[4] + A[5] * C[4] * D[2] + C[2] * D[5] * A[4] - A[4] * C[5] * D[2] - C[4] * D[5] * A[2] - A[5] * C[2] * D[4]);
            //+
            deltaC_3 = C[1] * (A[2] * B[5] * D[4] + A[5] * B[4] * D[2] + A[4] * B[2] * D[5] - D[2] * B[5] * A[4] - D[5] * B[4] * A[2] - D[4] * B[2] * A[5]);
            //-
            deltaC_4 = D[1] * (A[2] * B[5] * C[4] + A[5] * B[4] * C[2] + A[4] * B[2] * C[5] - C[2] * B[5] * A[4] - C[5] * B[4] * A[2] - C[4] * B[2] * A[5]);

            deltaC = deltaC_1 - deltaC_2 + deltaC_3 - deltaC_4;


            deltaD_1 = A[1] * (B[2] * C[3] * D[5] + B[3] * C[5] * D[2] + B[5] * C[2] * D[3] - B[5] * C[3] * D[2] - C[5] * D[3] * B[2] - B[3] * C[2] * D[5]);
            //-
            deltaD_2 = B[1] * (A[2] * C[3] * D[5] + A[3] * C[5] * D[2] + C[2] * D[3] * A[5] - A[5] * C[3] * D[2] - C[5] * D[3] * A[2] - A[3] * C[2] * D[5]);
            //+
            deltaD_3 = C[1] * (A[2] * B[3] * D[5] + A[3] * B[5] * D[2] + A[5] * B[2] * D[3] - D[2] * B[3] * A[5] - D[3] * B[5] * A[2] - D[5] * B[2] * A[3]);
            //-
            deltaD_4 = D[1] * (A[2] * B[3] * C[5] + A[3] * B[5] * C[2] + A[5] * B[2] * C[3] - C[2] * B[3] * A[5] - C[3] * B[5] * A[2] - C[5] * B[2] * A[3]);

            deltaD = deltaD_1 - deltaD_2 + deltaD_3 - deltaD_4;

            A_res = deltaA / delta;
            B_res = deltaB / delta;
            C_res = deltaC / delta;
            D_res = deltaD / delta;

            Center.X = A_res / -2;
            Center.Y = B_res / -2;
            Center.Z = C_res / -2;

            A_val = System.Convert.ToDouble(A_res);
            B_val = System.Convert.ToDouble(B_res);
            C_val = System.Convert.ToDouble(C_res);
            D_val = System.Convert.ToDouble(D_res);



            r = Math.Sqrt(Math.Pow(A_val, 2) + Math.Pow(B_val, 2) + Math.Pow(C_val, 2) - 4 * D_val) / 2;

        }
        
        //回傳孔徑圓心
        public Vector3 get_center() {
            return Center;
        }


        //使用Math.sqrt回傳double
        public double get_radius() {
            //1.5是紅寶石的半徑
            return this.r + 1.5f;
        }


        //兩中心之間距離D1~D4
        //與五通
        public double get_AxisDistance(Double x1, Double x2 , Double z1, Double z2) {
            double x, z;
            x = x1 - x2;
            z = z1 - z2;
            distance = Math.Sqrt(Math.Pow(x, 2)  + Math.Pow(z, 2));
            return distance;

        }

        double R;
        //取得直徑
        public double get_R() {
            R = (this.r + 1.5f)*2;
            return R ;
        }

        double Vectical;
         public double get_Vectical(Vector3 vec1, Vector3 vec2)
        {
            double v1, v2;
            v1 = Vector3.Dot(vec1, vec2);
            v2 = Math.Sqrt(Math.Pow(vec1.X, 2) + Math.Pow(vec1.Y, 2) + Math.Pow(vec1.Z, 2))* Math.Sqrt(Math.Pow(vec2.X, 2) + Math.Pow(vec2.Y, 2) + Math.Pow(vec2.Z, 2));
            Vectical = v1 / v2;
            return Vectical;
            /*
             * Vector3 a = new Vector3(0.0f, -41.5f, 0.0f);
                Vector3 b = new Vector3(489.7286f, -5f, 468.5091f);
                double bbbb;
                bbbb = Math.Acos(bicycle.get_Vectical(a, b)) * 180 / Math.PI;//向量垂直度運算
                MessageBox.Show(bbbb.ToString());*/
        }

        static public double[] get_CenterOpposite(Vector3 a,Vector3 b ) {
            double t1,t2;
            double[] CenterOpposite = new double[2];
            t1 = a.X - b.X;
            CenterOpposite[0] = t1;
            t2 = a.Z - b.Z;
            CenterOpposite[1] = t2;
            return CenterOpposite;
        }
    }
}
