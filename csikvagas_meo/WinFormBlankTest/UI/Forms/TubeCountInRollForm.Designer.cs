﻿using System.Drawing;
namespace WinFormBlankTest.UI.Forms
{
    partial class TubeCountInRollForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TubeCountInRollForm));
            this.label2 = new System.Windows.Forms.Label();
            this.lbTubeCount = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.warningLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLOT = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbTemperature = new System.Windows.Forms.TextBox();
            this.tbHumidity = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.cbTubeCount = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bookman Old Style", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "A Rollban található tubusok száma:";
            // 
            // lbTubeCount
            // 
            this.lbTubeCount.Location = new System.Drawing.Point(257, 60);
            this.lbTubeCount.Name = "lbTubeCount";
            this.lbTubeCount.Size = new System.Drawing.Size(94, 20);
            this.lbTubeCount.TabIndex = 2;
            this.lbTubeCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbTubeCount_KeyDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(368, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 22);
            this.button1.TabIndex = 3;
            this.button1.Text = "Tovább";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.ForeColor = System.Drawing.Color.Red;
            this.warningLabel.Location = new System.Drawing.Point(132, 87);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(187, 13);
            this.warningLabel.TabIndex = 4;
            this.warningLabel.Text = "A minimális tubus mennyiség: 19 tubus";
            this.warningLabel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bookman Old Style", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "LOT azonosító:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tbLOT
            // 
            this.tbLOT.Location = new System.Drawing.Point(123, 33);
            this.tbLOT.Name = "tbLOT";
            this.tbLOT.Size = new System.Drawing.Size(100, 20);
            this.tbLOT.TabIndex = 6;
            this.tbLOT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLOT_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bookman Old Style", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "Hőmérséklet:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bookman Old Style", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(230, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "Páratartalom:";
            // 
            // tbTemperature
            // 
            this.tbTemperature.Location = new System.Drawing.Point(112, 6);
            this.tbTemperature.Name = "tbTemperature";
            this.tbTemperature.Size = new System.Drawing.Size(87, 20);
            this.tbTemperature.TabIndex = 9;
            // 
            // tbHumidity
            // 
            this.tbHumidity.Location = new System.Drawing.Point(332, 3);
            this.tbHumidity.Name = "tbHumidity";
            this.tbHumidity.Size = new System.Drawing.Size(87, 20);
            this.tbHumidity.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(200, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "°C";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(425, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "RH%";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button2.ForeColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(15, 102);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Program bezárása";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbTubeCount
            // 
            this.cbTubeCount.FormattingEnabled = true;
            this.cbTubeCount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "",
            ""});
            this.cbTubeCount.Location = new System.Drawing.Point(368, 59);
            this.cbTubeCount.Name = "cbTubeCount";
            this.cbTubeCount.Size = new System.Drawing.Size(73, 21);
            this.cbTubeCount.TabIndex = 14;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(233, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(127, 22);
            this.button3.TabIndex = 15;
            this.button3.Text = "LOT törlése";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // TubeCountInRollForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 127);
            this.ControlBox = false;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cbTubeCount);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbHumidity);
            this.Controls.Add(this.tbTemperature);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLOT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbTubeCount);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TubeCountInRollForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LotCountForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox lbTubeCount;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLOT;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbTemperature;
        private System.Windows.Forms.TextBox tbHumidity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cbTubeCount;
        private System.Windows.Forms.Button button3;
    }
}