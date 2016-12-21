using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace e77.MeasureBase.GUI.DialogWindows
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(String), typeof(ProgressWindow), new UIPropertyMetadata(""));

        public ProgressWindow()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        public ProgressWindow(Window owner_in)
        {
            InitializeComponent();
            this.Owner = owner_in;
        }

        public bool CancellationPending = false;

        public void ChangeProgressBarRemotely(int percentProgress)
        {
            if (!CancellationPending)
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.progressBar1.Value = percentProgress;
                }));
        }

        private void bt_Cancel_Click(object sender, RoutedEventArgs e)
        {
            CancellationPending = true;
            this.Close();
        }
    }
}