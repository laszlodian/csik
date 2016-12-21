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
using System.Windows.Shapes;

namespace e77.MeasureBase.MeterDevices
{
    /// <summary>
    /// Interaction logic for InvalidRangeWindow.xaml
    /// </summary>
    public partial class InvalidRangeWindow : Window
    {
        public InvalidRangeWindow()
        {
            InitializeComponent();
            this.Closing += new System.ComponentModel.CancelEventHandler(InvalidRangeWindow_Closing);
        }

        void InvalidRangeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        public string Message
        {
            get
            {
                return (string)label1.Content;
            }
            set
            {
                label1.Content = value;
            }
        }
    }
}
