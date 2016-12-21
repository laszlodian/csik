using System.Windows.Forms;
using Npgsql;
using System;
using System.Runtime.Serialization;
using System.Configuration;
using WinFormBlankTest;
using System.Threading;
using WinFormBlankTest.UI.Forms;
using System.Drawing;

namespace WinFormBlankTest.UI.Forms
{

public class LoginForm :Form
{
    public Label lb;
    public ProgressBar pb = new ProgressBar();
    const string MasterPassword = "e77";

    public string controllerUser = Environment.GetEnvironmentVariable("USERNAME");
    private MaskedTextBox maskedTextBox1;
    private Label label1;
    private Label lbLoggedIn;
    private Button button1;
    private Label label3;
    private Label label2;
    private Button button2;
    private Button button3;
    private ComboBox comboBox1;
    public string machineName = Environment.GetEnvironmentVariable("COMPUTERNAME");


    public LoginForm()
    {

        InitializeComponent();
        this.Text = string.Format("Belépés az Ideál Csikvágás Teszt Szoftverbe");
        lbLoggedIn.Text = string.Format("Bejelentkezve {0} felhasználóként",Environment.UserName);
    }

    public void OK_Click(object sender , EventArgs e ) 
    {
        this.Close();
    }

    public void Cancel_Click(object sender,EventArgs e)
        {this.Close();}

    private void InitializeComponent()
    {
        this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.lbLoggedIn = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        this.label3 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.button2 = new System.Windows.Forms.Button();
        this.button3 = new System.Windows.Forms.Button();
        this.comboBox1 = new System.Windows.Forms.ComboBox();
        this.SuspendLayout();
        // 
        // maskedTextBox1
        // 
        this.maskedTextBox1.BackColor = System.Drawing.Color.Azure;
        this.maskedTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.maskedTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.maskedTextBox1.Location = new System.Drawing.Point(329, 126);
        this.maskedTextBox1.Name = "maskedTextBox1";
        this.maskedTextBox1.Size = new System.Drawing.Size(203, 24);
        this.maskedTextBox1.TabIndex = 0;
        this.maskedTextBox1.Text = "<Jelszó beírása>";
        this.maskedTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        this.maskedTextBox1.Enter += new System.EventHandler(this.maskedTextBox1_Enter);
        // 
        // label1
        // 
        this.label1.Font = new System.Drawing.Font("Georgia", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.Image = global::WinFormBlankTest.Properties.Resources.elektronik;
        this.label1.Location = new System.Drawing.Point(224, 126);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(93, 25);
        this.label1.TabIndex = 1;
        this.label1.Text = "Jelszó:";
        this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // lbLoggedIn
        // 
        this.lbLoggedIn.Font = new System.Drawing.Font("Georgia", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.lbLoggedIn.Image = global::WinFormBlankTest.Properties.Resources.elektronik;
        this.lbLoggedIn.Location = new System.Drawing.Point(99, 78);
        this.lbLoggedIn.Name = "lbLoggedIn";
        this.lbLoggedIn.Size = new System.Drawing.Size(515, 29);
        this.lbLoggedIn.TabIndex = 2;
        this.lbLoggedIn.Text = "Bejelentkezve felhasználóként";
        this.lbLoggedIn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // button1
        // 
        this.button1.BackColor = System.Drawing.SystemColors.InactiveCaption;
        this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.button1.ForeColor = System.Drawing.SystemColors.WindowText;
        this.button1.Location = new System.Drawing.Point(581, 172);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(93, 31);
        this.button1.TabIndex = 3;
        this.button1.Text = "Belépés";
        this.button1.UseVisualStyleBackColor = false;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // label3
        // 
        this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.label3.Dock = System.Windows.Forms.DockStyle.Top;
        this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)
                        | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
        this.label3.ForeColor = System.Drawing.Color.DarkBlue;
        this.label3.Image = global::WinFormBlankTest.Properties.Resources.elektronik;
        this.label3.Location = new System.Drawing.Point(0, 0);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(686, 53);
        this.label3.TabIndex = 5;
        this.label3.Text = "Ideál Csikvágás Teszt Szoftver";
        this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // label2
        // 
        this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.label2.Cursor = System.Windows.Forms.Cursors.NoMove2D;
        this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.label2.Image = global::WinFormBlankTest.Properties.Resources.elektronik;
        this.label2.Location = new System.Drawing.Point(99, 176);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(211, 27);
        this.label2.TabIndex = 6;
        this.label2.Text = "Mérés típus választása:";
        this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // button2
        // 
        this.button2.BackColor = System.Drawing.SystemColors.InactiveCaption;
        this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.button2.ForeColor = System.Drawing.SystemColors.WindowText;
        this.button2.Location = new System.Drawing.Point(597, 257);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(89, 26);
        this.button2.TabIndex = 7;
        this.button2.Text = "Bezárás";
        this.button2.UseVisualStyleBackColor = false;
        this.button2.Click += new System.EventHandler(this.button2_Click);
        // 
        // button3
        // 
        this.button3.BackColor = System.Drawing.Color.Aqua;
        this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
        this.button3.FlatAppearance.BorderSize = 0;
        this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
        this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
        this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.button3.Location = new System.Drawing.Point(647, 56);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(27, 29);
        this.button3.TabIndex = 8;
        this.button3.Text = "?";
        this.button3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        this.button3.UseVisualStyleBackColor = false;
        this.button3.Click += new System.EventHandler(this.button3_Click);
        // 
        // comboBox1
        // 
        this.comboBox1.FormattingEnabled = true;
        this.comboBox1.Items.AddRange(new object[] {
            "Blank Check",
            "Homogenity Check",
            "Roll invalidálás, újramérés",
            "Teszteredmények mutatása"});
        this.comboBox1.Location = new System.Drawing.Point(329, 179);
        this.comboBox1.Name = "comboBox1";
        this.comboBox1.Size = new System.Drawing.Size(183, 21);
        this.comboBox1.TabIndex = 9;
        // 
        // LoginForm
        // 
        this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
        this.BackgroundImage = global::WinFormBlankTest.Properties.Resources.elektronik;
        this.ClientSize = new System.Drawing.Size(686, 286);
        this.Controls.Add(this.comboBox1);
        this.Controls.Add(this.button3);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.lbLoggedIn);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.maskedTextBox1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
        this.HelpButton = true;
        this.Name = "LoginForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Belépés az Ideál Csikvágás Teszt Szoftver";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    private void label2_Click(object sender, System.EventArgs e)
    {

    }

    delegate void SetOpacityToNullDelegate();
    public void SetOpacityToNull()
    {

        if (this.InvokeRequired)
        {
            this.Invoke(new SetOpacityToNullDelegate(SetOpacityToNull));
        }else
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(50);
                this.Opacity -= 2;
            }
    }
    private void button1_Click(object sender, System.EventArgs e)
    {
        ThreadStart ts=new ThreadStart(WaitForLotSelectorFormReady);
        Thread th = new Thread(ts);
        th.Start();
           
        string pass=maskedTextBox1.Text;

        object res = null;
        bool access=false;
        int attempt = 5;
        
        ///if masterpassword is known it is not needed to login with own account
        if (pass.Equals(MasterPassword))
        {
            access= true;
        }

        if (!access)
        {
            #region if master password not known

            using (NpgsqlConnection conn = new NpgsqlConnection(Program.dbConnection))
            {
                try
                {
                    conn.Open();
                    ///Get password from database where username is the logged in user
                    using (NpgsqlCommand checkpass = new NpgsqlCommand(string.Format("select password from account_table where user='{0}'", controllerUser), conn))
                    {


                        res = checkpass.ExecuteScalar();

                        if (res == DBNull.Value)
                        {
                            throw new ArgumentNullException("No value for this query: {0}", checkpass.CommandText);
                        }
                    }

                    ///Check password if it valid
                    if (Convert.ToString(res).Equals(pass))
                    {
                        access = true;

                        StartApplicationWithCode();
                    }
                    else
                    {
                        access = false;
                        attempt--;
                    }

                    ///No more attempt to try a new password
                   if (attempt <= 0)
                   {
                       MessageBox.Show("Megvolt az 5 próbálkozás, 20 perc múlva tud újra próbálkozni,\n vagy értesítse a fejlesztést!");
                       Environment.Exit(Environment.ExitCode);
                   }
                   else
                   {    
                       this.Opacity = 100;
                       MessageBox.Show(string.Format("Rossz jelszó lett megadva a '{0}' felhasználónévhez!\nPróbálkozzon újra, még {1} lehetőség áll rendelkezésre.", controllerUser, attempt));
                       maskedTextBox1.Text = string.Empty;                    
                   }
                    
                }
                catch (System.Exception)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }

            }
            #endregion
        }
        StartApplicationWithCode();
        
    }
    public void WaitForLotSelectorFormReady()
    {

      
        pb.Width = 200;
        pb.Maximum = 100;
        pb.Minimum = 1;
        pb.Value = 1;
        pb.ForeColor = Color.Green;
        pb.Style = ProgressBarStyle.Continuous;

        Label lb = new Label();
        lb.Text = "Folyamat lezárása...";


    }
    delegate void RemoveLoginControlsDelegate();  
    public void RemoveLoginControls()
    {
      if (this.InvokeRequired)
	    {
          this.Invoke(new RemoveLoginControlsDelegate(RemoveLoginControls));
	    }else
        {
            for (int i = 0; i < this.Controls.Count; i++)
               {
                   this.Controls.Remove(this.Controls[i]);
               }

        }


         Controls.Add(lb);
        Controls.Add(pb);

        new Thread(new ThreadStart(RaisePbValue)).Start();
        
        while (!LotNumberForm.ActiveForm.IsRestrictedWindow)
        {
            Thread.Sleep(100);
            
         
        }

  
       
       
    }

