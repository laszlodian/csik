namespace WinFormBlankTest.UI.Forms
{
    partial class CalculatedColumnValuesForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.colname = new System.Windows.Forms.TextBox();
            this.colsum = new System.Windows.Forms.TextBox();
            this.coltype = new System.Windows.Forms.TextBox();
            this.colavg = new System.Windows.Forms.TextBox();
            this.colstddev = new System.Windows.Forms.TextBox();
            this.colCV = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.acceptcols = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.not_acceptcol = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.timeSpendedCol = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, -4);
            this.label1.MaximumSize = new System.Drawing.Size(270, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 2);
            this.label1.TabIndex = 0;
            this.label1.Text = "A {0} számú LOT-ból a {1} számú roll {2} típusú mérése következik";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Bookman Old Style", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(226, 69);
            this.label2.TabIndex = 1;
            this.label2.Text = "A kiválasztott oszlopból kiszámolt adatok";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightGray;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(2, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Oszlop neve:";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightGray;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(4, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Oszlop típusa:";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightGray;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(4, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(226, 21);
            this.label5.TabIndex = 4;
            this.label5.Text = "A cellák értékeinek összege:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightGray;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(1, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(225, 19);
            this.label6.TabIndex = 5;
            this.label6.Text = "A cellák értékeinek átlaga:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.LightGray;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(1, 263);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(225, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "A cellák értékeinek szórása:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.LightGray;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(1, 314);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(225, 44);
            this.label8.TabIndex = 7;
            this.label8.Text = "A cellák értékeinek CV százaléka:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // colname
            // 
            this.colname.Location = new System.Drawing.Point(130, 67);
            this.colname.Name = "colname";
            this.colname.Size = new System.Drawing.Size(96, 20);
            this.colname.TabIndex = 8;
            // 
            // colsum
            // 
            this.colsum.Location = new System.Drawing.Point(60, 201);
            this.colsum.Name = "colsum";
            this.colsum.Size = new System.Drawing.Size(96, 20);
            this.colsum.TabIndex = 9;
            // 
            // coltype
            // 
            this.coltype.Location = new System.Drawing.Point(130, 87);
            this.coltype.Name = "coltype";
            this.coltype.Size = new System.Drawing.Size(96, 20);
            this.coltype.TabIndex = 10;
            // 
            // colavg
            // 
            this.colavg.Location = new System.Drawing.Point(60, 240);
            this.colavg.Name = "colavg";
            this.colavg.Size = new System.Drawing.Size(96, 20);
            this.colavg.TabIndex = 11;
            // 
            // colstddev
            // 
            this.colstddev.Location = new System.Drawing.Point(60, 289);
            this.colstddev.Name = "colstddev";
            this.colstddev.Size = new System.Drawing.Size(96, 20);
            this.colstddev.TabIndex = 12;
            // 
            // colCV
            // 
            this.colCV.Location = new System.Drawing.Point(60, 361);
            this.colCV.Name = "colCV";
            this.colCV.Size = new System.Drawing.Size(96, 20);
            this.colCV.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightGray;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(0, 384);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(226, 39);
            this.label9.TabIndex = 14;
            this.label9.Text = "A cellák Megfelelő értékeinek száma:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // acceptcols
            // 
            this.acceptcols.Location = new System.Drawing.Point(55, 426);
            this.acceptcols.Name = "acceptcols";
            this.acceptcols.Size = new System.Drawing.Size(96, 20);
            this.acceptcols.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.LightGray;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label10.Location = new System.Drawing.Point(2, 449);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(224, 39);
            this.label10.TabIndex = 16;
            this.label10.Text = "A cellák Nem Megfelelő értékeinek száma:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // not_acceptcol
            // 
            this.not_acceptcol.Location = new System.Drawing.Point(56, 491);
            this.not_acceptcol.Name = "not_acceptcol";
            this.not_acceptcol.Size = new System.Drawing.Size(96, 20);
            this.not_acceptcol.TabIndex = 17;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.LightGray;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label11.Location = new System.Drawing.Point(2, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(224, 37);
            this.label11.TabIndex = 18;
            this.label11.Text = "Kiválasztott táblázat méréseinek lefutsi ideje:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timeSpendedCol
            // 
            this.timeSpendedCol.Location = new System.Drawing.Point(60, 154);
            this.timeSpendedCol.Name = "timeSpendedCol";
            this.timeSpendedCol.Size = new System.Drawing.Size(96, 20);
            this.timeSpendedCol.TabIndex = 19;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.button1.Location = new System.Drawing.Point(167, 488);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CalculatedColumnValuesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BackgroundImage = global::WinFormBlankTest.Properties.Resources.elektronik;
            this.ClientSize = new System.Drawing.Size(226, 509);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.timeSpendedCol);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.not_acceptcol);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.acceptcols);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.colCV);
            this.Controls.Add(this.colstddev);
            this.Controls.Add(this.colavg);
            this.Controls.Add(this.coltype);
            this.Controls.Add(this.colsum);
            this.Controls.Add(this.colname);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CalculatedColumnValuesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kiszámolt adatok";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox colname;
        private System.Windows.Forms.TextBox colsum;
        private System.Windows.Forms.TextBox coltype;
        private System.Windows.Forms.TextBox colavg;
        private System.Windows.Forms.TextBox colstddev;
        private System.Windows.Forms.TextBox colCV;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox acceptcols;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox not_acceptcol;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox timeSpendedCol;
        private System.Windows.Forms.Button button1;
    }
}