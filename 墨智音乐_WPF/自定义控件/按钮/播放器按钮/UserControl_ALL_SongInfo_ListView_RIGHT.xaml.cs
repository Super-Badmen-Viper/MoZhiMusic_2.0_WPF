using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

using System.Windows.Threading;
using System.Xml;

namespace 墨智音乐_WPF.自定义控件.按钮.播放器按钮
{
    /// <summary>
    /// UserControl_ALL_SongInfo_ListView_RIGHT.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl_ALL_SongInfo_ListView_RIGHT : UserControl
    {
        public UserControl_ALL_SongInfo_ListView_RIGHT()
        {
            InitializeComponent();
        }

        int Mouse_Enter_Count = 0;//记录 鼠标单击次数
        DispatcherTimer timer_Mouse_Enter_Count;//规定 鼠标双击时前后的时间间隔

        /// <summary>
        /// 计时器，不建议使用timer,记录鼠标双击事件（WPF无内置鼠标双击事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MainTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //记录鼠标单击次数
            Mouse_Enter_Count += 1;

            //开启计时器，时间为0.6s
            timer_Mouse_Enter_Count = new DispatcherTimer();
            timer_Mouse_Enter_Count.Interval = new TimeSpan(0, 0, 0, 0, 600);
            timer_Mouse_Enter_Count.Tick += (s, e1) => { timer_Mouse_Enter_Count.IsEnabled = false; Mouse_Enter_Count = 0; };
            timer_Mouse_Enter_Count.IsEnabled = true;

            //如果鼠标单击次数为2，也就是当鼠标双击 且 前后间隔在计时器时间区间内，则判定为双击事件
            if (Mouse_Enter_Count == 2 && timer_Mouse_Enter_Count.IsEnabled == true)
            {
                timer_Mouse_Enter_Count.IsEnabled = false;//计时器归零
                Mouse_Enter_Count = 0;//鼠标单击次数归零

                Get_Selected_MouseDoubleClick_Song_Info();//进入鼠标双击事件
            }
        }

        /// <summary>
        /// 鼠标双击后获取播放列表(ListView_Temp)中选定的项(Items)，提取指定的信息(序号，歌曲名，......)
        /// </summary>
        public void Get_Selected_MouseDoubleClick_Song_Info()
        {
            dynamic tag = (dynamic)ListView_Temp_Info.SelectedItems;
            string song_url = tag;


            MessageBox.Show("双击" + song_url);
        }


        /// <summary>
        /// 解析ListView(歌单)中选定的项(ListView_Selected_Items)中包含的信息
        /// </summary>
        /// <param name="listView_temp">播放列表(ListView_Temp)</param>
        /// <returns>当前选定歌曲文件的绝对路径</returns>
        public string Analyze_ListView_Selected_Items_Song_Info(ListView listView_temp)
        {


            return "";
        }


        /// <summary>
        /// 自动生成ListView中指定样式的Items
        /// </summary>
        /// <param name="Song_No">歌曲序号</param>
        /// <param name="Song_Name">歌曲名</param>
        /// <param name="Singer_Name">歌手名</param>
        /// <param name="Song_Time">歌曲时间</param>
        public void Create_ListView_Items_SongInfo(int Song_No, string Song_Name, string Singer_Name, string Song_Time)
        {


        }

        /// <summary>
        /// 将自动生成的ListView中指定样式的Items添加至指定的播放列表中(ListView_Temp_Info)
        /// </summary>
        public void ListView_Add_CreateSelectItems(ListViewItem listViewItem_temp)
        {
            ListViewItem item = new ListViewItem();
            item.Content = "123123123";


            //this.ListView_Temp_Info.Items.Add(item);
        }

        //从外部文件中加载 Button控件
        public void LoadEmbeddedXaml2()
        {
            XmlTextReader xmlreader = new XmlTextReader(@"E:\墨智音乐 - KRC\墨智音乐_WPF\墨智音乐_WPF\XAML资源自动生成\歌单项自动生成\Create_ListView_Items.xaml");
            UIElement obj = XamlReader.Load(xmlreader) as UIElement;



            ListView_Temp_Info.Items.Add(obj);
            //ListView_Temp_Info.Items.Add(get);
        }

        private void ListView_Temp_Info_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadEmbeddedXaml2();
            Get_Selected_MouseDoubleClick_Song_Info();
        }
    }
}
