﻿#pragma checksum "..\..\..\MeterDevices\MeterSetupWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "318A96F4ED5BF05E880E4A4D397561C3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace e77.MeasureBase.MeterDevices {
    
    
    /// <summary>
    /// MeterSetupWindow
    /// </summary>
    public partial class MeterSetupWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox _tbSN;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock _tbSnState;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox _cbPorts;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _buttonCancel;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _buttonOk;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/e77.MeasureBase;component/meterdevices/metersetupwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
            ((e77.MeasureBase.MeterDevices.MeterSetupWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this._tbSN = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
            this._tbSN.LostFocus += new System.Windows.RoutedEventHandler(this.SnLostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this._tbSnState = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this._cbPorts = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this._buttonCancel = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
            this._buttonCancel.Click += new System.Windows.RoutedEventHandler(this._buttonCancel_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this._buttonOk = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\MeterDevices\MeterSetupWindow.xaml"
            this._buttonOk.Click += new System.Windows.RoutedEventHandler(this._buttonOk_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
