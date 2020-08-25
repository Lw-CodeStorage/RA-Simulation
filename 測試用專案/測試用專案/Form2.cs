using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.IO.Ports;
using System.Threading;

namespace 測試用專案
{
    public partial class Form2 : Form
    {
       
        public Form2()
        {
            InitializeComponent();
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Parent = this.pictureBox2;
            this.pictureBox2.BackColor = Color.Transparent;


        }


        private SerialPort comport;
        private Int32 totalLength = 0;
        private Thread t;
        delegate void Display(Byte[] buffer);

       

        private void Form2_Load(object sender, EventArgs e)
        {
            comport = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);
            comport.DataReceived += new SerialDataReceivedEventHandler(comport_DataReceived);
            if (!comport.IsOpen)
            {
                comport.Open();
            }

        }
        private void DisplayText(Byte[] buffer) {
            textBox1.Text += String.Format("{0}{1}", BitConverter.ToString(buffer), Environment.NewLine);
            totalLength = totalLength + buffer.Length;
            label1.Text = totalLength.ToString();
        }

        private void comport_DataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] buffer = new Byte[3];
            while (comport.BytesToRead < 19)
            {
                Thread.Sleep(16);
            }
            Int32 length = (sender as SerialPort).Read(buffer, 0, buffer.Length);
            Array.Resize(ref buffer, length);
            Display d = new Display(DisplayText);
            this.Invoke(d, new Object[] { buffer });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            
            pictureBox1.Image = CutImage(Properties.Resources._5, StartPiont, CutArea);

            
        }

        Point StartPiont = new Point (529, 353);
        Rectangle CutArea = new Rectangle(0,0,30,30);

        private Image CutImage(Image SourceImage, Point StartPoint, Rectangle CutArea)
        {
            Bitmap NewBitmap = new Bitmap(CutArea.Width, CutArea.Height);
            Graphics tmpGraph = Graphics.FromImage(NewBitmap);
            tmpGraph.DrawImage(SourceImage, CutArea, StartPoint.X, StartPoint.Y, CutArea.Width, CutArea.Height, GraphicsUnit.Pixel);
            tmpGraph.Dispose();
            return NewBitmap;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Create image.
            Image newImage = Properties.Resources.B1;

            // Create Point for upper-left corner of image.
            Point ulCorner = new Point(0, 0);

            // Draw image to screen.
            e.Graphics.DrawImage(newImage, ulCorner);
        }

        Size originalSize;   //**
        Point originalLoc;   //**
        int resize = 0;
        //區域放大
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (resize > 0 && pictureBox1.Width < originalSize.Width + 5)     //**
            {
                pictureBox1.Size = new Size(pictureBox1.Width + 2, pictureBox1.Height + 2);
                pictureBox1.Location = new Point(pictureBox1.Left - 1, pictureBox1.Top - 1);
            }
            else if (resize < 0 && pictureBox1.Width > originalSize.Width)    //**
            {
                pictureBox1.Size = new Size(pictureBox1.Width - 2, pictureBox1.Height - 2);
                pictureBox1.Location = new Point(pictureBox1.Left + 1, pictureBox1.Top + 1);
            }
            else timer1.Stop();
        }
        //區域放大
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            originalSize = pictureBox1.Size;   //**
            originalLoc = pictureBox1.Location;
            resize = 1;
            timer1.Interval = 10;
            timer1.Start();

            int x1 = 10;
            int y1 = 10;
            int x2 = 210;
            int y2 = 110;
            string text2 = "Use TextFormatFlags and Rectangle objects to"+ " center text in a rectangle.";
            Graphics g = pictureBox3.CreateGraphics();
            //g.Clear(Color.White);//清除
            using (Pen p = new Pen(Brushes.DeepSkyBlue,4))
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
        //區域放大
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            resize = -1;
            timer1.Start();
        }
        //區域放大
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            timer1.Stop();
            pictureBox1.Size = originalSize;
            pictureBox1.Location = originalLoc;
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            // Create a new pen.
            Pen skyBluePen = new Pen(Brushes.DeepSkyBlue);

            // Set the pen's width.
            skyBluePen.Width = 2.0F;

            // Set the LineJoin property.
            skyBluePen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            // Draw a rectangle.
            e.Graphics.DrawRectangle(skyBluePen,
                new Rectangle(40, 40, 40, 40));

            //Dispose of the pen.
            skyBluePen.Dispose();

            

        }

        

      
    }
}
