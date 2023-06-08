using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace 墨智音乐_WPF.自定义控件.悬浮歌词框
{
    /// <summary>
    /// Window_Desk_KRC.xaml 的交互逻辑
    /// </summary>
    public partial class Window_Desk_KRC : Window
    {
        public Window_Desk_KRC()
        {
            InitializeComponent();

            //窗口透明
            //WindowStyle = "None" Background = "#00FFFFFF" AllowsTransparency = "True"

            //不显示在任务栏
            this.ShowInTaskbar = false;

            //窗口顶层
            this.Topmost = true;

            //禁止最大化，不允许修改大小
            this.ResizeMode = ResizeMode.NoResize;


        }

        /// <summary>
        /// 拖动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }



        private void Window_Desk_KRC1_MouseLeave(object sender, MouseEventArgs e)
        {
            //#A8343434
            this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00000000"));
        }

        private void Window_Desk_KRC1_MouseMove(object sender, MouseEventArgs e)
        {
            //#A8343434
            this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A8343434"));
        }


    }
}
