namespace e77.MeasureBase.Reporting
{
    partial class DateTimeIntarvalForm
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
            this.components = new System.ComponentModel.Container();
            this._monthCalendarStart = new System.Windows.Forms.MonthCalendar();
            this._monthCalendarEnd = new System.Windows.Forms.MonthCalendar();
            this._lbResult = new System.Windows.Forms.Label();
            this._ButtonOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._lvPredefined = new System.Windows.Forms.ListView();
            this._tbStart = new System.Windows.Forms.TextBox();
            this._tbEnd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _monthCalendarStart
            // 
            this._monthCalendarStart.Location = new System.Drawing.Point(126, 72);
            this._monthCalendarStart.MaxSelectionCount = 1;
            this._monthCalendarStart.Name = "_monthCalendarStart";
            this._monthCalendarStart.TabIndex = 0;
            this._monthCalendarStart.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this._monthCalendarStart_DateChanged);
            // 
            // _monthCalendarEnd
            // 
            this._monthCalendarEnd.Location = new System.Drawing.Point(322, 72);
            this._monthCalendarEnd.MaxSelectionCount = 1;
            this._monthCalendarEnd.Name = "_monthCalendarEnd";
            this._monthCalendarEnd.TabIndex = 1;
            this._monthCalendarEnd.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this._monthCalendarEnd_DateChanged);
            // 
            // _lbResult
            // 
            this._lbResult.AutoSize = true;
            this._lbResult.Location = new System.Drawing.Point(12, 16);
            this._lbResult.Name = "_lbResult";
            this._lbResult.Size = new System.Drawing.Size(35, 13);
            this._lbResult.TabIndex = 2;
            this._lbResult.Text = "label1";
            // 
            // _ButtonOk
            // 
            this._ButtonOk.Location = new System.Drawing.Point(343, 16);
            this._ButtonOk.Name = "_ButtonOk";
            this._ButtonOk.Size = new System.Drawing.Size(75, 23);
            this._ButtonOk.TabIndex = 3;
            this._ButtonOk.Text = "OK";
            this._ButtonOk.UseVisualStyleBackColor = true;
            this._ButtonOk.Click += new System.EventHandler(this._ButtonOk_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(424, 16);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 4;
            this._btnCancel.Text = "Mégse";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Kezdeti időpont:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(319, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Intervallum vége: ";
            // 
            // _lvPredefined
            // 
            this._lvPredefined.FullRowSelect = true;
            this._lvPredefined.HideSelection = false;
            this._lvPredefined.Location = new System.Drawing.Point(15, 72);
            this._lvPredefined.Name = "_lvPredefined";
            this._lvPredefined.Size = new System.Drawing.Size(80, 90);
            this._lvPredefined.TabIndex = 7;
            this._lvPredefined.UseCompatibleStateImageBehavior = false;
            this._lvPredefined.View = System.Windows.Forms.View.List;
            this._lvPredefined.SelectedIndexChanged += new System.EventHandler(this.Predefined_SelectedIndexChanged);
            // 
            // _tbStart
            // 
            this._tbStart.Location = new System.Drawing.Point(229, 246);
            this._tbStart.Name = "_tbStart";
            this._tbStart.Size = new System.Drawing.Size(58, 20);
            this._tbStart.TabIndex = 9;
            this._tbStart.TextChanged += new System.EventHandler(this._tbStart_TextChanged);
            // 
            // _tbEnd
            // 
            this._tbEnd.Location = new System.Drawing.Point(439, 246);
            this._tbEnd.Name = "_tbEnd";
            this._tbEnd.Size = new System.Drawing.Size(48, 20);
            this._tbEnd.TabIndex = 10;
            this._tbEnd.TextChanged += new System.EventHandler(this._tbEnd_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(275, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "<= T <";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // DateTimeIntarvalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 282);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._tbEnd);
            this.Controls.Add(this._tbStart);
            this.Controls.Add(this._lvPredefined);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._ButtonOk);
            this.Controls.Add(this._lbResult);
            this.Controls.Add(this._monthCalendarEnd);
            this.Controls.Add(this._monthCalendarStart);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DateTimeIntarvalForm";
            this.Text = "Idő beállítás";
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.MonthCalendar _monthCalendarStart;
        private System.Windows.Forms.MonthCalendar _monthCalendarEnd;
        private System.Windows.Forms.Label _lbResult;
        private System.Windows.Forms.Button _ButtonOk;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView _lvPredefined;
        private System.Windows.Forms.TextBox _tbStart;
        private System.Windows.Forms.TextBox _tbEnd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}