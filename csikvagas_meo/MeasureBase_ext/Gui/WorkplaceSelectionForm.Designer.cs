namespace e77.MeasureBase.GUI
{
    partial class WorkplaceSelectionForm
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
            this._listView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // _listView
            // 
            this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listView.FullRowSelect = true;
            this._listView.Location = new System.Drawing.Point(0, 0);
            this._listView.Name = "_listView";
            this._listView.Size = new System.Drawing.Size(513, 283);
            this._listView.TabIndex = 0;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.List;
            this._listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DoubleClickHandler);
            // 
            // WorkplaceSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 283);
            this.Controls.Add(this._listView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkplaceSelectionForm";
            this.Text = "Helyiség beállítás (Dupla klikk a kiválasztáshoz)";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _listView;
    }
}