    public void RaisePbValue()
    {
        while (pb.Value!=pb.Maximum || LotNumberForm.ActiveForm.IsRestrictedWindow)
        {
            pb.Value += 4;
            Thread.Sleep(40);
         }
      
    }

    private void StartApplicationWithCode()
    {
        this.Close();

        switch (this.comboBox1.SelectedItem.ToString())
        {
            case "Blank Check":
                Program.measureType = "blank";
                break;
            case "Homogenity Check":
                Program.measureType = "homogenity";
                break;
            case "Teszteredmények mutatása":
                Program.measureType = "show";
                break;
            case "Roll invalidálás, újramérés":
                Program.measureType = "showall";
                break;
            case "Accuracy Check":
                Program.measureType = "accuracy";
                break;
            default: Program.measureType = "show";
                break;
        }

        Program.GenerateUIAndStartApp(new string[] {Program.measureType});
    
}
    private void button2_Click(object sender, EventArgs e)
    {
        this.Close();
        Environment.Exit(Environment.ExitCode);
    }

    private void maskedTextBox1_Enter(object sender, EventArgs e)
    {
        maskedTextBox1.Text = string.Empty;
        maskedTextBox1.UseSystemPasswordChar = true;
    }

    private void button3_Click(object sender, EventArgs e)
    {
        MessageBox.Show(string.Format(helpString));
    }
    public string helpString = "Üdvözöljük az Ideál teszt csík vágás folyamatában,\nez a szoftver fogja segíteni többségét irényítani Önöknek\n a rendkívűl pontos,\n és bonyolult tesztelési folyamatoknak";
}
}