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

namespace 墨智音乐_WPF.自定义控件.音频波形图
{
    /// <summary>
    /// UserControl_Music_MAV_GDI.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl_Music_MAV_GDI : UserControl
    {
        public UserControl_Music_MAV_GDI()
        {
            InitializeComponent();
        }


        /*
         * 
         * 1.仅中间部分textblock  同步当前音频
         * 2.两边textblock同步之前的音频，呈两边波浪形过渡同步之前的音频
         * 3.音频停止时，所有textblock静止
         * 4.音频初始化时,所有textblick高度同步为0
         * 
         */
    }
}
