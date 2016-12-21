using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace WinFormBlankTest
{
    partial class UserPanel
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.tbBarcode2 = new System.Windows.Forms.TextBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.MasterChb = new System.Windows.Forms.CheckBox();
            this.lbMeasured = new Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(14, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(250, 180);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Silver;
            this.button1.Location = new System.Drawing.Point(100, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 1;
            this.button1.TabStop = false;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(130, 227);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Append";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // tbBarcode
            // 
            this.tbBarcode.Location = new System.Drawing.Point(150, 225);
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Size = new System.Drawing.Size(100, 20);
            this.tbBarcode.TabIndex = 0;
            this.tbBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBarcode_KeyDown);
            // 
            // tbBarcode2
            // 
            this.tbBarcode2.Location = new System.Drawing.Point(30, 225);
            this.tbBarcode2.Name = "tbBarcode2";
            this.tbBarcode2.Size = new System.Drawing.Size(100, 20);
            this.tbBarcode2.TabIndex = 0;
            this.tbBarcode2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBarcode2_KeyDown);
            // 
            // lbMeasured
            // 
            this.lbMeasured.Location = new System.Drawing.Point(100, 190);
            this.lbMeasured.Name = "lbMeasured";
            this.lbMeasured.Size = new System.Drawing.Size(10, 20);
            this.lbMeasured.TabIndex = 0;
            this.lbMeasured.Text = "Itt van";
            Image image1 = Properties.Resources._801;
            this.lbMeasured.Image = image1;
          
            // 
            // errorLabel
            // 
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(100, 212);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(150, 20);
            this.errorLabel.TabIndex = 1;
            this.errorLabel.Text = "Olvassa be a kódot";
            this.errorLabel.Visible = false;
            // 
            // MasterChb
            // 
            this.MasterChb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MasterChb.BackColor = System.Drawing.SystemColors.Window;
            this.MasterChb.ForeColor = System.Drawing.Color.Gray;
            this.MasterChb.Location = new System.Drawing.Point(200, 200);
            this.MasterChb.Name = "MasterChb";
            this.MasterChb.Size = new System.Drawing.Size(104, 24);
            this.MasterChb.TabIndex = 2;
            this.MasterChb.Text = "MasterLOT";
            this.MasterChb.UseVisualStyleBackColor = false;
            this.MasterChb.Click += new System.EventHandler(this.MasterChb_Click);
            // 
            // UserPanel
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbBarcode);
            this.Controls.Add(this.tbBarcode2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.MasterChb);
            this.ForeColor = System.Drawing.Color.Black;
            this.Size = new System.Drawing.Size(284, 262);
            this.Text = "UserPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void MasterChb_Click(object sender, EventArgs e)
        {
            this.dev.MasterLot = MasterChb.Checked;
        }
        
        void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            tbBarcode.Text = e.LinkText;
        }
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public System.Windows.Forms.RichTextBox richTextBox1;
        public System.Windows.Forms.TextBox tbBarcode;
        public System.Windows.Forms.TextBox tbBarcode2;
        public Label errorLabel;
        public CheckBox MasterChb;
        public Label lbMeasured;
        #endregion
    }
}