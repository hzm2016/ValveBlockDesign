namespace ValveBlockDesign
{
    partial class DeleteCavityForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteCavityForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBCavity = new System.Windows.Forms.ListBox();
            this.checkBoxChoose = new System.Windows.Forms.CheckBox();
            this.btnsure = new System.Windows.Forms.Button();
            this.btnsec = new System.Windows.Forms.Button();
            this.checkBoxChooseOutline = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBCavity);
            this.groupBox1.Controls.Add(this.checkBoxChoose);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择元件特征";
            // 
            // listBCavity
            // 
            this.listBCavity.FormattingEnabled = true;
            this.listBCavity.ItemHeight = 10;
            this.listBCavity.Location = new System.Drawing.Point(76, 20);
            this.listBCavity.Name = "listBCavity";
            this.listBCavity.Size = new System.Drawing.Size(120, 104);
            this.listBCavity.TabIndex = 4;
            // 
            // checkBoxChoose
            // 
            this.checkBoxChoose.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxChoose.AutoSize = true;
            this.checkBoxChoose.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxChoose.Image")));
            this.checkBoxChoose.Location = new System.Drawing.Point(17, 20);
            this.checkBoxChoose.Name = "checkBoxChoose";
            this.checkBoxChoose.Size = new System.Drawing.Size(38, 38);
            this.checkBoxChoose.TabIndex = 3;
            this.checkBoxChoose.UseVisualStyleBackColor = true;
            this.checkBoxChoose.CheckedChanged += new System.EventHandler(this.checkBoxChoose_CheckedChanged);
            this.checkBoxChoose.Click += new System.EventHandler(this.checkBoxChoose_Click);
            // 
            // btnsure
            // 
            this.btnsure.Location = new System.Drawing.Point(88, 178);
            this.btnsure.Name = "btnsure";
            this.btnsure.Size = new System.Drawing.Size(60, 23);
            this.btnsure.TabIndex = 8;
            this.btnsure.Text = "确定";
            this.btnsure.UseVisualStyleBackColor = true;
            this.btnsure.Click += new System.EventHandler(this.btnsure_Click);
            // 
            // btnsec
            // 
            this.btnsec.Location = new System.Drawing.Point(166, 178);
            this.btnsec.Name = "btnsec";
            this.btnsec.Size = new System.Drawing.Size(60, 23);
            this.btnsec.TabIndex = 9;
            this.btnsec.Text = "取消";
            this.btnsec.UseVisualStyleBackColor = true;
            this.btnsec.Click += new System.EventHandler(this.btnsec_Click);
            // 
            // checkBoxChooseOutline
            // 
            this.checkBoxChooseOutline.AutoSize = true;
            this.checkBoxChooseOutline.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxChooseOutline.Location = new System.Drawing.Point(142, 153);
            this.checkBoxChooseOutline.Name = "checkBoxChooseOutline";
            this.checkBoxChooseOutline.Size = new System.Drawing.Size(90, 14);
            this.checkBoxChooseOutline.TabIndex = 10;
            this.checkBoxChooseOutline.Text = "删除安装外形";
            this.checkBoxChooseOutline.UseVisualStyleBackColor = true;
            // 
            // DeleteCavityForm
            // 
            this.AcceptButton = this.btnsure;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 213);
            this.Controls.Add(this.checkBoxChooseOutline);
            this.Controls.Add(this.btnsec);
            this.Controls.Add(this.btnsure);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteCavityForm";
            this.Text = "删除元件";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeleteCavityForm_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DeleteCavityForm_KeyPress);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckBox checkBoxChoose;
        private System.Windows.Forms.Button btnsec;
        public System.Windows.Forms.Button btnsure;
        public System.Windows.Forms.ListBox listBCavity;
        public System.Windows.Forms.CheckBox checkBoxChooseOutline;
    }
}