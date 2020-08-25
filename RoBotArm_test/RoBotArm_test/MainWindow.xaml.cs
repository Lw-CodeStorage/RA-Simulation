using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using System.IO;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.Data;
using System.Numerics;
using System.Net.NetworkInformation;
using System.Management;

namespace RoBotArm_test
{
    class Joint
    {
        public Model3D model = null;
        public double angle = 0;//角度
        public double angleMin = -180;
        public double angleMax = 180;
        public int rotPointX = 0;
        public int rotPointY = 0;
        public int rotPointZ = 0;
        public int rotAxisX = 0;
        public int rotAxisY = 0;
        public int rotAxisZ = 0;

        public Joint(Model3D pModel)
        {
            model = pModel;
        }
    }
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
       
        Model3DGroup RA = new Model3DGroup(); //機器人手臂3d組
        Model3DGroup mechanism_Group = new Model3DGroup();

        Model3D geom = null; //調試球體以檢查關節旋轉的點

        List<Joint> joints = null;
        List<Joint> models = null;

        bool switchingJoint = false;
        bool isAnimating = false;

        Color oldColor = Colors.White;
        GeometryModel3D oldSelectedModel = null;
        string basePath = "";
        ModelVisual3D visual;
        double LearningRate = 0.01;
        double SamplingDistance = 0.15;
        double DistanceThreshold = 10;
        //為model3d對象提供渲染
        ModelVisual3D RoboticArm = new ModelVisual3D();
        ModelVisual3D mechanism = new ModelVisual3D();//機構
        Transform3DGroup F1;
        Transform3DGroup F2;
        Transform3DGroup F3;
        Transform3DGroup F4;
        Transform3DGroup F5;
        Transform3DGroup F6;
        RotateTransform3D R;
        TranslateTransform3D T;
        Vector3D reachingPoint;
        int movements = 10;
        System.Windows.Forms.Timer timer1;
        System.Windows.Forms.Timer timer2;
        System.Windows.Forms.Timer timer3;
        System.Windows.Forms.Timer Mysql_timer;
        private const string MODEL_PATH1 = "000_gp7 - 00_GP7_S_AXIS_ASM-1.STL";   
        private const string MODEL_PATH2 = "000_gp7 - 00_GP7_L_AXIS_ASM-1.STL";
        private const string MODEL_PATH3 = "000_gp7 - 00_GP7_U_AXIS_ASM-1.STL";
        private const string MODEL_PATH4 = "000_gp7 - 00_GP7_R_AXIS_ASM-1.STL";
        private const string MODEL_PATH5 = "000_gp7 - 00_GP8_B_AXIS_ASM-1.STL";
        private const string MODEL_PATH6 = "000_gp7 - 00_GP8_T_AXIS_ASM-1.STL";
        private const string MODEL_PATH7 = "000_gp7 - PH6-1.STL";
        private const string MODEL_PATH8 = "000_gp7 - 00_GP8_BASE_ASM-1.STL";
      
        private const string MODEL_PATH10 = "模擬車架(模擬).STL";
        private const string MODEL_PATH9 = "模擬固定(模擬).STL";
    
