namespace ValveBlockDesign
{
    partial class BoltHoleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoltHoleForm));
            this.groB1 = new System.Windows.Forms.GroupBox();
            this.comBNumber = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comBLibrary = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grouB2 = new System.Windows.Forms.GroupBox();
            this.radioFour = new System.Windows.Forms.RadioButton();
            this.radioTwo = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.grouB3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.tboffset = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxFace = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnsure = new System.Windows.Forms.Button();
            this.btnsec = new System.Windows.Forms.Button();
            this.groB1.SuspendLayout();
            this.grouB2.SuspendLayout();
            this.grouB3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groB1
            // 
            this.groB1.Controls.Add(this.comBNumber);
            this.groB1.Controls.Add(this.label2);
            this.groB1.Controls.Add(this.comBLibrary);
            this.groB1.Controls.Add(this.label1);
            this.groB1.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groB1.Location = new System.Drawing.Point(12, 2);
            this.groB1.Name = "groB1";
            this.groB1.Size = new System.Drawing.Size(203, 86);
            this.groB1.TabIndex = 0;
            this.groB1.TabStop = false;
            this.groB1.Text = "选择螺纹孔";
            // 
            // comBNumber
            // 
            this.comBNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBNumber.FormattingEnabled = true;
            this.comBNumber.Location = new System.Drawing.Point(67, 53);
            this.comBNumber.Name = "comBNumber";
            this.comBNumber.Size = new System.Drawing.Size(121, 18);
            this.comBNumber.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(20, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 10);
            this.label2.TabIndex = 2;
            this.label2.Text = "型号";
            // 
            // comBLibrary
            // 
            this.comBLibrary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBLibrary.FormattingEnabled = true;
            this.comBLibrary.Items.AddRange(new object[] {
            "BoltHoles"});
            this.comBLibrary.Location = new System.Drawing.Point(67, 18);
            this.comBLibrary.Name = "comBLibrary";
            this.comBLibrary.Size = new System.Drawing.Size(121, 18);
            this.comBLibrary.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(20, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 10);
            this.label1.TabIndex = 0;
            this.label1.Text = "库名";
            // 
            // grouB2
            // 
            this.grouB2.Controls.Add(this.radioFour);
            this.grouB2.Controls.Add(this.radioTwo);
            this.grouB2.Controls.Add(this.label4);
            this.grouB2.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grouB2.Location = new System.Drawing.Point(12, 94);
            this.grouB2.Name = "grouB2";
            this.grouB2.Size = new System.Drawing.Size(203, 86);
            this.grouB2.TabIndex = 1;
            this.grouB2.TabStop = false;
            this.grouB2.Text = "选择孔的数量";
            // 
            // radioFour
            // 
            this.radioFour.AutoSize = true;
            this.radioFour.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioFour.Location = new System.Drawing.Point(26, 53);
            this.radioFour.Name = "radioFour";
            this.radioFour.Size = new System.Drawing.Size(29, 14);
            this.radioFour.TabIndex = 4;
            this.radioFour.TabStop = true;
            this.radioFour.Text = "4";
            this.radioFour.UseVisualStyleBackColor = true;
            // 
            // radioTwo
            // 
            this.radioTwo.AutoSize = true;
            this.radioTwo.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioTwo.Location = new System.Drawing.Point(26, 21);
            this.radioTwo.Name = "radioTwo";
            this.radioTwo.Size = new System.Drawing.Size(29, 14);
            this.radioTwo.TabIndex = 3;
            this.radioTwo.TabStop = true;
            this.radioTwo.Text = "2";
            this.radioTwo.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(20, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 10);
            this.label4.TabIndex = 0;
            // 
            // grouB3
            // 
            this.grouB3.Controls.Add(this.label7);
            this.grouB3.Controls.Add(this.tbID);
            this.grouB3.Controls.Add(this.tboffset);
            this.grouB3.Controls.Add(this.label6);
            this.grouB3.Controls.Add(this.label5);
            this.grouB3.Controls.Add(this.checkBoxFace);
            this.grouB3.Controls.Add(this.label3);
            this.grouB3.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grouB3.Location = new System.Drawing.Point(12, 186);
            this.grouB3.Name = "grouB3";
            this.grouB3.Size = new System.Drawing.Size(203, 130);
            this.grouB3.TabIndex = 2;
            this.grouB3.TabStop = false;
            this.grouB3.Text = "选择面和基准";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(52, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 10);
            this.label7.TabIndex = 7;
            this.label7.Text = "元件ID";
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(97, 99);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(91, 19);
            this.tbID.TabIndex = 6;
            this.tbID.Text = "MBolt";
            this.tbID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbID_KeyPress);
            // 
            // tboffset
            // 
            this.tboffset.Location = new System.Drawing.Point(97, 71);
            this.tboffset.Name = "tboffset";
            this.tboffset.Size = new System.Drawing.Size(91, 19);
            this.tboffset.TabIndex = 5;
            this.tboffset.TextChanged += new System.EventHandler(this.tboffset_TextChanged);
            this.tboffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboffset_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(20, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 10);
            this.label6.TabIndex = 4;
            this.label6.Text = "距离边界尺寸";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(77, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 10);
            this.label5.TabIndex = 3;
            this.label5.Text = "选择面";
            // 
            // checkBoxFace
            // 
            this.checkBoxFace.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxFace.AutoSize = true;
            this.checkBoxFace.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxFace.Image")));
            this.checkBoxFace.Location = new System.Drawing.Point(22, 21);
            this.checkBoxFace.Name = "checkBoxFace";
            this.checkBoxFace.Size = new System.Drawing.Size(38, 38);
            this.checkBoxFace.TabIndex = 2;
            this.checkBoxFace.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(20, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 10);
            this.label3.TabIndex = 0;
            // 
            // btnsure
            // 
            this.btnsure.Location = new System.Drawing.Point(79, 322);
            this.btnsure.Name = "btnsure";
            this.btnsure.Size = new System.Drawing.Size(60, 23);
            this.btnsure.TabIndex = 3;
            this.btnsure.Text = "确定";
            this.btnsure.UseVisualStyleBackColor = true;
            this.btnsure.Click += new System.EventHandler(this.btnsure_Click);
            // 
            // btnsec
            // 
            this.btnsec.Location = new System.Drawing.Point(155, 322);
            this.btnsec.Name = "btnsec";
            this.btnsec.Size = new System.Drawing.Size(60, 23);
            this.btnsec.TabIndex = 4;
            this.btnsec.Text = "取消";
            this.btnsec.UseVisualStyleBackColor = true;
            this.btnsec.Click += new System.EventHandler(this.btnsec_Click);
            // 
            // BoltHoleForm
            // 
            this.AcceptButton = this.btnsure;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 348);
            this.Controls.Add(this.btnsec);
            this.Controls.Add(this.btnsure);
            this.Controls.Add(this.grouB3);
            this.Controls.Add(this.grouB2);
            this.Controls.Add(this.groB1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BoltHoleForm";
            this.Text = "插入安装螺纹孔";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BoltHoleForm_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BoltHoleForm_KeyPress);
            this.groB1.ResumeLayout(false);
            this.groB1.PerformLayout();
            this.grouB2.ResumeLayout(false);
            this.grouB2.PerformLayout();
            this.grouB3.ResumeLayout(false);
            this.grouB3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groB1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grouB2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grouB3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnsec;
        public System.Windows.Forms.ComboBox comBNumber;
        public System.Windows.Forms.ComboBox comBLibrary;
        public System.Windows.Forms.RadioButton radioFour;
        public System.Windows.Forms.RadioButton radioTwo;
        public System.Windows.Forms.TextBox tboffset;
        public System.Windows.Forms.CheckBox checkBoxFace;
        public System.Windows.Forms.Button btnsure;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox tbID;
    }
}