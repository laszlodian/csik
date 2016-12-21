namespace e77.MeasureBase.GUI
{
    partial class LoadMeasureSelectorForm
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
            this._dataGridView = new System.Windows.Forms.DataGridView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this._tbSqlId = new System.Windows.Forms.TextBox();
            this._buttonOk = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // _dataGridView
            // 
            this._dataGridView.AllowUserToAddRows = false;
            this._dataGridView.AllowUserToDeleteRows = false;
            this._dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridView.Location = new System.Drawing.Point(0, 31);
            this._dataGridView.MultiSelect = false;
            this._dataGridView.Name = "_dataGridView";
            this._dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dataGridView.Size = new System.Drawing.Size(899, 374);
            this._dataGridView.TabIndex = 0;
            this.toolTip1.SetToolTip(this._dataGridView, global::e77.MeasureBase.Properties.Resources.DOUBLE_CLICK_TO_SELECT);
            this._dataGridView.DoubleClick += new System.EventHandler(this.DoubleClickHandler);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(331, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selection be doubleclick at table or manual here:";
            // 
            // _tbSqlId
            // 
            this._tbSqlId.Location = new System.Drawing.Point(349, 5);
            this._tbSqlId.Name = "_tbSqlId";
            this._tbSqlId.Size = new System.Drawing.Size(100, 20);
            this._tbSqlId.TabIndex = 2;
            this._tbSqlId.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // _buttonOk
            // 
            this._buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._buttonOk.Enabled = false;
            this._buttonOk.Location = new System.Drawing.Point(455, 4);
            this._buttonOk.Name = "_buttonOk";
            this._buttonOk.Size = new System.Drawing.Size(75, 23);
            this._buttonOk.TabIndex = 3;
            this._buttonOk.Text = global::e77.MeasureBase.Properties.Resources.DIALOG_RESULT_OK;
            this._buttonOk.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // LoadSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AcceptButton = this._buttonOk;
            this.ClientSize = new System.Drawing.Size(899, 404);
            this.Controls.Add(this._buttonOk);
            this.Controls.Add(this._tbSqlId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._dataGridView);
            this.Name = "LoadSelectorForm";
            this.Text = "LoadSelectorForm";
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView _dataGridView;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _tbSqlId;
        private System.Windows.Forms.Button _buttonOk;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}