using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace 墨智音乐_WPF.自定义Page分页信息.Page_Music组件
{
    /// <summary>
    /// ToDay_Song.xaml 的交互逻辑
    /// </summary>
    public partial class Select_Song_For_You : UserControl
    {
        public Select_Song_For_You()
        {
            InitializeComponent();

            Image_Play_This_SongList.Visibility = System.Windows.Visibility.Hidden;
        }

        string Path_App = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory) + @"资源";

        /// <summary>
        /// StAckPanel背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void This_Xaml_BackGround_MouseLeave(object sender, MouseEventArgs e)
        {
            //#A8343434
            StackPanel_Move_Black.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00000000"));//无色

            Image_Play_This_SongList.Visibility = System.Windows.Visibility.Hidden;
        }
        private void This_Xaml_BackGround_MouseMove(object sender, MouseEventArgs e)
        {
            //#A8343434
            StackPanel_Move_Black.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A8343434"));

            Image_Play_This_SongList.Visibility = System.Windows.Visibility.Visible;
        }


        string Song_Image_Url;
        /// <summary>
        /// 播放键背景色5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      3
        private void This_Image_BackGround_MouseLeave(object sender, MouseEventArgs e)
        {
            Song_Image_Url = Path_App + @"\图片资源\按键图片\List_播放键_Play_Leave.png";
            //#A8343434
            Image_Play_This_SongList.Source = new BitmapImage(new Uri(Song_Image_Url));

            StackPanel_Move_Black.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00000000"));//无色

        }
        private void This_Image_BackGround_MouseMove(object sender, MouseEventArgs e)
        {
            Song_Image_Url = Path_App + @"\图片资源\按键图片\List_播放键_Play_Over.png";
            //#A8343434
            Image_Play_This_SongList.Source = new BitmapImage(new Uri(Song_Image_Url));

            StackPanel_Move_Black.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A8343434"));

            Image_Play_This_SongList.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
