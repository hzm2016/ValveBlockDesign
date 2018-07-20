namespace ValveBlockDesign
{
    partial class MoveCmdDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoveCmdDlg));
            this.chooseGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxChoose = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.xOffsetText = new System.Windows.Forms.TextBox();
            this.yOffsetText = new System.Windows.Forms.TextBox();
            this.moveCancelButton = new System.Windows.Forms.Button();
            this.moveOkButton = new System.Windows.Forms.Button();
            this.chooseGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // chooseGroupBox
            // 
            this.chooseGroupBox.Controls.Add(this.label1);
            this.chooseGroupBox.Controls.Add(this.checkBoxChoose);
            this.chooseGroupBox.Font = new System.Drawing.Font("宋体", 9F);
            this.chooseGroupBox.Location = new System.Drawing.Point(12, 11);
            this.chooseGroupBox.Name = "chooseGroupBox";
            this.chooseGroupBox.Size = new System.Drawing.Size(120, 87);
            this.chooseGroupBox.TabIndex = 0;
            this.chooseGroupBox.TabStop = false;
            this.chooseGroupBox.Text = "选择";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择孔";
            // 
            // checkBoxChoose
            // 
            this.checkBoxChoose.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxChoose.AutoSize = true;
            this.checkBoxChoose.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxChoose.Image")));
            this.checkBoxChoose.Location = new System.Drawing.Point(17, 30);
            this.checkBoxChoose.Name = "checkBoxChoose";
            this.checkBoxChoose.Size = new System.Drawing.Size(38, 38);
            this.checkBoxChoose.TabIndex = 0;
            this.checkBoxChoose.UseVisualStyleBackColor = true;
            this.checkBoxChoose.Click += new System.EventHandler(this.CheckBoxChoose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 10.5F);
            this.label2.Location = new System.Drawing.Point(153, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "△X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 10.5F);
            this.label3.Location = new System.Drawing.Point(153, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "△Y";
            // 
            // xOffsetText
            // 
            this.xOffsetText.Location = new System.Drawing.Point(189, 23);
            this.xOffsetText.Name = "xOffsetText";
            this.xOffsetText.Size = new System.Drawing.Size(73, 21);
            this.xOffsetText.TabIndex = 2;
            this.xOffsetText.Text = "0";
            this.xOffsetText.TextChanged += new System.EventHandler(this.OnChangeXOffsetEdit);
            // 
            // yOffsetText
            // 
            this.yOffsetText.Location = new System.Drawing.Point(189, 63);
            this.yOffsetText.Name = "yOffsetText";
            this.yOffsetText.Size = new System.Drawing.Size(73, 21);
            this.yOffsetText.TabIndex = 2;
            this.yOffsetText.Text = "0";
            this.yOffsetText.TextChanged += new System.EventHandler(this.OnChangeYOffsetEdit);
            // 
            // moveCancelButton
            // 
            this.moveCancelButton.Font = new System.Drawing.Font("宋体", 9F);
            this.moveCancelButton.Location = new System.Drawing.Point(175, 113);
            this.moveCancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.moveCancelButton.Name = "moveCancelButton";
            this.moveCancelButton.Size = new System.Drawing.Size(62, 25);
            this.moveCancelButton.TabIndex = 4;
            this.moveCancelButton.Text = "取消";
            this.moveCancelButton.UseVisualStyleBackColor = true;
            this.moveCancelButton.Click += new System.EventHandler(this.MoveCancelButton_Click);
            // 
            // moveOkButton
            // 
            this.moveOkButton.Font = new System.Drawing.Font("宋体", 9F);
            this.moveOkButton.Location = new System.Drawing.Point(46, 113);
            this.moveOkButton.Margin = new System.Windows.Forms.Padding(2);
            this.moveOkButton.Name = "moveOkButton";
            this.moveOkButton.Size = new System.Drawing.Size(63, 25);
            this.moveOkButton.TabIndex = 3;
            this.moveOkButton.Text = "确定";
            this.moveOkButton.UseVisualStyleBackColor = true;
            this.moveOkButton.Click += new System.EventHandler(this.MoveOkButton_Click);
            // 
            // MoveCmdDlg
            // 
            this.AcceptButton = this.moveOkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 149);
            this.Controls.Add(this.moveCancelButton);
            this.Controls.Add(this.moveOkButton);
            this.Controls.Add(this.yOffsetText);
            this.Controls.Add(this.xOffsetText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chooseGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MoveCmdDlg";
            this.Text = "移动孔";
            this.Closed += new System.EventHandler(this.OnClose);
            this.Load += new System.EventHandler(this.MoveCmdDlg_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MoveCmdDlg_KeyPress);
            this.chooseGroupBox.ResumeLayout(false);
            this.chooseGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox chooseGroupBox;
        public System.Windows.Forms.CheckBox checkBoxChoose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox xOffsetText;
        public System.Windows.Forms.TextBox yOffsetText;
        private System.Windows.Forms.Button moveCancelButton;
        public System.Windows.Forms.Button moveOkButton;
    }
}