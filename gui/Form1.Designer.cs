namespace FEV_routingtool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox_routecounts = new System.Windows.Forms.ComboBox();
            this.comboBox_unit = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_midpoints = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_midpoint = new System.Windows.Forms.Label();
            this.label_endpoint = new System.Windows.Forms.Label();
            this.label_startpoint = new System.Windows.Forms.Label();
            this.textBox_desiredlength = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.richTextBox_serverlogs = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_radius = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_generateroutes = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button_move_all_right = new System.Windows.Forms.Button();
            this.button_move_one_left = new System.Windows.Forms.Button();
            this.button_move_one_right = new System.Windows.Forms.Button();
            this.listBox_dst = new System.Windows.Forms.ListBox();
            this.listBox_src = new System.Windows.Forms.ListBox();
            this.textBox_endpoint = new System.Windows.Forms.TextBox();
            this.textBox_startpoint = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_searchtype = new System.Windows.Forms.ComboBox();
            this.button_gpx_text = new System.Windows.Forms.Button();
            this.button_clearmap = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_numberofroutes = new System.Windows.Forms.ComboBox();
            this.label_routelength = new System.Windows.Forms.Label();
            this.label_signscount = new System.Windows.Forms.Label();
            this.label_coordinates = new System.Windows.Forms.Label();
            this.button_exit = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip5 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip6 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip7 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip8 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip9 = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip10 = new System.Windows.Forms.ToolTip(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.comboBox_routecounts);
            this.panel1.Controls.Add(this.comboBox_unit);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.textBox_midpoints);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label_midpoint);
            this.panel1.Controls.Add(this.label_endpoint);
            this.panel1.Controls.Add(this.label_startpoint);
            this.panel1.Controls.Add(this.textBox_desiredlength);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.richTextBox_serverlogs);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.textBox_radius);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button_generateroutes);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.textBox_endpoint);
            this.panel1.Controls.Add(this.textBox_startpoint);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox_searchtype);
            this.panel1.Location = new System.Drawing.Point(0, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 968);
            this.panel1.TabIndex = 0;
            // 
            // comboBox_routecounts
            // 
            this.comboBox_routecounts.FormattingEnabled = true;
            this.comboBox_routecounts.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.comboBox_routecounts.Location = new System.Drawing.Point(101, 44);
            this.comboBox_routecounts.Name = "comboBox_routecounts";
            this.comboBox_routecounts.Size = new System.Drawing.Size(164, 23);
            this.comboBox_routecounts.TabIndex = 21;
            // 
            // comboBox_unit
            // 
            this.comboBox_unit.FormattingEnabled = true;
            this.comboBox_unit.Items.AddRange(new object[] {
            "km",
            "m"});
            this.comboBox_unit.Location = new System.Drawing.Point(101, 158);
            this.comboBox_unit.Name = "comboBox_unit";
            this.comboBox_unit.Size = new System.Drawing.Size(164, 23);
            this.comboBox_unit.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label12.ForeColor = System.Drawing.SystemColors.Window;
            this.label12.Location = new System.Drawing.Point(6, 158);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 20);
            this.label12.TabIndex = 19;
            this.label12.Text = "Length Unit";
            // 
            // textBox_midpoints
            // 
            this.textBox_midpoints.Location = new System.Drawing.Point(101, 129);
            this.textBox_midpoints.Name = "textBox_midpoints";
            this.textBox_midpoints.Size = new System.Drawing.Size(274, 23);
            this.textBox_midpoints.TabIndex = 18;
            this.textBox_midpoints.Enter += new System.EventHandler(this.textBox_midpoints_Enter);
            this.textBox_midpoints.Leave += new System.EventHandler(this.textBox_midpoints_Leave);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.Window;
            this.label10.Location = new System.Drawing.Point(380, 217);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(12, 15);
            this.label10.TabIndex = 17;
            this.label10.Text = "?";
            this.label10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label10_MouseDown);
            this.label10.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label10_MouseUp);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.Window;
            this.label9.Location = new System.Drawing.Point(380, 191);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 15);
            this.label9.TabIndex = 16;
            this.label9.Text = "?";
            this.label9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label9_MouseDown);
            this.label9.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label9_MouseUp);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Window;
            this.label6.Location = new System.Drawing.Point(380, 252);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "?";
            this.label6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label6_MouseDown);
            this.label6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label6_MouseUp);
            // 
            // label_midpoint
            // 
            this.label_midpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_midpoint.AutoSize = true;
            this.label_midpoint.BackColor = System.Drawing.SystemColors.Window;
            this.label_midpoint.Location = new System.Drawing.Point(381, 132);
            this.label_midpoint.Name = "label_midpoint";
            this.label_midpoint.Size = new System.Drawing.Size(12, 15);
            this.label_midpoint.TabIndex = 14;
            this.label_midpoint.Text = "?";
            this.label_midpoint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_midpoint_MouseDown);
            this.label_midpoint.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_midpoint_MouseUp);
            // 
            // label_endpoint
            // 
            this.label_endpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_endpoint.AutoSize = true;
            this.label_endpoint.BackColor = System.Drawing.SystemColors.Window;
            this.label_endpoint.Location = new System.Drawing.Point(381, 104);
            this.label_endpoint.Name = "label_endpoint";
            this.label_endpoint.Size = new System.Drawing.Size(12, 15);
            this.label_endpoint.TabIndex = 14;
            this.label_endpoint.Text = "?";
            this.label_endpoint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_endpoint_MouseDown);
            this.label_endpoint.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_endpoint_MouseUp);
            // 
            // label_startpoint
            // 
            this.label_startpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_startpoint.AutoSize = true;
            this.label_startpoint.BackColor = System.Drawing.SystemColors.Window;
            this.label_startpoint.Location = new System.Drawing.Point(381, 76);
            this.label_startpoint.Name = "label_startpoint";
            this.label_startpoint.Size = new System.Drawing.Size(12, 15);
            this.label_startpoint.TabIndex = 13;
            this.label_startpoint.Text = "?";
            this.label_startpoint.Click += new System.EventHandler(this.label_startpoint_Click);
            this.label_startpoint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_startpoint_MouseDown);
            this.label_startpoint.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_startpoint_MouseUp);
            // 
            // textBox_desiredlength
            // 
            this.textBox_desiredlength.Location = new System.Drawing.Point(101, 214);
            this.textBox_desiredlength.Name = "textBox_desiredlength";
            this.textBox_desiredlength.Size = new System.Drawing.Size(274, 23);
            this.textBox_desiredlength.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.ForeColor = System.Drawing.SystemColors.Window;
            this.label8.Location = new System.Drawing.Point(6, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 20);
            this.label8.TabIndex = 11;
            this.label8.Text = "Route Length";
            // 
            // richTextBox_serverlogs
            // 
            this.richTextBox_serverlogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_serverlogs.Location = new System.Drawing.Point(6, 807);
            this.richTextBox_serverlogs.Name = "richTextBox_serverlogs";
            this.richTextBox_serverlogs.Size = new System.Drawing.Size(386, 152);
            this.richTextBox_serverlogs.TabIndex = 10;
            this.richTextBox_serverlogs.Text = "";
            this.richTextBox_serverlogs.TextChanged += new System.EventHandler(this.richTextBox_serverlogs_TextChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.Window;
            this.label7.Location = new System.Drawing.Point(6, 786);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "Server Logs";
            // 
            // textBox_radius
            // 
            this.textBox_radius.Location = new System.Drawing.Point(101, 187);
            this.textBox_radius.Name = "textBox_radius";
            this.textBox_radius.Size = new System.Drawing.Size(274, 23);
            this.textBox_radius.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(6, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Radius";
            // 
            // button_generateroutes
            // 
            this.button_generateroutes.BackColor = System.Drawing.Color.White;
            this.button_generateroutes.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_generateroutes.ForeColor = System.Drawing.SystemColors.MenuText;
            this.button_generateroutes.Image = ((System.Drawing.Image)(resources.GetObject("button_generateroutes.Image")));
            this.button_generateroutes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_generateroutes.Location = new System.Drawing.Point(136, 720);
            this.button_generateroutes.Name = "button_generateroutes";
            this.button_generateroutes.Size = new System.Drawing.Size(153, 73);
            this.button_generateroutes.TabIndex = 1;
            this.button_generateroutes.Text = "Generate Routes";
            this.button_generateroutes.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_generateroutes.UseVisualStyleBackColor = false;
            this.button_generateroutes.Click += new System.EventHandler(this.button_generateroutes_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button_move_all_right);
            this.groupBox1.Controls.Add(this.button_move_one_left);
            this.groupBox1.Controls.Add(this.button_move_one_right);
            this.groupBox1.Controls.Add(this.listBox_dst);
            this.groupBox1.Controls.Add(this.listBox_src);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Location = new System.Drawing.Point(6, 259);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 455);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Static Features Selection";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.BackgroundImage = global::FEV_routingtool.Properties.Resources.right_all;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Location = new System.Drawing.Point(6, 269);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(50, 50);
            this.button3.TabIndex = 5;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button_move_all_right
            // 
            this.button_move_all_right.BackColor = System.Drawing.Color.White;
            this.button_move_all_right.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_move_all_right.BackgroundImage")));
            this.button_move_all_right.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_move_all_right.Location = new System.Drawing.Point(319, 269);
            this.button_move_all_right.Name = "button_move_all_right";
            this.button_move_all_right.Size = new System.Drawing.Size(50, 50);
            this.button_move_all_right.TabIndex = 4;
            this.button_move_all_right.UseVisualStyleBackColor = false;
            this.button_move_all_right.Click += new System.EventHandler(this.button_move_all_right_Click);
            // 
            // button_move_one_left
            // 
            this.button_move_one_left.BackColor = System.Drawing.Color.White;
            this.button_move_one_left.BackgroundImage = global::FEV_routingtool.Properties.Resources.right;
            this.button_move_one_left.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_move_one_left.Location = new System.Drawing.Point(62, 269);
            this.button_move_one_left.Name = "button_move_one_left";
            this.button_move_one_left.Size = new System.Drawing.Size(50, 50);
            this.button_move_one_left.TabIndex = 3;
            this.button_move_one_left.UseVisualStyleBackColor = false;
            this.button_move_one_left.Click += new System.EventHandler(this.button_move_one_left_Click);
            // 
            // button_move_one_right
            // 
            this.button_move_one_right.BackColor = System.Drawing.Color.White;
            this.button_move_one_right.BackgroundImage = global::FEV_routingtool.Properties.Resources.left;
            this.button_move_one_right.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_move_one_right.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_move_one_right.Location = new System.Drawing.Point(263, 269);
            this.button_move_one_right.Name = "button_move_one_right";
            this.button_move_one_right.Size = new System.Drawing.Size(50, 50);
            this.button_move_one_right.TabIndex = 2;
            this.button_move_one_right.UseVisualStyleBackColor = false;
            this.button_move_one_right.Click += new System.EventHandler(this.button_move_one_right_Click);
            // 
            // listBox_dst
            // 
            this.listBox_dst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_dst.FormattingEnabled = true;
            this.listBox_dst.ItemHeight = 20;
            this.listBox_dst.Items.AddRange(new object[] {
            "stop_signs"});
            this.listBox_dst.Location = new System.Drawing.Point(6, 325);
            this.listBox_dst.Name = "listBox_dst";
            this.listBox_dst.Size = new System.Drawing.Size(363, 124);
            this.listBox_dst.TabIndex = 1;
            this.listBox_dst.DoubleClick += new System.EventHandler(this.listBox_dst_DoubleClick);
            // 
            // listBox_src
            // 
            this.listBox_src.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox_src.FormattingEnabled = true;
            this.listBox_src.ItemHeight = 20;
            this.listBox_src.Items.AddRange(new object[] {
            "visit_charge_station",
            "highway",
            "avoid_highway",
            "urban",
            "oneway",
            "both_ways",
            "limited_access",
            "not_paved",
            "ramp",
            "manoeuvre",
            "roundabout",
            "one_lane",
            "multiple_lanes",
            "speed_130km_80mph",
            "speed_101kph_to_130kph_65_to_80mph",
            "speed_91kph_to_100kph_55mph_to_64mph",
            "speed_71kph_to_90kph_41mph_to_54mph",
            "speed_51kph_to_70kph_31mph_to_40mph",
            "speed_31kph_to_50kph_21mph_to_30mph",
            "speed_11kph_to_30kph_6mph_to_20mph",
            "speed_11kph_6mph",
            "traffic_lights",
            "railway_crossing_signs",
            "no_overtaking_signs",
            "traffic_signs",
            "stop_signs",
            "school_zone_signs",
            "icy_road_signs",
            "crosswalk_signs",
            "animal_crossing_signs",
            "lane_merge_right_signs",
            "lane_merge_left_signs",
            "falling_rocks_signs",
            "hills_signs",
            "tunnel",
            "bridge",
            "road_good",
            "road_fair",
            "road_poor",
            "speed_bumps",
            "toll_station",
            "lane_marker_no_marker",
            "lane_marker_long_dashed",
            "lane_marker_short_dashed",
            "lane_marker_double_dashed",
            "lane_marker_double_solid",
            "lane_marker_single_solid",
            "lane_marker_inner_solid_outter_dashed",
            "lane_marker_inner_dashed_outter_solid",
            "lane_marker_no_divider",
            "lane_marker_physical_divider",
            "hov_lane",
            "reversible_lane",
            "express_lane",
            "slow_lane",
            "auxiliary_lane",
            "shoulder_lane",
            "passing_lane",
            "turn_lane",
            "parking_lane",
            "center_turn_lane",
            "parking_lot"});
            this.listBox_src.Location = new System.Drawing.Point(6, 22);
            this.listBox_src.Name = "listBox_src";
            this.listBox_src.Size = new System.Drawing.Size(363, 244);
            this.listBox_src.TabIndex = 0;
            this.listBox_src.DoubleClick += new System.EventHandler(this.listBox_src_DoubleClick);
            // 
            // textBox_endpoint
            // 
            this.textBox_endpoint.Location = new System.Drawing.Point(101, 100);
            this.textBox_endpoint.Name = "textBox_endpoint";
            this.textBox_endpoint.Size = new System.Drawing.Size(274, 23);
            this.textBox_endpoint.TabIndex = 4;
            this.textBox_endpoint.TextChanged += new System.EventHandler(this.textBox_endpoint_TextChanged);
            this.textBox_endpoint.Enter += new System.EventHandler(this.textBox_endpoint_Enter);
            this.textBox_endpoint.Leave += new System.EventHandler(this.textBox_endpoint_Leave);
            // 
            // textBox_startpoint
            // 
            this.textBox_startpoint.Location = new System.Drawing.Point(101, 71);
            this.textBox_startpoint.Name = "textBox_startpoint";
            this.textBox_startpoint.Size = new System.Drawing.Size(274, 23);
            this.textBox_startpoint.TabIndex = 3;
            this.textBox_startpoint.Enter += new System.EventHandler(this.textBox_startpoint_Enter);
            this.textBox_startpoint.Leave += new System.EventHandler(this.textBox_startpoint_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label11.ForeColor = System.Drawing.SystemColors.Window;
            this.label11.Location = new System.Drawing.Point(6, 128);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 20);
            this.label11.TabIndex = 2;
            this.label11.Text = "Mid Points";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(6, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "End Point";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label13.ForeColor = System.Drawing.SystemColors.Window;
            this.label13.Location = new System.Drawing.Point(7, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 20);
            this.label13.TabIndex = 2;
            this.label13.Text = "# of Routes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(7, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start Point";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search Type";
            // 
            // comboBox_searchtype
            // 
            this.comboBox_searchtype.FormattingEnabled = true;
            this.comboBox_searchtype.Items.AddRange(new object[] {
            "Point to Point",
            "Point to Anywhere",
            "Closed Route"});
            this.comboBox_searchtype.Location = new System.Drawing.Point(101, 15);
            this.comboBox_searchtype.Name = "comboBox_searchtype";
            this.comboBox_searchtype.Size = new System.Drawing.Size(164, 23);
            this.comboBox_searchtype.TabIndex = 0;
            // 
            // button_gpx_text
            // 
            this.button_gpx_text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_gpx_text.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_gpx_text.Location = new System.Drawing.Point(1139, 992);
            this.button_gpx_text.Name = "button_gpx_text";
            this.button_gpx_text.Size = new System.Drawing.Size(100, 29);
            this.button_gpx_text.TabIndex = 11;
            this.button_gpx_text.Text = "GPX Test";
            this.button_gpx_text.UseVisualStyleBackColor = true;
            this.button_gpx_text.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_clearmap
            // 
            this.button_clearmap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_clearmap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_clearmap.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_clearmap.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_clearmap.Location = new System.Drawing.Point(401, 990);
            this.button_clearmap.Name = "button_clearmap";
            this.button_clearmap.Size = new System.Drawing.Size(100, 29);
            this.button_clearmap.TabIndex = 11;
            this.button_clearmap.Text = "Clear Data";
            this.button_clearmap.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_clearmap.UseVisualStyleBackColor = true;
            this.button_clearmap.Click += new System.EventHandler(this.button_clearmap_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(0, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(279, 42);
            this.panel2.TabIndex = 3;
            // 
            // gMapControl1
            // 
            this.gMapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl1.LevelsKeepInMemory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(401, 51);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 2;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomEnabled = true;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(944, 927);
            this.gMapControl1.TabIndex = 2;
            this.gMapControl1.Zoom = 0D;
            this.gMapControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gMapControl1_MouseClick);
            this.gMapControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gMapControl1_MouseMove);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(425, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Select Route:";
            // 
            // comboBox_numberofroutes
            // 
            this.comboBox_numberofroutes.FormattingEnabled = true;
            this.comboBox_numberofroutes.Items.AddRange(new object[] {
            "route0_staticfeaturesfile.gpx"});
            this.comboBox_numberofroutes.Location = new System.Drawing.Point(526, 25);
            this.comboBox_numberofroutes.Name = "comboBox_numberofroutes";
            this.comboBox_numberofroutes.Size = new System.Drawing.Size(227, 23);
            this.comboBox_numberofroutes.TabIndex = 6;
            this.comboBox_numberofroutes.SelectedIndexChanged += new System.EventHandler(this.comboBox_numberofroutes_SelectedIndexChanged);
            // 
            // label_routelength
            // 
            this.label_routelength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_routelength.AutoSize = true;
            this.label_routelength.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_routelength.ForeColor = System.Drawing.SystemColors.Window;
            this.label_routelength.Location = new System.Drawing.Point(1003, 25);
            this.label_routelength.Name = "label_routelength";
            this.label_routelength.Size = new System.Drawing.Size(189, 20);
            this.label_routelength.TabIndex = 7;
            this.label_routelength.Text = "Route Length = 00.00 miles";
            // 
            // label_signscount
            // 
            this.label_signscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_signscount.AutoSize = true;
            this.label_signscount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_signscount.ForeColor = System.Drawing.SystemColors.Window;
            this.label_signscount.Location = new System.Drawing.Point(1196, 25);
            this.label_signscount.Name = "label_signscount";
            this.label_signscount.Size = new System.Drawing.Size(149, 20);
            this.label_signscount.TabIndex = 8;
            this.label_signscount.Text = "Features Count = 000";
            // 
            // label_coordinates
            // 
            this.label_coordinates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_coordinates.AutoSize = true;
            this.label_coordinates.ForeColor = System.Drawing.SystemColors.Window;
            this.label_coordinates.Location = new System.Drawing.Point(681, 995);
            this.label_coordinates.Name = "label_coordinates";
            this.label_coordinates.Size = new System.Drawing.Size(34, 15);
            this.label_coordinates.TabIndex = 13;
            this.label_coordinates.Text = "GPS: ";
            // 
            // button_exit
            // 
            this.button_exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exit.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_exit.Location = new System.Drawing.Point(1245, 992);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(100, 29);
            this.button_exit.TabIndex = 14;
            this.button_exit.Text = "Exit";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(1007, 992);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 29);
            this.button1.TabIndex = 11;
            this.button1.Text = "Load GPX File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button2.Location = new System.Drawing.Point(877, 992);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 29);
            this.button2.TabIndex = 15;
            this.button2.Text = "Update Routes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button4.Location = new System.Drawing.Point(749, 992);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(122, 29);
            this.button4.TabIndex = 16;
            this.button4.Text = "Run HERE";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1354, 1045);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_gpx_text);
            this.Controls.Add(this.label_coordinates);
            this.Controls.Add(this.button_clearmap);
            this.Controls.Add(this.label_signscount);
            this.Controls.Add(this.label_routelength);
            this.Controls.Add(this.comboBox_numberofroutes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.gMapControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FEV Routing Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Panel panel2;
        private ComboBox comboBox_searchtype;
        private Label label1;
        private TextBox textBox_endpoint;
        private TextBox textBox_startpoint;
        private Label label3;
        private Label label2;
        private GroupBox groupBox1;
        private ListBox listBox_dst;
        private ListBox listBox_src;
        private Button button3;
        private Button button_move_all_right;
        private Button button_move_one_left;
        private Button button_move_one_right;
        private Button button_generateroutes;
        private TextBox textBox_radius;
        private Label label4;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private Label label5;
        private ComboBox comboBox_numberofroutes;
        private Label label_routelength;
        private Label label_signscount;
        private Button button_clearmap;
        private Button button_showlegend;
        private RichTextBox richTextBox_serverlogs;
        private Label label7;
        private Button button_gpx_text;
        private Label label_coordinates;
        private TextBox textBox_desiredlength;
        private Label label8;
        private Button button_exit;
        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private ToolTip toolTip3;
        private ToolTip toolTip4;
        private ToolTip toolTip5;
        private ToolTip toolTip6;
        private ToolTip toolTip7;
        private ToolTip toolTip8;
        private ToolTip toolTip9;
        private Label label_startpoint;
        private Label label_endpoint;
        private Label label6;
        private Label label9;
        private Label label10;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button1;
        private ComboBox comboBox_unit;
        private Label label12;
        private TextBox textBox_midpoints;
        private Label label11;
        private Label label13;
        private ComboBox comboBox_routecounts;
        private Label label_midpoint;
        private ToolTip toolTip10;
        private Button button2;
        private FolderBrowserDialog folderBrowserDialog2;
        private Button button4;
    }
}