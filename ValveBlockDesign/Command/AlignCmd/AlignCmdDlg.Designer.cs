namespace ValveBlockDesign
{
    partial class AlignCmdDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlignCmdDlg));
            this.alignGroupBox = new System.Windows.Forms.GroupBox();
            this.checkBoxWithThisCavity = new System.Windows.Forms.CheckBox();
            this.checkBoxCavity = new System.Windows.Forms.CheckBox();
            this.alignLabel2 = new System.Windows.Forms.Label();
            this.alignLabel1 = new System.Windows.Forms.Label();
            this.alignOkButton = new System.Windows.Forms.Button();
            this.alignCancelButton = new System.Windows.Forms.Button();
            this.XDirectRadioButton = new System.Windows.Forms.RadioButton();
            this.YDirectRadioButton = new System.Windows.Forms.RadioButton();
            this.alignDirectGroupBox = new System.Windows.Forms.GroupBox();
            this.alignGroupBox.SuspendLayout();
            this.alignDirectGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // alignGroupBox
            // 
            this.alignGroupBox.Controls.Add(this.checkBoxWithThisCavity);
            this.alignGroupBox.Controls.Add(this.checkBoxCavity);
            this.alignGroupBox.Controls.Add(this.alignLabel2);
            this.alignGroupBox.Controls.Add(this.alignLabel1);
            this.alignGroupBox.Font = new System.Drawing.Font("宋体", 9F);
            this.alignGroupBox.Location = new System.Drawing.Point(12, 18);
            this.alignGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.alignGroupBox.Name = "alignGroupBox";
            this.alignGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.alignGroupBox.Size = new System.Drawing.Size(140, 156);
            this.alignGroupBox.TabIndex = 0;
            this.alignGroupBox.TabStop = false;
            this.alignGroupBox.Text = "对齐";
            // 
            // checkBoxWithThisCavity
            // 
            this.checkBoxWithThisCavity.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxWithThisCavity.AutoSize = true;
            this.checkBoxWithThisCavity.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxWithThisCavity.Image")));
            this.checkBoxWithThisCavity.Location = new System.Drawing.Point(23, 103);
            this.checkBoxWithThisCavity.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxWithThisCavity.Name = "checkBoxWithThisCavity";
            this.checkBoxWithThisCavity.Size = new System.Drawing.Size(38, 38);
            this.checkBoxWithThisCavity.TabIndex = 5;
            this.checkBoxWithThisCavity.UseVisualStyleBackColor = true;
            this.checkBoxWithThisCavity.Click += new System.EventHandler(this.OnCheckBoxWithThisCavity);
            // 
            // checkBoxCavity
            // 
            this.checkBoxCavity.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCavity.AutoSize = true;
            this.checkBoxCavity.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxCavity.Image")));
            this.checkBoxCavity.Location = new System.Drawing.Point(23, 28);
            this.checkBoxCavity.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxCavity.Name = "checkBoxCavity";
            this.checkBoxCavity.Size = new System.Drawing.Size(38, 38);
            this.checkBoxCavity.TabIndex = 4;
            this.checkBoxCavity.UseVisualStyleBackColor = true;
            this.checkBoxCavity.Click += new System.EventHandler(this.OnCheckBoxCavity);
            // 
            // alignLabel2
            // 
            this.alignLabel2.AutoSize = true;
            this.alignLabel2.Font = new System.Drawing.Font("宋体", 9F);
            this.alignLabel2.Location = new System.Drawing.Point(67, 114);
            this.alignLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.alignLabel2.Name = "alignLabel2";
            this.alignLabel2.Size = new System.Drawing.Size(53, 12);
            this.alignLabel2.TabIndex = 3;
            this.alignLabel2.Text = "对齐到孔";
            // 
            // alignLabel1
            // 
            this.alignLabel1.AutoSize = true;
            this.alignLabel1.Font = new System.Drawing.Font("宋体", 9F);
            this.alignLabel1.Location = new System.Drawing.Point(67, 36);
            this.alignLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.alignLabel1.Name = "alignLabel1";
            this.alignLabel1.Size = new System.Drawing.Size(41, 12);
            this.alignLabel1.TabIndex = 2;
            this.alignLabel1.Text = "选择孔";
            // 
            // alignOkButton
            // 
            this.alignOkButton.Font = new System.Drawing.Font("宋体", 9F);
            this.alignOkButton.Location = new System.Drawing.Point(46, 190);
            this.alignOkButton.Margin = new System.Windows.Forms.Padding(2);
            this.alignOkButton.Name = "alignOkButton";
            this.alignOkButton.Size = new System.Drawing.Size(63, 28);
            this.alignOkButton.TabIndex = 1;
            this.alignOkButton.Text = "确定";
            this.alignOkButton.UseVisualStyleBackColor = true;
            this.alignOkButton.Click += new System.EventHandler(this.AlignOkButton_Click);
            // 
            // alignCancelButton
            // 
            this.alignCancelButton.Font = new System.Drawing.Font("宋体", 9F);
            this.alignCancelButton.Location = new System.Drawing.Point(171, 190);
            this.alignCancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.alignCancelButton.Name = "alignCancelButton";
            this.alignCancelButton.Size = new System.Drawing.Size(63, 28);
            this.alignCancelButton.TabIndex = 1;
            this.alignCancelButton.Text = "取消";
            this.alignCancelButton.UseVisualStyleBackColor = true;
            this.alignCancelButton.Click += new System.EventHandler(this.AlignCancelButton_Click);
            // 
            // XDirectRadioButton
            // 
            this.XDirectRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.XDirectRadioButton.AutoSize = true;
            this.XDirectRadioButton.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.XDirectRadioButton.Enabled = false;
            this.XDirectRadioButton.Image = ((System.Drawing.Image)(resources.GetObject("XDirectRadioButton.Image")));
            this.XDirectRadioButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.XDirectRadioButton.Location = new System.Drawing.Point(23, 28);
            this.XDirectRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.XDirectRadioButton.MaximumSize = new System.Drawing.Size(40, 40);
            this.XDirectRadioButton.MinimumSize = new System.Drawing.Size(40, 40);
            this.XDirectRadioButton.Name = "XDirectRadioButton";
            this.XDirectRadioButton.Size = new System.Drawing.Size(40, 40);
            this.XDirectRadioButton.TabIndex = 2;
            this.XDirectRadioButton.TabStop = true;
            this.XDirectRadioButton.UseVisualStyleBackColor = true;
            this.XDirectRadioButton.Click += new System.EventHandler(this.OnXDirectRadioButton);
            // 
            // YDirectRadioButton
            // 
            this.YDirectRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.YDirectRadioButton.AutoSize = true;
            this.YDirectRadioButton.Enabled = false;
            this.YDirectRadioButton.Image = ((System.Drawing.Image)(resources.GetObject("YDirectRadioButton.Image")));
            this.YDirectRadioButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.YDirectRadioButton.Location = new System.Drawing.Point(23, 98);
            this.YDirectRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.YDirectRadioButton.MaximumSize = new System.Drawing.Size(40, 40);
            this.YDirectRadioButton.MinimumSize = new System.Drawing.Size(40, 40);
            this.YDirectRadioButton.Name = "YDirectRadioButton";
            this.YDirectRadioButton.Size = new System.Drawing.Size(40, 40);
            this.YDirectRadioButton.TabIndex = 3;
            this.YDirectRadioButton.TabStop = true;
            this.YDirectRadioButton.UseVisualStyleBackColor = true;
            this.YDirectRadioButton.Click += new System.EventHandler(this.OnYDirectRadioButton);
            // 
            // alignDirectGroupBox
            // 
            this.alignDirectGroupBox.Controls.Add(this.XDirectRadioButton);
            this.alignDirectGroupBox.Controls.Add(this.YDirectRadioButton);
            this.alignDirectGroupBox.Font = new System.Drawing.Font("宋体", 9F);
            this.alignDirectGroupBox.Location = new System.Drawing.Point(171, 18);
            this.alignDirectGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.alignDirectGroupBox.Name = "alignDirectGroupBox";
            this.alignDirectGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.alignDirectGroupBox.Size = new System.Drawing.Size(88, 156);
            this.alignDirectGroupBox.TabIndex = 4;
            this.alignDirectGroupBox.TabStop = false;
            this.alignDirectGroupBox.Text = "方向";
            // 
            // AlignCmdDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 229);
            this.Controls.Add(this.alignDirectGroupBox);
            this.Controls.Add(this.alignCancelButton);
            this.Controls.Add(this.alignOkButton);
            this.Controls.Add(this.alignGroupBox);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlignCmdDlg";
            this.Text = "对齐孔";
            this.Closed += new System.EventHandler(this.OnClose);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AlignCmdDlg_KeyPress);
            this.alignGroupBox.ResumeLayout(false);
            this.alignGroupBox.PerformLayout();
            this.alignDirectGroupBox.ResumeLayout(false);
            this.alignDirectGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox alignGroupBox;
        private System.Windows.Forms.Label alignLabel2;
        private System.Windows.Forms.Label alignLabel1;
        public System.Windows.Forms.Button alignOkButton;
        private System.Windows.Forms.Button alignCancelButton;
        public System.Windows.Forms.CheckBox checkBoxWithThisCavity;
        public System.Windows.Forms.CheckBox checkBoxCavity;
        public System.Windows.Forms.RadioButton XDirectRadioButton;
        public System.Windows.Forms.RadioButton YDirectRadioButton;
        private System.Windows.Forms.GroupBox alignDirectGroupBox;

    }
}