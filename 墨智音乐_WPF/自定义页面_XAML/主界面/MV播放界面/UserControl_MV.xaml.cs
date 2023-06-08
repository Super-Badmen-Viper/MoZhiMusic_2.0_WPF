using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using 墨智音乐_WPF.Dao_类封装.歌单信息类;

namespace 墨智音乐_WPF.自定义页面_XAML.主界面.MV播放界面
{
    /// <summary>
    /// xaml 的交互逻辑
    /// </summary>
    public partial class UserControl_MV : UserControl
    {
        public UserControl_MV()
        {
            InitializeComponent();

            userControl_MV_Take.Visibility = Visibility.Hidden;
            StackPanel_Voice.Visibility = Visibility.Hidden;

            //所有重叠层绑定鼠标移入移出事件
            userControl_MV_Take.MouseEnter += Mouse_Over_Silder_Music_Width;
            userControl_MV_Take.MouseLeave += Mouse_Leave_Silder_Music_Width;
            userControl_MV_Take_TextBlock.MouseEnter += Mouse_Over_Silder_Music_Width;
            userControl_MV_Take_TextBlock.MouseLeave += Mouse_Leave_Silder_Music_Width;

            StackPanel_Voice.MouseEnter += Mouse_Over_Silder_Music_Width;
            StackPanel_Voice.MouseLeave += Mouse_Leave_Silder_Voice_Width;

            //音量控制
            userControl_MV_Take.Button_Music_Voice_Speed.Click += Show_Voice_Silder;
            Slider_Voice.Maximum = 1;
            Slider_Voice.Value = 0.5;
            Slider_Voice.ValueChanged += Slider_Voice_ValueChanged;


        }

        /// <summary>
        /// 鼠标移入silder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mouse_Over_Silder_Music_Width(object sender, MouseEventArgs e)
        {
            userControl_MV_Take.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 鼠标移除silder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mouse_Leave_Silder_Music_Width(object sender, MouseEventArgs e)
        {
            userControl_MV_Take.Visibility = Visibility.Hidden;
        }
        public void Mouse_Leave_Silder_Voice_Width(object sender, MouseEventArgs e)
        { 
            StackPanel_Voice.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// 开启音量条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Show_Voice_Silder(object sender, EventArgs e)
        {
            if (StackPanel_Voice.Visibility == Visibility.Hidden)
            {
                StackPanel_Voice.Visibility = Visibility.Visible;
            }
            else
            {
                StackPanel_Voice.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 更改音量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_Voice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaMent_MV.Volume = Slider_Voice.Value;

            Voice_Nums.Text = (Slider_Voice.Value * 100).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MediaMent_MV.Stop();
            MediaMent_MV.LoadedBehavior = MediaState.Stop;
            this.Visibility = Visibility.Hidden;
        }
    }
}
