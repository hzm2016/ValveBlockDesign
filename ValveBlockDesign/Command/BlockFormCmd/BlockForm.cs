using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Inventor;

namespace ValveBlockDesign
{
    internal class BlockForm : Form
    {
        private System.Windows.Forms.Button blockCancelButton;
        public System.Windows.Forms.Button blockGenerateButton;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.Label Heightlabel;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Label Widthlabel;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.Label Lengthlabel;
        private System.Windows.Forms.GroupBox paramGroupBox;

        private Inventor.Application m_inventorApplication;
        private BlockFormCmd m_blockFormCmd;

        public string m_blockLength;
        public string m_blockWidth;
        public string m_blockHeight;

        public BlockForm(Inventor.Application application, BlockFormCmd blockFormCmd)
        {
            m_inventorApplication = application;
            m_blockFormCmd = blockFormCmd;
            InitializeComponent();
        }


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlockForm));
            this.blockCancelButton = new System.Windows.Forms.Button();
            this.blockGenerateButton = new System.Windows.Forms.Button();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.Heightlabel = new System.Windows.Forms.Label();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.Widthlabel = new System.Windows.Forms.Label();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.Lengthlabel = new System.Windows.Forms.Label();
            this.paramGroupBox = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // blockCancelButton
            // 
            this.blockCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.blockCancelButton.Location = new System.Drawing.Point(146, 178);
            this.blockCancelButton.Name = "blockCancelButton";
            this.blockCancelButton.Size = new System.Drawing.Size(76, 23);
            this.blockCancelButton.TabIndex = 40;
            this.blockCancelButton.Text = "取消";
            this.blockCancelButton.UseVisualStyleBackColor = true;
            this.blockCancelButton.Click += new System.EventHandler(this.BlockCancelButton_Click);
            // 
            // blockGenerateButton
            // 
            this.blockGenerateButton.Enabled = false;
            this.blockGenerateButton.Location = new System.Drawing.Point(26, 178);
            this.blockGenerateButton.Name = "blockGenerateButton";
            this.blockGenerateButton.Size = new System.Drawing.Size(78, 23);
            this.blockGenerateButton.TabIndex = 39;
            this.blockGenerateButton.Text = "确定";
            this.blockGenerateButton.UseVisualStyleBackColor = true;
            this.blockGenerateButton.Click += new System.EventHandler(this.BlockGenerateButton_Click);
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.AcceptsTab = true;
            this.textBoxHeight.ForeColor = System.Drawing.Color.Red;
            this.textBoxHeight.Location = new System.Drawing.Point(98, 126);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(95, 21);
            this.textBoxHeight.TabIndex = 38;
            this.textBoxHeight.Text = "0";
            this.textBoxHeight.TextChanged += new System.EventHandler(this.OnChangeHeightEdit);
            // 
            // Heightlabel
            // 
            this.Heightlabel.AutoSize = true;
            this.Heightlabel.Location = new System.Drawing.Point(53, 126);
            this.Heightlabel.Name = "Heightlabel";
            this.Heightlabel.Size = new System.Drawing.Size(17, 12);
            this.Heightlabel.TabIndex = 37;
            this.Heightlabel.Text = "高";
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.AcceptsTab = true;
            this.textBoxWidth.ForeColor = System.Drawing.Color.Red;
            this.textBoxWidth.Location = new System.Drawing.Point(98, 80);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(95, 21);
            this.textBoxWidth.TabIndex = 36;
            this.textBoxWidth.Text = "0";
            this.textBoxWidth.TextChanged += new System.EventHandler(this.OnChangeWidthEdit);
            // 
            // Widthlabel
            // 
            this.Widthlabel.AutoSize = true;
            this.Widthlabel.Location = new System.Drawing.Point(53, 83);
            this.Widthlabel.Name = "Widthlabel";
            this.Widthlabel.Size = new System.Drawing.Size(17, 12);
            this.Widthlabel.TabIndex = 35;
            this.Widthlabel.Text = "宽";
            // 
            // textBoxLength
            // 
            this.textBoxLength.AcceptsTab = true;
            this.textBoxLength.ForeColor = System.Drawing.Color.Red;
            this.textBoxLength.Location = new System.Drawing.Point(98, 36);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.Size = new System.Drawing.Size(95, 21);
            this.textBoxLength.TabIndex = 34;
            this.textBoxLength.Text = "0";
            this.textBoxLength.TextChanged += new System.EventHandler(this.OnChangeLengthEdit);
            // 
            // Lengthlabel
            // 
            this.Lengthlabel.AutoSize = true;
            this.Lengthlabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lengthlabel.Location = new System.Drawing.Point(53, 39);
            this.Lengthlabel.Name = "Lengthlabel";
            this.Lengthlabel.Size = new System.Drawing.Size(17, 12);
            this.Lengthlabel.TabIndex = 32;
            this.Lengthlabel.Text = "长";
            // 
            // paramGroupBox
            // 
            this.paramGroupBox.Location = new System.Drawing.Point(25, 13);
            this.paramGroupBox.Name = "paramGroupBox";
            this.paramGroupBox.Size = new System.Drawing.Size(197, 152);
            this.paramGroupBox.TabIndex = 41;
            this.paramGroupBox.TabStop = false;
            this.paramGroupBox.Text = "参数";
            // 
            // BlockForm
            // 
            this.AcceptButton = this.blockGenerateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 216);
            this.Controls.Add(this.blockCancelButton);
            this.Controls.Add(this.blockGenerateButton);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.Heightlabel);
            this.Controls.Add(this.textBoxWidth);
            this.Controls.Add(this.Widthlabel);
            this.Controls.Add(this.textBoxLength);
            this.Controls.Add(this.Lengthlabel);
            this.Controls.Add(this.paramGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlockForm";
            this.Text = "生成块";
            this.Closed += new System.EventHandler(this.OnClose);
            this.Load += new System.EventHandler(this.BlockForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BlockForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private void BlockForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            //"Esc" key cancels command
            if (e.KeyChar == 27)
            {
                m_blockFormCmd.StopCommand();
            }
        }

        private void OnClose(object sender, EventArgs e)
        {
            m_blockFormCmd.StopCommand();
        }

        private void BlockCancelButton_Click(object sender, EventArgs e)
        {
            m_blockFormCmd.StopCommand();
        }

        private void BlockGenerateButton_Click(object sender, EventArgs e)
        {
            m_blockFormCmd.ExecuteCommand();
        }

        private bool ValidateDoubleInput(string input)
        {
            try
            {
                double num = double.Parse(input);
                if (num <= 0)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool AllParametersAreValid()
        {
            return (
                (textBoxLength.ForeColor != System.Drawing.Color.Red)
                &&
                (textBoxWidth.ForeColor != System.Drawing.Color.Red)
                &&
                (textBoxHeight.ForeColor != System.Drawing.Color.Red)
                );
        }

        private void OnChangeLengthEdit(object sender, System.EventArgs e)
        {
           // m_blockFormCmd = new BlockFormCmd();
            double dLength = m_blockFormCmd.GetValueFromExpression(textBoxLength.Text.Trim());

            //reject invalid input
            if (!ValidateDoubleInput(dLength.ToString()))
            {
                textBoxLength.ForeColor = System.Drawing.Color.Red;
                blockGenerateButton.Enabled = false;
            }
            else
            {
                textBoxLength.ForeColor = System.Drawing.Color.Black;
                m_blockLength = textBoxLength.Text;
                m_blockFormCmd.UpdateCommandStatus();
            }
 
        }

        private void OnChangeWidthEdit(object sender, System.EventArgs e)
        {
           // m_blockFormCmd = new BlockFormCmd();
            double dWidth = m_blockFormCmd.GetValueFromExpression(textBoxWidth.Text.Trim());

            //reject invalid input
            if (!ValidateDoubleInput(dWidth.ToString()))
            {
                textBoxWidth.ForeColor = System.Drawing.Color.Red;
                blockGenerateButton.Enabled = false;
            }
            else
            {
                textBoxWidth.ForeColor = System.Drawing.Color.Black;
                m_blockWidth = textBoxWidth.Text;
                m_blockFormCmd.UpdateCommandStatus();
            }

        }

        private void OnChangeHeightEdit(object sender, System.EventArgs e)
        {
            //m_blockFormCmd = new BlockFormCmd();
            double dHeight = m_blockFormCmd.GetValueFromExpression(textBoxHeight.Text.Trim());

            //reject invalid input
            if (!ValidateDoubleInput(dHeight.ToString()))
            {
                textBoxHeight.ForeColor = System.Drawing.Color.Red;
                blockGenerateButton.Enabled = false;
            }
            else
            {
                textBoxHeight.ForeColor = System.Drawing.Color.Black;
                m_blockHeight = textBoxHeight.Text;
                m_blockFormCmd.UpdateCommandStatus();
            }

        }

        private void BlockForm_Load(object sender, EventArgs e)
        {

        }

 
    }

}