        public MainWindow()
        {  
            InitializeComponent();
            bicycle_id.Text = "190505";
            
            basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\GP70\\";
            List<string> modelsNames = new List<string>();
            List<string> mechanism_mode = new List<string>();

            modelsNames.Add(MODEL_PATH1);
            modelsNames.Add(MODEL_PATH2);            
            modelsNames.Add(MODEL_PATH3);
            modelsNames.Add(MODEL_PATH4);
            modelsNames.Add(MODEL_PATH5);
            modelsNames.Add(MODEL_PATH6);
            modelsNames.Add(MODEL_PATH7);
            modelsNames.Add(MODEL_PATH8);

            mechanism_mode.Add(MODEL_PATH9);
            mechanism_mode.Add(MODEL_PATH10);
            /*
            mechanism_mode.Add(MODEL_PATH11);
            mechanism_mode.Add(MODEL_PATH12);
            mechanism_mode.Add(MODEL_PATH13);
            */
            //ModelVisual3D
            RoboticArm.Content = Initialize_Environment(modelsNames);
            mechanism.Content = Initialize_mechanism_Environment(mechanism_mode);
            /** 調試球體以檢查關節旋轉的點**/
            var builder = new MeshBuilder(true, true);
            var position = new Point3D(0, 0, 0);
            builder.AddSphere(position, 10, 10, 10);//紅球大小
            geom = new GeometryModel3D(builder.ToMesh(), Materials.Brown);
            visual = new ModelVisual3D();
            visual.Content = geom;
            
            viewPort3d.RotateGesture = new MouseGesture(MouseAction.RightClick);
            viewPort3d.PanGesture = new MouseGesture(MouseAction.LeftClick);
            viewPort3d.Children.Add(visual);
            viewPort3d.Children.Add(RoboticArm);
            viewPort3d.Children.Add(mechanism);
            viewPort3d.Camera.LookDirection = new Vector3D(2038, -5200, -2930);
            viewPort3d.Camera.UpDirection = new Vector3D(-0.145, 0.372, 0.917);
            viewPort3d.Camera.Position = new Point3D(-1571, 4801, 3774);

            double[] angles = { joints[0].angle, joints[1].angle, joints[2].angle, joints[3].angle, joints[4].angle, joints[5].angle };
            ForwardKinematics(angles);

            changeSelectedJoint();
            //反向運動timer
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 5;
            timer1.Tick += new System.EventHandler(timer1_Tick);
            //讀取手臂資料timer
            timer2 = new System.Windows.Forms.Timer();
            timer2.Interval = 300;
            timer2.Tick += new System.EventHandler(timer2_Tick);

            //讀取手臂資料timer
            timer3 = new System.Windows.Forms.Timer();
            timer3.Interval = 500;
            timer3.Tick += new System.EventHandler(timer3_Tick);

            //資料庫timer
            /*
            Mysql_timer = new System.Windows.Forms.Timer();
            Mysql_timer.Interval = 1000;
            Mysql_timer.Tick += new System.EventHandler(Mysql_timer_Tick);
            */
        }
          private Model3DGroup Initialize_mechanism_Environment(List<string> mechanism_mode)
        {
            try
            {
                ModelImporter imp = new ModelImporter();
                models = new List<Joint>();

                foreach (string modelName in mechanism_mode)
                {
                    var materialGroup = new MaterialGroup();
                    Color mainColor = Colors.White;
                    EmissiveMaterial emissMat = new EmissiveMaterial(new SolidColorBrush(mainColor));
                    DiffuseMaterial diffMat = new DiffuseMaterial(new SolidColorBrush(mainColor));
                    SpecularMaterial specMat = new SpecularMaterial(new SolidColorBrush(mainColor), 200);
                    materialGroup.Children.Add(emissMat);
                    materialGroup.Children.Add(diffMat);
                    materialGroup.Children.Add(specMat);

                    var link = imp.Load(basePath + modelName);                 
                    GeometryModel3D model = link.Children[0] as GeometryModel3D;
                    model.Material = materialGroup;
                    model.BackMaterial = materialGroup;
                    models.Add(new Joint(link));
                }
                mechanism_Group.Children.Add(models[0].model);
                mechanism_Group.Children.Add(models[1].model);
                
                mechanism_Group.Children.Add(models[2].model);
                mechanism_Group.Children.Add(models[3].model);
                mechanism_Group.Children.Add(models[4].model);
                


            }
            catch (Exception e)
            {
                MessageBox.Show("Exception Error:" + e.StackTrace);
            }

            return mechanism_Group;
        }
        //環境初始化 
        //將載入所有stl模型
        //材質球套用
        private Model3DGroup Initialize_Environment(List<string> modelsNames)
        {
            try
            {
                ModelImporter import = new ModelImporter();
                joints = new List<Joint>();

                foreach (string modelName in modelsNames)
                {
                    var materialGroup = new MaterialGroup();
                    Color mainColor = Colors.DodgerBlue;
                    EmissiveMaterial emissMat = new EmissiveMaterial(new SolidColorBrush(mainColor));
                    DiffuseMaterial diffMat = new DiffuseMaterial(new SolidColorBrush(mainColor));
                    SpecularMaterial specMat = new SpecularMaterial(new SolidColorBrush(mainColor), 200);
                    materialGroup.Children.Add(emissMat);
                    materialGroup.Children.Add(diffMat);
                    materialGroup.Children.Add(specMat);

                    var link = import.Load(basePath + modelName);
                    GeometryModel3D model = link.Children[0] as GeometryModel3D;
                    model.Material = materialGroup;
                    model.BackMaterial = materialGroup;
                    joints.Add(new Joint(link));
                }
                RA.Children.Add(joints[0].model);
                RA.Children.Add(joints[1].model);
                RA.Children.Add(joints[2].model);
                RA.Children.Add(joints[3].model);
                RA.Children.Add(joints[4].model);
                RA.Children.Add(joints[5].model);
                RA.Children.Add(joints[6].model);
                RA.Children.Add(joints[7].model);
                /*
                RA.Children.Add(joints[7].model);
                RA.Children.Add(joints[8].model);
                RA.Children.Add(joints[9].model);
                RA.Children.Add(joints[10].model);
                RA.Children.Add(joints[11].model);
               */




                joints[0].angleMin = -170;
                joints[0].angleMax = 170;
                joints[0].rotAxisX = 0;
                joints[0].rotAxisY = 0;
                joints[0].rotAxisZ = 1;
                joints[0].rotPointX = 0;
                joints[0].rotPointY = 0;
                joints[0].rotPointZ = 0;

                joints[1].angleMin = -65;
                joints[1].angleMax = 145;
                joints[1].rotAxisX = 0;
                joints[1].rotAxisY = 1;
                joints[1].rotAxisZ = 0;
                joints[1].rotPointX = 39;
                joints[1].rotPointY = 149;
                joints[1].rotPointZ = 328;

                joints[2].angleMin = 116;
                joints[2].angleMax = -255;
                joints[2].rotAxisX = 0;
                joints[2].rotAxisY = -1;//U軸接手臂值時可能需要加負號
                joints[2].rotAxisZ = 0;
                joints[2].rotPointX = 40;
                joints[2].rotPointY = 160;
                joints[2].rotPointZ = 776;

                joints[3].angleMin = -190;
                joints[3].angleMax = 190;
                joints[3].rotAxisX = -1;
                joints[3].rotAxisY = 0;
                joints[3].rotAxisZ = 0;
                joints[3].rotPointX = 565;
                joints[3].rotPointY = -7;
                joints[3].rotPointZ = 815;

                joints[4].angleMin = -135;
                joints[4].angleMax = 135;
                joints[4].rotAxisX = 0;
                joints[4].rotAxisY = -1;
                joints[4].rotAxisZ = 0;
                joints[4].rotPointX = 480;
                joints[4].rotPointY = -7;
                joints[4].rotPointZ = 815;

                joints[5].angleMin = -360;
                joints[5].angleMax = 360;
                joints[5].rotAxisX = 1;
                joints[5].rotAxisY = 0;
                joints[5].rotAxisZ = 0;
                joints[5].rotPointX = 711;
                joints[5].rotPointY = -7;
                joints[5].rotPointZ = 815;

               
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception Error:" + e.StackTrace);
            }

            return RA;
        }

