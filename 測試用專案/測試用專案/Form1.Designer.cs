namespace 測試用專案
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPORT1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.readTextBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.simulation = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.writeTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer_stop_btn = new System.Windows.Forms.Button();
            this.survey_result_btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.point1_r = new System.Windows.Forms.Label();
            this.point1_center = new System.Windows.Forms.Label();
            this.point2_r = new System.Windows.Forms.Label();
            this.point2_center = new System.Windows.Forms.Label();
            this.point3_r = new System.Windows.Forms.Label();
            this.point3_center = new System.Windows.Forms.Label();
            this.point4_r = new System.Windows.Forms.Label();
            this.point4_center = new System.Windows.Forms.Label();
            this.point5_r = new System.Windows.Forms.Label();
            this.point5_center = new System.Windows.Forms.Label();
            this.redPoint = new System.Windows.Forms.PictureBox();
            this.bicycle_img = new System.Windows.Forms.PictureBox();
            this.cutImage1 = new System.Windows.Forms.PictureBox();
            this.cutImage5 = new System.Windows.Forms.PictureBox();
            this.cutImage4 = new System.Windows.Forms.PictureBox();
            this.cutImage3 = new System.Windows.Forms.PictureBox();
            this.cutImage2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bicycle_label = new System.Windows.Forms.Label();
            this.bicycle_num_input = new System.Windows.Forms.TextBox();
            this.start = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bicycle_img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // serialPORT1
            // 
            this.serialPORT1.PortName = "COM5";
            this.serialPORT1.ReadTimeout = 50;
            this.serialPORT1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serial1_DataReceived);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.ForeColor = System.Drawing.Color.Gray;
            this.button1.Location = new System.Drawing.Point(306, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "手臂連線";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.serial1_Open_Click);
            // 
            // readTextBox
            // 
            this.readTextBox.Location = new System.Drawing.Point(12, 21);
            this.readTextBox.Multiline = true;
            this.readTextBox.Name = "readTextBox";
            this.readTextBox.ReadOnly = true;
            this.readTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.readTextBox.Size = new System.Drawing.Size(405, 230);
            this.readTextBox.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button3.Location = new System.Drawing.Point(306, 115);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 39);
            this.button3.TabIndex = 5;
            this.button3.Text = "手臂離線";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.serialPortClose_Click);
            // 
            // button4
            // 
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.Location = new System.Drawing.Point(335, 258);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 27);
            this.button4.TabIndex = 6;
            this.button4.Text = "清除";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.simulation);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(955, 529);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(409, 299);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "控制項";
            // 
            // simulation
            // 
            this.simulation.Location = new System.Drawing.Point(306, 199);
            this.simulation.Name = "simulation";
            this.simulation.Size = new System.Drawing.Size(85, 35);
            this.simulation.TabIndex = 20;
            this.simulation.Text = "模擬";
            this.simulation.UseVisualStyleBackColor = true;
            this.simulation.Click += new System.EventHandler(this.simulation_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.writeTextbox);
            this.groupBox4.Location = new System.Drawing.Point(6, 21);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(294, 248);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "資料發送";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(111, 219);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 1;
            this.button5.Text = "送出";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // writeTextbox
            // 
            this.writeTextbox.Location = new System.Drawing.Point(6, 21);
            this.writeTextbox.Multiline = true;
            this.writeTextbox.Name = "writeTextbox";
            this.writeTextbox.Size = new System.Drawing.Size(282, 192);
            this.writeTextbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(354, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "COM5";
            // 
            // timer_stop_btn
            // 
            this.timer_stop_btn.BackColor = System.Drawing.Color.IndianRed;
            this.timer_stop_btn.Cursor = System.Windows.Forms.Cursors.Default;
            this.timer_stop_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.timer_stop_btn.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.timer_stop_btn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.timer_stop_btn.Location = new System.Drawing.Point(343, 257);
            this.timer_stop_btn.Name = "timer_stop_btn";
            this.timer_stop_btn.Size = new System.Drawing.Size(76, 27);
            this.timer_stop_btn.TabIndex = 11;
            this.timer_stop_btn.Text = "暫停";
            this.timer_stop_btn.UseVisualStyleBackColor = false;
            this.timer_stop_btn.Click += new System.EventHandler(this.timer_stop_btn_Click);
            // 
            // survey_result_btn
            // 
            this.survey_result_btn.BackColor = System.Drawing.Color.RoyalBlue;
            this.survey_result_btn.Cursor = System.Windows.Forms.Cursors.Default;
            this.survey_result_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.survey_result_btn.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.survey_result_btn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.survey_result_btn.Location = new System.Drawing.Point(265, 257);
            this.survey_result_btn.Name = "survey_result_btn";
            this.survey_result_btn.Size = new System.Drawing.Size(72, 27);
            this.survey_result_btn.TabIndex = 0;
            this.survey_result_btn.Text = "量測結果";
            this.survey_result_btn.UseVisualStyleBackColor = false;
            this.survey_result_btn.Click += new System.EventHandler(this.survey_result_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.readTextBox);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Location = new System.Drawing.Point(49, 529);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(423, 299);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "量測數據";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.Location = new System.Drawing.Point(12, 21);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(407, 230);
            this.dataGridView1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Controls.Add(this.survey_result_btn);
            this.groupBox3.Controls.Add(this.timer_stop_btn);
            this.groupBox3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox3.Location = new System.Drawing.Point(501, 529);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(431, 299);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "歷史資料";
            // 
            // point1_r
            // 
            this.point1_r.AutoSize = true;
            this.point1_r.BackColor = System.Drawing.Color.Transparent;
            this.point1_r.Location = new System.Drawing.Point(984, 415);
            this.point1_r.Name = "point1_r";
            this.point1_r.Size = new System.Drawing.Size(44, 17);
            this.point1_r.TabIndex = 10;
            this.point1_r.Text = "label3";
            // 
            // point1_center
            // 
            this.point1_center.AutoSize = true;
            this.point1_center.BackColor = System.Drawing.Color.Transparent;
            this.point1_center.Location = new System.Drawing.Point(984, 398);
            this.point1_center.Name = "point1_center";
            this.point1_center.Size = new System.Drawing.Size(44, 17);
            this.point1_center.TabIndex = 11;
            this.point1_center.Text = "label4";
            // 
            // point2_r
            // 
            this.point2_r.AutoSize = true;
            this.point2_r.BackColor = System.Drawing.Color.Transparent;
            this.point2_r.Location = new System.Drawing.Point(763, 206);
            this.point2_r.Name = "point2_r";
            this.point2_r.Size = new System.Drawing.Size(44, 17);
            this.point2_r.TabIndex = 12;
            this.point2_r.Text = "label5";
            // 
            // point2_center
            // 
            this.point2_center.AutoSize = true;
            this.point2_center.BackColor = System.Drawing.Color.Transparent;
            this.point2_center.Location = new System.Drawing.Point(763, 223);
            this.point2_center.Name = "point2_center";
            this.point2_center.Size = new System.Drawing.Size(44, 17);
            this.point2_center.TabIndex = 13;
            this.point2_center.Text = "label6";
            // 
            // point3_r
            // 
            this.point3_r.AutoSize = true;
            this.point3_r.BackColor = System.Drawing.Color.Transparent;
            this.point3_r.Location = new System.Drawing.Point(794, 325);
            this.point3_r.Name = "point3_r";
            this.point3_r.Size = new System.Drawing.Size(44, 17);
            this.point3_r.TabIndex = 14;
            this.point3_r.Text = "label7";
            // 
            // point3_center
            // 
            this.point3_center.AutoSize = true;
            this.point3_center.BackColor = System.Drawing.Color.Transparent;
            this.point3_center.Location = new System.Drawing.Point(794, 308);
            this.point3_center.Name = "point3_center";
            this.point3_center.Size = new System.Drawing.Size(44, 17);
            this.point3_center.TabIndex = 15;
            this.point3_center.Text = "label8";
            // 
            // point4_r
            // 
            this.point4_r.AutoSize = true;
            this.point4_r.BackColor = System.Drawing.Color.Transparent;
            this.point4_r.Location = new System.Drawing.Point(414, 44);
            this.point4_r.Name = "point4_r";
            this.point4_r.Size = new System.Drawing.Size(44, 17);
            this.point4_r.TabIndex = 16;
            this.point4_r.Text = "label9";
            // 
            // point4_center
            // 
            this.point4_center.AutoSize = true;
            this.point4_center.BackColor = System.Drawing.Color.Transparent;
            this.point4_center.Location = new System.Drawing.Point(406, 61);
            this.point4_center.Name = "point4_center";
            this.point4_center.Size = new System.Drawing.Size(52, 17);
            this.point4_center.TabIndex = 17;
            this.point4_center.Text = "label10";
            // 
            // point5_r
            // 
            this.point5_r.AutoSize = true;
            this.point5_r.BackColor = System.Drawing.Color.Transparent;
            this.point5_r.Location = new System.Drawing.Point(406, 189);
            this.point5_r.Name = "point5_r";
            this.point5_r.Size = new System.Drawing.Size(52, 17);
            this.point5_r.TabIndex = 18;
            this.point5_r.Text = "label11";
            // 
            // point5_center
            // 
            this.point5_center.AutoSize = true;
            this.point5_center.BackColor = System.Drawing.Color.Transparent;
            this.point5_center.Location = new System.Drawing.Point(406, 206);
            this.point5_center.Name = "point5_center";
            this.point5_center.Size = new System.Drawing.Size(52, 17);
            this.point5_center.TabIndex = 19;
            this.point5_center.Text = "label12";
            // 
            // redPoint
            // 
            this.redPoint.BackColor = System.Drawing.Color.Transparent;
            this.redPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.redPoint.Location = new System.Drawing.Point(451, 107);
            this.redPoint.Name = "redPoint";
            this.redPoint.Size = new System.Drawing.Size(37, 37);
            this.redPoint.TabIndex = 2;
            this.redPoint.TabStop = false;
            // 
            // bicycle_img
            // 
            this.bicycle_img.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.bicycle_img.Image = global::測試用專案.Properties.Resources.B1;
            this.bicycle_img.Location = new System.Drawing.Point(0, 0);
            this.bicycle_img.Name = "bicycle_img";
            this.bicycle_img.Size = new System.Drawing.Size(1419, 513);
            this.bicycle_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.bicycle_img.TabIndex = 1;
            this.bicycle_img.TabStop = false;
            // 
            // cutImage1
            // 
            this.cutImage1.BackColor = System.Drawing.Color.Transparent;
            this.cutImage1.Location = new System.Drawing.Point(945, 376);
            this.cutImage1.Name = "cutImage1";
            this.cutImage1.Size = new System.Drawing.Size(50, 50);
            this.cutImage1.TabIndex = 20;
            this.cutImage1.TabStop = false;
            this.cutImage1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cutImage1_MouseDown);
            // 
            // cutImage5
            // 
            this.cutImage5.Location = new System.Drawing.Point(451, 107);
            this.cutImage5.Name = "cutImage5";
            this.cutImage5.Size = new System.Drawing.Size(50, 50);
            this.cutImage5.TabIndex = 21;
            this.cutImage5.TabStop = false;
            // 
            // cutImage4
            // 
            this.cutImage4.Location = new System.Drawing.Point(467, 36);
            this.cutImage4.Name = "cutImage4";
            this.cutImage4.Size = new System.Drawing.Size(50, 50);
            this.cutImage4.TabIndex = 22;
            this.cutImage4.TabStop = false;
            // 
            // cutImage3
            // 
            this.cutImage3.Location = new System.Drawing.Point(883, 372);
            this.cutImage3.Name = "cutImage3";
            this.cutImage3.Size = new System.Drawing.Size(50, 50);
            this.cutImage3.TabIndex = 23;
            this.cutImage3.TabStop = false;
            // 
            // cutImage2
            // 
            this.cutImage2.Location = new System.Drawing.Point(875, 223);
            this.cutImage2.Name = "cutImage2";
            this.cutImage2.Size = new System.Drawing.Size(50, 50);
            this.cutImage2.TabIndex = 24;
            this.cutImage2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(5, 364);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(323, 145);
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // bicycle_label
            // 
            this.bicycle_label.AutoSize = true;
            this.bicycle_label.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.bicycle_label.Location = new System.Drawing.Point(1121, 14);
            this.bicycle_label.Name = "bicycle_label";
            this.bicycle_label.Size = new System.Drawing.Size(114, 24);
            this.bicycle_label.TabIndex = 26;
            this.bicycle_label.Text = "車架流水號 :";
            // 
            // bicycle_num_input
            // 
            this.bicycle_num_input.Location = new System.Drawing.Point(1237, 14);
            this.bicycle_num_input.Name = "bicycle_num_input";
            this.bicycle_num_input.Size = new System.Drawing.Size(179, 25);
            this.bicycle_num_input.TabIndex = 27;
            // 
            // start
            // 
            this.start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.start.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.start.FlatAppearance.BorderSize = 0;
            this.start.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.start.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.start.ForeColor = System.Drawing.Color.Transparent;
            this.start.Location = new System.Drawing.Point(1322, 50);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(94, 28);
            this.start.TabIndex = 28;
            this.start.Text = "開始量測";
            this.start.UseVisualStyleBackColor = false;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // Form1
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1424, 854);
            this.Controls.Add(this.start);
            this.Controls.Add(this.bicycle_num_input);
            this.Controls.Add(this.bicycle_label);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.point5_center);
            this.Controls.Add(this.point5_r);
            this.Controls.Add(this.point4_center);
            this.Controls.Add(this.point4_r);
            this.Controls.Add(this.point3_center);
            this.Controls.Add(this.point3_r);
            this.Controls.Add(this.point2_center);
            this.Controls.Add(this.point2_r);
            this.Controls.Add(this.point1_center);
            this.Controls.Add(this.point1_r);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.redPoint);
            this.Controls.Add(this.bicycle_img);
            this.Controls.Add(this.cutImage4);
            this.Controls.Add(this.cutImage5);
            this.Controls.Add(this.cutImage2);
            this.Controls.Add(this.cutImage3);
            this.Controls.Add(this.cutImage1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.redPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bicycle_img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutImage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.IO.Ports.SerialPort serialPORT1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox redPoint;
        private System.Windows.Forms.TextBox readTextBox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.PictureBox bicycle_img;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox writeTextbox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button survey_result_btn;
        private System.Windows.Forms.Button timer_stop_btn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label point1_r;
        private System.Windows.Forms.Label point1_center;
        private System.Windows.Forms.Label point2_r;
        private System.Windows.Forms.Label point2_center;
        private System.Windows.Forms.Label point3_r;
        private System.Windows.Forms.Label point3_center;
        private System.Windows.Forms.Label point4_r;
        private System.Windows.Forms.Label point4_center;
        private System.Windows.Forms.Label point5_r;
        private System.Windows.Forms.Label point5_center;
        private System.Windows.Forms.Button simulation;
        private System.Windows.Forms.PictureBox cutImage1;
        private System.Windows.Forms.PictureBox cutImage5;
        private System.Windows.Forms.PictureBox cutImage4;
        private System.Windows.Forms.PictureBox cutImage3;
        private System.Windows.Forms.PictureBox cutImage2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label bicycle_label;
        private System.Windows.Forms.TextBox bicycle_num_input;
        private System.Windows.Forms.Button start;
    }
}

