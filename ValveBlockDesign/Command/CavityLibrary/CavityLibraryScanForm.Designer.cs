namespace ValveBlockDesign
{
    partial class CavityLibraryScanForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CavityLibraryScanForm));
            this.tb1 = new System.Windows.Forms.TextBox();
            this.lablibpath = new System.Windows.Forms.Label();
            this.groBface = new System.Windows.Forms.GroupBox();
            this.lv2 = new System.Windows.Forms.ListView();
            this.coH3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.coH4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsure = new System.Windows.Forms.Button();
            this.groBlibrary = new System.Windows.Forms.GroupBox();
            this.lv1 = new System.Windows.Forms.ListView();
            this.coH1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.coH2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnscan = new System.Windows.Forms.Button();
            this.btnbrowse = new System.Windows.Forms.Button();
            this.groBface.SuspendLayout();
            this.groBlibrary.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb1
            // 
            this.tb1.Font = new System.Drawing.Font("宋体", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb1.Location = new System.Drawing.Point(80, 5);
            this.tb1.Name = "tb1";
            this.tb1.Size = new System.Drawing.Size(258, 18);
            this.tb1.TabIndex = 29;
            // 
            // lablibpath
            // 
            this.lablibpath.AutoSize = true;
            this.lablibpath.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lablibpath.Location = new System.Drawing.Point(13, 9);
            this.lablibpath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lablibpath.Name = "lablibpath";
            this.lablibpath.Size = new System.Drawing.Size(60, 10);
            this.lablibpath.TabIndex = 25;
            this.lablibpath.Text = "特征库路径";
            this.lablibpath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groBface
            // 
            this.groBface.Controls.Add(this.lv2);
            this.groBface.Controls.Add(this.btnsure);
            this.groBface.Font = new System.Drawing.Font("宋体", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groBface.Location = new System.Drawing.Point(222, 33);
            this.groBface.Name = "groBface";
            this.groBface.Size = new System.Drawing.Size(198, 361);
            this.groBface.TabIndex = 27;
            this.groBface.TabStop = false;
            this.groBface.Text = "安装面及孔编号";
            // 
            // lv2
            // 
            this.lv2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.coH3,
            this.coH4});
            this.lv2.FullRowSelect = true;
            this.lv2.GridLines = true;
            this.lv2.Location = new System.Drawing.Point(6, 17);
            this.lv2.Name = "lv2";
            this.lv2.Size = new System.Drawing.Size(186, 310);
            this.lv2.TabIndex = 20;
            this.lv2.UseCompatibleStateImageBehavior = false;
            this.lv2.View = System.Windows.Forms.View.Details;
            this.lv2.DoubleClick += new System.EventHandler(this.btnsure_Click);
            // 
            // coH3
            // 
            this.coH3.Text = "编号";
            this.coH3.Width = 94;
            // 
            // coH4
            // 
            this.coH4.Text = "索引编号";
            this.coH4.Width = 82;
            // 
            // btnsure
            // 
            this.btnsure.Font = new System.Drawing.Font("宋体", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnsure.Location = new System.Drawing.Point(115, 328);
            this.btnsure.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnsure.Name = "btnsure";
            this.btnsure.Size = new System.Drawing.Size(73, 27);
            this.btnsure.TabIndex = 20;
            this.btnsure.Text = "元件详细信息";
            this.btnsure.UseVisualStyleBackColor = true;
            this.btnsure.Click += new System.EventHandler(this.btnsure_Click);
            // 
            // groBlibrary
            // 
            this.groBlibrary.Controls.Add(this.lv1);
            this.groBlibrary.Controls.Add(this.btnscan);
            this.groBlibrary.Font = new System.Drawing.Font("宋体", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groBlibrary.Location = new System.Drawing.Point(12, 32);
            this.groBlibrary.Name = "groBlibrary";
            this.groBlibrary.Size = new System.Drawing.Size(201, 362);
            this.groBlibrary.TabIndex = 28;
            this.groBlibrary.TabStop = false;
            this.groBlibrary.Text = "库类型浏览";
            // 
            // lv1
            // 
            this.lv1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.coH1,
            this.coH2});
            this.lv1.FullRowSelect = true;
            this.lv1.GridLines = true;
            this.lv1.Location = new System.Drawing.Point(10, 17);
            this.lv1.Name = "lv1";
            this.lv1.Size = new System.Drawing.Size(185, 311);
            this.lv1.TabIndex = 19;
            this.lv1.UseCompatibleStateImageBehavior = false;
            this.lv1.View = System.Windows.Forms.View.Details;
            this.lv1.DoubleClick += new System.EventHandler(this.btnscan_Click);
            // 
            // coH1
            // 
            this.coH1.Text = "索引编号";
            this.coH1.Width = 71;
            // 
            // coH2
            // 
            this.coH2.Text = "名称";
            this.coH2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.coH2.Width = 110;
            // 
            // btnscan
            // 
            this.btnscan.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnscan.Location = new System.Drawing.Point(136, 331);
            this.btnscan.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnscan.Name = "btnscan";
            this.btnscan.Size = new System.Drawing.Size(58, 24);
            this.btnscan.TabIndex = 19;
            this.btnscan.Text = "浏览库";
            this.btnscan.UseVisualStyleBackColor = true;
            this.btnscan.Click += new System.EventHandler(this.btnscan_Click);
            // 
            // btnbrowse
            // 
            this.btnbrowse.Font = new System.Drawing.Font("宋体", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnbrowse.Location = new System.Drawing.Point(359, 2);
            this.btnbrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnbrowse.Name = "btnbrowse";
            this.btnbrowse.Size = new System.Drawing.Size(51, 24);
            this.btnbrowse.TabIndex = 26;
            this.btnbrowse.Text = "浏览";
            this.btnbrowse.UseVisualStyleBackColor = true;
            this.btnbrowse.Click += new System.EventHandler(this.btnbrowse_Click);
            // 
            // CavityLibraryScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 399);
            this.Controls.Add(this.tb1);
            this.Controls.Add(this.lablibpath);
            this.Controls.Add(this.groBface);
            this.Controls.Add(this.groBlibrary);
            this.Controls.Add(this.btnbrowse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CavityLibraryScanForm";
            this.Text = "浏览元件";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CavityLibraryScanForm_FormClosed);
            this.Load += new System.EventHandler(this.CavityLibraryScanForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CavityLibraryScanForm_KeyPress);
            this.groBface.ResumeLayout(false);
            this.groBlibrary.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lablibpath;
        private System.Windows.Forms.GroupBox groBface;
        private System.Windows.Forms.ListView lv2;
        private System.Windows.Forms.ColumnHeader coH3;
        private System.Windows.Forms.ColumnHeader coH4;
        private System.Windows.Forms.Button btnsure;
        private System.Windows.Forms.GroupBox groBlibrary;
        private System.Windows.Forms.ColumnHeader coH1;
        private System.Windows.Forms.ColumnHeader coH2;
        private System.Windows.Forms.Button btnscan;
        private System.Windows.Forms.Button btnbrowse;
        public System.Windows.Forms.ListView lv1;
        public System.Windows.Forms.TextBox tb1;
    }
}