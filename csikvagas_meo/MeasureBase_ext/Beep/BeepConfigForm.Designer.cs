using e77.MeasureBase.Properties;
namespace e77.MeasureBase.Beep
{
    partial class BeepConfigForm
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
            this._rbSoundCard = new System.Windows.Forms.RadioButton();
            this._rbBeep = new System.Windows.Forms.RadioButton();
            this._rbNone = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _rbSoundCard
            // 
            this._rbSoundCard.AutoSize = true;
            this._rbSoundCard.Location = new System.Drawing.Point(12, 35);
            this._rbSoundCard.Name = "_rbSoundCard";
            this._rbSoundCard.Size = new System.Drawing.Size(80, 17);
            this._rbSoundCard.TabIndex = 1;
            this._rbSoundCard.TabStop = true;
            this._rbSoundCard.Text = "Hangkártya";
            this._rbSoundCard.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this._rbBeep.AutoSize = true;
            this._rbBeep.Location = new System.Drawing.Point(12, 58);
            this._rbBeep.Name = "radioButton1";
            this._rbBeep.Size = new System.Drawing.Size(63, 17);
            this._rbBeep.TabIndex = 2;
            this._rbBeep.TabStop = true;
            this._rbBeep.Text = "Csipogó";
            this._rbBeep.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this._rbNone.AutoSize = true;
            this._rbNone.Location = new System.Drawing.Point(12, 12);
            this._rbNone.Name = "radioButton2";
            this._rbNone.Size = new System.Drawing.Size(53, 17);
            this._rbNone.TabIndex = 0;
            this._rbNone.TabStop = true;
            this._rbNone.Text = "Néma";
            this._rbNone.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(191, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = Resources.DIALOG_RESULT_OK;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(191, 41);
            this._btnCancel.Name = Resources.DIALOG_RESULT_CANCEL;
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 4;
            this._btnCancel.Text = "_btnCancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // BeepConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 88);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._rbNone);
            this.Controls.Add(this._rbBeep);
            this.Controls.Add(this._rbSoundCard);
            this.Name = "BeepConfigForm";
            this.Text = "BeepConfigForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton _rbSoundCard;
        private System.Windows.Forms.RadioButton _rbBeep;
        private System.Windows.Forms.RadioButton _rbNone;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button _btnCancel;
    }
}