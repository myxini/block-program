using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Point position_down;

        private bool is_down = false;

        public System.Drawing.Rectangle Area { get; private set; }

		public ImageSource CameraImageSource { set { this.CameraImage.Source = value; } }

        private void ContentPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(ContentPanel);
            position_down = position;
            is_down = true;
            Rectangle1.Margin = new Thickness(position.X, position.Y, 0, 0);
            Rectangle1.Width = 0;
            Rectangle1.Height = 0;
            Rectangle1.Visibility = System.Windows.Visibility.Visible;
        }

        private void ContentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!is_down)
            {
                return;
            }
            Point position = e.GetPosition(ContentPanel);
            Rectangle1.Width = Math.Abs(position.X - position_down.X);
            Rectangle1.Height = Math.Abs(position.Y - position_down.Y);
            Rectangle1.Margin = new Thickness(
                Math.Min(position.X, position_down.X),
                Math.Min(position.Y, position_down.Y),
                0,
                0
            );
        }

        private void ContentPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            is_down = false;
            Area = new System.Drawing.Rectangle(
                (int) Rectangle1.Margin.Left,
                (int) Rectangle1.Margin.Top,
                (int) Rectangle1.Width,
                (int) Rectangle1.Height
            );

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Rectangle1.Visibility == Visibility.Collapsed)
            {
                e.Cancel = true;
            }
        }
    }
}
