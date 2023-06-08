using System.Windows.Controls;

namespace 墨智音乐_WPF.自定义控件.按钮.播放器按钮
{
    /// <summary>
    /// UserControl_ALL_Button_Music_PANEL.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl_ALL_Button_Music_PANEL_BUTTOM : UserControl
    {
        public UserControl_ALL_Button_Music_PANEL_BUTTOM()
        {
            InitializeComponent();

            MediaElement_Song.Visibility = System.Windows.Visibility.Hidden;
        }


        //UseLayoutRounding="True"   防止图片模糊

    }
}
