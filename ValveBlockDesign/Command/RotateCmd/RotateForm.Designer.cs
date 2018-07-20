namespace ValveBlockDesign
{
    partial class RotateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RotateForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxChoose = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxAngle = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnsure = new System.Windows.Forms.Button();
            this.btnsec = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBoxChoose);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(88, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 10);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择旋转安装面";
            // 
            // checkBoxChoose
            // 
            this.checkBoxChoose.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxChoose.AutoSize = true;
            this.checkBoxChoose.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxChoose.Image")));
            this.checkBoxChoose.Location = new System.Drawing.Point(25, 20);
            this.checkBoxChoose.Name = "checkBoxChoose";
            this.checkBoxChoose.Size = new System.Drawing.Size(38, 38);
            this.checkBoxChoose.TabIndex = 2;
            this.checkBoxChoose.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(35, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 10);
            this.label2.TabIndex = 4;
            this.label2.Text = "旋转角度";
            // 
            // comboBoxAngle
            // 
            this.comboBoxAngle.FormattingEnabled = true;
            this.comboBoxAngle.Items.AddRange(new object[] {
            "0",
            "90",
            "180",
            "270"});
            this.comboBoxAngle.Location = new System.Drawing.Point(102, 113);
            this.comboBoxAngle.Name = "comboBoxAngle";
            this.comboBoxAngle.Size = new System.Drawing.Size(70, 20);
            this.comboBoxAngle.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(187, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 10);
            this.label3.TabIndex = 6;
            this.label3.Text = "度";
            // 
            // btnsure
            // 
            this.btnsure.Location = new System.Drawing.Point(37, 155);
            this.btnsure.Name = "btnsure";
            this.btnsure.Size = new System.Drawing.Size(60, 23);
            this.btnsure.TabIndex = 7;
            this.btnsure.Text = "确定";
            this.btnsure.UseVisualStyleBackColor = true;
            this.btnsure.Click += new System.EventHandler(this.btnsure_Click);
            // 
            // btnsec
            // 
            this.btnsec.Location = new System.Drawing.Point(143, 155);
            this.btnsec.Name = "btnsec";
            this.btnsec.Size = new System.Drawing.Size(60, 23);
            this.btnsec.TabIndex = 8;
            this.btnsec.Text = "取消";
            this.btnsec.UseVisualStyleBackColor = true;
            this.btnsec.Click += new System.EventHandler(this.btnsec_Click);
            // 
            // RotateForm
            // 
            this.AcceptButton = this.btnsure;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 189);
            this.Controls.Add(this.btnsec);
            this.Controls.Add(this.btnsure);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxAngle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RotateForm";
            this.Text = "旋转安装面特征";
            this.TopMost = true;
            this.Closed += new System.EventHandler(this.RotateForm_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RotateForm_KeyPress);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox checkBoxChoose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnsec;
        public System.Windows.Forms.Button btnsure;
        public System.Windows.Forms.ComboBox comboBoxAngle;
    }
}