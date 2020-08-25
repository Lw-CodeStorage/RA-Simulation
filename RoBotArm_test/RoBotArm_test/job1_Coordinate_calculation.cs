using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RoBotArm_test
{
    class job1_Coordinate_calculation
    {
        public Vector3 center1 ,center2;
        double angle;
        public job1_Coordinate_calculation(Vector3 c11, Vector3 c12, Vector3 c13, Vector3 c21, Vector3 c22, Vector3 c23)
        {
            
            Vector3 center1to11, center1to12, center2to21, center2to22;
            Vector3 n1, n2;
           
            double a, b,x;
            center1.X = (c11.X + c13.X) / 2;
            center1.Y = (c11.Y + c13.Y) / 2;
            center1.Z = (c11.Z + c13.Z) / 2;

            center2.X = (c21.X + c23.X) / 2;
            center2.Y = (c21.Y + c23.Y) / 2;
            center2.Z = (c21.Z + c23.Z) / 2;

            center1to11 = new Vector3(c11.X - center1.X, c11.Y - center1.Y, c11.Z - center1.Z);
            center1to12 = new Vector3(c12.X - center1.X, c12.Y - center1.Y, c12.Z - center1.Z);

            center2to21 = new Vector3(c21.X - center2.X, c21.Y - center2.Y, c21.Z - center2.Z);
            center2to22 = new Vector3(c22.X - center2.X, c22.Y - center2.Y, c22.Z - center2.Z);

            n1.X = (center1to11.Y * center1to12.Z) - (center1to11.Z * center1to12.Y);
            n1.Y = (center1to11.X * center1to12.Z) - (center1to11.Z * center1to12.X);
            n1.Z = (center1to11.X * center1to12.Y) - (center1to11.Y * center1to12.X);

            n2.X = (center2to21.Y * center2to22.Z) - (center2to21.Z * center2to22.Y);
            n2.Y = (center2to21.X * center2to22.Z) - (center2to21.Z * center2to22.X);
            n2.Z = (center2to21.X * center2to22.Y) - (center2to21.Y * center2to22.X);

            a = Math.Sqrt(Math.Pow(n1.X, 2) + Math.Pow(n1.Y, 2) + Math.Pow(n1.Z, 2));
            b = Math.Sqrt(Math.Pow(n2.X, 2) + Math.Pow(n2.Y, 2) + Math.Pow(n2.Z, 2));
            MessageBox.Show(a.ToString());
            MessageBox.Show(b.ToString());
            x = Vector3.Dot(n1, n2) ;
            MessageBox.Show(x.ToString());
            x = x / (a * b);
            angle = Math.Acos(x);
            //MessageBox.Show(angle.ToString());
            angle = angle * 180f;
            angle = angle / Math.PI;
            MessageBox.Show(angle.ToString());
        }

        public double[] AxisAreaParallel_Check(Vector3 A, Vector3 B, Vector3 C, Vector3 D, Vector3 E, Vector3 F, Vector3 G, Vector3 H)
        {
            double[] distance = new double[4];

            distance[0] = Vector3.Distance(A, E)-3;//扣掉兩個紅球半徑\\
            distance[1] = Vector3.Distance(B, F)-3;
            distance[2] = Vector3.Distance(C, G)-3;
            distance[3] = Vector3.Distance(D, H)-3;
            return distance;
        }
        public double  get_angle() {

            return angle;
        }


    }
}
