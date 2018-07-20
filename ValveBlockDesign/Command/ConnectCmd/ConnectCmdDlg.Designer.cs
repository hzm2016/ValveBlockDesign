namespace ValveBlockDesign
{
    partial class ConnectCmdDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectCmdDlg));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.othersideRadioButton = new System.Windows.Forms.RadioButton();
            this.onesideRadioButton = new System.Windows.Forms.RadioButton();
            this.centerRadioButton = new System.Windows.Forms.RadioButton();
            this.diaTextBox = new System.Windows.Forms.TextBox();
            this.portIndexComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxToCav = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numerUPY = new System.Windows.Forms.NumericUpDown();
            this.numerUPX = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBoxCav = new System.Windows.Forms.CheckBox();
            this.connectCancelButton = new System.Windows.Forms.Button();
            this.connectOkButton = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numerUPY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numerUPX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.othersideRadioButton);
            this.groupBox3.Controls.Add(this.onesideRadioButton);
            this.groupBox3.Controls.Add(this.centerRadioButton);
            this.groupBox3.Controls.Add(this.diaTextBox);
            this.groupBox3.Controls.Add(this.portIndexComboBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.checkBoxToCav);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(12, 132);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(322, 95);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "连接到";
            // 
            // othersideRadioButton
            // 
            this.othersideRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.othersideRadioButton.AutoSize = true;
            this.othersideRadioButton.Location = new System.Drawing.Point(284, 27);
            this.othersideRadioButton.Name = "othersideRadioButton";
            this.othersideRadioButton.Size = new System.Drawing.Size(27, 22);
            this.othersideRadioButton.TabIndex = 8;
            this.othersideRadioButton.TabStop = true;
            this.othersideRadioButton.Text = "右";
            this.othersideRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.othersideRadioButton.UseVisualStyleBackColor = true;
            this.othersideRadioButton.Click += new System.EventHandler(this.othersideRadioButton_Click);
            // 
            // onesideRadioButton
            // 
            this.onesideRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.onesideRadioButton.AutoSize = true;
            this.onesideRadioButton.Location = new System.Drawing.Point(249, 27);
            this.onesideRadioButton.Name = "onesideRadioButton";
            this.onesideRadioButton.Size = new System.Drawing.Size(27, 22);
            this.onesideRadioButton.TabIndex = 8;
            this.onesideRadioButton.TabStop = true;
            this.onesideRadioButton.Text = "左";
            this.onesideRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.onesideRadioButton.UseVisualStyleBackColor = true;
            this.onesideRadioButton.Click += new System.EventHandler(this.onesideRadioButton_Click);
            // 
            // centerRadioButton
            // 
            this.centerRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.centerRadioButton.AutoSize = true;
            this.centerRadioButton.Location = new System.Drawing.Point(216, 27);
            this.centerRadioButton.Name = "centerRadioButton";
            this.centerRadioButton.Size = new System.Drawing.Size(27, 22);
            this.centerRadioButton.TabIndex = 8;
            this.centerRadioButton.TabStop = true;
            this.centerRadioButton.Text = "中";
            this.centerRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.centerRadioButton.UseVisualStyleBackColor = true;
            this.centerRadioButton.Click += new System.EventHandler(this.centerRadioButton_Click);
            // 
            // diaTextBox
            // 
            this.diaTextBox.Location = new System.Drawing.Point(122, 58);
            this.diaTextBox.Name = "diaTextBox";
            this.diaTextBox.Size = new System.Drawing.Size(74, 21);
            this.diaTextBox.TabIndex = 6;
            // 
            // portIndexComboBox
            // 
            this.portIndexComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portIndexComboBox.FormattingEnabled = true;
            this.portIndexComboBox.Location = new System.Drawing.Point(122, 25);
            this.portIndexComboBox.Name = "portIndexComboBox";
            this.portIndexComboBox.Size = new System.Drawing.Size(74, 20);
            this.portIndexComboBox.TabIndex = 7;
            this.portIndexComboBox.SelectedIndexChanged += new System.EventHandler(this.portIndexComboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(87, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "直径";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(87, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "端口";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "孔";
            // 
            // checkBoxToCav
            // 
            this.checkBoxToCav.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxToCav.AutoSize = true;
            this.checkBoxToCav.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxToCav.Image")));
            this.checkBoxToCav.Location = new System.Drawing.Point(11, 34);
            this.checkBoxToCav.Name = "checkBoxToCav";
            this.checkBoxToCav.Size = new System.Drawing.Size(38, 38);
            this.checkBoxToCav.TabIndex = 0;
            this.checkBoxToCav.UseVisualStyleBackColor = true;
            this.checkBoxToCav.Click += new System.EventHandler(this.checkBoxToCav_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.checkBoxCav);
            this.groupBox2.Location = new System.Drawing.Point(11, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(323, 118);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "孔";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numerUPY);
            this.groupBox1.Controls.Add(this.numerUPX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(87, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 95);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // numerUPY
            // 
            this.numerUPY.DecimalPlaces = 2;
            this.numerUPY.Location = new System.Drawing.Point(123, 63);
            this.numerUPY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numerUPY.Name = "numerUPY";
            this.numerUPY.Size = new System.Drawing.Size(86, 21);
            this.numerUPY.TabIndex = 7;
            this.numerUPY.ValueChanged += new System.EventHandler(this.numerUPY_ValueChanged);
            // 
            // numerUPX
            // 
            this.numerUPX.Cursor = System.Windows.Forms.Cursors.Default;
            this.numerUPX.DecimalPlaces = 2;
            this.numerUPX.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.numerUPX.Location = new System.Drawing.Point(123, 20);
            this.numerUPX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numerUPX.Name = "numerUPX";
            this.numerUPX.Size = new System.Drawing.Size(86, 21);
            this.numerUPX.TabIndex = 7;
            this.numerUPX.ValueChanged += new System.EventHandler(this.numerUPX_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(101, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "X";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(15, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(67, 67);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // checkBoxCav
            // 
            this.checkBoxCav.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCav.AutoSize = true;
            this.checkBoxCav.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxCav.Image")));
            this.checkBoxCav.Location = new System.Drawing.Point(12, 44);
            this.checkBoxCav.Name = "checkBoxCav";
            this.checkBoxCav.Size = new System.Drawing.Size(38, 38);
            this.checkBoxCav.TabIndex = 0;
            this.checkBoxCav.UseVisualStyleBackColor = true;
            this.checkBoxCav.Click += new System.EventHandler(this.checkBoxCav_Click);
            // 
            // connectCancelButton
            // 
            this.connectCancelButton.Font = new System.Drawing.Font("宋体", 9F);
            this.connectCancelButton.Location = new System.Drawing.Point(260, 237);
            this.connectCancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.connectCancelButton.Name = "connectCancelButton";
            this.connectCancelButton.Size = new System.Drawing.Size(74, 28);
            this.connectCancelButton.TabIndex = 8;
            this.connectCancelButton.Text = "取消";
            this.connectCancelButton.UseVisualStyleBackColor = true;
            this.connectCancelButton.Click += new System.EventHandler(this.connectCancelButton_Click);
            // 
            // connectOkButton
            // 
            this.connectOkButton.Font = new System.Drawing.Font("宋体", 9F);
            this.connectOkButton.Location = new System.Drawing.Point(161, 237);
            this.connectOkButton.Margin = new System.Windows.Forms.Padding(2);
            this.connectOkButton.Name = "connectOkButton";
            this.connectOkButton.Size = new System.Drawing.Size(74, 28);
            this.connectOkButton.TabIndex = 7;
            this.connectOkButton.Text = "确定";
            this.connectOkButton.UseVisualStyleBackColor = true;
            this.connectOkButton.Click += new System.EventHandler(this.connectOkButton_Click);
            // 
            // ConnectCmdDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 274);
            this.Controls.Add(this.connectCancelButton);
            this.Controls.Add(this.connectOkButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ConnectCmdDlg";
            this.Text = "连接";
            this.Closed += new System.EventHandler(this.OnClose);
            this.Load += new System.EventHandler(this.ConnectCmdDlg_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ConnectCmdDlg_KeyPress);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numerUPY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numerUPX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.RadioButton othersideRadioButton;
        public System.Windows.Forms.RadioButton onesideRadioButton;
        public System.Windows.Forms.RadioButton centerRadioButton;
        public System.Windows.Forms.TextBox diaTextBox;
        public System.Windows.Forms.ComboBox portIndexComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBox checkBoxToCav;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.CheckBox checkBoxCav;
        private System.Windows.Forms.Button connectCancelButton;
        public System.Windows.Forms.Button connectOkButton;
        public System.Windows.Forms.NumericUpDown numerUPY;
        public System.Windows.Forms.NumericUpDown numerUPX;

    }
}