namespace ValveBlockDesign
{
    partial class InsertOutlineForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertOutlineForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxChoose = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxName = new System.Windows.Forms.ListBox();
            this.btnsure = new System.Windows.Forms.Button();
            this.btnsec = new System.Windows.Forms.Button();
            this.btninsert = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(141, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(167, 133);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "外形预览";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(5, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(157, 110);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.checkBoxChoose);
            this.groupBox2.Location = new System.Drawing.Point(141, 148);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(167, 67);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择元件特征";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 10);
            this.label1.TabIndex = 4;
            this.label1.Text = "单一孔或安装面";
            // 
            // checkBoxChoose
            // 
            this.checkBoxChoose.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxChoose.AutoSize = true;
            this.checkBoxChoose.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxChoose.Image")));
            this.checkBoxChoose.Location = new System.Drawing.Point(18, 17);
            this.checkBoxChoose.Name = "checkBoxChoose";
            this.checkBoxChoose.Size = new System.Drawing.Size(38, 38);
            this.checkBoxChoose.TabIndex = 3;
            this.checkBoxChoose.UseVisualStyleBackColor = true;
            this.checkBoxChoose.Click += new System.EventHandler(this.checkBoxChoose_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxName);
            this.groupBox3.Location = new System.Drawing.Point(10, 10);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(126, 205);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "选择安装外形";
            // 
            // listBoxName
            // 
            this.listBoxName.FormattingEnabled = true;
            this.listBoxName.ItemHeight = 10;
            this.listBoxName.Location = new System.Drawing.Point(6, 18);
            this.listBoxName.Name = "listBoxName";
            this.listBoxName.Size = new System.Drawing.Size(115, 184);
            this.listBoxName.TabIndex = 0;
            // 
            // btnsure
            // 
            this.btnsure.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnsure.Location = new System.Drawing.Point(186, 220);
            this.btnsure.Name = "btnsure";
            this.btnsure.Size = new System.Drawing.Size(49, 23);
            this.btnsure.TabIndex = 3;
            this.btnsure.Text = "确定";
            this.btnsure.UseVisualStyleBackColor = true;
            this.btnsure.Click += new System.EventHandler(this.btnsure_Click);
            // 
            // btnsec
            // 
            this.btnsec.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnsec.Location = new System.Drawing.Point(250, 220);
            this.btnsec.Name = "btnsec";
            this.btnsec.Size = new System.Drawing.Size(53, 23);
            this.btnsec.TabIndex = 4;
            this.btnsec.Text = "取消";
            this.btnsec.UseVisualStyleBackColor = true;
            this.btnsec.Click += new System.EventHandler(this.btnsec_Click);
            // 
            // btninsert
            // 
            this.btninsert.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btninsert.Location = new System.Drawing.Point(122, 220);
            this.btninsert.Name = "btninsert";
            this.btninsert.Size = new System.Drawing.Size(49, 23);
            this.btninsert.TabIndex = 5;
            this.btninsert.Text = "插入";
            this.btninsert.UseVisualStyleBackColor = true;
            this.btninsert.Click += new System.EventHandler(this.btninsert_Click);
            // 
            // InsertOutlineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 10F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 248);
            this.Controls.Add(this.btninsert);
            this.Controls.Add(this.btnsec);
            this.Controls.Add(this.btnsure);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InsertOutlineForm";
            this.Text = "插入元件安装外形";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InsertOutlineForm_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InsertOutlineForm_KeyPress);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox checkBoxChoose;
        private System.Windows.Forms.Button btnsure;
        private System.Windows.Forms.Button btnsec;
        public System.Windows.Forms.Button btninsert;
        public System.Windows.Forms.ListBox listBoxName;
    }
}