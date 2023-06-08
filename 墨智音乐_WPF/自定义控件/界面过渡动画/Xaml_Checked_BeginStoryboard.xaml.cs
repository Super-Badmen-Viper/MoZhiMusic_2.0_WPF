using System;
using System.Collections.Generic;
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

namespace 墨智音乐_WPF.自定义控件.界面过渡动画
{
    /// <summary>
    /// Xaml_Checked_BeginStoryboard.xaml 的交互逻辑
    /// </summary>
    public partial class Xaml_Checked_BeginStoryboard : UserControl
    {
        public Xaml_Checked_BeginStoryboard()
        {
            InitializeComponent();
        }

        public void Begin_Start_board()
        {
            Text_DoubleAnimation_slider_Up.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            Text_Storyboard_slider_Up.SpeedRatio = 1;
            Text_Storyboard_slider_Up.Begin();         
        }

        /// <summary>
        /// 动画完成后触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Text_Storyboard_slider_Up_Completed(object sender, EventArgs e)
        {
            GradientStop_Up.Color = Color.FromArgb(0,0,0,0);
        }
    }
}
