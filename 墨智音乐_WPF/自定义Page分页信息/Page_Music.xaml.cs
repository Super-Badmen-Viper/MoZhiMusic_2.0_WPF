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

using System.IO;
using System.Windows.Threading;
using System.Diagnostics;

namespace 墨智音乐_WPF.自定义Page分页信息
{
    /// <summary>
    /// Page_Music.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Music : Page
    {
        public Page_Music()
        {
            InitializeComponent();

            Init_Page_Image_Info();
        }

        public class Page_Music_Top_Image
        {
            public BitmapImage Top_Image;
        }
        List<Page_Music_Top_Image> page_Music_Top_Images = new List<Page_Music_Top_Image>();

        public class Page_Music_ToDay_Song_Info
        {
            public BitmapImage ToDay_Song_Image;
        }
        List<Page_Music_ToDay_Song_Info> page_Music_ToDay_Song_Infos = new List<Page_Music_ToDay_Song_Info>();


        #region 今日专属音乐推荐




        #endregion



        #region 初始化加载

        /// <summary>
        /// 初始化加载页面图片
        /// </summary>
        public void Init_Page_Image_Info()
        {
            //初始化加载TOP滚动图片
            Init_Top_Image_Info();
            //初始化加载今日专属音乐推荐
            Init_ToDay_Song_Image_Info();
            //初始化加载为你挑选的歌曲
            Init_Select_Song_For_You_Info();
        }

        #region 初始化加载TOP滚动图片
        public void Init_Top_Image_Info()
        {



            //数据源绑定至page_Music_Top_Images
            //ListView_Top_Image_Info.ItemsSource = page_Music_Top_Images;
        }
        #endregion

        #region 初始化加载今日专属音乐推荐

        string Path_App = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
        string[] ToDay_Song_Image_Url = new string[12];
        public void Init_ToDay_Song_Image_Info()
        {
            /*ToDay_Song_Image_Url[0] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\24k Magic.jpg";
            ToDay_Song_Image_Url[1] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\Uptown Funk.jpg";
            ToDay_Song_Image_Url[2] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\别.jpg";
            ToDay_Song_Image_Url[3] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\不爱我就拉倒.jpg";
            ToDay_Song_Image_Url[4] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\不能说的秘密 电影原声.jpg";
            ToDay_Song_Image_Url[5] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\丑八怪.jpg";
            ToDay_Song_Image_Url[6] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\初学者.jpg";
            ToDay_Song_Image_Url[7] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\等你下课.jpg";
            ToDay_Song_Image_Url[8] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\范特西.jpg";
            ToDay_Song_Image_Url[9] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\黄金甲EP.jpg";
            ToDay_Song_Image_Url[10] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\霍元甲.jpg";
            ToDay_Song_Image_Url[11] = Path_App + @"\资源\歌曲信息资源\专辑图片\Output\听！我们的歌.jpg";

            ToDay_Song_Text_1.Text = "24k Magic";
            ToDay_Song_Text_2.Text = "Uptown Funk";
            ToDay_Song_Text_3.Text = "别";
            ToDay_Song_Text_4.Text = "不爱我就拉倒";
            ToDay_Song_Text_5.Text = "不能说的秘密 电影原声";
            ToDay_Song_Text_6.Text = "丑八怪";
            ToDay_Song_Text_7.Text = "初学者";
            ToDay_Song_Text_8.Text = "等你下课";
            ToDay_Song_Text_9.Text = "范特西";
            ToDay_Song_Text_10.Text = "黄金甲EP";
            ToDay_Song_Text_11.Text = "霍元甲";
            ToDay_Song_Text_12.Text = "听！我们的歌";

            ToDay_Song_1.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[0])));
            ToDay_Song_2.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[1])));
            ToDay_Song_3.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[2])));
            ToDay_Song_4.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[3])));
            ToDay_Song_5.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[4])));
            ToDay_Song_6.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[5])));
            ToDay_Song_7.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[6])));
            ToDay_Song_8.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[7])));
            ToDay_Song_9.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[8])));
            ToDay_Song_10.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[9])));
            ToDay_Song_11.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[10])));
            ToDay_Song_12.This_BackGround_To_Border.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[11])));*/

        
        }
        #endregion

        #region 初始化加载为你挑选的歌曲

        public void Init_Select_Song_For_You_Info()
        {
            /*Select_Song_For_You_Image_1.Song_Like_Image.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[6])));
            Select_Song_For_You_Image_1.Select_Song_For_You_SongName.Text = "初学者";
            Select_Song_For_You_Image_1.Select_Song_For_You_SingerName.Text = "薛之谦";
            Select_Song_For_You_Image_2.Song_Like_Image.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[7])));
            Select_Song_For_You_Image_2.Select_Song_For_You_SongName.Text = "等你下课";
            Select_Song_For_You_Image_2.Select_Song_For_You_SingerName.Text = "周杰伦";
            Select_Song_For_You_Image_3.Song_Like_Image.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[8])));
            Select_Song_For_You_Image_3.Select_Song_For_You_SongName.Text = "范特西";
            Select_Song_For_You_Image_3.Select_Song_For_You_SingerName.Text = "周杰伦";

            Select_Song_For_You_Image_4.Song_Like_Image.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[6])));
            Select_Song_For_You_Image_4.Select_Song_For_You_SongName.Text = "初学者";
            Select_Song_For_You_Image_4.Select_Song_For_You_SingerName.Text = "薛之谦";
            Select_Song_For_You_Image_5.Song_Like_Image.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[7])));
            Select_Song_For_You_Image_5.Select_Song_For_You_SongName.Text = "等你下课";
            Select_Song_For_You_Image_5.Select_Song_For_You_SingerName.Text = "周杰伦";
            Select_Song_For_You_Image_6.Song_Like_Image.Background = new ImageBrush(new BitmapImage(new Uri(ToDay_Song_Image_Url[8])));
            Select_Song_For_You_Image_6.Select_Song_For_You_SongName.Text = "范特西";
            Select_Song_For_You_Image_6.Select_Song_For_You_SingerName.Text = "周杰伦";*/
        }

        #endregion

        #endregion
    }
}
