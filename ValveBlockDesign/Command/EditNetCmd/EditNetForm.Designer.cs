namespace ValveBlockDesign
{
    partial class EditNetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditNetForm));
            this.groupBport = new System.Windows.Forms.GroupBox();
            this.dataportNET = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnedit = new System.Windows.Forms.Button();
            this.comboBNETs = new System.Windows.Forms.ComboBox();
            this.btnsure = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxChoose = new System.Windows.Forms.CheckBox();
            this.groupBport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataportNET)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBport
            // 
            this.groupBport.Controls.Add(this.dataportNET);
            this.groupBport.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBport.Location = new System.Drawing.Point(12, 89);
            this.groupBport.Name = "groupBport";
            this.groupBport.Size = new System.Drawing.Size(217, 224);
            this.groupBport.TabIndex = 0;
            this.groupBport.TabStop = false;
            // 
            // dataportNET
            // 
            this.dataportNET.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataportNET.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataportNET.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataportNET.Location = new System.Drawing.Point(6, 18);
            this.dataportNET.MultiSelect = false;
            this.dataportNET.Name = "dataportNET";
            this.dataportNET.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataportNET.RowTemplate.Height = 23;
            this.dataportNET.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataportNET.Size = new System.Drawing.Size(200, 200);
            this.dataportNET.TabIndex = 1;
            this.dataportNET.SelectionChanged += new System.EventHandler(this.dataportNET_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "油孔名称";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "网络名称";
            this.Column2.Name = "Column2";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnedit);
            this.groupBox2.Controls.Add(this.comboBNETs);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(12, 319);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(217, 54);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnedit
            // 
            this.btnedit.Location = new System.Drawing.Point(146, 18);
            this.btnedit.Name = "btnedit";
            this.btnedit.Size = new System.Drawing.Size(60, 23);
            this.btnedit.TabIndex = 1;
            this.btnedit.Text = "修改";
            this.btnedit.UseVisualStyleBackColor = true;
            this.btnedit.Click += new System.EventHandler(this.btnedit_Click);
            // 
            // comboBNETs
            // 
            this.comboBNETs.FormattingEnabled = true;
            this.comboBNETs.Items.AddRange(new object[] {
            "NET1",
            "NET2",
            "NET3",
            "NET4",
            "NET5",
            "NET6",
            "NET7",
            "NET8",
            "NET9",
            "NET10",
            "NET11",
            "NET12",
            "NET13",
            "NET14",
            "NULL"});
            this.comboBNETs.Location = new System.Drawing.Point(15, 20);
            this.comboBNETs.Name = "comboBNETs";
            this.comboBNETs.Size = new System.Drawing.Size(90, 18);
            this.comboBNETs.TabIndex = 0;
            this.comboBNETs.TextChanged += new System.EventHandler(this.comboBNETs_TextChanged);
            // 
            // btnsure
            // 
            this.btnsure.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnsure.Location = new System.Drawing.Point(158, 379);
            this.btnsure.Name = "btnsure";
            this.btnsure.Size = new System.Drawing.Size(60, 23);
            this.btnsure.TabIndex = 2;
            this.btnsure.Text = "完成";
            this.btnsure.UseVisualStyleBackColor = true;
            this.btnsure.Click += new System.EventHandler(this.btnsure_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxChoose);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(217, 68);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "选择元件";
            // 
            // checkBoxChoose
            // 
            this.checkBoxChoose.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxChoose.AutoSize = true;
            this.checkBoxChoose.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxChoose.Image")));
            this.checkBoxChoose.Location = new System.Drawing.Point(26, 20);
            this.checkBoxChoose.Name = "checkBoxChoose";
            this.checkBoxChoose.Size = new System.Drawing.Size(38, 38);
            this.checkBoxChoose.TabIndex = 2;
            this.checkBoxChoose.UseVisualStyleBackColor = true;
            this.checkBoxChoose.Click += new System.EventHandler(this.checkBoxChoose_Click);
            // 
            // EditNetForm
            // 
            this.AcceptButton = this.btnsure;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 406);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnsure);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditNetForm";
            this.ShowIcon = false;
            this.Text = "修改油孔网络";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditNetForm_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditNetForm_KeyPress);
            this.groupBport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataportNET)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataportNET;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnsure;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.CheckBox checkBoxChoose;
        public System.Windows.Forms.Button btnedit;
        public System.Windows.Forms.ComboBox comboBNETs;
        public System.Windows.Forms.GroupBox groupBport;
    }
}