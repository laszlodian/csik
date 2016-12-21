using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace e77.MeasureBase
{
    public class ProductSelectorWindow : Form
    {
        public ProductSelectorWindow()
        {

            InitializeComponent();

        }
        private Label lbDeviceType;
        private Button buttonOK;
        private ComboBox comboBoxWorkPhase;
        private Label lbWorkphase;
        private ComboBox comboBoxDevice;

        public string LabelDeviceTypeText { get{ return lbDeviceType.Text; }set{lbDeviceType.Text=value;} }


        private void InitializeComponent()
        {
            this.comboBoxDevice = new System.Windows.Forms.ComboBox();
            this.lbDeviceType = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxWorkPhase = new System.Windows.Forms.ComboBox();
            this.lbWorkphase = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxDevice
            // 
            this.comboBoxDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Items.AddRange(new object[] {
            "UD2",
            "UA3",
            "LFR"});
            this.comboBoxDevice.Location = new System.Drawing.Point(234, 12);
            this.comboBoxDevice.Name = "comboBoxDevice";
            this.comboBoxDevice.Size = new System.Drawing.Size(189, 21);
            this.comboBoxDevice.TabIndex = 0;
            this.comboBoxDevice.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBoxDevice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxDevice_KeyDown);
            // 
            // lbDeviceType
            // 
            this.lbDeviceType.AutoSize = true;
            this.lbDeviceType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbDeviceType.Location = new System.Drawing.Point(41, 15);
            this.lbDeviceType.Name = "lbDeviceType";
            this.lbDeviceType.Size = new System.Drawing.Size(155, 13);
            this.lbDeviceType.TabIndex = 1;
            this.lbDeviceType.Text = "Válasszon készüléktípust:";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(234, 48);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(128, 21);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBoxWorkPhase
            // 
            this.comboBoxWorkPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorkPhase.FormattingEnabled = true;
            this.comboBoxWorkPhase.Location = new System.Drawing.Point(602, 12);
            this.comboBoxWorkPhase.Name = "comboBoxWorkPhase";
            this.comboBoxWorkPhase.Size = new System.Drawing.Size(272, 21);
            this.comboBoxWorkPhase.TabIndex = 1;
            this.comboBoxWorkPhase.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxWorkPhase_KeyDown);
            // 
            // lbWorkphase
            // 
            this.lbWorkphase.AutoSize = true;
            this.lbWorkphase.Location = new System.Drawing.Point(515, 15);
            this.lbWorkphase.Name = "lbWorkphase";
            this.lbWorkphase.Size = new System.Drawing.Size(64, 13);
            this.lbWorkphase.TabIndex = 4;
            this.lbWorkphase.Text = "Munkafázis:";
            // 
            // ProductSelectorWindow
            // 
            this.ClientSize = new System.Drawing.Size(920, 118);
            this.ControlBox = false;
            this.Controls.Add(this.lbWorkphase);
            this.Controls.Add(this.comboBoxWorkPhase);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.lbDeviceType);
            this.Controls.Add(this.comboBoxDevice);
            this.DoubleBuffered = true;
            this.Name = "ProductSelectorWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public class WorkphaseComboboxItem
        {
            public string Text { get; set; }
            public string Phase { get; set; }

            public override string ToString()
            {
                return Text;
            }

            public WorkphaseComboboxItem(string Text, string Phase)
            {
                this.Text = Text;
                this.Phase = Phase;
            }

        }

        private bool needWorkPhasesSelect = true;

        public bool NeedWorkPhasesSelect
        {
            get
            {
                return needWorkPhasesSelect;
            }
            set
            {
                needWorkPhasesSelect = value;  
                

                lbWorkphase.Visible = needWorkPhasesSelect; ;
                comboBoxWorkPhase.Visible = needWorkPhasesSelect;
            }
        }



        private void AddItems(string device)
        {
            comboBoxWorkPhase.Items.Clear();

            switch (device)
            {
                case "UD2":
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Fázis - 1 : Készülék fedél nélkül", "gyartas_ud2"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Fázis - 2 : Készülék burkolattal", "gyartas_ud2_full"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Gyártás Mainpanel", "gyartas_mainpanel"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Gyártás Backpanel", "gyartas_backpanel"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Szervíz Mainpanel", "service_ud2"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("MEO funfcióteszt", "meo_fasttest"));
                    break;
                case "UA3":
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Fázis - 1 : Készülék fedél nélkül", "gyartas_ua3"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Fázis - 2 : Készülék burkolattal", "gyartas_ua3_full"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Gyártás Backpanel", "gyartas_backpanel"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("MEO funfcióteszt", "meo_fasttest"));
                    break;
                case "LFR":
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Fázis - 1 : Készülék fedél nélkül", "gyartas_lfr"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Fázis - 2 : Készülék burkolattal", "gyartas_lfr_full"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("Gyártás Mainpanel", "gyartas_mainpanel"));
                    comboBoxWorkPhase.Items.Add(new WorkphaseComboboxItem("MEO funfcióteszt", "meo_fasttest"));
                    break;
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            DeviceName = Convert.ToString(comboBoxDevice.SelectedItem);

            //switch (wpComboBox.SelectedIndex)
            //{
            //    case 0:
            //        WorkPhase = string.Format("gyartas_{0}", DeviceName.ToLower());
            //        break;
            //    case 1:
            //        WorkPhase = string.Format("gyartas_{0}_full", DeviceName.ToLower());
            //        break;
            //    case 2:
            //        WorkPhase = string.Format("gyartas_mainpanel");
            //        break;
            //    case 3:
            //        WorkPhase = string.Format("gyartas_backpanel");
            //        break;
            //    case 4:
            //        WorkPhase = string.Format("ud2_service");
            //        break;
            //    default:
            //        break;
            //}

            if (comboBoxDevice.SelectedItem == null)
            {
                MessageBox.Show("Nincs készülék típus kiválasztva!");
            }
            else
            {
                if ((comboBoxWorkPhase.SelectedItem == null) && (NeedWorkPhasesSelect))
                {
                    MessageBox.Show("Nincs munkafázis kiválasztva!");
                }
                else
                {
                    if (NeedWorkPhasesSelect) WorkPhase = ((WorkphaseComboboxItem)comboBoxWorkPhase.SelectedItem).Phase;
                    DBSelector = DeviceName.ToUpper();
                    //MessageBox.Show(DeviceName + " " + WorkPhase);
                    this.Hide();
                }
            }
        }

        public string DeviceName { get; private set; }

        public string DBSelector { get; set; }

        public string WorkPhase { get; private set; }
        //TODO high prio + test LFR Phases and Test&Debug UA3 Phases
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddItems(comboBoxDevice.SelectedItem.ToString());
            
            //switch (Convert.ToString(comboBox1.SelectedItem))
            //{
            //    case "UD2":
            //        if (!wpComboBox.Items.Contains("Gyártás Backpanel"))
            //        {
            //            wpComboBox.Items.AddRange(new object[] { "Gyártás Backpanel" });
            //        }
            //        wpComboBox.Items.AddRange(new object[] { "Szervíz Mainpanel" });
            //        break;

            //    case "UA3":
            //        if (wpComboBox.Items.Contains("Szervíz Mainpanel"))
            //        {
            //            wpComboBox.Items.Remove("Szervíz Mainpanel");
            //        }
            //        if (!wpComboBox.Items.Contains("Gyártás Backpanel"))
            //        {
            //            wpComboBox.Items.AddRange(new object[] { "Gyártás Backpanel" });
            //        }

            //        break;

            //    case "LFR":
            //        if (wpComboBox.Items.Contains("Szervíz Mainpanel"))
            //        {
            //            wpComboBox.Items.Remove("Szervíz Mainpanel");
            //        }
            //        if (wpComboBox.Items.Contains("Gyártás Backpanel"))
            //        {
            //            wpComboBox.Items.Remove("Gyártás Backpanel");
            //        }

            //        break;

            //    default:
            //        break;
            //}
        }

        private void comboBoxDevice_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (comboBoxDevice.SelectedItem != null))
            {
                if (NeedWorkPhasesSelect)
                {
                    comboBoxWorkPhase.Focus();
                }
                else
                {
                    buttonOK.Focus();
                }
            }
        }

        private void comboBoxWorkPhase_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (comboBoxWorkPhase.SelectedItem != null))
            {
                buttonOK.Focus();
            }
        }
    }
}