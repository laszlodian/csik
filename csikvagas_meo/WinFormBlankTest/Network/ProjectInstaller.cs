using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using WinFormBlankTest.Network;

namespace WinFormBlankTest.Network
{
  [RunInstaller(true)]
  public partial class ProjectInstaller : Installer
  {

    /// <summary>
    /// Default constructor
    /// </summary>
    public ProjectInstaller() : base()
    {
      InitializeComponent();

      
        
    }  

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
      this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
      // 
      // serviceProcessInstaller1
      // 
      this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.User;
      this.serviceProcessInstaller1.Password = null;
      this.serviceProcessInstaller1.Username = null;
      
      // 
      // serviceInstaller1
      // 
      this.serviceInstaller1.ServiceName = "Ping Monitor Service";
      this.serviceInstaller1.StartType = ServiceStartMode.Manual;
      
      //
      // ProjectInstaller
      // 
      this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

    }

      
    private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
    private System.ServiceProcess.ServiceInstaller serviceInstaller1;
   
  }
}