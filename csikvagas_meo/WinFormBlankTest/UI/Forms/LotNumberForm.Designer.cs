using System.Drawing;
namespace WinFormBlankTest.UI.Forms
{
    partial class LotNumberForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLOTIDs = new System.Windows.Forms.TextBox();
            this.cbLOTID = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Adja meg a LOT számát:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(238, 110);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Érvényes LOT azonosítók:";
            this.label2.Visible = false;
            // 
            // tbLOTIDs
            // 
            this.tbLOTIDs.Location = new System.Drawing.Point(168, 14);
            this.tbLOTIDs.Name = "tbLOTIDs";
            this.tbLOTIDs.Size = new System.Drawing.Size(102, 20);
            this.tbLOTIDs.TabIndex = 4;
            this.tbLOTIDs.Visible = false;
            // 
            // cbLOTID
            // 
            this.cbLOTID.FormattingEnabled = true;
            this.cbLOTID.Location = new System.Drawing.Point(168, 54);
            this.cbLOTID.Name = "cbLOTID";
            this.cbLOTID.Size = new System.Drawing.Size(121, 21);
            this.cbLOTID.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.ForeColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(9, 110);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 22);
            this.button2.TabIndex = 6;
            this.button2.Text = "Program bezárása";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // LotNumberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(325, 145);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cbLOTID);
            this.Controls.Add(this.tbLOTIDs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LotNumberForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LotNumberForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLOTIDs;
        private System.Windows.Forms.ComboBox cbLOTID;
        private System.Windows.Forms.Button button2;
    }
}