        public static T Clamp<T>(T value, T min, T max)
                  where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }
        //輸入框，給的紅球座標
        private void ReachingPoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                reachingPoint = new Vector3D(Double.Parse(TbX.Text), Double.Parse(TbY.Text), Double.Parse(TbZ.Text));
                
                geom.Transform = new TranslateTransform3D(reachingPoint);
            }
            catch (Exception exc)
            {

            }
        }
        private void jointSelector_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            changeSelectedJoint();
        }

        private void changeSelectedJoint()
        {
            if (joints == null)
                return;

            int sel = ((int)jointSelector.Value) - 1;
            switchingJoint = true;
            unselectModel();
            if (sel < 0)
            {
                jointX.IsEnabled = false;
                jointY.IsEnabled = false;
                jointZ.IsEnabled = false;
                jointXAxis.IsEnabled = false;
                jointYAxis.IsEnabled = false;
                jointZAxis.IsEnabled = false;
            }
            else
            {
                if (!jointX.IsEnabled)
                {
                    jointX.IsEnabled = true;
                    jointY.IsEnabled = true;
                    jointZ.IsEnabled = true;
                    jointXAxis.IsEnabled = true;
                    jointYAxis.IsEnabled = true;
                    jointZAxis.IsEnabled = true;
                }
                jointX.Value = joints[sel].rotPointX;
                jointY.Value = joints[sel].rotPointY;
                jointZ.Value = joints[sel].rotPointZ;
                jointXAxis.IsChecked = joints[sel].rotAxisX == 1 ? true : false;
                jointYAxis.IsChecked = joints[sel].rotAxisY == 1 ? true : false;
                jointZAxis.IsChecked = joints[sel].rotAxisZ == 1 ? true : false;
                selectModel(joints[sel].model);
                updateSpherePosition();
            }
            switchingJoint = false;
        }
        private void CheckBox_StateChanged(object sender, RoutedEventArgs e)
        {
            if (switchingJoint)
                return;

            int sel = ((int)jointSelector.Value) - 1;
            joints[sel].rotAxisX = jointXAxis.IsChecked.Value ? 1 : 0;
            joints[sel].rotAxisY = jointYAxis.IsChecked.Value ? 1 : 0;
            joints[sel].rotAxisZ = jointZAxis.IsChecked.Value ? 1 : 0;
        }

        private void updateSpherePosition()
        {
            int sel = ((int)jointSelector.Value) - 1;
            if (sel < 0)
                return;

            Transform3DGroup F = new Transform3DGroup();
            F.Children.Add(new TranslateTransform3D(joints[sel].rotPointX, joints[sel].rotPointY, joints[sel].rotPointZ));
            F.Children.Add(joints[sel].model.Transform);
            geom.Transform = F;
        }


        private void execute_fk()
        {
             //調試球體，它採用textBoxes的x，y，z並更新其位置
             //在定義新的RotateTransform3D（）時，在新的Point3D（x，y，z）*中使用x，y，z時，這非常有用，以檢查關節實際旋轉的位置
            double[] angles = { joints[0].angle, joints[1].angle, joints[2].angle, joints[3].angle, joints[4].angle, joints[5].angle };
            ForwardKinematics(angles);//正向運動學
            updateSpherePosition();//更新球位置
        }
        
        private void joint_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isAnimating)
                return;

            joints[0].angle = joint1.Value;
            joints[1].angle = joint2.Value;
            joints[2].angle = joint3.Value;
            joints[3].angle = joint4.Value;
            joints[4].angle = joint5.Value;
            joints[5].angle = joint6.Value;
            execute_fk();
        }

        private void rotationPointChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (switchingJoint)
                return;

            int sel = ((int)jointSelector.Value) - 1;
            joints[sel].rotPointX = (int)jointX.Value;
            joints[sel].rotPointY = (int)jointY.Value;
            joints[sel].rotPointZ = (int)jointZ.Value;
            updateSpherePosition();
        }

        private Color changeModelColor(Joint pJoint, Color newColor)
        {
            Model3DGroup models = ((Model3DGroup)pJoint.model);
            return changeModelColor(models.Children[0] as GeometryModel3D, newColor);
        }

        private Color changeModelColor(GeometryModel3D pModel, Color newColor)
        {
            if (pModel == null)
                return oldColor;

            Color previousColor = Colors.Black;

            MaterialGroup mg = (MaterialGroup)pModel.Material;
            if (mg.Children.Count > 0)
            {
                try
                {
                    previousColor = ((EmissiveMaterial)mg.Children[0]).Color;
                    ((EmissiveMaterial)mg.Children[0]).Color = newColor;
                    ((DiffuseMaterial)mg.Children[1]).Color = newColor;
                }
                catch (Exception exc)
                {
                    previousColor = oldColor;
                }
            }

            return previousColor;
        }


        private void selectModel(Model3D pModel)
        {
            try
            {
                Model3DGroup models = ((Model3DGroup)pModel);
                oldSelectedModel = models.Children[0] as GeometryModel3D;
            }
            catch (Exception exc)
            {
                oldSelectedModel = (GeometryModel3D)pModel;
            }
            oldColor = changeModelColor(oldSelectedModel, ColorHelper.HexToColor("#ff3333"));
        }

        private void unselectModel()
        {
            changeModelColor(oldSelectedModel, oldColor);
        }

        private void ViewPort3D_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(viewPort3d);
            PointHitTestParameters hitParams = new PointHitTestParameters(mousePos);
            VisualTreeHelper.HitTest(viewPort3d, null, ResultCallback, hitParams);
        }

        private void ViewPort3D_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Perform the hit test on the mouse's position relative to the viewport.
            HitTestResult result = VisualTreeHelper.HitTest(viewPort3d, e.GetPosition(viewPort3d));
            RayMeshGeometry3DHitTestResult mesh_result = result as RayMeshGeometry3DHitTestResult;

            if (oldSelectedModel != null)
                unselectModel();

            if (mesh_result != null)
            {
                selectModel(mesh_result.ModelHit);
            }
        }

        public HitTestResultBehavior ResultCallback(HitTestResult result)
        {
            // Did we hit 3D?
            RayHitTestResult rayResult = result as RayHitTestResult;
            if (rayResult != null)
            {
                // Did we hit a MeshGeometry3D?
                RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;
                geom.Transform = new TranslateTransform3D(new Vector3D(rayResult.PointHit.X, rayResult.PointHit.Y, rayResult.PointHit.Z));

                if (rayMeshResult != null)
                {
                    // Yes we did!
                }
            }

            return HitTestResultBehavior.Continue;
        }

        //開始逆向運動按鈕
        public void StartInverseKinematics(object sender, RoutedEventArgs e)
        {
            if (timer1.Enabled)
            {
                button.Content = "Go to position";
                isAnimating = false;
                timer1.Stop();
                movements = 0;
            }
            else
            {
                geom.Transform = new TranslateTransform3D(reachingPoint);//將球位移
                movements = 5000;
                button.Content = "STOP";
                isAnimating = true;
                timer1.Start();
            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            double[] angles = { joints[0].angle, joints[1].angle, joints[2].angle, joints[3].angle, joints[4].angle, joints[5].angle };//手臂個軸預設角度
            angles = InverseKinematics(reachingPoint, angles);//(紅球點,預設角度)
            //改變滑桿的質
            joint1.Value = joints[0].angle = angles[0];
            joint2.Value = joints[1].angle = angles[1];
            joint3.Value = joints[2].angle = angles[2];
            joint4.Value = joints[3].angle = angles[3];
            joint5.Value = joints[4].angle = angles[4];
            joint6.Value = joints[5].angle = angles[5];

            if ((--movements) <= 0)
            {
                button.Content = "Go to position";
                isAnimating = false;
                timer1.Stop();
            }
        }

        public double[] InverseKinematics(Vector3D target, double[] angles)
        {   //到達目標的距離
            if (DistanceFromTarget(target, angles) < DistanceThreshold)
            {
                movements = 0;
                return angles;
            }

            double[] oldAngles = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            angles.CopyTo(oldAngles, 0);
            for (int i = 0; i <= 5; i++)
            {
                // 梯度下降
                // 更新：解決方案 -  = LearningRate * Gradient
                double gradient = PartialGradient(target, angles, i);
                angles[i] -= LearningRate * gradient;

                // Clamp
                angles[i] = Clamp(angles[i], joints[i].angleMin, joints[i].angleMax);

                // 提前終止
                if (DistanceFromTarget(target, angles) < DistanceThreshold || checkAngles(oldAngles, angles))
                {
                    movements = 0;
                    return angles;
                }
            }
            return angles;
        }

        //到達目標的距離
        public double DistanceFromTarget(Vector3D target, double[] angles)
        {
            Vector3D point = ForwardKinematics(angles);////正向運動，手臂目前正在觸摸三維空間的哪個點!!!
            return Math.Sqrt(Math.Pow((point.X - target.X), 2.0) + Math.Pow((point.Y - target.Y), 2.0) + Math.Pow((point.Z - target.Z), 2.0));//回傳離目標多遠
        }

        public bool checkAngles(double[] oldAngles, double[] angles)
        {
            for (int i = 0; i <= 5; i++)
            {
                if (oldAngles[i] != angles[i])
                    return false;
            }

            return true;
        }

        //部分梯度
        public double PartialGradient(Vector3D target, double[] angles, int i)
        {
            // 保存角度
            
            double angle = angles[i];

            // Gradient : [F(x+SamplingDistance) - F(x)] / h
            double f_x = DistanceFromTarget(target, angles);

            angles[i] += SamplingDistance;
            double f_x_plus_d = DistanceFromTarget(target, angles);

            double gradient = (f_x_plus_d - f_x) / SamplingDistance;

            // Restores
            angles[i] = angle;

            return gradient;
        }

       
        //正向運動，手臂目前正在觸摸三維空間的哪個點!!!
        public Vector3D ForwardKinematics(double[] angles)
        {
            //angles[]是取得手臂新傳入的角度
            F1 = new Transform3DGroup();
            R = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(joints[0].rotAxisX, joints[0].rotAxisY, joints[0].rotAxisZ), angles[0]), new Point3D(joints[0].rotPointX, joints[0].rotPointY, joints[0].rotPointZ));
            F1.Children.Add(R);

            //angles[1] = 0.0f - angles[1];
            F2 = new Transform3DGroup();
            T = new TranslateTransform3D(0, 0, 0);
            R = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(joints[1].rotAxisX, joints[1].rotAxisY, joints[1].rotAxisZ), angles[1]), new Point3D(joints[1].rotPointX, joints[1].rotPointY, joints[1].rotPointZ));
            F2.Children.Add(T);
            F2.Children.Add(R);
            F2.Children.Add(F1);


            F3 = new Transform3DGroup();
            T = new TranslateTransform3D(0, 0, 0);
            R = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(joints[2].rotAxisX, joints[2].rotAxisY, joints[2].rotAxisZ), angles[2]), new Point3D(joints[2].rotPointX, joints[2].rotPointY, joints[2].rotPointZ));
            F3.Children.Add(T);
            F3.Children.Add(R);
            F3.Children.Add(F2);

           
            F4 = new Transform3DGroup();
            T = new TranslateTransform3D(0, 0, 0);
            R = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(joints[3].rotAxisX, joints[3].rotAxisY, joints[3].rotAxisZ), angles[3]), new Point3D(joints[3].rotPointX, joints[3].rotPointY, joints[3].rotPointZ));
            F4.Children.Add(T);
            F4.Children.Add(R);
            F4.Children.Add(F3);
    
            F5 = new Transform3DGroup();
            T = new TranslateTransform3D(0, 0, 0);
            R = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(joints[4].rotAxisX, joints[4].rotAxisY, joints[4].rotAxisZ), angles[4]), new Point3D(joints[4].rotPointX, joints[4].rotPointY, joints[4].rotPointZ));
            F5.Children.Add(T);
            F5.Children.Add(R);
            F5.Children.Add(F4);


          
            //注意：我正在做一場噩夢試圖理解為什麼它總是以一種奇怪的方式旋轉......所以我意識到它的順序
            //你添加孩子實際上在我應用F然後是T和R之前是非常重要的，但是之前的轉換
            //應始終作為最後一個應用（FORWARD Kinematics）
            F6 = new Transform3DGroup();
            T = new TranslateTransform3D(0, 0, 0);
            R = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(joints[5].rotAxisX, joints[5].rotAxisY, joints[5].rotAxisZ), angles[5]), new Point3D(joints[5].rotPointX, joints[5].rotPointY, joints[5].rotPointZ));
            F6.Children.Add(T);
            F6.Children.Add(R);
            F6.Children.Add(F5);


            joints[0].model.Transform = F1; //第一次關節
            joints[1].model.Transform = F2; //第二關節（“二頭肌”）
            joints[2].model.Transform = F3; //第三關節（“膝蓋”或“肘部”）
            joints[3].model.Transform = F4; 
            joints[4].model.Transform = F5; 
            joints[5].model.Transform = F6; 
            
            Tx.Content = joints[6].model.Bounds.Location.X;
            Ty.Content = joints[6].model.Bounds.Location.Y;
            Tz.Content = joints[6].model.Bounds.Location.Z;
            Tx_Copy.Content = geom.Bounds.Location.X;
            Ty_Copy.Content = geom.Bounds.Location.Y;
            Tz_Copy.Content = geom.Bounds.Location.Z;

            joints[6].model.Transform = F6;//tp20
           
            return new Vector3D(joints[5].model.Bounds.Location.X, joints[5].model.Bounds.Location.Y, joints[5].model.Bounds.Location.Z);
         
        }
        //接收腳踏車資訊
        /********************************************************************************************************************************************************************************************************/
        YRC YRC = new YRC();
        Mysql mysql = new Mysql();
        int startStatus =0;
        int stopStatus = 0;
        double tt1_R, tts1_R, st1_R, tt1_Wy, tts1_Wy, st1_Wy;
        double tt2_R, tts2_R, st2_R, tt2_Wy, tts2_Wy, st2_Wy;
        float centerPlane = -38.891f;//y
        float[,] getposbase1_res2D, getposbase2_res2D;
        float[] Axis_Angle;
        job1_Coordinate_calculation job1_culation;
        job2_Coordinate_calculation tt1, tts1, st1, tt2, tts2, st2;
        DataTable d;
        Vector3 c11, c12, c13, c14, c21, c22, c23, c24;
        int num = 1;
        private void z(object sender, KeyEventArgs e)
        {

            switch (num)
            {
                case 1:
                    YRC_Info_Textbox.Text = "Frame Center : -333.5";
                    num += 1;
                    break;
                case 2:
                    YRC_Info_Textbox.Text = "T/T rotating shaft inside diameter : 15 mm";
                    num += 1;
                    break;
                case 3:
                    YRC_Info_Textbox.Text = "B.B. rotating shaft and H/T :θ = 90";
                    num += 1;
                    break;
                case 4:
                    YRC_Info_Textbox.Text = "B.B. rotating shaft and T/T rotating shaft :θ = 0?";
                    num += 1;
                    break;
                default:
                    num = 0;
                    YRC_Info_Textbox.Text = "";
                    break;
            }
        }

        double Insert_tmp;
        double width;
        double angle;



        private void YRC_Connect_Btn_Click(object sender, RoutedEventArgs e)
        {
            
            YRC.YRC_On();
        }

        private void YRC_Disconnect_Btn_Click(object sender, RoutedEventArgs e)
        {
            YRC.YRC_Off();
            timer2.Stop();
        }


        //讀取手臂資料timer
        private void timer3_Tick(object sender, EventArgs e)
        {
            stopStatus = YRC.checkQueryFinish();
            Axis_Angle = YRC.get_Axis_Angle();

            //動畫執行手臂
            joints[0].angle = Axis_Angle[0];
            joints[1].angle = Axis_Angle[1];
            joints[2].angle = Axis_Angle[2];
            joints[3].angle = Axis_Angle[3];
            joints[4].angle = Axis_Angle[4];
            joints[5].angle = Axis_Angle[5];
            Arm();

            if (stopStatus.ToString() == "1")
            { timer3.Stop();
                int pass;
                pass = job1_TT();
                pass += job1_TTS();
                pass += job1_ST();
                
                MessageBoxResult mes;
                if (pass == 9)
                {
                    MessageBox.Show("ALL_PASS");

                }
                else {
                    mes = MessageBox.Show("There is an error in the initial measurement,if want to continue measure please press the OK button", "Confirm Message", MessageBoxButton.OKCancel);
                    if (mes == MessageBoxResult.OK)
                    {
                        MessageBox.Show("call JOB2");
                        YRC.select_job2();
                        timer2.Start();
                    }
                    else {

                        MessageBox.Show(pass.ToString());
                    }
                }
            }
        }

        //讀取手臂資料timer
        private void timer2_Tick(object sender, EventArgs e)
        {

            stopStatus = YRC.checkQueryFinish();
            Axis_Angle = YRC.get_Axis_Angle();
       
            //動畫執行手臂
            joints[0].angle = Axis_Angle[0];
            joints[1].angle = Axis_Angle[1];
            joints[2].angle = Axis_Angle[2];
            joints[3].angle = Axis_Angle[3];
            joints[4].angle = Axis_Angle[4];
            joints[5].angle = Axis_Angle[5];
            Arm();

            //若為2，job2量測結束中止timer 開始計算
            if (stopStatus.ToString() == "2")
            {
                job2_TT();
                job2_TTS();
                job2_ST();
            }
        }
        
        private void Arm() {
            double[] angles = { joints[0].angle, joints[1].angle, joints[2].angle, joints[3].angle, joints[4].angle, joints[5].angle };//取得修改後的質
            ForwardKinematics(angles);//正向運動學
        }         
        
        private void Window_Closed(object sender, EventArgs e)
        {
            YRC.YRC_Off();
        }

       
        DataSet ds = new DataSet();

        private void settingClick(object sender, MouseButtonEventArgs e)
        {
            Window window1 = new Window1();
            window1.Show();
        }

        

        private void Start_job(object sender, RoutedEventArgs e)
        {

            if (bicycle_id.Text != "")
            {
                YRC.select_job1();
                timer3.Start();
            }
            else
            {
                MessageBox.Show("請輸入車架號碼");
                bicycle_id.Text = "job123";
                YRC.select_job2();
                timer2.Start();
            }
        }


        private void Search_result_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt;
            string resultid;
            resultid = search_result_id.Text;
            dt = mysql.mysql_Select_SearchResult(resultid);
            try
            {
                if (dt.Rows[0][1].ToString() == resultid || dt.Rows[0][1].ToString() != null)
                {
                    Window resultPage = new resultPage();
                    resultPage.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No Search Result!");
            }

        }
        int pass_TT;
        private int job1_TT() {

            d = mysql.mysql_Select_ForSetting("Error_Setting");
            getposbase1_res2D = YRC.getposbase1();

            c11.X = getposbase1_res2D[73, 0]; c11.Y = getposbase1_res2D[73, 1]; c11.Z = getposbase1_res2D[73, 2];
            c12.X = getposbase1_res2D[70, 0]; c12.Y = getposbase1_res2D[70, 1]; c12.Z = getposbase1_res2D[70, 2];
            c13.X = getposbase1_res2D[71, 0]; c13.Y = getposbase1_res2D[71, 1]; c13.Z = getposbase1_res2D[71, 2];
            c14.X = getposbase1_res2D[72, 0]; c14.Y = getposbase1_res2D[72, 1]; c14.Z = getposbase1_res2D[72, 2];

            c21.X = getposbase1_res2D[85, 0]; c21.Y = getposbase1_res2D[85, 1]; c21.Z = getposbase1_res2D[85, 2];
            c22.X = getposbase1_res2D[82, 0]; c22.Y = getposbase1_res2D[82, 1]; c22.Z = getposbase1_res2D[82, 2];
            c23.X = getposbase1_res2D[83, 0]; c23.Y = getposbase1_res2D[83, 1]; c23.Z = getposbase1_res2D[83, 2];
            c24.X = getposbase1_res2D[84, 0]; c24.Y = getposbase1_res2D[84, 1]; c24.Z = getposbase1_res2D[84, 2];

            //確認兩孔面使否平行 回傳平行角度
            job1_culation = new job1_Coordinate_calculation(c11, c12, c13, c21, c22, c23);
            //確認兩孔面使否平行 回傳4個長度

            angle = job1_culation.get_angle();
            if (angle < Convert.ToDouble(d.Rows[0][20]))
            {
                //YRC_Info_Textbox.Text += "TT外孔面角度:" + job1_culation.get_angle().ToString() + "度  (OK)\r\n";
                // mysql.mysql_Insert(bicycle_id.Text, "TT外孔面角度", angle.ToString(), "Go");
                YRC_Info_Textbox.Text += "TT Axis Angle Error:" + job1_culation.get_angle().ToString() + " degree  (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT Axis Angle Error", angle.ToString(), "Go");
                pass_TT++;
            }
            else
            {
                //YRC_Info_Textbox.Text += "TT外孔面角度:" + job1_culation.get_angle().ToString() + "度 (Fail)\r\n ";
                //mysql.mysql_Insert(bicycle_id.Text, "TT外孔面角度", angle.ToString(), "NGo");
                YRC_Info_Textbox.Text += "TT Axis Angle Error:" + job1_culation.get_angle().ToString() + " degree (Fail)\r\n ";
                mysql.mysql_Insert(bicycle_id.Text, "TT Axis Angle Error", angle.ToString(), "NGo");
            }

            width = job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3];

            width = width / 4;

            if (width < Convert.ToDouble(d.Rows[0][5]) + Convert.ToDouble(d.Rows[0][6]) && width > Convert.ToDouble(d.Rows[0][5]) - Convert.ToDouble(d.Rows[0][6]))
            {  /*
                YRC_Info_Textbox.Text += "TT寬度(平均):" + width + "OK\r\n";
                YRC_Info_Textbox.Text += "TT寬度A:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + "\r\n";
                YRC_Info_Textbox.Text += "TT寬度B:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + "\r\n";
                YRC_Info_Textbox.Text += "TT寬度C:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + "\r\n";
                YRC_Info_Textbox.Text += "TT寬度D:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT寬度", width.ToString(), "GO");
                */
                YRC_Info_Textbox.Text += "TT Width(average):" + width + "mm OK\r\n";
                YRC_Info_Textbox.Text += "TT WidthA:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + " mm\r\n";
                YRC_Info_Textbox.Text += "TT WidthB:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + " mm\r\n";
                YRC_Info_Textbox.Text += "TT WidthC:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + " mm\r\n";
                YRC_Info_Textbox.Text += "TT WidthD:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + " mm\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT Width", width.ToString(), "GO");
                pass_TT++;
            }
            else
            {
                YRC_Info_Textbox.Text += "TT Width(average):" + width + "mm OK\r\n";
                YRC_Info_Textbox.Text += "TT WidthA:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + " mm\r\n";
                YRC_Info_Textbox.Text += "TT WidthB:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + " mm\r\n";
                YRC_Info_Textbox.Text += "TT WidthC:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + " mm\r\n";
                YRC_Info_Textbox.Text += "TT WidthD:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + " mm\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT Width", width.ToString(), "NGO");
                /*
                YRC_Info_Textbox.Text += "TT寬度(平均):" + width + "Fail\r\n";
                YRC_Info_Textbox.Text += "TT寬度A:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + "\r\n";
                YRC_Info_Textbox.Text += "TT寬度B:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + "\r\n";
                YRC_Info_Textbox.Text += "TT寬度C:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + "\r\n";
                YRC_Info_Textbox.Text += "TT寬度D:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT寬度", width.ToString(), "NGO");*/
            }

            if ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane < Convert.ToDouble(d.Rows[0][19]) + centerPlane && (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane > Convert.ToDouble(d.Rows[0][19]) - centerPlane)
            {/*
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "TT中心面偏移量:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(OK!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT中心面偏移量", Insert_tmp.ToString(), "GO");
                */
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "TT Center plane offset:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(OK!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT Center plane offset", Insert_tmp.ToString(), "GO");
                pass_TT++;
            }
            else
            {/*
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "TT中心面偏移量:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(Fail!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT中心面偏移量", Insert_tmp.ToString(), "NGO");*/
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "TT Center plane offset:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(Fail!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT Center plane offset", Insert_tmp.ToString(), "NGO");
            }
            return pass_TT;

        }
        int pass_TTS; 
        private int  job1_TTS()
        {

            d = mysql.mysql_Select_ForSetting("Error_Setting");
            getposbase1_res2D = YRC.getposbase1();

            c11.X = getposbase1_res2D[77, 0]; c11.Y = getposbase1_res2D[77, 1]; c11.Z = getposbase1_res2D[77, 2];
            c12.X = getposbase1_res2D[74, 0]; c12.Y = getposbase1_res2D[74, 1]; c12.Z = getposbase1_res2D[74, 2];
            c13.X = getposbase1_res2D[75, 0]; c13.Y = getposbase1_res2D[75, 1]; c13.Z = getposbase1_res2D[75, 2];
            c14.X = getposbase1_res2D[76, 0]; c14.Y = getposbase1_res2D[76, 1]; c14.Z = getposbase1_res2D[76, 2];

            c21.X = getposbase1_res2D[89, 0]; c21.Y = getposbase1_res2D[89, 1]; c21.Z = getposbase1_res2D[89, 2];
            c22.X = getposbase1_res2D[86, 0]; c22.Y = getposbase1_res2D[86, 1]; c22.Z = getposbase1_res2D[86, 2];
            c23.X = getposbase1_res2D[87, 0]; c23.Y = getposbase1_res2D[87, 1]; c23.Z = getposbase1_res2D[87, 2];
            c24.X = getposbase1_res2D[88, 0]; c24.Y = getposbase1_res2D[88, 1]; c24.Z = getposbase1_res2D[88, 2];

            //確認兩孔面使否平行 回傳平行角度
            job1_culation = new job1_Coordinate_calculation(c11, c12, c13, c21, c22, c23);
            angle = job1_culation.get_angle();
            if (angle < Convert.ToDouble(d.Rows[0][20]))
            {/*
                YRC_Info_Textbox.Text += "TTS外孔面角度:" + angle.ToString() + "度  (OK)\r\n)";
                mysql.mysql_Insert(bicycle_id.Text, "TTS外孔面角度", angle.ToString(), "Go");*/
                YRC_Info_Textbox.Text += "TTS Axis Angle Error:" + angle.ToString() + "degree  (OK)\r\n)";
                mysql.mysql_Insert(bicycle_id.Text, "TTS Axis Angle Error", angle.ToString(), "Go"); 
                pass_TTS++;
            }
            else
            {
                /*
                YRC_Info_Textbox.Text += "TTS外孔面角度:" +angle.ToString() + "度  (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS外孔面角度", angle.ToString(), "NGo");*/
                YRC_Info_Textbox.Text += "TTS Axis Angle Error:" + angle.ToString() + "degree  (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS Axis Angle Error", angle.ToString(), "NGo");

            }

            //確認兩孔面使否平行 回傳4個長度

            width = job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3];

            width = width / 4;

            if (width < Convert.ToDouble(d.Rows[0][17]) + Convert.ToDouble(d.Rows[0][18]) && width > Convert.ToDouble(d.Rows[0][17]) - Convert.ToDouble(d.Rows[0][18]))
            {
                YRC_Info_Textbox.Text += "TTS Width(average):" + width + " mm OK\r\n";
                YRC_Info_Textbox.Text += "TTS WidthA:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + " mm\r\n";
                YRC_Info_Textbox.Text += "TTS WidthB:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + " mm\r\n";
                YRC_Info_Textbox.Text += "TTS WidthC:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + " mm\r\n";
                YRC_Info_Textbox.Text += "TTS WidthD:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + " mm\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS Width", width.ToString(), "GO");
                /*
                YRC_Info_Textbox.Text += "TTS寬度(平均):" + width + " OK\r\n";
                YRC_Info_Textbox.Text += "TTS寬度A:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + "\r\n";
                YRC_Info_Textbox.Text += "TTS寬度B:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + "\r\n";
                YRC_Info_Textbox.Text += "TTS寬度C:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + "\r\n";
                YRC_Info_Textbox.Text += "TTS寬度D:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS寬度", width.ToString(), "GO");*/
                pass_TTS++;
            }
            else
            {/*
                YRC_Info_Textbox.Text += "TTS寬度(平均):" + width + " Fail\r\n";
                YRC_Info_Textbox.Text += "TTS寬度A:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + "\r\n";
                YRC_Info_Textbox.Text += "TTS寬度B:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + "\r\n";
                YRC_Info_Textbox.Text += "TTS寬度C:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + "\r\n";
                YRC_Info_Textbox.Text += "TTS寬度D:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS寬度", width.ToString(), "NGO");*/
                YRC_Info_Textbox.Text += "TTS Width(average):" + width + " mm OK\r\n";
                YRC_Info_Textbox.Text += "TTS WidthA:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + " mm\r\n";
                YRC_Info_Textbox.Text += "TTS WidthB:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + " mm\r\n";
                YRC_Info_Textbox.Text += "TTS WidthC:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + " mm\r\n";
                YRC_Info_Textbox.Text += "TTS WidthD:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + " mm\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS Width", width.ToString(), "NGO");

            }

            if ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane < Convert.ToDouble(d.Rows[0][19]) + centerPlane && (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane > Convert.ToDouble(d.Rows[0][19]) - centerPlane)
            {/*
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "TTS中心面偏移量:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(OK!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS中心面偏移量", Insert_tmp.ToString(), "GO");*/
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "TTS Center plane offset:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(OK!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS Center plane offset", Insert_tmp.ToString(), "GO");
                pass_TTS++;
            }
            else
            {/*
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "TTS中心面偏移量:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(Fail!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS中心面偏移量", Insert_tmp.ToString(), "NGO");*/
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "TTS Center plane offset:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(Fail!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS Center plane offset", Insert_tmp.ToString(), "NGO");
            }
            return pass_TTS;
        }
        int pass_ST;
        private int job1_ST() {
            
            d = mysql.mysql_Select_ForSetting("Error_Setting");
            getposbase1_res2D = YRC.getposbase1();

            c11.X = getposbase1_res2D[81, 0]; c11.Y = getposbase1_res2D[81, 1]; c11.Z = getposbase1_res2D[81, 2];
            c12.X = getposbase1_res2D[78, 0]; c12.Y = getposbase1_res2D[78, 1]; c12.Z = getposbase1_res2D[78, 2];
            c13.X = getposbase1_res2D[79, 0]; c13.Y = getposbase1_res2D[79, 1]; c13.Z = getposbase1_res2D[79, 2];
            c14.X = getposbase1_res2D[80, 0]; c14.Y = getposbase1_res2D[80, 1]; c14.Z = getposbase1_res2D[80, 2];

            c21.X = getposbase1_res2D[93, 0]; c21.Y = getposbase1_res2D[93, 1]; c21.Z = getposbase1_res2D[93, 2];
            c22.X = getposbase1_res2D[90, 0]; c22.Y = getposbase1_res2D[90, 1]; c22.Z = getposbase1_res2D[90, 2];
            c23.X = getposbase1_res2D[91, 0]; c23.Y = getposbase1_res2D[91, 1]; c23.Z = getposbase1_res2D[91, 2];
            c24.X = getposbase1_res2D[92, 0]; c24.Y = getposbase1_res2D[92, 1]; c24.Z = getposbase1_res2D[92, 2];

            //確認兩孔面使否平行 回傳平行角度

            job1_culation = new job1_Coordinate_calculation(c11, c12, c13, c21, c22, c23);
            angle = job1_culation.get_angle();
            //確認兩孔面使否平行 回傳4個長度
           
            if (angle < Convert.ToDouble(d.Rows[0][20]))
            {
               /*
                YRC_Info_Textbox.Text += "ST外孔面角度:" + angle.ToString() + "度  (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST外孔面角度", angle.ToString(), "Go");*/
                YRC_Info_Textbox.Text += "ST Axis Angle Error:" + angle.ToString() + "degree  (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST Axis Angle Error", angle.ToString(), "Go");
                pass_ST++; 
            }
            else
            {/*
                YRC_Info_Textbox.Text += "ST外孔面角度:" + angle.ToString() + "度  (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST外孔面角度", angle.ToString(), "NGo");*/
                YRC_Info_Textbox.Text += "ST Axis Angle Error:" + angle.ToString() + "degree  (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST Axis Angle Error", angle.ToString(), "NGo");

            }

            width = job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] +
                    job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3];

            width = width / 4;

            if (width < Convert.ToDouble(d.Rows[0][11]) + Convert.ToDouble(d.Rows[0][12]) && width > Convert.ToDouble(d.Rows[0][11]) - Convert.ToDouble(d.Rows[0][12]))
            {
                YRC_Info_Textbox.Text += "ST Width(average):" + width + " mm OK\r\n";
                YRC_Info_Textbox.Text += "ST WidthA:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + " mm\r\n";
                YRC_Info_Textbox.Text += "ST WidthB:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + " mm\r\n";
                YRC_Info_Textbox.Text += "ST WidthC:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + " mm\r\n";
                YRC_Info_Textbox.Text += "ST WidthD:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + " mm\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST Width", width.ToString(), "GO");
                pass_ST++;
            }
            else
            {
                YRC_Info_Textbox.Text += "ST Width(average):" + width + " Fail\r\n";
                YRC_Info_Textbox.Text += "ST WidthA:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[0] + "\r\n";
                YRC_Info_Textbox.Text += "ST WidthB:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[1] + "\r\n";
                YRC_Info_Textbox.Text += "ST WidthC:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[2] + "\r\n";
                YRC_Info_Textbox.Text += "ST WidthD:" + job1_culation.AxisAreaParallel_Check(c11, c12, c13, c14, c21, c22, c23, c24)[3] + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST Width", width.ToString(), "NGO");
            }

            if ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane < Convert.ToDouble(d.Rows[0][19]) + centerPlane && (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane > Convert.ToDouble(d.Rows[0][19]) - centerPlane)
            {
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "ST Center plane offset:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(OK!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST Center plane offset", Insert_tmp.ToString(), "GO");
                pass_ST++;
            }
            else
            {
                Insert_tmp = (job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane;
                YRC_Info_Textbox.Text += "ST Center plane offset:" + ((job1_culation.center1.Y + job1_culation.center2.Y) / 2 - centerPlane).ToString() + "mm" + "(Fail!)" + "\r\n" + "\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST Center plane offset", Insert_tmp.ToString(), "NGO");
            }

            return pass_ST;
        }
        private void job2_TT()
        {

            d = mysql.mysql_Select_ForSetting("Error_Setting");
            getposbase2_res2D = YRC.getposbase2();

            c11.X = getposbase2_res2D[101, 0]; c11.Y = getposbase2_res2D[101, 1]; c11.Z = getposbase2_res2D[101, 2];
            c12.X = getposbase2_res2D[102, 0]; c12.Y = getposbase2_res2D[102, 1]; c12.Z = getposbase2_res2D[102, 2];
            c13.X = getposbase2_res2D[103, 0]; c13.Y = getposbase2_res2D[103, 1]; c13.Z = getposbase2_res2D[103, 2];
            

            c21.X = getposbase2_res2D[110, 0]; c21.Y = getposbase2_res2D[110, 1]; c21.Z = getposbase2_res2D[110, 2];
            c22.X = getposbase2_res2D[111, 0]; c22.Y = getposbase2_res2D[111, 1]; c22.Z = getposbase2_res2D[111, 2];
            c23.X = getposbase2_res2D[112, 0]; c23.Y = getposbase2_res2D[112, 1]; c23.Z = getposbase2_res2D[112, 2];

            tt1 = new job2_Coordinate_calculation(c11, c12, c13);
            tt2 = new job2_Coordinate_calculation(c21, c22, c23);
            tt1_R = tt1.get_R();
            tt2_R = tt2.get_R();
            if (tt1_R < Convert.ToDouble(d.Rows[0][1])+ Convert.ToDouble(d.Rows[0][2]) && tt1_R < Convert.ToDouble(d.Rows[0][1]) - Convert.ToDouble(d.Rows[0][2]))
            {
                YRC_Info_Textbox.Text += "TT1 Diameter :" + tt1_R.ToString() + " mm (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT1 Diameter", tt1_R.ToString(), "Go"); 
            }
            else
            {
                YRC_Info_Textbox.Text += "TT1 Diameter:" + tt1_R.ToString() + " mm (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT1 Diameter", tt1_R.ToString(), "NGo");
            }

            if (tt2_R < Convert.ToDouble(d.Rows[0][3])+ Convert.ToDouble(d.Rows[0][4]) && tt2_R < Convert.ToDouble(d.Rows[0][3]) - Convert.ToDouble(d.Rows[0][4]))
            {
                YRC_Info_Textbox.Text += "TT2 Diameter:" + tt2_R.ToString() + " mm (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT2 Diameter", tt2_R.ToString(), "Go");
            }
            else
            {
                YRC_Info_Textbox.Text += "TT2 Diameter:" + tt2_R.ToString() + " mm (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TT2 Diameter", tt2_R.ToString(), "NGo");
            }

            YRC_Info_Textbox.Text += "TT Center of opposite:" + job2_Coordinate_calculation.get_CenterOpposite(tt1.get_center(),tt2.get_center())[0].ToString() + "\r\n";
            YRC_Info_Textbox.Text += "TT Center of opposite:" + job2_Coordinate_calculation.get_CenterOpposite(tt1.get_center(), tt2.get_center())[1].ToString() + "\r\n";
        }
        private void job2_TTS()
        {

            d = mysql.mysql_Select_ForSetting("Error_Setting");
            getposbase2_res2D = YRC.getposbase2();

            c11.X = getposbase2_res2D[104, 0]; c11.Y = getposbase2_res2D[104, 1]; c11.Z = getposbase2_res2D[104, 2];
            c12.X = getposbase2_res2D[105, 0]; c12.Y = getposbase2_res2D[105, 1]; c12.Z = getposbase2_res2D[105, 2];
            c13.X = getposbase2_res2D[106, 0]; c13.Y = getposbase2_res2D[106, 1]; c13.Z = getposbase2_res2D[106, 2];


            c21.X = getposbase2_res2D[113, 0]; c21.Y = getposbase2_res2D[113, 1]; c21.Z = getposbase2_res2D[113, 2];
            c22.X = getposbase2_res2D[114, 0]; c22.Y = getposbase2_res2D[114, 1]; c22.Z = getposbase2_res2D[114, 2];
            c23.X = getposbase2_res2D[115, 0]; c23.Y = getposbase2_res2D[115, 1]; c23.Z = getposbase2_res2D[115, 2];

            tts1 = new job2_Coordinate_calculation(c11, c12, c13);
            tts2 = new job2_Coordinate_calculation(c21, c22, c23);
            tts1_R = tts1.get_R();
            tts2_R = tts2.get_R();
            if (tts1_R < Convert.ToDouble(d.Rows[0][13]) + Convert.ToDouble(d.Rows[0][14]) && tts1_R < Convert.ToDouble(d.Rows[0][13]) - Convert.ToDouble(d.Rows[0][14]))
            {
                YRC_Info_Textbox.Text += "TTS1 Diameter:" + tts1_R.ToString() + " mm (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS1 Diameter", tts1_R.ToString(), "Go");
            }
            else
            {
                YRC_Info_Textbox.Text += "TTS1 Diameter:" + tts1_R.ToString() + " mm (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS1 Diameter", tts1_R.ToString(), "NGo");
            }

            if (tts2_R < Convert.ToDouble(d.Rows[0][15]) + Convert.ToDouble(d.Rows[0][16]) && tts2_R < Convert.ToDouble(d.Rows[0][15]) - Convert.ToDouble(d.Rows[0][16]))
            {
                YRC_Info_Textbox.Text += "TTS2 Diameter:" + tts2_R.ToString() + " mm (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS2 Diameter", tts2_R.ToString(), "Go");
            }
            else
            {
                YRC_Info_Textbox.Text += "TTS2 Diameter:" + tts2_R.ToString() + " mm (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "TTS2 Diameter", tts2_R.ToString(), "NGo");
            }

            YRC_Info_Textbox.Text += "TTS Center of opposite:" + job2_Coordinate_calculation.get_CenterOpposite(tts1.get_center(), tts2.get_center())[0].ToString() + "\r\n";
            YRC_Info_Textbox.Text += "TTS Center of opposite:" + job2_Coordinate_calculation.get_CenterOpposite(tts1.get_center(), tts2.get_center())[1].ToString() + "\r\n";
        }
        private void job2_ST()
        {

            d = mysql.mysql_Select_ForSetting("Error_Setting");
            getposbase2_res2D = YRC.getposbase2();

            c11.X = getposbase2_res2D[107, 0]; c11.Y = getposbase2_res2D[107, 1]; c11.Z = getposbase2_res2D[107, 2];
            c12.X = getposbase2_res2D[108, 0]; c12.Y = getposbase2_res2D[108, 1]; c12.Z = getposbase2_res2D[108, 2];
            c13.X = getposbase2_res2D[109, 0]; c13.Y = getposbase2_res2D[109, 1]; c13.Z = getposbase2_res2D[109, 2];


            c21.X = getposbase2_res2D[116, 0]; c21.Y = getposbase2_res2D[116, 1]; c21.Z = getposbase2_res2D[116, 2];
            c22.X = getposbase2_res2D[117, 0]; c22.Y = getposbase2_res2D[117, 1]; c22.Z = getposbase2_res2D[117, 2];
            c23.X = getposbase2_res2D[118, 0]; c23.Y = getposbase2_res2D[118, 1]; c23.Z = getposbase2_res2D[118, 2];

            st1 = new job2_Coordinate_calculation(c11, c12, c13);
            st2= new job2_Coordinate_calculation(c21, c22, c23);
            st1_R = st1.get_R();
            st2_R = st2.get_R();
            if (st1_R < Convert.ToDouble(d.Rows[0][7]) + Convert.ToDouble(d.Rows[0][8]) && st1_R < Convert.ToDouble(d.Rows[0][7]) - Convert.ToDouble(d.Rows[0][8]))
            {
                YRC_Info_Textbox.Text += "ST1 Diameter:" + st1_R.ToString() + " mm (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST1 Diameter", st1_R.ToString(), "Go");
            }
            else
            {
                YRC_Info_Textbox.Text += "ST1 Diameter:" + st1_R.ToString() + " mm (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST1 Diameter", st1_R.ToString(), "NGo");
            }

            if (st2_R < Convert.ToDouble(d.Rows[0][9]) + Convert.ToDouble(d.Rows[0][10]) && st2_R < Convert.ToDouble(d.Rows[0][9]) - Convert.ToDouble(d.Rows[0][10]))
            {
                YRC_Info_Textbox.Text += "ST2 Diameter:" + st2_R.ToString() + " mm (OK)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST2 Diameter", st2_R.ToString(), "Go");
            }
            else
            {
                YRC_Info_Textbox.Text += "ST2 Diameter:" + st2_R.ToString() + " mm (Fail)\r\n";
                mysql.mysql_Insert(bicycle_id.Text, "ST2 Diameter", st2_R.ToString(), "NGo");
            }

        
            YRC_Info_Textbox.Text += "ST Center of opposite:" + job2_Coordinate_calculation.get_CenterOpposite(st1.get_center(), st2.get_center())[0].ToString() + "\r\n";
            YRC_Info_Textbox.Text += "ST Center of opposite:" + job2_Coordinate_calculation.get_CenterOpposite(st1.get_center(), st2.get_center())[1].ToString() + "\r\n";
        }
    }

}
