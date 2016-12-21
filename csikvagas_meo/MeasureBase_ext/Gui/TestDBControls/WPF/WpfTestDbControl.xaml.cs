using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using e77.MeasureBase;

namespace e77.MeasureBase.GUI.WPF
{
    /// <summary>
    /// Interaction logic for WpfTestDbControl.xaml
    /// </summary>
    public partial class WpfTestDbControl : UserControl
    {
        public WpfTestDbControl() :base()
        {
            InitializeComponent();
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            if (!DesignerProperties.GetIsInDesignMode(this) &&
                MeasureConfig.TheConfig.SqlIsReleaseDb)
                this.Visibility = Visibility.Hidden;

            base.OnVisualParentChanged(oldParent);
        }
    }
}
