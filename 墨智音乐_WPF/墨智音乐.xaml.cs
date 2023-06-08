using Shell32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Threading;//定时器
using 墨智音乐_WPF.Dao_类封装.歌单信息类;
using 墨智音乐_WPF.Dao_类封装.歌词信息类;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using 墨智音乐_WPF.自定义控件.悬浮歌词框;
using System.Threading;

namespace 墨智音乐_WPF
{
    /// <summary>
    /// 墨智音乐.xaml 的交互逻辑
    /// </summary>
    public partial class 墨智音乐 : Window
    {
        public string Path_App;


        private static 墨智音乐 FrmMain;
        public static 墨智音乐 Retuen_This()
        {
            FrmMain = Return_This_listView_Item_Bing_ALL();
            return FrmMain;
        }
        public static 墨智音乐 Return_This_listView_Item_Bing_ALL()
        {
            if (FrmMain == null)
            {
                FrmMain = new 墨智音乐();
            }
            return FrmMain;
        }

        public 墨智音乐()
        {
            InitializeComponent();

            //显示位置在屏幕中心
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //设置主界面
            Grid_Music_Player.Visibility = Visibility.Hidden;
            Grid_主界面_FrmMain.Visibility = Visibility.Visible;
            Grid_MV.Visibility = Visibility.Hidden;

            //
            Path_App = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory) + @"资源";
            Image_墨智音乐 = new BitmapImage(new Uri(Path_App + @"\歌曲信息资源\歌手图片\歌手图片1\巨浪.jpg"));
            Image_唱片4 = new BitmapImage(new Uri(Path_App + @"\图片资源\歌曲图片\唱片4.png"));

            //开启内存清理
            //Clear_Memo_Thread();

            Image_Song_Storyboard = Resources["Image_Song_Animation"] as Storyboard;

            Load_Start = 1;
        }

        //记录已初始化完成  （0：未完成，1：正在进行，2：完成）
        public int Load_Start;

        /// <summary>
        /// 性能·优化    歌词匹配1%，歌词滚动一行 约3% ，歌手图片切换动画5%
        /// listview 更改为 listbox
        /// 图片切换动画性能优化，采用其它动画
        /// </summary>
        #region UI更新

        public void DoEvents()
        {
           /* DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(delegate (object f)
                {
                    ((DispatcherFrame)f).Continue = false;

                    return null;
                }
                    ), frame);
            Dispatcher.PushFrame(frame);*/
        }

        #endregion

        #region 内存清理

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]//指定系统回收内存的工具：kernel32.dll
        public static extern int SetProcessWorkingSetSize(IntPtr Delete_All_Info, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();//调用系统的垃圾回收器——处理未使用闲置的服务进程
            GC.WaitForPendingFinalizers();//将当前所占用的服务进程排成队列，当指定的服务被清除后关闭
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)//获取当前的.Net应用程序
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        public void Clear_Memo(object sender,EventArgs e)
        {
            //内存清理
            ClearMemory();
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

            //UI更新
            //DoEvents();
        }
        public void Clear_Memo_Thread()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10000);
            dispatcherTimer.Tick += Clear_Memo;
            dispatcherTimer.Start();
        }

        #endregion

        #region 初始化加载

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            singer_photo[0] = "歌手图片1";
            singer_photo[1] = "歌手图片2";
            singer_photo[2] = "歌手图片3";
            singer_photo[3] = "歌手图片4";
            singer_photo[4] = "歌手图片5";
            singer_photo[5] = "歌手图片6";
            singer_photo[6] = "歌手图片7";
            singer_photo[7] = "歌手图片8";
            singer_photo[8] = "歌手图片9";
            singer_photo[9] = "歌手图片10";
            singer_photo[10] = "歌手图片11";
            singer_photo[11] = "歌手图片12";
            singer_photo[12] = "歌手图片13";
            singer_photo[13] = "歌手图片14";
            singer_photo[14] = "歌手图片15";
            singer_photo[15] = "歌手图片16";
            singer_photo[16] = "歌手图片17";
            singer_photo[17] = "歌手图片18";
            singer_photo[18] = "歌手图片19";
            singer_photo[19] = "歌手图片20";
            singer_photo[20] = "歌手图片21";
            singer_photo[21] = "歌手图片22";
            singer_photo[22] = "歌手图片23";
            singer_photo[23] = "歌手图片24";

            //初始化背景切换动画事件加载
            bgstoryboard = new Storyboard();
            bgstoryboard.AutoReverse = false;
            bgstoryboard.FillBehavior = FillBehavior.HoldEnd;
            bgstoryboard.RepeatBehavior = new RepeatBehavior(1);
            BgSwitchIni();

            

            timer_Singer_Photo_One = new DispatcherTimer();
            timer_Singer_Photo_One.Interval = TimeSpan.FromMilliseconds(7777);
            timer_Singer_Photo_One.Tick += Change_Singer_Photo_To_Grid_Back;

            timer_Singer_Photo_One_Lot = new DispatcherTimer();
            timer_Singer_Photo_One_Lot.Interval = TimeSpan.FromMilliseconds(7777);
            timer_Singer_Photo_One_Lot.Tick += Change_Singer_Photo_To_Grid_Back_Lot;


            userControl_主界面_FrmMain.Loaded += userControl_主界面_FrmMain_Loaded;

        }
        #endregion

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


        Storyboard Image_Song_Storyboard;
        #region 定时器   (Image控件)专辑旋转  
        //界面默认背景
        BitmapImage Image_墨智音乐;
        BitmapImage Image_唱片4;

        /// <summary>
        /// 启动定时器   专辑(Image控件)旋转
        /// </summary>
        public void Start_Timer_Image_Trans_Tick()
        {
            //Timer_Image_Trans.Start();
            Image_Song_Storyboard.Begin();
            Bool_Timer_Image_Trans = true;
        }
        /// <summary>
        /// 关闭定时器   专辑(Image控件)旋转
        /// </summary>
        public void Stop_Timer_Image_Trans_Tick()
        {
            //Timer_Image_Trans.Stop();
            Image_Song_Storyboard.Stop();
            Bool_Timer_Image_Trans = false;
        }

        string Song_Image_Url = @"";
        ShellClass Shell32_Class = new ShellClass();//调用Shell32.dll  ,   查找mp3文件信息
        Folder Folderdir;
        FolderItem FolderItemitem;
        /// <summary>
        /// 切换歌曲专辑图片
        /// </summary>
        public void Change_Image_Song()
        {
            //获取歌曲专辑名
            string Song_Image_Name = "";
            /*            try
                        {  */
            Folderdir = Shell32_Class.NameSpace(System.IO.Path.GetDirectoryName(userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Source.ToString()));
            FolderItemitem = Folderdir.ParseName(System.IO.Path.GetFileName(userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Source.ToString()));
            Song_Image_Name = Folderdir.GetDetailsOf(FolderItemitem, 14);
           /* }
            catch
            {
                //防止非法字符
            }*/

            //如果读取到专辑名
            if (Song_Image_Name.Length > 0) //专辑模式
            {
                //显示专辑名
                TextBox_SongListName.Text = Song_Image_Name;
                TextBox_SongListName.TextAlignment = TextAlignment.Center;

                //生成专辑名所在路径
                Song_Image_Url = Path_App + @"\歌曲信息资源\专辑图片\Output\" + Song_Image_Name + @".jpg";
                //如果专辑文件存在
                if (File.Exists(Song_Image_Url))
                {
                    Image_Song.Source = new BitmapImage(new Uri(Song_Image_Url));
                    //Panel_Image.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));

                    userControl_ALL_Button_Music_PANEL_BUTTOM.Image_Song_Buttom.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                }
                //如果专辑文件不存在
                else
                {
                    //如果当前歌曲名不为空
                    if (Song_Name != null)
                    {
                        //专辑名为歌曲名
                        Song_Image_Name = Song_Name;

                        //生成专辑名所在路径
                        Song_Image_Url = Path_App + @"\歌曲信息资源\专辑图片\Output\" + Song_Image_Name + @".jpg";
                        //如果歌曲图片文件存在
                        if (File.Exists(Song_Image_Url))
                        {
                            Image_Song.Source = new BitmapImage(new Uri(Song_Image_Url));
                            //Panel_Image.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                            userControl_ALL_Button_Music_PANEL_BUTTOM.Image_Song_Buttom.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                        }
                        //默认图片
                        else
                        {
                            //获取歌手名
                            string Singer_Image_Name = Singer_Name.Trim();
                            //生成专辑名所在路径
                            Song_Image_Url = Path_App + @"\歌曲信息资源\专辑图片\Output\" + Singer_Image_Name + @".jpg";
                            //如果歌曲图片存在
                            if (File.Exists(Song_Image_Url))
                            {
                                Image_Song.Source = new BitmapImage(new Uri(Song_Image_Url));
                                //Panel_Image.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                                userControl_ALL_Button_Music_PANEL_BUTTOM.Image_Song_Buttom.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                            }
                            else
                            {
                                Image_Song.Source = null;
                                //Panel_Image.Background = new ImageBrush(Image_唱片4);
                                userControl_ALL_Button_Music_PANEL_BUTTOM.Image_Song_Buttom.Background = new ImageBrush(Image_墨智音乐);
                            }
                        }
                    }
                }
            }
            //未读取到专辑名
            else
            {
                //不显示专辑名
                TextBox_SongListName.Text = "";
                TextBox_SongListName.TextAlignment = TextAlignment.Center;

                //如果当前歌曲名不为空
                if (Song_Name != null)
                {
                    string Song_Name_Temp = Song_Name;
                    int Song_Name_Temp_Last_Num = Song_Name_Temp.LastIndexOf("-");
                    if (Song_Name_Temp_Last_Num > 0)
                    {
                        Song_Name_Temp.Substring(Song_Name_Temp_Last_Num + 1).Trim();
                    }

                    //专辑名为歌曲名
                    Song_Image_Name = Song_Name_Temp.Substring(Song_Name_Temp_Last_Num + 1).Trim();


                    //生成专辑名所在路径
                    Song_Image_Url = Path_App + @"\歌曲信息资源\专辑图片\Output\" + Song_Image_Name + @".jpg";
                    //如果歌曲图片存在
                    if (File.Exists(Song_Image_Url))
                    {
                        Image_Song.Source = new BitmapImage(new Uri(Song_Image_Url));
                        //Panel_Image.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                        userControl_ALL_Button_Music_PANEL_BUTTOM.Image_Song_Buttom.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                    }
                    //默认图片
                    else
                    {
                        //获取歌手名
                        string Singer_Image_Name = Singer_Name.Trim();
                        //生成专辑名所在路径
                        Song_Image_Url = Path_App + @"\歌曲信息资源\专辑图片\Output\" + Singer_Image_Name + @".jpg";
                        //如果歌曲图片存在
                        if (File.Exists(Song_Image_Url))
                        {
                            Image_Song.Source = new BitmapImage(new Uri(Song_Image_Url));
                            //Panel_Image.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                            userControl_ALL_Button_Music_PANEL_BUTTOM.Image_Song_Buttom.Background = new ImageBrush(new BitmapImage(new Uri(Song_Image_Url)));
                        }
                        else
                        {
                            Image_Song.Source = null;
                            //Panel_Image.Background = new ImageBrush(Image_唱片4);
                            userControl_ALL_Button_Music_PANEL_BUTTOM.Image_Song_Buttom.Background = new ImageBrush(Image_墨智音乐);
                        }
                    }

                }
            }
        }

        #endregion

        #region   切换歌手图片背景

        string Singer_Image_Url;
        string Singer_Name_Temp="未知歌手";//记录当前歌手名
        int Singer_Name_Temp_Nums;//记录当前歌手图片动画状态

        public void Change_Image_Singer_FrmMain_OnePhoto()
        {
            //如果歌手名不为空
            if (Singer_Name != null)
            {

                
                //如果当前播放的歌曲信息不为空
                if (Singer_Name != null)
                {
                    //获取歌手名
                    string Singer_Image_Name = Singer_Name.Trim();
                    //生成歌手图片所在路径
                    Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[0] + @"\" + Singer_Image_Name + @".jpg";
                    //如果歌手图片存在
                    if (File.Exists(Singer_Image_Url))
                    {
                        FrmMain_Music.Background = new ImageBrush(new BitmapImage(new Uri(Singer_Image_Url)));
                    }
                    else
                    {
                        //多歌手
                        if (Singer_Image_Name.IndexOf("、") > 0)
                        {
                            Singer_Image_Name = Singer_Image_Name.Substring(0, Singer_Image_Name.IndexOf("、"));

                            Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[0] + @"\" + Singer_Image_Name + @".jpg";
                            //如果歌手图片存在
                            if (File.Exists(Singer_Image_Url))
                            {
                                FrmMain_Music.Background = new ImageBrush(new BitmapImage(new Uri(Singer_Image_Url)));
                            }
                            else
                            {
                                Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\歌手图片1\巨浪.jpg";
                                FrmMain_Music.Background = new ImageBrush(Image_墨智音乐);
                            }
                        }
                        else
                        {
                            Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\歌手图片1\巨浪.jpg";
                            FrmMain_Music.Background = new ImageBrush(Image_墨智音乐);
                        }

                            
                    }

                    if (Bool_Windows_Wallpaper == true)
                    {
                        Change_Windows_Background();//切换桌面写真
                    }

                    
                }
            }
        }

        /// <summary>
        /// 切换歌手背景图片
        /// </summary>
        public void Change_Image_Singer()
        {
            //如果歌手名不为空
            if (Singer_Name != null)
            {
                //如果当前歌手不是上一位歌手
                if (Singer_Name_Temp != Singer_Name.Trim())
                {
                    //关闭多歌手模式
                    timer_Singer_Photo_One.Stop();
                    timer_Singer_Photo_One_Lot.Stop();

                    //如果当前播放的歌曲信息不为空
                    if (Singer_Name != null)
                    {
                        //获取歌手名
                        string Singer_Image_Name = Singer_Name.Trim();
                        //生成歌手图片所在路径
                        Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[0] + @"\" + Singer_Image_Name + @".jpg";
                        //如果歌手图片存在
                        if (File.Exists(Singer_Image_Url))
                        {
                            Create_Arraylist_Singer_Photo(Take_Singer_Nums(Singer_Image_Name));
                            Singer_Name_Temp = Singer_Image_Name;
                        }
                        //默认图片
                        else
                        {
                            //多歌手
                            if (Singer_Image_Name.IndexOf("、") > 0)
                            {
                                Create_Arraylist_Singer_Photo(Take_Singer_Nums(Singer_Image_Name));
                                Singer_Name_Temp = Singer_Image_Name;
                            }
                            else
                            {

                                FrmMain_Music.Background = new ImageBrush(Image_墨智音乐);
                                Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\歌手图片1\巨浪.jpg";

                                //清空歌手图片轮播信息
                                //周杰伦、梁心颐、杨瑞代
                                temp = null;
                                //每个歌手的照片数
                                Singer_Photo_Nums = null;
                                //多图歌手序号
                                singer_photo_nums_More = null;
                                List_Singer_Names = null;

                                Singer_Name_Temp = "未知歌手";
                                Singer_Name_Temp_Nums = 0;

                                if (Bool_Windows_Wallpaper == true)
                                {
                                    Change_Windows_Background();//切换桌面写真
                                }
                            }
                        }
                    }
                    //播放的歌曲信息为空
                    //默认图片
                    else
                    {
                        FrmMain_Music.Background = new ImageBrush(Image_墨智音乐);
                        Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\歌手图片1\巨浪.jpg";

                        //清空歌手图片轮播信息
                        //周杰伦、梁心颐、杨瑞代
                        temp = null;
                        //每个歌手的照片数
                        Singer_Photo_Nums = null;
                        //多图歌手序号
                        singer_photo_nums_More = null;
                        List_Singer_Names = null;

                        Singer_Name_Temp = "未知歌手";
                        Singer_Name_Temp_Nums = 0;

                        if (Bool_Windows_Wallpaper == true)
                        {
                            Change_Windows_Background();//切换桌面写真
                        }
                    }
                }
                else
                {
                    if (Singer_Name_Temp_Nums == 1)
                    {
                        //MessageBox.Show("1");
                        timer_Singer_Photo_One.Start();
                        timer_Singer_Photo_One_Lot.Stop();

                        Bool_Timer_Singer_Photo_1 = true;
                        Bool_Timer_Singer_Photo_1_lot = false;
                    }
                    else if (Singer_Name_Temp_Nums == 2)
                    {
                        //MessageBox.Show("2");
                        timer_Singer_Photo_One.Stop();
                        timer_Singer_Photo_One_Lot.Start();

                        Bool_Timer_Singer_Photo_1 = false;
                        Bool_Timer_Singer_Photo_1_lot = true;
                    }
                    
                }
            }
        }

        /// <summary>
        /// 切换歌曲，歌手，专辑名
        /// </summary>
        public void Change_TextBox_To_SingerSong_Name()
        {
            //如果当前播放的歌曲信息不为空
            if (Song_Name != null)
            {
                string Song_Name_Temp = Song_Name;

                int Song_Name_Temp_Last_Num = Song_Name_Temp.LastIndexOf(" - ");

                /*if (Song_Name_Temp_Last_Num > 0)
                {
                    Song_Name_Temp.Substring(Song_Name_Temp_Last_Num + 3).Trim();
                }*/

                //设置歌手名
                TextBox_SingerName.Text = Singer_Name;
                TextBox_SingerName.TextAlignment = TextAlignment.Center;
                //设置歌曲名
                //TextBox_SongName.Text = Song_Name_Temp.Substring(Song_Name_Temp_Last_Num + 3).Trim();
                TextBox_SongName.Text = Song_Name;
                TextBox_SongName.TextAlignment = TextAlignment.Center;
                //设置歌曲全名
                userControl_ALL_Button_Music_PANEL_BUTTOM.Song_Name.Text = Song_Name;

            }

        }

        /// <summary>
        /// 歌手数量
        /// </summary>
        public int Take_Singer_Nums(string Singer_Image_Name)
        {
            int nums = 1;

            for(int i = 0;i < Singer_Image_Name.Length;i++)
            {
                if (Singer_Image_Name.IndexOf("、") > 0)
                {
                    nums++;
                    Singer_Image_Name = Singer_Image_Name.Substring(Singer_Image_Name.IndexOf("、") + 1);
                }
                else
                {
                    break;
                }
            }
          
            return nums;//歌手的数量
        }

        string[] temp;
        int Singer_Nums = 0;
        //存储当前歌曲的歌手名
        string[] List_Singer_Names;
        /// <summary>
        /// 创建歌手数组
        /// </summary>
        /// <param name="singer_nums"></param>
        public void Create_Arraylist_Singer_Photo(int singer_nums)
        {
            Singer_Nums = singer_nums;
            List_Singer_Names_nums = singer_nums - 1;

            //周杰伦、梁心颐、杨瑞代
            temp = new string[singer_nums];
            //每个歌手的照片数
            Singer_Photo_Nums = new int[singer_nums];
            //多图歌手序号
            singer_photo_nums_More = new int[singer_nums];


            for (int i = 0;i < singer_nums;i++)
            {
                if(temp[i] == null)
                {
                    temp[i] = Singer_Name.Trim();
                }
            }


            List_Singer_Names = new string[singer_nums];

            for(int i = 0;i < List_Singer_Names.Length;i++)
            {
                if(List_Singer_Names[i] == null)
                {
                    if(temp[i] != null)
                    {
                        if (temp[i].IndexOf("、") > 0)
                        {
                            List_Singer_Names[i] = temp[i].Substring(0, temp[i].IndexOf("、"));
                            Temp_Singer_Photo_Nums(i,List_Singer_Names[i]);//检测每个歌手的照片数量

                            if (temp[i + 1] != null)
                            {         
                                string tempstring = temp[i + 1].Substring(temp[i].IndexOf("、") + 1);
                                //清除所有字符前一位的
                                for (int j = i + 1;j < List_Singer_Names.Length;j++)
                                {
                                    if(temp[j] != null)
                                    {
                                        temp[j] = tempstring;
                                    }                                    
                                }
                            }
                        }
                        else
                        {
                            List_Singer_Names[i] = temp[i];
                            Temp_Singer_Photo_Nums(i,List_Singer_Names[i]);//检测每个歌手的照片数量
                            break;
                        }
                    }
                    
                }
            }

            

            singer_times = 0;
            Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[0] + @"\" + List_Singer_Names[singer_times] + @".jpg";
            //如果歌手图片存在
            if (File.Exists(Singer_Image_Url))
            {
                //1719,10
                FrmMain_Music.Background = new ImageBrush(new BitmapImage(new Uri(Singer_Image_Url)));

                if (List_Singer_Names.Length - 1 == 0)
                {
                    singer_times = 0;
                }
                else
                {
                    singer_times = 1;

                    singer_photo_nums_More[0] = 1;
                }

                singer_photo_nums = 1;

                Start_Singer_Photo_Change_Timer();
            }
            else
            {
                FrmMain_Music.Background = new ImageBrush(Image_墨智音乐);

                Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\歌手图片1\巨浪.jpg";
            }
        }

        DispatcherTimer timer_Singer_Photo_One;
        DispatcherTimer timer_Singer_Photo_One_Lot;
        /// <summary>
        /// 开始歌手图片切换轮播
        /// </summary>
        public void Start_Singer_Photo_Change_Timer()
        {
           
            //单个歌手
            if (singer_times == 0)
            {
                timer_Singer_Photo_One_Lot.Stop();
                timer_Singer_Photo_One.Start();

                Bool_Timer_Singer_Photo_1 = true;//记录状态
                Bool_Timer_Singer_Photo_1_lot = false;

                Singer_Name_Temp_Nums = 1;
            }
            else
            {
                timer_Singer_Photo_One.Stop();
                timer_Singer_Photo_One_Lot.Start();

                Bool_Timer_Singer_Photo_1 = false;//记录状态
                Bool_Timer_Singer_Photo_1_lot = true;

                Singer_Name_Temp_Nums = 2;
            }
        }

        //存储歌手含有的照片数
        int[] Singer_Photo_Nums = new int[6];
        public void Temp_Singer_Photo_Nums(int singer_index,string singer_name)
        {
            Singer_Photo_Nums[singer_index] = 0;

            //List_Singer_Names[singer_times]
            for (int i = 0;i < singer_photo.Length - 1;i++)
            {
                Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[i] + @"\" + singer_name + @".jpg";
                if (File.Exists(Singer_Image_Url))
                {
                    Singer_Photo_Nums[singer_index]++;
                }
            }
        }

        //存储歌手照片所在文件夹名
        string[] singer_photo = new string[24];
        int singer_photo_nums = 0;

        int List_Singer_Names_nums = 0;
        int singer_times = 0;
        /// <summary>
        /// Timer  切换歌手背景图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Change_Singer_Photo_To_Grid_Back(object sender, EventArgs e)
        {         
            Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[singer_photo_nums] + @"\" + List_Singer_Names[singer_times] + @".jpg";           
            if (!File.Exists(Singer_Image_Url))
            {
                singer_photo_nums = 0;
                Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[singer_photo_nums] + @"\" + List_Singer_Names[singer_times] + @".jpg";
            }
            if (File.Exists(Singer_Image_Url))
            {
                Windows_FrmMain.Background = new ImageBrush(new BitmapImage(new Uri(Singer_Image_Url)));
                BgSwitch(Singer_Image_Url);
                bgstoryboard.Begin(this);
                if (Bool_Windows_Wallpaper == true)
                {
                    Change_Windows_Background();//切换桌面写真
                }

                singer_times++;
                singer_photo_nums++;
                if (singer_times > List_Singer_Names_nums)
                {
                    singer_times = 0;
                }
                if (singer_photo_nums > singer_photo.Length - 1)
                {
                    singer_photo_nums = 0;
                }
            }
            else
            {
                timer_Singer_Photo_One.Stop();
            }
        }


        int[] singer_photo_nums_More = new int[6];
        /// <summary>
        /// Timer  切换歌手背景图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Change_Singer_Photo_To_Grid_Back_Lot(object sender, EventArgs e)
        {
            
            //如果该歌手图片数量超过1张
            if (Singer_Photo_Nums[singer_times] > 1)
            {
                Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[singer_photo_nums_More[singer_times]] + @"\" + List_Singer_Names[singer_times] + @".jpg";
                if (File.Exists(Singer_Image_Url))
                {
                    Windows_FrmMain.Background = new ImageBrush(new BitmapImage(new Uri(Singer_Image_Url)));
                    BgSwitch(Singer_Image_Url);
                    bgstoryboard.Begin(this);
                    if (Bool_Windows_Wallpaper == true)
                    {
                        Change_Windows_Background();//切换桌面写真
                    }

                    //超过当前文件夹序号
                    if (singer_photo_nums_More[singer_times] > Singer_Photo_Nums[singer_times] - 2)
                    {
                        singer_photo_nums_More[singer_times] = 0;
                    }
                    else
                    {
                        singer_photo_nums_More[singer_times]++;
                    }

                    //歌手名下标变化
                    singer_times++;
                    if (singer_times > List_Singer_Names_nums)
                    {
                        singer_times = 0;
                    }
                }
                else
                {
                    timer_Singer_Photo_One_Lot.Stop();
                }
            }
            else if (Singer_Photo_Nums[singer_times] == 1)
            {
                Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[singer_photo_nums] + @"\" + List_Singer_Names[singer_times] + @".jpg";
                if (!File.Exists(Singer_Image_Url))
                {
                    singer_photo_nums = 0;
                    Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[singer_photo_nums] + @"\" + List_Singer_Names[singer_times] + @".jpg";
                }

                if (File.Exists(Singer_Image_Url))
                {
                    Windows_FrmMain.Background = new ImageBrush(new BitmapImage(new Uri(Singer_Image_Url)));
                    BgSwitch(Singer_Image_Url);
                    bgstoryboard.Begin(this);
                    if (Bool_Windows_Wallpaper == true)
                    {
                        Change_Windows_Background();//切换桌面写真
                    }

                    singer_times++;
                    singer_photo_nums++;
                    if (singer_times > List_Singer_Names_nums)
                    {
                        singer_times = 0;
                    }
                    if (singer_photo_nums > singer_photo.Length - 1)
                    {
                        singer_photo_nums = 0;
                    }
                }
                else
                {
                    timer_Singer_Photo_One_Lot.Stop();
                }
            }
            else //如果该歌手没有图片
            if (Singer_Photo_Nums[singer_times] == 0)
            {
                List_Singer_Names_nums -= 1;

                //从歌手图片切换列表中清除该歌手
                int[] temp_nums = new int[Singer_Photo_Nums.Length - 1];
                for(int i = 0;i < Singer_Photo_Nums.Length; i++)
                {
                    if(Singer_Photo_Nums[i] != 0)
                    {
                        if(temp_nums[i] == 0)
                        {
                            temp_nums[i] = Singer_Photo_Nums[i];
                        }
                    }
                }
                Singer_Photo_Nums = new int[temp_nums.Length];
                for (int i = 0; i < Singer_Photo_Nums.Length; i++)
                {
                    if (temp_nums[i] != 0)
                    {
                        if (Singer_Photo_Nums[i] == 0)
                        {
                            Singer_Photo_Nums[i] = temp_nums[i];
                        }
                    }
                }
                //歌手名下标变化
                singer_times++;
                singer_photo_nums++;
                if (singer_times > List_Singer_Names_nums)
                {
                    singer_times = 0;
                }
                if (singer_photo_nums > singer_photo.Length - 1)
                {
                    singer_photo_nums = 0;
                }

                //如果该歌手图片数量超过1张
                if (Singer_Photo_Nums[singer_times] > 1)
                {
                    Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[singer_photo_nums_More[singer_times]] + @"\" + List_Singer_Names[singer_times] + @".jpg";
                    if (File.Exists(Singer_Image_Url))
                    {
                        Windows_FrmMain.Background = new ImageBrush(new BitmapImage(new Uri(Singer_Image_Url)));
                        BgSwitch(Singer_Image_Url);
                        bgstoryboard.Begin(this);
                        if (Bool_Windows_Wallpaper == true)
                        {
                            Change_Windows_Background();//切换桌面写真
                        }

                        //超过当前文件夹序号
                        if (singer_photo_nums_More[singer_times] > Singer_Photo_Nums[singer_times] - 2)
                        {
                            singer_photo_nums_More[singer_times] = 0;
                        }
                        else
                        {
                            singer_photo_nums_More[singer_times]++;
                        }

                        //歌手名下标变化
                        singer_times++;
                        if (singer_times > List_Singer_Names_nums)
                        {
                            singer_times = 0;
                        }
                    }
                    else
                    {
                        timer_Singer_Photo_One_Lot.Stop();
                    }
                }
                else if (Singer_Photo_Nums[singer_times] == 1)
                {
                    Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[singer_photo_nums] + @"\" + List_Singer_Names[singer_times] + @".jpg";
                    if (!File.Exists(Singer_Image_Url))
                    {
                        singer_photo_nums = 0;
                        Singer_Image_Url = Path_App + @"\歌曲信息资源\歌手图片\" + singer_photo[singer_photo_nums] + @"\" + List_Singer_Names[singer_times] + @".jpg";
                    }

                    if (File.Exists(Singer_Image_Url))
                    {
                        Windows_FrmMain.Background = new ImageBrush(new BitmapImage(new Uri(Singer_Image_Url)));
                        BgSwitch(Singer_Image_Url);
                        bgstoryboard.Begin(this);
                        if (Bool_Windows_Wallpaper == true)
                        {
                            Change_Windows_Background();//切换桌面写真
                        }

                        singer_times++;
                        singer_photo_nums++;
                        if (singer_times > List_Singer_Names_nums)
                        {
                            singer_times = 0;
                        }
                        if (singer_photo_nums > singer_photo.Length - 1)
                        {
                            singer_photo_nums = 0;
                        }
                    }
                    else
                    {
                        timer_Singer_Photo_One_Lot.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// 过渡动画效果占用5%CPU使用率
        /// </summary>
        ObjectAnimationUsingKeyFrames oa;
        /// <summary>
        /// 对指定的图片路径进行动画处理
        /// </summary>
        /// <param name="imgPath"></param>
        private void BgSwitch(string imgPath)
        {
            /*var obj = bgstoryboard.Children.FirstOrDefault(c => c is ObjectAnimationUsingKeyFrames);
            if (obj != null)
            {
                oa = obj as ObjectAnimationUsingKeyFrames;
                if (oa.KeyFrames.Count > 0)
                {
                    oa.KeyFrames[0].Value = new BitmapImage(new Uri(imgPath));
                }
            }*/
            oa = bgstoryboard.Children.FirstOrDefault(c => c is ObjectAnimationUsingKeyFrames) as ObjectAnimationUsingKeyFrames;
            oa.KeyFrames[0].Value = new BitmapImage(new Uri(imgPath));
        }


        Storyboard bgstoryboard = null;
        /// <summary>
        /// 动画生成
        /// </summary>
        private void BgSwitchIni()
        {
            //动画占用过高CPU及GPU
            //此动画效果为渲染所有的像素，效率过低
            //应更改为     多区块渲染过渡 / 线性渲染过渡 / 淡化渲染过渡 / 模糊重叠过渡渲染
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            EasingDoubleKeyFrame sd = new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(666)));
            da.KeyFrames.Add(sd);
            Storyboard.SetTargetName(da, FrmMain_Music.Name);
            DependencyProperty[] propertyChain = new DependencyProperty[]
            {
                    Panel.BackgroundProperty,
                    Brush.OpacityProperty
            };
            Storyboard.SetTargetProperty(da, new PropertyPath("(0).(1)", propertyChain));

            ObjectAnimationUsingKeyFrames oa = new ObjectAnimationUsingKeyFrames();
            DiscreteObjectKeyFrame diso = new DiscreteObjectKeyFrame(new BitmapImage(new Uri(@"/Test;component/Image/bg.jpg", UriKind.Relative)), KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(10)));
            oa.KeyFrames.Add(diso);
            oa.BeginTime = new TimeSpan(0, 0, 0, 1, 0);
            Storyboard.SetTargetName(oa, FrmMain_Music.Name);
            DependencyProperty[] propertyChain2 = new DependencyProperty[]
            {
                    Panel.BackgroundProperty,
                    ImageBrush.ImageSourceProperty
            };
            Storyboard.SetTargetProperty(oa, new PropertyPath("(0).(1)", propertyChain2));

            DoubleAnimationUsingKeyFrames da2 = new DoubleAnimationUsingKeyFrames();
            da2.BeginTime = new TimeSpan(0, 0, 0, 1, 5);
            EasingDoubleKeyFrame sd2 = new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(666)));
            da2.KeyFrames.Add(sd2);
            Storyboard.SetTargetName(da2, FrmMain_Music.Name);
            Storyboard.SetTargetProperty(da2, new PropertyPath("(0).(1)", propertyChain));

            bgstoryboard.Children.Add(da);
            bgstoryboard.Children.Add(oa);
            bgstoryboard.Children.Add(da2);



            
        }


        #endregion


        #region PANEL_BUTTOM_Loaded初始化加载

        public ImageBrush brush_Play = new ImageBrush();//最大化
        public ImageBrush brush_Pause = new ImageBrush();//正常窗口

        /// <summary>
        /// userControl_ALL_Button_Music_PANEL_BUTTOM_Loaded初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userControl_ALL_Button_Music_PANEL_BUTTOM_Loaded(object sender, RoutedEventArgs e)
        {
            //隐藏部分控件
            ListBox_Select_ListView.Visibility = Visibility.Hidden;
            ListView_Temp_Info.Visibility = Visibility.Hidden;


            //加载歌曲进度条
            //userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.MediaOpened += MediaElement_MediaOpened;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.ValueChanged += Timeline_ValueChanged;
            userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.ValueChanged += Timeline_ValueChanged_MV;
            //加载siler鼠标是否悬浮事件          
            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.MouseMove += Mouse_Over_Silder_Music_Width;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.MouseLeave += Mouse_Leave_Silder_Music_Width;
            userControl_ALL_Button_Music_PANEL_BUTTOM.MouseLeave += Mouse_Leave_Silder_Music_Width;

            //加载siler鼠标是否悬浮事件          
            userControl_MV.userControl_MV_Take.Silder_Music_Width.MouseMove += Mouse_Over_Silder_Music_Width_MV;
            userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.MouseLeave += Mouse_Leave_Silder_Music_Width_MV;



            //加载播放按键图片按键
            //pack://application:,,,/Resources/home_back.png
            brush_Play.ImageSource = new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\播放键_Play.png"));
            brush_Pause.ImageSource = new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\播放键_Pause.png"));
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background = brush_Play;

            //加载播放按键绑定事件
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Click += Button_Music_Play_Pause_Song;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Before.Click += Button_Music_Up_Song;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Next.Click += Button_Music_Next_Song;


            //绑定右下角功能Click事件
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_ListView_Selected.Click += Button_Open_KRC_Click;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Open_Song_Image_Tran.Click += Button_Open_Song_Image_Tran_Click;


            Thickness_Temp_TextBox = new Thickness();
            Thickness_Temp_ListView = new Thickness();
            //保存歌词控件的位置
            Thickness_Temp_TextBox = TextBox_ListViewKRC_Up.Margin;
            Thickness_Temp_ListView = ListView_Temp_KRC.Margin;

            //播放器缓冲流绑定
            userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.MediaOpened += MediaElement_Song_MediaOpened;
            userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.MediaEnded += MediaElement_Song_MediaEnded;

            //绑定桌面歌词Click
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Desk_KRC.Click += Button_Desk_KRC_Click;

            //初始化桌面歌词
            DispatcherTimer_KRC = new DispatcherTimer();
            DispatcherTimer_KRC.Tick += new EventHandler(Media_Song_KRC_Play_Tick); // 超过计时器间隔时发生，时间轴向前走1秒       
            DispatcherTimer_KRC.Interval = TimeSpan.FromMilliseconds(111); // 间隔1秒

            //初始化歌曲进度
            dispatcherTimer_Silder = new DispatcherTimer();
            dispatcherTimer_Silder.Tick += new EventHandler(DispatcherTimer_Silder_Tick); // 超过计时器间隔时发生，时间轴向前走1秒       
            dispatcherTimer_Silder.Interval = TimeSpan.FromMilliseconds(111); // 间隔1秒



            //初始化音频波形图  UserControl_Music_MAV_GDI
            userControl_Music_MAV_GDI.Loaded += Load_UserControl_Music_MAV_GDI;

        }




        #endregion


        #region 歌曲资源初始化加载

        private void MediaElement_Song_MediaOpened(object sender, RoutedEventArgs e)//一定几率导致双缓冲,同时执行开启与结束事件
        {
            Load_MediaElement_Song_MediaOpened();
        }
        public void Load_MediaElement_Song_MediaOpened()
        {

            Bool_Timer_Image_Trans = true;

            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Maximum = userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.NaturalDuration.TimeSpan.TotalMilliseconds;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Maximum = userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Maximum;

            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value = 0;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Value = 0;

            test2 = userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.NaturalDuration.TimeSpan;

            dispatcherTimer_Silder.Start();

            //切换Windows背景
            if (Bool_Windows_Wallpaper == true)
            {
                Change_Windows_Background();
            }

            //内存清理
            ClearMemory();
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

            //如果位于主界面
            if (Bool_Button_Back_Click == true)
            {
                if (DispatcherTimer_KRC.IsEnabled == true)
                {
                    timer_Singer_Photo_One.Stop();//图片切换动画
                    Bool_Timer_Singer_Photo_1 = false;

                    timer_Singer_Photo_One_Lot.Stop();//图片切换动画
                    Bool_Timer_Singer_Photo_1_lot = false;

                    Image_Song_Storyboard.Stop();//专辑旋转
                    Bool_Timer_Image_Trans = false;

                    /*if (Bool_Desk_Krc == false)
                    {
                        DispatcherTimer_KRC.Stop();//歌词同步
                    }*/
                    //Bool_Timer_KRC = false;
                    Bool_Button_MV_CLick = true;
                    Bool_Button_Back_Click = true;
                }
            }
        }

        private void MediaElement_Song_MediaEnded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer_Silder.Stop();

            WMP_Song_Play_Ids_UP_DOWN = 1;

            if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Cycle)
            {
                Change_MediaElement_Source();

                Load_MediaElement_Song_MediaOpened();
            }
            else
            {
                Change_MediaElement_Song_id_incrse();
                Change_MediaElement_Source();
            }
        }

        #endregion

        #region 跳转歌曲进度

        /// <summary>
        /// 当ListView_Temp_KRC   鼠标滚轮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ListView_Temp_KRC_MouseWheel(object sender, EventArgs e)
        {
            if (ListView_Temp_KRC.Visibility == Visibility.Visible)
            {
                if (ListView_Temp_KRC.SelectedIndex != -1)
                {
                    ListView_Temp_KRC_Temp.SelectedIndex = ListView_Temp_KRC.SelectedIndex;
                    ListView_Temp_KRC_Temp.ScrollIntoView(ListView_Temp_KRC_Temp.Items[ListView_Temp_KRC_Temp.SelectedIndex + KRC_Line_Nums]);//移动到指定行   

                    ListView_Temp_KRC_Temp.Visibility = Visibility.Visible;
                    ListView_Temp_KRC.Visibility = Visibility.Hidden;
                    //ListView_KRC.Visibility = Visibility.Hidden;
                }
            }

        }
        /// <summary>
        /// 当ListView_Temp_KRC   鼠标移出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ListView_Temp_KRC_MouseLeave(object sender, EventArgs e)
        {
            Show_Media_Siler();
        }
        /// <summary>
        /// 开启歌曲进度同步滑块
        /// </summary>
        public void Show_Media_Siler()
        {
            //ListView_KRC.Visibility = Visibility.Visible;
            ListView_Temp_KRC.Visibility = Visibility.Visible;
            ListView_Temp_KRC_Temp.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// 根据歌词跳转歌曲进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_Temp_KRC_Temp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //歌词滚动
            if (ListBox_KRC_Song_KRC_Time != null)
            {
                int line_num = this.ListView_Temp_KRC_Temp.SelectedIndex;

                if (ListBox_KRC_Song_KRC_Time[line_num] != 0)
                {

                    //跳转至指定Value的进度
                    //ts_Song = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(ListBox_KRC_Song_KRC_Time[line_num]));
                    userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Position = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(ListBox_KRC_Song_KRC_Time[line_num]));

                    //如果选中的  跳转的播放进度  在  当前播放进度  之前
                    if (line_num < ListView_Temp_KRC.SelectedIndex)
                    {
                        ListView_Temp_KRC.ScrollIntoView(ListView_Temp_KRC.Items[0]);//先滚动至第一行歌词
                        //ListView_KRC.ScrollIntoView(ListView_KRC.Items[0]);//先滚动至第一行歌词
                    }
                    //歌词时间匹配方法  会自动跳转至指定选中歌词行

                    //关闭歌词选择进度面板
                    Show_Media_Siler();
                }
            }
        }

        #endregion
        #region 时间轴sidler
        /// <summary>
        /// 鼠标移入silder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mouse_Over_Silder_Music_Width(object sender, MouseEventArgs e)
        {
            TimeLine_Nums = userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value;

            //同步两个silder的长度
            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Value = userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value;

            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Visibility = Visibility.Visible;
        }
        public void Mouse_Over_Silder_Music_Width_MV(object sender, MouseEventArgs e)
        {
            TimeLine_Nums_MV = userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value;

            //同步两个silder的长度
            userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Value = userControl_MV.userControl_MV_Take.Silder_Music_Width.Value;

            userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 鼠标移除silder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mouse_Leave_Silder_Music_Width(object sender, MouseEventArgs e)
        {
            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Visibility = Visibility.Hidden;

            userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value = userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Value;
        }
        public void Mouse_Leave_Silder_Music_Width_MV(object sender, MouseEventArgs e)
        {
            userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Visibility = Visibility.Hidden;
        }



        DispatcherTimer dispatcherTimer_Silder;    // 用于时间轴  
        public static double TimeLine_Nums;
        public static double TimeLine_Nums_MV;

        /// <summary>
        /// 直接跳转会导致播放器控件连续触发读取完成，读完事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //只有在silder_temp值改变才执行歌曲进度跳转
            if (TimeLine_Nums != userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Value)
            {
                //只有在鼠标悬浮与silder_temp上才执行歌曲进度跳转
                if (userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.IsMouseOver)
                {
                    dispatcherTimer_Silder.Stop();

                    userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Position = new TimeSpan(0, 0, 0, 0, (int)userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Value);

                    userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Visibility = Visibility.Hidden;


                    //歌词滚动
                    if (ListBox_KRC_Song_KRC_Time != null)
                    {
                        //如果选中的  跳转的播放进度  在  当前播放进度  之前
                        if (userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Value < userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value)
                        {
                            ListView_Temp_KRC.ScrollIntoView(ListView_Temp_KRC.Items[0]);//先滚动至第一行歌词
                            //ListView_KRC.ScrollIntoView(ListView_KRC.Items[0]);//先滚动至第一行歌词
                        }
                        //歌词时间匹配方法  会自动跳转至指定选中歌词行
                    }

                    dispatcherTimer_Silder.Start();

                }
            }
        }
        private void Timeline_ValueChanged_MV(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //只有在silder_temp值改变才执行歌曲进度跳转
            if (TimeLine_Nums_MV != userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Value)
            {
                //决定两个是否显示
                userControl_MV.userControl_MV_Take.Silder_Music_Width.Visibility = Visibility.Visible;
                userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Visibility = Visibility.Hidden;

                if (userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.IsMouseOver)
                {
                    dispatcherTimer_Silder.Stop();

                    if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
                    {
                        userControl_MV.MediaMent_MV.Position = new TimeSpan(0, 0, 0, 0, (int)userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Value);
                    }

                    userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Visibility = Visibility.Hidden;
                    userControl_MV.userControl_MV_Take.Silder_Music_Width.Visibility = Visibility.Visible;

                    userControl_MV.MediaMent_MV.Play();
                    userControl_MV.MediaMent_MV.LoadedBehavior = MediaState.Play;
                    userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Pause;
                    userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Pause();
                    userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior = MediaState.Pause;
                    userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background = brush_Play;

                    dispatcherTimer_Silder.Start();
                }
            }
        }

        TimeSpan test1;
        TimeSpan test2;
        TimeSpan test1_MV;
        TimeSpan test2_MV;
        private void DispatcherTimer_Silder_Tick(object sender, EventArgs e)
        {
            if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null && userControl_MV.MediaMent_MV.LoadedBehavior == MediaState.Play)
            {
                test1_MV = userControl_MV.MediaMent_MV.Position;
                // 时间轴slider滑动值随播放内容位置变化
                userControl_MV.userControl_MV_Take.Silder_Music_Width.Value = userControl_MV.MediaMent_MV.Position.TotalMilliseconds;
                TimeLine_Nums = userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value;
                userControl_MV.userControl_MV_Take.TextBox_Song_Time.Text = test1_MV.ToString(@"mm\:ss") + @"\" + test2_MV.ToString(@"mm\:ss");
                userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Value = userControl_MV.userControl_MV_Take.Silder_Music_Width.Value;
            }
            else if(userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior == MediaState.Play && userControl_MV.MediaMent_MV.LoadedBehavior == MediaState.Pause)
            {

                test1 = userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Position;

                // 时间轴slider滑动值随播放内容位置变化
                userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value = userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Position.TotalMilliseconds;

                TimeLine_Nums = userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value;

                userControl_ALL_Button_Music_PANEL_BUTTOM.TextBox_Song_Time.Text = test1.ToString(@"mm\:ss") + @"\" + test2.ToString(@"mm\:ss");

                userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Temp_Width.Value = userControl_ALL_Button_Music_PANEL_BUTTOM.Silder_Music_Width.Value;
            }
        }
        #endregion

        #region 音频波形图同步音频

        //定时器   音频波形图同步音频
        public static DispatcherTimer Timer_Textblock_Height = new DispatcherTimer();
        //音频波形图同步音频  同步音频的textblick高度
        double nums_textblock_height = 0;

        /// <summary>
        /// 启动定时器   音频波形图同步音频
        /// </summary>
        public void Start_Timer_Textblock_Height_Tick()
        {


        }
        /// <summary>
        /// 关闭定时器   音频波形图同步音频
        /// </summary>
        public void Stop_Timer_Textblock_Height_Tick()
        {


        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Textblock_Height_Tick(object sender, EventArgs e)
        {


            //测试同步高度
            userControl_Music_MAV_GDI.TextBlock_Middle.Height = nums_textblock_height;
            userControl_Music_MAV_GDI.TextBlock_Middle.Height = nums_textblock_height;
            userControl_Music_MAV_GDI.TextBlock_Middle.Height = nums_textblock_height;
        }


        /// <summary>
        /// 音频波形图 Load_UserControl_Music_MAV_GDI 初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Load_UserControl_Music_MAV_GDI(object sender, RoutedEventArgs e)
        {
            //初始化歌曲进度
            Timer_Textblock_Height = new DispatcherTimer();
            Timer_Textblock_Height.Tick += new EventHandler(Timer_Textblock_Height_Tick); // 超过计时器间隔时发生，时间轴向前走1秒       
            Timer_Textblock_Height.Interval = TimeSpan.FromMilliseconds(11); // 间隔1秒


        }

        #endregion


        //不能采用接口式，会导致BUG,(CPU调度导致数值不正确)
        public int WMP_Song_Play_Ids;
        public int WMP_Song_Play_Ids_UP_DOWN;

        public static int Check_MediaElement_Song_State_SourceUpdated_Nums_1 = 0;
        public static int Check_MediaElement_Song_State_SourceUpdated_Nums_2 = 0;

        Random rd = new Random();

        //int Song_No;
        string Singer_Name;
        string Song_Name;
        string Song_Url;
        public int Select_DoubleClick_ListView = 0;
        #region 歌曲切换

        /// <summary>
        /// 双击播放音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_Temp_Info_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {           
            if (Panel_Image.Visibility == Visibility.Visible)
            {
                Image_Song_Storyboard.Begin();
                Bool_Timer_Image_Trans = true;
            }

            Get_ListView_SelectedItem_SongUrl();
        }
        public void Get_ListView_SelectedItem_SongUrl()
        {
            Select_DoubleClick_ListView = 1;

            WMP_Song_Play_Ids_UP_DOWN = 1;

            Change_MediaElement_Song_id_incrse();
            Change_MediaElement_Source();
        }
        /// <summary>
        /// 播放/暂停音乐
        /// </summary>
        public void Button_Music_Play_Pause_Song(object sender, EventArgs e)
        {
            //休眠50ms
            Thread.Sleep(50);

            if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background == brush_Play)
            {
                userControl_MV.MediaMent_MV.Pause();
                userControl_MV.MediaMent_MV.LoadedBehavior = MediaState.Pause;
                userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Play;

                userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Play();
                userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior = MediaState.Play;

                userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background = brush_Pause;

                if (Panel_Image.Visibility == Visibility.Visible)
                {
                    Image_Song_Storyboard.Resume();
                    Bool_Timer_Image_Trans = true;
                }

                dispatcherTimer_Silder.Start();

                if (Window_Desk_krc.Text_Storyboard != null)
                {
                    Window_Desk_krc.Text_Storyboard.Resume();
                    Window_Desk_krc.Text_Storyboard_slider_Up.Resume();
                    Window_Desk_krc.Text_Storyboard_slider_Down.Resume();
                }

                

            }
            else
            {
                userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Pause();
                userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior = MediaState.Pause;

                userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background = brush_Play;

                //关闭旋转
                //Stop_Timer_Image_Trans_Tick();
                Image_Song_Storyboard.Pause();
                Bool_Timer_Image_Trans = false;

                dispatcherTimer_Silder.Stop();

                if (Window_Desk_krc.Text_Storyboard != null)
                {
                    Window_Desk_krc.Text_Storyboard.Pause();
                    Window_Desk_krc.Text_Storyboard_slider_Up.Pause();
                    Window_Desk_krc.Text_Storyboard_slider_Down.Pause();
                }

            }


        }
        /// <summary>
        /// 上一首
        /// </summary>
        public void Button_Music_Up_Song(object sender, EventArgs e)
        {
            WMP_Song_Play_Ids_UP_DOWN = -1;
            Change_MediaElement_Song_id_incrse();
            Change_MediaElement_Source();        
        }
        /// <summary>
        /// 下一首
        /// </summary>
        public void Button_Music_Next_Song(object sender, EventArgs e)
        {           
            WMP_Song_Play_Ids_UP_DOWN = 1;
            Change_MediaElement_Song_id_incrse();
            Change_MediaElement_Source();           
        }

        /// <summary>
        /// 指定歌曲id的值
        /// </summary>
        public void Change_MediaElement_Song_id_incrse()
        {
            //顺序播放
            if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Normal)
            {
                if (WMP_Song_Play_Ids_UP_DOWN == 1)
                {
                    if (WMP_Song_Play_Ids != ListView_Temp_Info.Items.Count)
                        WMP_Song_Play_Ids++;
                    else
                        WMP_Song_Play_Ids = 1;
                }
                else if (WMP_Song_Play_Ids_UP_DOWN == -1)
                {
                    if (WMP_Song_Play_Ids != 1)
                        WMP_Song_Play_Ids--;
                    else
                        WMP_Song_Play_Ids = ListView_Temp_Info.Items.Count;
                }
            }
            //单曲循环
            else if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Cycle)
            {
                if (WMP_Song_Play_Ids_UP_DOWN == 1)
                {
                    if (WMP_Song_Play_Ids != ListView_Temp_Info.Items.Count)
                        WMP_Song_Play_Ids++;
                    else
                        WMP_Song_Play_Ids = 1;
                }
                else if (WMP_Song_Play_Ids_UP_DOWN == -1)
                {
                    if (WMP_Song_Play_Ids != 1)
                        WMP_Song_Play_Ids--;
                    else
                        WMP_Song_Play_Ids = ListView_Temp_Info.Items.Count;
                }
            }
            //随机播放
            else if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Random)
            {
                WMP_Song_Play_Ids = rd.Next(1, this.ListView_Temp_Info.Items.Count);//(生成1~10之间的随机数，不包括10)
            }
        }

        //ListView_Item_Bing listView_Temp_Infos_temp = new ListView_Item_Bing();
        /// <summary>
        /// 根据歌曲id的值，播放指定路径
        /// </summary>
        public void Change_MediaElement_Source()
        {         
            Window_Desk_krc.TextBlock_1.Text = "科技源于生活，技术源于创新";
            Window_Desk_krc.TextBlock_2.Text = "毒蛇云生态，致力于生活更便捷";
            if (Window_Desk_krc.Visibility == Visibility.Visible)
            {        
                Window_Desk_krc.Text_DoubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 3333));
                Window_Desk_krc.Text_Storyboard.Begin();             
            }


            if (Select_DoubleClick_ListView == 1)
            {
                WMP_Song_Play_Ids = ListView_Temp_Info.SelectedIndex + 1;

                Select_DoubleClick_ListView = 0;
            }

            try
            {
                //同步listview之间的数据
                //ListView_Temp_Info.ItemsSource = userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource;

                string uri = /*Path_App + @"\歌曲信息资源\歌曲文件\" + */((ListView_Item_Bing)ListView_Temp_Info.Items[WMP_Song_Play_Ids - 1]).Song_Url;
                if (!File.Exists(uri))
                    uri = Path_App + @"\歌曲信息资源\歌曲文件\" + ((ListView_Item_Bing)ListView_Temp_Info.Items[WMP_Song_Play_Ids - 1]).Song_Url;

                //指定播放路径
                userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Source = new Uri(uri);
                Song_Url = userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Source.ToString();

                //保存当前正在播放的歌曲信息
                //Song_No = ((ListView_Item_Bing)ListView_Temp_Info.Items[WMP_Song_Play_Ids - 1]).Song_No;
                Song_Name = ((ListView_Item_Bing)ListView_Temp_Info.Items[WMP_Song_Play_Ids - 1]).Song_Name;
                Singer_Name = ((ListView_Item_Bing)ListView_Temp_Info.Items[WMP_Song_Play_Ids - 1]).Singer_Name;
                //Song_Url = ((ListView_Item_Bing)ListView_Temp_Info.Items[WMP_Song_Play_Ids - 1]).Song_Url;

                //开始播放
                userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Play();
                //设置播放器播放状态为play
                userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior = MediaState.Play;
                //设置播放
                userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background = brush_Pause;

                //歌曲逻辑
                Change_MediaElement_Song_Source();
                //生成歌曲名
                Song_KRC_Path = ((ListView_Item_Bing)ListView_Temp_Info.Items[WMP_Song_Play_Ids - 1]).Song_Name;
                //切换专辑图片
                Change_Image_Song();
                //切换歌曲，歌手，专辑名
                Change_TextBox_To_SingerSong_Name();
                //选中播放的列
                ListView_Temp_Info.SelectedIndex = WMP_Song_Play_Ids - 1;
                if(ListView_Temp_Info_ItemSource_Name.Equals(bool_ListView_Temp_Info_End_Clear.userControl_主界面_FrmMain_ListView_Temp_Info_ItemSource_Name))
                    userControl_主界面_FrmMain.ListView_Temp_Info.SelectedIndex = WMP_Song_Play_Ids - 1;


                //暂停MV播放
                userControl_MV.MediaMent_MV.Pause();
                userControl_MV.MediaMent_MV.LoadedBehavior = MediaState.Pause;
                userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Play;

                //检测歌手数量，设置歌手动画循环方式
                if (Singer_Name.IndexOf("、") <= 0)//单歌手
                {
                    Bool_Timer_Singer_Photo_1 = true;
                    Bool_Timer_Singer_Photo_1_lot = false;
                }
                else//多歌手
                {
                    Bool_Timer_Singer_Photo_1 = false;
                    Bool_Timer_Singer_Photo_1_lot = true;
                }

                //生成歌词路径
                Create_Steam_Song_KRC();

                //如果位于主界面
                if (Grid_Music_Player.Visibility == Visibility.Visible)
                {
                    //生成歌手图片切换——仅播放器模式
                    Load_GridMusicPlayer_Song_Info();
                }
                else
                {
                    //生成歌手图片切换——仅主页模式
                    Change_Image_Singer_FrmMain_OnePhoto();

                    Bool_Load_GridMusicPlayer_Song_Info = false;
                }

                //歌单歌曲排序
                userControl_主界面_FrmMain.Sort_SongList();

                //专辑旋转
                if (Panel_Image.Visibility == Visibility.Visible)
                {
                    Image_Song_Storyboard.Begin();
                    Bool_Timer_Image_Trans = true;
                }
            }
            catch
            {
                MessageBox.Show("此音乐文件路径不存在");
            }
        }
        public bool Bool_Load_GridMusicPlayer_Song_Info;
        /// <summary>
        /// 歌曲资源加载
        /// </summary>
        public void Load_GridMusicPlayer_Song_Info()
        {                   
            //切换歌手图片
            Change_Image_Singer();

            Bool_Load_GridMusicPlayer_Song_Info = true;
        }


        /// <summary>
        /// 歌曲切换事件，歌曲逻辑
        /// </summary>
        public void Change_MediaElement_Song_Source()
        {
            //开启歌曲时间轴silder
            Show_Media_Siler();           
        }

        #endregion


        string ListView_Temp_Info_ItemSource_Name;
        //检测处于哪个歌单
        Bool_listView_Temp_Info_End_Clear bool_ListView_Temp_Info_End_Clear = Bool_listView_Temp_Info_End_Clear.Retuen_This();
        #region 歌单切换

        /// <summary>
        /// 主界面初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_主界面_FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            userControl_主界面_FrmMain.SongList_ALL.Click += SongList_ALL_Click;
            userControl_主界面_FrmMain.SongList_Love.Click += SongList_Love_Click;
            userControl_主界面_FrmMain.SongList_Auto.Click += SongList_Auto_Click;
            userControl_主界面_FrmMain.ListView_Temp_Info.MouseDoubleClick += userControl_主界面_FrmMain_ListView_Temp_Info_MouseDoubleClick;

        }

        /// <summary>
        /// 歌单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SongList_ALL_Click(object sender, EventArgs e)
        {
            if (listView_Item_Bing_ALL.listView_Temp_Info_End_ALL != null)
            {
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_ALL;
                bool_ListView_Temp_Info_End_Clear.userControl_主界面_FrmMain_ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_ALL";

                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_ALL_Clear = true;
                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Love_Clear = false;
                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Auto_Clear = false;

                //隐藏ListView_Temp_Info中listviewitem中Check控件
                userControl_主界面_FrmMain.GridViewColumn_Check_ListView_Song.Width = 0;

                //歌单歌曲排序
                userControl_主界面_FrmMain.Sort_SongList();
                //保存歌单信息
                Save_DataGridView();
            }
        }
        public void SongList_Love_Click(object sender, EventArgs e)
        {
            if (listView_Item_Bing_ALL.listView_Temp_Info_End_Love != null)
            {
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_Love;
                bool_ListView_Temp_Info_End_Clear.userControl_主界面_FrmMain_ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_Love";

                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_ALL_Clear = false;
                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Love_Clear = true;
                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Auto_Clear = false;

                //隐藏ListView_Temp_Info中listviewitem中Check控件
                userControl_主界面_FrmMain.GridViewColumn_Check_ListView_Song.Width = 0;

                //歌单歌曲排序
                userControl_主界面_FrmMain.Sort_SongList();
                //保存歌单信息
                Save_DataGridView();
            }
        }
        public void SongList_Auto_Click(object sender, EventArgs e)
        {
            if (listView_Item_Bing_ALL.listView_Temp_Info_End_Auto != null)
            {
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_Auto;
                bool_ListView_Temp_Info_End_Clear.userControl_主界面_FrmMain_ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_Auto";

                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_ALL_Clear = false;
                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Love_Clear = false;
                bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Auto_Clear = true;



                //隐藏ListView_Temp_Info中listviewitem中Check控件
                userControl_主界面_FrmMain.GridViewColumn_Check_ListView_Song.Width = 0;

                //歌单歌曲排序
                userControl_主界面_FrmMain.Sort_SongList();
                //保存歌单信息
                Save_DataGridView();
            }
        }

        /// <summary>
        /// 双击播放音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userControl_主界面_FrmMain_ListView_Temp_Info_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //歌单歌曲排序
            //userControl_主界面_FrmMain.Sort_SongList();


            //切换歌曲播放列表
            if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_ALL_Clear == true)
            {
                if (listView_Item_Bing_ALL.listView_Temp_Info_End_ALL != null)
                {
                    ListView_Temp_Info.ItemsSource = null;
                    ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_ALL;
                    ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_ALL";
                }
            }
            else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Love_Clear == true)
            {
                if (listView_Item_Bing_ALL.listView_Temp_Info_End_Love != null)
                {
                    ListView_Temp_Info.ItemsSource = null;
                    ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_Love;
                    ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_Love";
                }
            }
            else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Auto_Clear == true)
            {
                if (listView_Item_Bing_ALL.listView_Temp_Info_End_Auto != null)
                {
                    ListView_Temp_Info.ItemsSource = null;
                    ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_Auto;
                    ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_Auto";
                }
            }



            //同步歌单选中项
            if (userControl_主界面_FrmMain.ListView_Temp_Info.SelectedIndex != -1)
            {
                ListView_Temp_Info.SelectedIndex = userControl_主界面_FrmMain.ListView_Temp_Info.SelectedIndex;
            }


            Get_ListView_SelectedItem_SongUrl();
        }


        #endregion


        //桌面歌词
        private Window_Desk_KRC Window_Desk_krc = new Window_Desk_KRC();
        #region 歌词切换

        public static string Song_KRC_Path;//歌词文件所在的路径
        public string[] ListBox_KRC_Song_KRC_Text;//歌词文件文本歌词内容的集合
        public double[] ListBox_KRC_Song_KRC_Time;//歌词文件文本歌词时间的集合
        public double Start_Song_KRC_Time;
        public double End_Song_KRC_Time;

        //歌词信息类
        Dao_ListBox_Temp_KRC dao_ListBox_Temp_KRC = new Dao_ListBox_Temp_KRC();
        /// <summary>
        /// 生成歌词路径
        /// </summary>
        public void Create_Steam_Song_KRC()
        {
            //歌词数组信息输出类指代为 Dao_ListBox_Temp_KRC

            //歌词文件文本歌词内容的集合
            ListBox_KRC_Song_KRC_Text = new string[999];
            //歌词文件文本歌词时间的集合
            ListBox_KRC_Song_KRC_Time = new double[999];
            //创建获取 歌词数组信息输出类 
            dao_ListBox_Temp_KRC = new Dao_ListBox_Temp_KRC();
            //设置要分析的歌词文件（krc）路径
            string KRC_URL = Path_App + @"\歌曲信息资源\歌词资源\" + Singer_Name + " - " + Song_Name + @".krc";

            try
            {
                //如果歌词文件存在
                if (File.Exists(KRC_URL))
                {
                    //调用 Dao_ListBox_Temp_KRC内的方法 生成歌词数组方法，分析歌词数组信息并存储在 Dao_ListBox_Temp_KRC内
                    dao_ListBox_Temp_KRC.player_lrc_Save_Text(KRC_URL);

                    //传递歌词数组，将listview的数据源绑定至 分析完成的在Dao_ListBox_Temp_KRC内存储的歌词数组信息
                    ListView_Temp_KRC.ItemsSource = dao_ListBox_Temp_KRC.Return_ListBox_Temp_KRC_Bing();
                    ListView_Temp_KRC_Temp.ItemsSource = ListView_Temp_KRC.ItemsSource;

                    //获取当前歌词文件文本的   歌词内容数组和歌词时间数组
                    //获取分析完成的在 Dao_ListBox_Temp_KRC 内存储的 歌词文件文本歌词内容Text的集合
                    ListBox_KRC_Song_KRC_Text = dao_ListBox_Temp_KRC.Return_ListBox_Temp_KRC_Text();
                    //获取分析完成的在 Dao_ListBox_Temp_KRC 内存储的 歌词文件文本歌词内容Time的集合
                    ListBox_KRC_Song_KRC_Time = dao_ListBox_Temp_KRC.Return_ListBox_Temp_KRC_Time();

                    //生成歌曲第一句和最后一句的时间
                    //获取分析完成的在 Dao_ListBox_Temp_KRC 内存储的 歌曲第一句的时间（毫秒）
                    Start_Song_KRC_Time = dao_ListBox_Temp_KRC.Return_Start_Song_KRC_Time();
                    //获取分析完成的在 Dao_ListBox_Temp_KRC 内存储的 歌曲最后一句的时间（毫秒）
                    End_Song_KRC_Time = dao_ListBox_Temp_KRC.Return_End_Song_KRC_Time();

                    //歌词滚动
                    if (ListBox_KRC_Song_KRC_Time != null)
                    {
                        ListView_Temp_KRC.ScrollIntoView(ListView_Temp_KRC.Items[0]);//先滚动至第一行歌词              
                        //ListView_KRC.ScrollIntoView(ListView_KRC.Items[0]);//先滚动至第一行歌词             
                    }

                    //开启定时器，歌词同步           
                    DispatcherTimer_KRC.Start();
                    //Bool_Timer_KRC = true;

                    //分析运算Dao_ListBox_Temp_KRC 内存储的 树状歌词结构
                    dao_ListBox_Temp_KRC.Take_TreeKrcInfo();

                }
                else
                {
                    //获取krc歌词失败，转而获取Lrc歌词
                    try
                    {
                        KRC_URL = Path_App + @"\歌曲信息资源\歌词资源\" + Song_KRC_Path + @".lrc";
                        if (File.Exists(KRC_URL))
                        {
                            //生成lrc歌词数组
                            dao_ListBox_Temp_KRC.Create_LrcSongInfo(KRC_URL);

                            //传递歌词数组
                            ListView_Temp_KRC.ItemsSource = dao_ListBox_Temp_KRC.Return_ListBox_Temp_KRC_Bing();
                            ListView_Temp_KRC_Temp.ItemsSource = ListView_Temp_KRC.ItemsSource;

                            //获取当前歌词文件文本的   歌词内容数组和歌词时间数组
                            ListBox_KRC_Song_KRC_Text = dao_ListBox_Temp_KRC.Return_ListBox_Temp_KRC_Text();
                            ListBox_KRC_Song_KRC_Time = dao_ListBox_Temp_KRC.Return_ListBox_Temp_KRC_Time();

                            //生成歌曲第一句和最后一句的时间
                            Start_Song_KRC_Time = dao_ListBox_Temp_KRC.Return_Start_Song_KRC_Time();
                            End_Song_KRC_Time = dao_ListBox_Temp_KRC.Return_End_Song_KRC_Time();

                            //歌词滚动
                            if (ListBox_KRC_Song_KRC_Time != null)
                            {
                                ListView_Temp_KRC.ScrollIntoView(ListView_Temp_KRC.Items[0]);//先滚动至第一行歌词              
                                //ListView_KRC.ScrollIntoView(ListView_KRC.Items[0]);//先滚动至第一行歌词             
                            }

                            //歌词同步           
                            DispatcherTimer_KRC.Start();
                            //Bool_Timer_KRC = true;
                        }
                        else
                        {
                            //停止歌词同步
                            DispatcherTimer_KRC.Stop();
                            //Bool_Timer_KRC = false;

                            //ListView_KRC.ItemsSource = null;
                            ListView_Temp_KRC.ItemsSource = null;
                            ListView_Temp_KRC_Temp.ItemsSource = null;

                            ListBox_KRC_Song_KRC_Text = null;
                            ListBox_KRC_Song_KRC_Time = null;
                            Start_Song_KRC_Time = 0;
                            End_Song_KRC_Time = 0;
                        }
                    }
                    catch
                    {
                        //停止歌词同步
                        DispatcherTimer_KRC.Stop();
                        //Bool_Timer_KRC = false;

                        //ListView_KRC.ItemsSource = null;
                        ListView_Temp_KRC.ItemsSource = null;
                        ListView_Temp_KRC_Temp.ItemsSource = null;

                        ListBox_KRC_Song_KRC_Text = null;
                        ListBox_KRC_Song_KRC_Time = null;
                        Start_Song_KRC_Time = 0;
                        End_Song_KRC_Time = 0;
                    }
                }
            }
            catch
            {
                //获取krc歌词失败，转而获取Lrc歌词
                try
                {

                }
                catch
                {
                    //停止歌词同步
                    DispatcherTimer_KRC.Stop();
                    //Bool_Timer_KRC = false;

                    //ListView_KRC.ItemsSource = null;
                    ListView_Temp_KRC.ItemsSource = null;
                    ListView_Temp_KRC_Temp.ItemsSource = null;

                    ListBox_KRC_Song_KRC_Text = null;
                    ListBox_KRC_Song_KRC_Time = null;
                    Start_Song_KRC_Time = 0;
                    End_Song_KRC_Time = 0;
                }             
            }
        }


      
        #endregion

        #region ListView_Temp_KRC歌词行同步
        DispatcherTimer DispatcherTimer_KRC;

        /// <summary>
        /// ListView_Temp_KRC_Loaded初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_Temp_KRC_Loaded(object sender, RoutedEventArgs e)
        {
            //选中项更改
            ListView_Temp_KRC.SelectionChanged += ListView_Temp_KRC_ScrollIntoView;
            //鼠标滚轮
            TextBox_ListViewKRC_Up.MouseWheel += ListView_Temp_KRC_MouseWheel;
            //鼠标移出
            ListView_Temp_KRC_Temp.MouseLeave += ListView_Temp_KRC_MouseLeave;

        }

        TimeSpan Krc_Span;
        int KRC_Line_Nums = 5;

        ListBoxItem myListBoxItem;
        ContentPresenter myContentPresenter;
        DataTemplate myDataTemplate;
        Storyboard myTextBlock_Storyboard;
        DoubleAnimation myTextBlock_DoubleAnimation;
        /// <summary>
        /// 当ListView_Temp_KRC选中项发送改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ListView_Temp_KRC_ScrollIntoView(object sender, EventArgs e)
        {
            KRC_Line_Nums = 5;
            if (DispatcherTimer_KRC.IsEnabled) {
                if (ListView_Temp_KRC.SelectedIndex != -1 && ListView_Temp_KRC.SelectedIndex < ListView_Temp_KRC.Items.Count)
                {
                    ListView_Temp_KRC.ScrollIntoView(ListView_Temp_KRC.Items[ListView_Temp_KRC.SelectedIndex + KRC_Line_Nums]);//移动到指定行 
                    //ListView_KRC.ScrollIntoView(ListView_KRC.Items[ListView_Temp_KRC.SelectedIndex + KRC_Line_Nums]);//移动到指定行 
                    


                    
                    if (ListBox_KRC_Song_KRC_Time[ListView_Temp_KRC.SelectedIndex] != 0)
                    {
                        int temp = dao_ListBox_Temp_KRC.dao_ListBox_Temp_Krc_OneLine[ListView_Temp_KRC.SelectedIndex].Krc_Line_Continue_Time;
                        
                        if (temp > 0)
                        {
                            Window_Desk_krc.Text_DoubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, temp));
                            Window_Desk_krc.Text_Storyboard.Begin();

                            //temp = (int)(ListBox_KRC_Song_KRC_Time[ListView_Temp_KRC.SelectedIndex + 1] - ListBox_KRC_Song_KRC_Time[ListView_Temp_KRC.SelectedIndex]);

                            if (temp < 0)
                                temp = (int)(userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.NaturalDuration.TimeSpan.TotalMilliseconds - ListBox_KRC_Song_KRC_Time[ListView_Temp_KRC.SelectedIndex]);


                            if (myTextBlock_Storyboard != null)
                                myTextBlock_Storyboard.Remove();//清空渐变过的歌词行颜色

                            myListBoxItem =
                                (ListBoxItem)(ListView_Temp_KRC.ItemContainerGenerator.ContainerFromItem(ListView_Temp_KRC.SelectedItem));
                            myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);
                            myDataTemplate = myContentPresenter.ContentTemplate;

                            myTextBlock_Storyboard = (Storyboard)myDataTemplate.FindName("Text_Storyboard", myContentPresenter);
                            myTextBlock_DoubleAnimation = (DoubleAnimation)myDataTemplate.FindName("Text_DoubleAnimation", myContentPresenter);
                            myTextBlock_DoubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, temp));
                            myTextBlock_Storyboard.Begin();

                            //生成歌词提词同步动画
                            if (Window_Desk_krc.Visibility == Visibility.Visible)
                            {
                                temp = (int)(ListBox_KRC_Song_KRC_Time[ListView_Temp_KRC.SelectedIndex + 1] - ListBox_KRC_Song_KRC_Time[ListView_Temp_KRC.SelectedIndex]);
                                if (temp < 0)
                                    temp = (int)(userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.NaturalDuration.TimeSpan.TotalMilliseconds - ListBox_KRC_Song_KRC_Time[ListView_Temp_KRC.SelectedIndex]);
                                //生成歌词同步进度Silder动画
                                Window_Desk_krc.Text_DoubleAnimation_slider_Up.Duration = new Duration(new TimeSpan(0, 0, 0, 0, temp));
                                Window_Desk_krc.Text_Storyboard_slider_Up.Begin();
                                Window_Desk_krc.Text_DoubleAnimation_slider_Down.Duration = new Duration(new TimeSpan(0, 0, 0, 0, temp));
                                Window_Desk_krc.Text_Storyboard_slider_Down.Begin();
                            }

                        }
                    }
                    


                   
                   
                }
            }
        }
        private childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;


            //如何：查找由 DataTemplate 生成的元素
            // Getting the currently selected ListBoxItem
            // Note that the ListBox must have
            // IsSynchronizedWithCurrentItem set to True for this to work
            ListBoxItem myListBoxItem =
                (ListBoxItem)(ListView_Temp_KRC.ItemContainerGenerator.ContainerFromItem(ListView_Temp_KRC.Items.CurrentItem));

            // Getting the ContentPresenter of myListBoxItem
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);

            // Finding textBlock from the DataTemplate that is set on that ContentPresenter
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            TextBlock myTextBlock = (TextBlock)myDataTemplate.FindName("textBlock", myContentPresenter);

            // Do something to the DataTemplate-generated TextBlock
            MessageBox.Show("The text of the TextBlock of the selected list item: "
                + myTextBlock.Text);
        }



        public void Media_Song_KRC_Play_Tick(object sender, EventArgs e)
        {
            //使用双区间来判定同步当前音频文件时间信息所处歌词时间信息的位置
            if (ListBox_KRC_Song_KRC_Time != null)
            {
                if (userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Position.TotalMilliseconds < Start_Song_KRC_Time)
                {
                    for (int i = 0; i < ListBox_KRC_Song_KRC_Time.Length; i++)
                    {
                        if (ListBox_KRC_Song_KRC_Time[i] != 0)
                        {
                            /*if (Bool_Timer_KRC == true)
                            {*/
                                //判定成功，选中该项，同时触发listview选中项Change事件
                            if(ListView_Temp_KRC.SelectedIndex != i)
                                ListView_Temp_KRC.SelectedIndex = i;
                                //ListView_KRC.SelectedIndex = i;
                            //}
                            //TextBlock_1.Text = ListBox_KRC_Song_KRC_Text[i];
                            Window_Desk_krc.TextBlock_1.Text = ListBox_KRC_Song_KRC_Text[i];
                            Window_Desk_krc.TextBlock_2.Text = ListBox_KRC_Song_KRC_Text[i + 1];

                            break;
                        }
                    }
                }
                else if (userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Position.TotalMilliseconds > End_Song_KRC_Time)
                {
                    for (int i = ListBox_KRC_Song_KRC_Time.Length - 1; i >= 0; i--)
                    {
                        if (ListBox_KRC_Song_KRC_Time[i] != 0)
                        {
                            /*if (Bool_Timer_KRC == true)
                            {*/
                            if (ListView_Temp_KRC.SelectedIndex != i)
                                ListView_Temp_KRC.SelectedIndex = i;
                                //ListView_KRC.SelectedIndex = i;
                            //}
                            //TextBlock_1.Text = ListBox_KRC_Song_KRC_Text[i];
                            Window_Desk_krc.TextBlock_1.Text = ListBox_KRC_Song_KRC_Text[i];
                            Window_Desk_krc.TextBlock_2.Text = ListBox_KRC_Song_KRC_Text[i + 1];

                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ListBox_KRC_Song_KRC_Time.Length; i++)
                    {
                        if (ListBox_KRC_Song_KRC_Time[i] != 0)
                        {
                            if (userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Position.TotalMilliseconds >= ListBox_KRC_Song_KRC_Time[i])
                            {
                                if (userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Position.TotalMilliseconds < ListBox_KRC_Song_KRC_Time[i + 1])
                                {
                                    /*if (Bool_Timer_KRC == true)
                                    {*/
                                        ListView_Temp_KRC.SelectedIndex = i;
                                        //ListView_KRC.SelectedIndex = i;
                                    //}
                                    //TextBlock_1.Text = ListBox_KRC_Song_KRC_Text[i];
                                    Window_Desk_krc.TextBlock_1.Text = ListBox_KRC_Song_KRC_Text[i];
                                    Window_Desk_krc.TextBlock_2.Text = ListBox_KRC_Song_KRC_Text[i + 1];

                                    break;
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                DispatcherTimer_KRC.Stop();
            }
        }


        #endregion



        #region 初始化自定义控件事件
        /// <summary>
        /// 初始化自定义控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Top_Loaded(object sender, RoutedEventArgs e)
        {
            //初始样式加载
            brush_Max.ImageSource = new BitmapImage(new Uri(Path_App + @"\图片资源\界面图片\Max.png"));
            brush_MaxNormal.ImageSource = new BitmapImage(new Uri(Path_App + @"\图片资源\界面图片\MaxNormal.png"));
            userControl_ALL_Button_Music_PANEL_TOP.Button_Max.Background = brush_Max;

            userControl_ALL_Button_Music_PANEL_TOP.Button_Back.Visibility = Visibility.Hidden;//返回键隐藏
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Open_Song_Image_Tran.Visibility = Visibility.Hidden;//专辑旋转隐藏

            timer_Singer_Photo_One.Stop();//图片切换动画
            Bool_Timer_Singer_Photo_1 = false;
            timer_Singer_Photo_One_Lot.Stop();//图片切换动画
            Bool_Timer_Singer_Photo_1_lot = false;
            //Image_Song_Storyboard.Stop();//专辑旋转
            //Bool_Timer_Image_Trans = false;

            /*if (Bool_Desk_Krc == false)
            {
                DispatcherTimer_KRC.Stop();//歌词同步
            }*/
            //Bool_Timer_KRC = false;
            Bool_Button_MV_CLick = true;
            Bool_Button_Back_Click = true;




            //最大化，最小化，退出
            userControl_ALL_Button_Music_PANEL_TOP.Button_Exit.Click += Button_Exit_Click;
            userControl_ALL_Button_Music_PANEL_TOP.Button_Max.Click += Button_Max_Click;
            userControl_ALL_Button_Music_PANEL_TOP.Button_Min.Click += Button_Min_Click;

            //播放顺序
            brush_Button_Music_Order_Normal.ImageSource = new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\a_style_0.png"));
            brush_Button_Music_Order_Cycle.ImageSource = new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\a_style_1.png"));
            brush_Button_Music_Order_Random.ImageSource = new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\a_style_2.png"));
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Click += Button_Music_Order_Click;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background = brush_Button_Music_Order_Normal;

            //歌词显示最大化
            Max_ListView_Krc();


            //存储Windows背景图片
            //定义存储缓冲区大小
            StringBuilder s = new StringBuilder(300);
            //获取Window 桌面背景图片地址，使用缓冲区
            SystemParametersInfo(SPI_GETDESKWALLPAPER, 300, s, 0);
            //缓冲区中字符进行转换
            wallpaper_path.Append(s.ToString()); //系统桌面背景图片路径


            //绑定桌面写真按钮事件
            userControl_ALL_Button_Music_PANEL_TOP.Button_Desk_KRC.Click += button_Open_Windows_Picture_Click;
            //绑定返回主界面按钮事件
            userControl_ALL_Button_Music_PANEL_TOP.Button_Back.Click += Button_Back_Click;
            //绑定返回音乐播放按钮事件
            userControl_ALL_Button_Music_PANEL_BUTTOM.Image_Song_Buttom.MouseLeftButtonDown += Image_Play_Music_CLick;
            //绑定播放MV按钮事件
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_MV.Click += Button_MV_Click;

            //播放器缓冲流绑定
            userControl_MV.MediaMent_MV.MediaEnded += Grid_MediaElement_MV_MediaClosing;
            userControl_MV.MediaMent_MV.MediaOpened += Grid_MediaElement_MV_MediaOpened;


        }
        #endregion

        #region 最大化，最小化，退出
        ImageBrush brush_Max = new ImageBrush();//最大化
        ImageBrush brush_MaxNormal = new ImageBrush();//正常窗口

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();//关闭
        }
        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Max_Click(object sender, RoutedEventArgs e)
        {
            if (userControl_ALL_Button_Music_PANEL_TOP.Button_Max.Background == brush_Max)//最大化按钮
            {
                this.WindowState = System.Windows.WindowState.Maximized;

                userControl_ALL_Button_Music_PANEL_TOP.Button_Max.Background = brush_MaxNormal;

                //KRC_Line_Nums = (int)((System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 240) / 40 / 2) + 2;


            }
            else//最小化按钮
            {
                this.WindowState = System.Windows.WindowState.Normal;

                userControl_ALL_Button_Music_PANEL_TOP.Button_Max.Background = brush_Max;

                //KRC_Line_Nums = 9;
            }


            //歌词上下行移动
            //生成歌词路径
            Create_Steam_Song_KRC();
        }
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        #endregion

        #region 返回主界面，返回音乐播放器，全屏

        public bool Bool_Button_Back_Click = true;
        public bool Bool_Timer_Singer_Photo_1;
        public bool Bool_Timer_Singer_Photo_1_lot;
        public bool Bool_Timer_Image_Trans;
       // public bool Bool_Timer_KRC;
        /// <summary>
        /// 返回主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {          
            if(Bool_Button_Back_Click == false)
            {
                Grid_Music_Player.Visibility = Visibility.Hidden;//播放器Grid隐藏
                Grid_主界面_FrmMain.Visibility = Visibility.Visible;//主界面Grid显示
                userControl_ALL_Button_Music_PANEL_TOP.Button_Back.Visibility = Visibility.Hidden;//返回键隐藏
                userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Open_Song_Image_Tran.Visibility = Visibility.Hidden;//专辑旋转隐藏


                timer_Singer_Photo_One.Stop();//图片切换动画
                Bool_Timer_Singer_Photo_1 = false;

                timer_Singer_Photo_One_Lot.Stop();//图片切换动画
                Bool_Timer_Singer_Photo_1_lot = false;

                Image_Song_Storyboard.Pause();//专辑旋转
                Bool_Timer_Image_Trans = false;

                /*if (Bool_Desk_Krc == false)
                {
                    DispatcherTimer_KRC.Stop();//歌词同步
                }*/
                //Bool_Timer_KRC = false;


                Bool_Button_MV_CLick = true;
                Bool_Button_Back_Click = true;
            }        
        }
        /// <summary>
        /// 返回音乐播放器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_Play_Music_CLick(object sender, RoutedEventArgs e)
        {
            if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
            {
                userControl_MV.MediaMent_MV.Pause();
                userControl_MV.MediaMent_MV.LoadedBehavior = MediaState.Pause;
                userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Play;

            }

            if (Bool_Button_Back_Click == true)
            {
                //如果未加载歌曲信息
                if (Bool_Load_GridMusicPlayer_Song_Info == false)
                {
                    Load_GridMusicPlayer_Song_Info();//加载歌曲信息   图片，歌词，专辑旋转
                }

                //切换歌手图片
                Change_Image_Singer();

                Grid_Music_Player.Visibility = Visibility.Visible;//播放器Grid隐藏
                Grid_主界面_FrmMain.Visibility = Visibility.Hidden;//主界面Grid显示

                userControl_ALL_Button_Music_PANEL_TOP.Button_Back.Visibility = Visibility.Visible;
                userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Open_Song_Image_Tran.Visibility = Visibility.Visible;//专辑旋转

                //如果专辑旋转未开启
                if (Bool_Timer_Image_Trans == false)
                {
                    //如果专辑旋转已打开
                    if (Panel_Image.Visibility == Visibility.Visible && userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior == MediaState.Play)//专辑旋转
                    {
                        Image_Song_Storyboard.Begin();
                    }
                }

                if (ListBox_KRC_Song_KRC_Time != null)
                {
                    DispatcherTimer_KRC.Start();//歌词同步
                }
                /*if (Bool_Timer_KRC == false)
                {
                    if (ListView_Temp_KRC.SelectedIndex < ListView_Temp_KRC.Items.Count)
                    {
                        if (ListBox_KRC_Song_KRC_Time != null)
                        {
                            DispatcherTimer_KRC.Start();//歌词同步
                            Bool_Timer_KRC = true;
                        }
                    }
*//*                    else
                    {
                        DispatcherTimer_KRC.Stop();
                    }*//*
                }*/

                Bool_Button_MV_CLick = false;
                Bool_Button_Back_Click = false;

                //xaml_Checked_BeginStoryboard_FrmMain.Begin_Start_board();
            }

            //如果位于主界面
            if (Grid_Music_Player.Visibility == Visibility.Visible)
            {
                //生成歌手图片切换——仅播放器模式
                Load_GridMusicPlayer_Song_Info();
            }
            else
            {
                //生成歌手图片切换——仅主页模式
                Change_Image_Singer_FrmMain_OnePhoto();

                Bool_Load_GridMusicPlayer_Song_Info = false;
            }
        }


        /// <summary>
        /// 全屏事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Screen_Click(object sender, RoutedEventArgs e)
        {

        }


        public ImageBrush brush_Button_Music_Order_Normal = new ImageBrush();//顺序循环
        public ImageBrush brush_Button_Music_Order_Cycle = new ImageBrush();//单曲循环
        public ImageBrush brush_Button_Music_Order_Random = new ImageBrush();//随机播放

        /// <summary>
        /// 播放顺序Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Button_Music_Order_Click(object sender, EventArgs e)
        {
            //顺序播放
            if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Normal)
            {
                //单曲循环
                userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background = brush_Button_Music_Order_Cycle;

            }
            //单曲循环
            else if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Cycle)
            {
                //随机播放
                userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background = brush_Button_Music_Order_Random;

            }
            //随机播放
            else if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Random)
            {
                //顺序播放
                userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background = brush_Button_Music_Order_Normal;

            }
        }


        #endregion

        #region PANEL_BUTTOM_Right_事件   桌面歌词，专辑旋转

        /// <summary>
        /// 打开播放列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Button_Open_KRC_Click(object sender, EventArgs e)
        {
            if (ListBox_Select_ListView.Visibility == Visibility.Hidden && ListView_Temp_Info.Visibility == Visibility.Visible)
            {
                ListView_Temp_Info.Visibility = Visibility.Hidden;
            }
            else if (ListBox_Select_ListView.Visibility == Visibility.Hidden)
            {
                ListBox_Select_ListView.Visibility = Visibility.Visible;
            }
            else
            {
                ListBox_Select_ListView.Visibility = Visibility.Hidden;
                ListView_Temp_Info.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 选择播放列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_Select_ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //显示歌曲列表
            this.ListView_Temp_Info.Visibility = Visibility.Visible;

            string selectedText = ListBox_Select_ListView.SelectedItem.ToString();

            this.ListView_Temp_Info.ItemsSource = null;
            //this.ListView_Temp_Info.Items.Clear();

            foreach (ListItem li in ListView_Temp_Info.Items)
            {
                ListView_Temp_Info.Items.Remove(li);
            }



            if (selectedText.Equals("本地音乐_listView_Temp_Info_End_ALL"))
            {
                if (listView_Item_Bing_ALL.listView_Temp_Info_End_ALL != null)
                {
                    this.ListView_Temp_Info.ItemsSource = null;
                    this.ListView_Temp_Info.ItemsSource = this.listView_Item_Bing_ALL.listView_Temp_Info_End_ALL;
                    ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_ALL";
                }
            }
            else if (selectedText.Equals("我的收藏_listView_Temp_Info_End_Love"))
            {
                if (listView_Item_Bing_ALL.listView_Temp_Info_End_Love != null)
                {
                    this.ListView_Temp_Info.ItemsSource = null;
                    this.ListView_Temp_Info.ItemsSource = this.listView_Item_Bing_ALL.listView_Temp_Info_End_Love;
                    ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_Love";
                }
            }
            else if (selectedText.Equals("默认列表_listView_Temp_Info_End_Auto"))
            {
                if (listView_Item_Bing_ALL.listView_Temp_Info_End_Auto != null)
                {
                    this.ListView_Temp_Info.ItemsSource = null;
                    this.ListView_Temp_Info.ItemsSource = this.listView_Item_Bing_ALL.listView_Temp_Info_End_Auto;
                    ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_Auto";
                }
            }

            ListBox_Select_ListView.Visibility = Visibility.Hidden;

        }

        Thickness Thickness_Temp_TextBox;
        Thickness Thickness_Temp_ListView;
        bool Open_Song_Image_Bool = false;
        /// <summary>
        /// 开启专辑旋转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Button_Open_Song_Image_Tran_Click(object sender, EventArgs e)
        {
            if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
            {

            }
            else
            {
                //关闭专辑旋转
                if (Open_Song_Image_Bool == true)
                {
                    Close_Song_Image_Tran();


                }
                //开启专辑旋转
                else
                {
                    Open_Song_Image_Tran();

                }
            }
        }

        /// <summary>
        /// 开启专辑旋转
        /// </summary>
        public void Open_Song_Image_Tran()
        {
            //ListView_KRC.Margin = thickness_All_KRC_Normal;
            ListView_Temp_KRC.Margin = thickness_All_KRC_Normal;
            ListView_Temp_KRC_Temp.Margin = thickness_All_KRC_Normal;
            TextBox_ListViewKRC_Up.Margin = thickness_TextBox_Normal;

            //ListView_KRC.Width = width_All_KRC_Normal;
            ListView_Temp_KRC.Width = width_All_KRC_Normal;
            ListView_Temp_KRC_Temp.Width = width_All_KRC_Normal;
            TextBox_ListViewKRC_Up.Width = width_TextBox_Normal;

            //ListView_KRC_GridViewColumn.Width = width_All_KRC_Normal - 6;
            ListView_Temp_KRC_GridViewColumn.Width = width_All_KRC_Normal - 6;
            ListView_Temp_KRC_Temp_GridViewColumn.Width = width_All_KRC_Normal - 6;

            Panel_Image.Visibility = Visibility.Visible;

            Open_Song_Image_Bool = true;

            //开启专辑旋转
            if (userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior == MediaState.Play)
                Image_Song_Storyboard.Begin();

            Bool_Timer_Image_Trans = true;
        }
        /// <summary>
        /// 关闭专辑旋转
        /// </summary>
        public void Close_Song_Image_Tran()
        {
            //ListView_KRC.Width = width_All_KRC;
            ListView_Temp_KRC.Width = width_All_KRC;
            ListView_Temp_KRC_Temp.Width = width_All_KRC;
            TextBox_ListViewKRC_Up.Width = width_TextBox;

            //ListView_KRC.Margin = thickness_All_KRC;
            ListView_Temp_KRC.Margin = thickness_All_KRC;
            ListView_Temp_KRC_Temp.Margin = thickness_All_KRC;
            TextBox_ListViewKRC_Up.Margin = thickness_TextBox;

            //ListView_KRC_GridViewColumn.Width = width_All_KRC - 6;
            ListView_Temp_KRC_GridViewColumn.Width = width_All_KRC - 6;
            ListView_Temp_KRC_Temp_GridViewColumn.Width = width_All_KRC - 6;

            Panel_Image.Visibility = Visibility.Hidden;

            Open_Song_Image_Bool = false;

            //关闭专辑旋转
            Image_Song_Storyboard.Stop();
            Bool_Timer_Image_Trans = false;
        }


        public Thickness thickness_All_KRC_Normal = new Thickness();
        public Thickness thickness_TextBox_Normal = new Thickness();
        public double width_All_KRC_Normal;
        public double width_TextBox_Normal;

        public Thickness thickness_All_KRC = new Thickness();
        public Thickness thickness_TextBox = new Thickness();
        public double width_All_KRC;
        public double width_TextBox;

        public void Max_ListView_Krc()
        {
            width_All_KRC_Normal = ListView_Temp_KRC.ActualWidth;
            width_TextBox_Normal = TextBox_ListViewKRC_Up.ActualWidth;

            thickness_All_KRC_Normal = ListView_Temp_KRC.Margin;
            thickness_TextBox_Normal = TextBox_ListViewKRC_Up.Margin;



            //定义TextBox_ListViewKRC_Up
            Thickness thickness = new Thickness();
            thickness = TextBox_ListViewKRC_Up.Margin;
            thickness.Left = (FrmMain_Music.ActualWidth - TextBox_ListViewKRC_Up.ActualWidth) / 2;
            thickness.Right = (FrmMain_Music.ActualWidth - TextBox_ListViewKRC_Up.ActualWidth) / 2;
            TextBox_ListViewKRC_Up.Margin = thickness;

            thickness_TextBox = thickness;

            //
            thickness = new Thickness();
            thickness = ListView_Temp_KRC.Margin;
            thickness.Left = (FrmMain_Music.ActualWidth - ListView_Temp_KRC.ActualWidth) / 2;
            thickness.Right = (FrmMain_Music.ActualWidth - ListView_Temp_KRC.ActualWidth) / 2;


            //ListView_KRC.Margin = thickness;
            ListView_Temp_KRC.Margin = thickness;
            ListView_Temp_KRC_Temp.Margin = ListView_Temp_KRC.Margin;


            //ListView_KRC.Width = FrmMain_Music.ActualWidth - thickness.Left * 2;
            ListView_Temp_KRC.Width = FrmMain_Music.ActualWidth - thickness.Left * 2;
            ListView_Temp_KRC_Temp.Width = FrmMain_Music.ActualWidth - thickness.Left * 2;


            //ListView_KRC_GridViewColumn.Width = ListView_Temp_KRC.Width - 6;
            ListView_Temp_KRC_GridViewColumn.Width = ListView_Temp_KRC.Width - 2;
            ListView_Temp_KRC_Temp_GridViewColumn.Width = ListView_Temp_KRC_Temp.Width - 2;

            //Margin="0,100,161,139"
            //{114.3,90,114.3,139}

            Panel_Image.Visibility = Visibility.Hidden;


            Stop_Timer_Image_Trans_Tick();

            Open_Song_Image_Bool = false;

            //关闭专辑旋转
            Image_Song_Storyboard.Stop();
            Bool_Timer_Image_Trans = false;

            thickness_All_KRC = thickness;


            //ListView_KRC.Width = ListView_Temp_KRC.Width;
            width_All_KRC = ListView_Temp_KRC.Width;
            width_TextBox = TextBox_ListViewKRC_Up.Width;

            //ListView_KRC.Margin = ListView_Temp_KRC.Margin;
            thickness_All_KRC = ListView_Temp_KRC.Margin;
            thickness_TextBox = TextBox_ListViewKRC_Up.Margin;


        }


        private bool Bool_Desk_Krc = false;
        /// <summary>
        /// 桌面歌词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Desk_KRC_Click(object sender, RoutedEventArgs e)
        {
            if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
            {
                if(userControl_MV.MediaMent_MV.LoadedBehavior == MediaState.Play)
                {
                    if (userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Source != null)
                        Open_Desk_KRC();
                }
                else
                {
                    if (userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Source != null)
                        Open_Desk_KRC();
                    else
                        MessageBox.Show("请播放音乐源");
                }
            }
            else
            {
                if (userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Source != null)
                    Open_Desk_KRC();
                else
                    MessageBox.Show("请播放音乐源");
            }
        }
        public void Open_Desk_KRC()
        {
            //开启
            if (Bool_Desk_Krc == false)
            {
                Window_Desk_krc.WindowStartupLocation = WindowStartupLocation.Manual;

                double screeHeight = SystemParameters.FullPrimaryScreenHeight;
                double screeWidth = SystemParameters.FullPrimaryScreenWidth;

                Window_Desk_krc.Top = (screeHeight - Window_Desk_krc.Height) / 4 * 3;
                Window_Desk_krc.Left = (screeWidth - Window_Desk_krc.Width) / 2;

                Window_Desk_krc.Show();

                Bool_Desk_Krc = true;

                DispatcherTimer_KRC.Start();
            }
            //关闭
            else
            {
                Window_Desk_krc.Hide();

                Bool_Desk_Krc = false;
            }
        }

        #endregion

        #region 音量，播放速度，音效插件

        /// <summary>
        /// 初始化事件 音乐声音按钮事件（音量，播放速度）   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userControl_Voice_Music_Speed_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化关闭该控件
            userControl_Voice_Music_Speed.Visibility = Visibility.Hidden;

            //初始化绑定Click事件
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Voice_Speed.Click += Button_Voice_Music_Speed_Click;


            //初始化滑块设置
            userControl_Voice_Music_Speed.Slider_Voice.Maximum = 1;
            userControl_Voice_Music_Speed.Slider_Voice.Minimum = 0;
            //初始化滑块值
            userControl_Voice_Music_Speed.Slider_Voice.Value = 0.5;
            //滑块值Change事件
            userControl_Voice_Music_Speed.Slider_Voice.ValueChanged += Slider_Voice_Change;

            //按钮绑定事件
            userControl_Voice_Music_Speed.Button_Prev.Click += Button_Music_Speed_Prev;
            userControl_Voice_Music_Speed.Button_Next.Click += Button_Music_Speed_Next;
            Music_Speed_Prev_Nums = 1.0;

        }

        public bool Bool_Voice_Music_Speed;
        /// <summary>
        /// 音乐声音按钮事件（音量，播放速度）
        /// </summary>
        public void Button_Voice_Music_Speed_Click(object sender, EventArgs e)
        {
            //开启
            if (Bool_Voice_Music_Speed == false)
            {
                userControl_Voice_Music_Speed.Visibility = Visibility.Visible;

                Bool_Voice_Music_Speed = true;
            }
            //关闭
            else
            {
                userControl_Voice_Music_Speed.Visibility = Visibility.Hidden;

                Bool_Voice_Music_Speed = false;
            }
        }

        /// <summary>
        /// 音量slider滑动事件
        /// </summary>
        public void Slider_Voice_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           /* if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
            {
                userControl_MV.MediaMent_MV.Volume = userControl_Voice_Music_Speed.Slider_Voice.Value;

                userControl_Voice_Music_Speed.Voice_Nums.Text = (userControl_Voice_Music_Speed.Slider_Voice.Value * 100).ToString();
            }
            else
            {*/
                userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Volume = userControl_Voice_Music_Speed.Slider_Voice.Value;

                userControl_Voice_Music_Speed.Voice_Nums.Text = (userControl_Voice_Music_Speed.Slider_Voice.Value * 100).ToString();
            
        }

        public double Music_Speed_Prev_Nums;//播放速度
        /// <summary>
        /// 播放速度--
        /// </summary>
        public void Button_Music_Speed_Prev(object sender, EventArgs e)
        {
            /*if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
            {
                if (Music_Speed_Prev_Nums >= 0)
                {
                    Music_Speed_Prev_Nums -= 0.1;

                    userControl_MV.MediaMent_MV.SpeedRatio = Music_Speed_Prev_Nums;

                    userControl_Voice_Music_Speed.Speed_Nums.Text = Music_Speed_Prev_Nums.ToString();
                    if (Music_Speed_Prev_Nums == 1)
                        userControl_Voice_Music_Speed.Speed_Nums.Text = "1.0";
                }
            }
            else
            {*/
                if (Music_Speed_Prev_Nums >= 0)
                {
                    Music_Speed_Prev_Nums -= 0.1;

                    userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.SpeedRatio = Music_Speed_Prev_Nums;

                    userControl_Voice_Music_Speed.Speed_Nums.Text = Music_Speed_Prev_Nums.ToString();
                    if (Music_Speed_Prev_Nums == 1)
                        userControl_Voice_Music_Speed.Speed_Nums.Text = "1.0";
                }
            
        }
        /// <summary>
        /// 播放速度++
        /// </summary>
        public void Button_Music_Speed_Next(object sender, EventArgs e)
        {
            /*if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
            {
                if (Music_Speed_Prev_Nums <= 2)
                {
                    Music_Speed_Prev_Nums += 0.1;

                    userControl_MV.MediaMent_MV.SpeedRatio = Music_Speed_Prev_Nums;

                    userControl_Voice_Music_Speed.Speed_Nums.Text = Music_Speed_Prev_Nums.ToString();
                    if (Music_Speed_Prev_Nums == 1)
                        userControl_Voice_Music_Speed.Speed_Nums.Text = "1.0";

                }
            }
            else
            {*/
                if (Music_Speed_Prev_Nums <= 2)
                {
                    Music_Speed_Prev_Nums += 0.1;

                    userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.SpeedRatio = Music_Speed_Prev_Nums;

                    userControl_Voice_Music_Speed.Speed_Nums.Text = Music_Speed_Prev_Nums.ToString();
                    if (Music_Speed_Prev_Nums == 1)
                        userControl_Voice_Music_Speed.Speed_Nums.Text = "1.0";

                }
            
        }




        #endregion

        #region MV

        bool Bool_Button_MV_CLick = true;
        /// <summary>
        /// 播放MV
        /// </summary>
        public void Button_MV_Click(object sender, RoutedEventArgs e)
        {
            userControl_Voice_Music_Speed.Speed_Nums.Text = "1.0";
            userControl_Voice_Music_Speed.Voice_Nums.Text = "50";
            userControl_Voice_Music_Speed.Slider_Voice.Value = 0.5;
            Music_Speed_Prev_Nums = 1.0;
            //切换声道时另一播放器播放速度 = 默认
            userControl_MV.MediaMent_MV.SpeedRatio = 1.0;
            userControl_MV.MediaMent_MV.Volume = 0.5;
            //切换声道时另一播放器播放速度 = 默认
            userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.SpeedRatio = 1.0;
            userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Volume = 0.5;


            userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Pause();
            userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior = MediaState.Pause;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background = brush_Play;

            //关闭旋转
            Stop_Timer_Image_Trans_Tick();


            if (Song_Url != null)
            {
                path_mv_src = Song_Url;
                path_mv_src = path_mv_src.Replace("mp3", "mkv");
                path_mv_src = path_mv_src.Substring(path_mv_src.LastIndexOf(@"/") + 1);
                //path_mv_src = Path_App + @"\视频资源\" + path_mv_src;
                path_mv_src = @"E:\KuGou\MV\" + path_mv_src;
                userControl_MV.Visibility = Visibility.Visible;

                if (File.Exists(path_mv_src))
                {
                    userControl_MV.MediaMent_MV.Source = new Uri(path_mv_src);

                    userControl_MV.MediaMent_MV.Play();
                    userControl_MV.MediaMent_MV.LoadedBehavior = MediaState.Play;
                    userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Pause;

                    //按钮背景图片
                    userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Pause;


                    WMP_Song_Play_Ids_MV = WMP_Song_Play_Ids;
                    Song_Url_MV = Song_Url;
                    WMP_Song_Play_Ids_UP_DOWN_MV = WMP_Song_Play_Ids_UP_DOWN;


                }
                else
                {
                    //提示显示为无该歌曲的视频MV资源
                    MessageBox.Show("无效歌曲的视频MV资源,将自动跳转其它视频");


                    userControl_MV.Visibility = Visibility.Hidden;
                    //WMP_Song_Play_Ids_MV = WMP_Song_Play_Ids;
                    //Next_MV_WhileTrue();

                }
            }
            else
            {
                //提示显示为无该歌曲的视频MV资源
                MessageBox.Show("无该歌曲的视频MV资源,将自动跳转其它视频");


                userControl_MV.Visibility = Visibility.Hidden;
                //WMP_Song_Play_Ids_MV = WMP_Song_Play_Ids;
                //Next_MV_WhileTrue();

            }


            Grid_MV.Visibility = Visibility.Visible;
            Grid_主界面_FrmMain.Visibility = Visibility.Visible;
            Grid_Music_Player.Visibility = Visibility.Hidden;
            userControl_ALL_Button_Music_PANEL_TOP.Button_Back.Visibility = Visibility.Hidden;//返回键隐藏
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Open_Song_Image_Tran.Visibility = Visibility.Hidden;//专辑旋转隐藏

            Bool_Button_MV_CLick = true;
            Bool_Button_Back_Click = true;        
        }


        public void Grid_MediaElement_MV_MediaClosing(object sender, RoutedEventArgs e)
        {
            if (WMP_Song_Play_Ids_MV > this.ListView_Temp_Info.Items.Count)
                WMP_Song_Play_Ids_MV = 0;

            if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
            {
                Next_MV_WhileTrue();
            }
        }
        public void Next_MV_WhileTrue()
        {
            while (WMP_Song_Play_Ids_MV != 0)
            {
                WMP_Song_Play_Ids_UP_DOWN_MV = 1;
                if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background != brush_Button_Music_Order_Cycle)//不是单曲循环
                {
                    Change_MediaElement_Song_id_incrse_MV();
                }

                Song_Url_MV = ((ListView_Item_Bing)ListView_Temp_Info.Items[WMP_Song_Play_Ids_MV - 1]).Song_Url;
                if (Song_Url_MV != null)
                {
                    path_mv_src = Song_Url_MV;
                    path_mv_src = path_mv_src.Replace("mp3", "mkv");
                    path_mv_src = path_mv_src.Substring(path_mv_src.LastIndexOf(@"\") + 1);
                    //path_mv_src = Path_App + @"\视频资源\" + path_mv_src;
                    path_mv_src = @"E:\墨智音乐 - KRC\Music_Player_Test(KRC)\WindowsFormsApplication1\bin\Debug\song_MV\" + path_mv_src;

                    if (File.Exists(path_mv_src))
                    {
                        userControl_MV.MediaMent_MV.Source = new Uri(path_mv_src);

                        userControl_MV.MediaMent_MV.Play();
                        userControl_MV.MediaMent_MV.LoadedBehavior = MediaState.Play;
                        userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Pause;

                        //选中播放的列
                        ListView_Temp_Info.SelectedIndex = WMP_Song_Play_Ids_MV - 1;

                        //暂停音乐
                        userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Pause();
                        userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior = MediaState.Pause;
                        userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background = brush_Play;

                        break;
                    }

                    if (WMP_Song_Play_Ids_MV > this.ListView_Temp_Info.Items.Count)
                    {
                        WMP_Song_Play_Ids_MV = 0;
                        break;
                    }

                    //读取到列表中最后一行，依然读取不到MV资源
                    /*                    if (WMP_Song_Play_Ids_MV == ListView_Temp_Info.Items.Count && userControl_MV.MediaMent_MV.Source == null)
                                        {
                                            break;
                                        }*/
                }
            }
        }

        public void Grid_MediaElement_MV_MediaOpened(object sender, RoutedEventArgs e)
        {
            userControl_MV.userControl_MV_Take.Silder_Music_Width.Maximum = userControl_MV.MediaMent_MV.NaturalDuration.TimeSpan.TotalMilliseconds;
            userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Maximum = userControl_MV.userControl_MV_Take.Silder_Music_Width.Maximum;

            userControl_MV.userControl_MV_Take.Silder_Music_Width.Value = 0;
            userControl_MV.userControl_MV_Take.Silder_Music_Temp_Width.Value = 0;

            test2_MV = userControl_MV.MediaMent_MV.NaturalDuration.TimeSpan;
        }


        /// <summary>
        /// 初始化MV控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MV_Loaded(object sender, RoutedEventArgs e)
        {
            userControl_MV.userControl_MV_Take.Button_Before.Click += Button_Music_Up_Song_MV;
            userControl_MV.userControl_MV_Take.Button_Next.Click += Button_Music_Next_Song_MV;
            userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Click += Button_Music_Play_Pause_Song_MV;

            //MV窗口最大化
            userControl_MV.userControl_MV_Take.Button_Screen.Click += Main_Full_MV;
        }

        bool Bool_Full_MV;
        Thickness thickness_Grid_MV = new Thickness();
        Thickness thickness_MV = new Thickness();
        Thickness thickness_MV_Take = new Thickness();
        Thickness thickness_StackPanel_Voice = new Thickness();
        //Thickness thickness_StackPanel_MV = new Thickness();
        Thickness thickness_userControl_MV = new Thickness();
        /// <summary>
        /// MV窗口最大化
        /// </summary>
        public void Main_Full_MV(object sender, EventArgs e)
        {
            if (Bool_Full_MV == false)
            {
                //Margin="326,146,0,99"
                Grid_MV.Margin = new Thickness(0);//为0
                //userControl_MV.StackPanel_MV.Margin = new Thickness(0);//为0
                userControl_MV.MediaMent_MV.Margin = new Thickness(0);//为0
                userControl_MV.Margin = new Thickness(0);//为0

                //
                userControl_MV.Width = Grid_MV.Width;
                userControl_MV.Height = Grid_MV.Height;

                //
                double nums_mv = userControl_MV.MediaMent_MV.ActualHeight / Grid_MV.Height;
                userControl_MV.MediaMent_MV.Width = Grid_MV.Width;
                userControl_MV.MediaMent_MV.Height = userControl_MV.MediaMent_MV.ActualHeight / nums_mv;

                //userControl_MV_Take
                userControl_MV.userControl_MV_Take.Width = Grid_MV.Width;
                thickness_MV_Take = userControl_MV.userControl_MV_Take.Margin;
                thickness_MV_Take.Left = 0;
                thickness_MV_Take.Right = 0;
                thickness_MV_Take.Bottom = 0;
                userControl_MV.userControl_MV_Take.Margin = thickness_MV_Take;
                //userControl_MV_Take_TextBlock
                userControl_MV.userControl_MV_Take_TextBlock.Width = Grid_MV.Width;
                thickness_MV_Take = userControl_MV.userControl_MV_Take_TextBlock.Margin;
                thickness_MV_Take.Left = 0;
                thickness_MV_Take.Right = 0;
                thickness_MV_Take.Bottom = 0;
                userControl_MV.userControl_MV_Take_TextBlock.Margin = thickness_MV_Take;

                //StackPanel_Voice
                thickness_StackPanel_Voice = userControl_MV.StackPanel_Voice.Margin;
                thickness_StackPanel_Voice.Right = 72;
                thickness_StackPanel_Voice.Bottom = 72;
                userControl_MV.StackPanel_Voice.Margin = thickness_StackPanel_Voice;


                userControl_ALL_Button_Music_PANEL_TOP.Visibility = Visibility.Hidden;
                userControl_ALL_Button_Music_PANEL_BUTTOM.Visibility = Visibility.Hidden;

                Bool_Full_MV = true;
            }
            else
            {
                thickness_MV.Left = 60;
                thickness_MV.Top = -2;
                thickness_MV.Right = 196;
                thickness_MV.Bottom = 68;
                userControl_MV.MediaMent_MV.Margin = thickness_MV;// Margin="197,0,196,0"
                


                thickness_MV_Take.Left = 197;
                thickness_MV_Take.Top = 0;
                thickness_MV_Take.Right = 197;
                thickness_MV_Take.Bottom = 114;
                userControl_MV.userControl_MV_Take.Margin = thickness_MV_Take;//Margin="197,-60,196,162"
                userControl_MV.userControl_MV_Take.Width = 857;
                thickness_MV_Take.Left = 197;
                thickness_MV_Take.Top = 0;
                thickness_MV_Take.Right = 197;
                thickness_MV_Take.Bottom = 114;
                userControl_MV.userControl_MV_Take_TextBlock.Margin = thickness_MV_Take;
                userControl_MV.userControl_MV_Take_TextBlock.Width = 857;


                thickness_StackPanel_Voice.Left = 0;
                thickness_StackPanel_Voice.Top = 0;
                thickness_StackPanel_Voice.Right = 263;
                thickness_StackPanel_Voice.Bottom = 187;
                userControl_MV.StackPanel_Voice.Margin = thickness_StackPanel_Voice;// Margin = "0,0,263,227"


                thickness_userControl_MV.Left = 2;
                thickness_userControl_MV.Top = 0;
                thickness_userControl_MV.Right = 2;
                thickness_userControl_MV.Bottom = 139;
                userControl_MV.Margin = thickness_userControl_MV;// Margin="197,0,196,0"
                userControl_MV.Width = 1250;
                userControl_MV.Height = 600;


                thickness_Grid_MV.Left = 326;
                thickness_Grid_MV.Top = 140;
                thickness_Grid_MV.Right = 0;
                thickness_Grid_MV.Bottom = 0;
                Grid_MV.Margin = thickness_Grid_MV;


                userControl_ALL_Button_Music_PANEL_TOP.Visibility = Visibility.Visible;
                userControl_ALL_Button_Music_PANEL_BUTTOM.Visibility = Visibility.Visible;

                Bool_Full_MV = false;
            }
        }


        int WMP_Song_Play_Ids_UP_DOWN_MV;
        public int WMP_Song_Play_Ids_MV = 1;
        string path_mv_src;
        public string Song_Url_MV;
        /// <summary>
        /// 播放/暂停音乐
        /// </summary>
        public void Button_Music_Play_Pause_Song_MV(object sender, EventArgs e)
        {
            if (userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background == brush_Play)
            {
                if (userControl_MV.MediaMent_MV.Source != null)//MV进度
                {
                    userControl_MV.MediaMent_MV.Play();
                    userControl_MV.MediaMent_MV.LoadedBehavior = MediaState.Play;
                    userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Pause;                   
                }
            }
            else
            {
                if (userControl_MV.MediaMent_MV.Source != null)//MV进度
                {
                    userControl_MV.MediaMent_MV.Pause();
                    userControl_MV.MediaMent_MV.LoadedBehavior = MediaState.Pause;
                    userControl_MV.userControl_MV_Take.Button_Play_Pause_Player.Background = brush_Play;

                }
            }

            //暂停音乐
            userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.Pause();
            userControl_ALL_Button_Music_PANEL_BUTTOM.MediaElement_Song.LoadedBehavior = MediaState.Pause;
            userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Play_Pause_Player.Background = brush_Play;
        }
        /// <summary>
        /// 上一首
        /// </summary>
        public void Button_Music_Up_Song_MV(object sender, EventArgs e)
        {
            if (WMP_Song_Play_Ids_MV > this.ListView_Temp_Info.Items.Count)
                WMP_Song_Play_Ids_MV = 0;


            if (userControl_MV.MediaMent_MV.Source != null)//MV进度
            {
                Next_MV_WhileTrue();
            }
        }
        /// <summary>
        /// 下一首
        /// </summary>
        public void Button_Music_Next_Song_MV(object sender, EventArgs e)
        {
            if (WMP_Song_Play_Ids_MV > this.ListView_Temp_Info.Items.Count)
                WMP_Song_Play_Ids_MV = 0;


            if (userControl_MV.MediaMent_MV.Source != null)//MV进度
            {
                Next_MV_WhileTrue();
            }
        }

        Random rd_MV = new Random();
        public void Change_MediaElement_Song_id_incrse_MV()
        {
            //顺序播放
            if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Normal)
            {
                if (WMP_Song_Play_Ids_UP_DOWN_MV == 1)
                {
                    if (WMP_Song_Play_Ids_MV != ListView_Temp_Info.Items.Count)
                        WMP_Song_Play_Ids_MV++;
                    else
                        WMP_Song_Play_Ids_MV = 1;
                }
                else if (WMP_Song_Play_Ids_UP_DOWN_MV == -1)
                {
                    if (WMP_Song_Play_Ids_MV != 1)
                        WMP_Song_Play_Ids_MV--;
                    else
                        WMP_Song_Play_Ids_MV = ListView_Temp_Info.Items.Count;
                }
            }
            //单曲循环
            else if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Cycle)
            {
                if (WMP_Song_Play_Ids_UP_DOWN_MV == 1)
                {
                    if (WMP_Song_Play_Ids_MV != ListView_Temp_Info.Items.Count)
                        WMP_Song_Play_Ids_MV++;
                    else
                        WMP_Song_Play_Ids_MV = 1;
                }
                else if (WMP_Song_Play_Ids_UP_DOWN_MV == -1)
                {
                    if (WMP_Song_Play_Ids_MV != 1)
                        WMP_Song_Play_Ids_MV--;
                    else
                        WMP_Song_Play_Ids_MV = ListView_Temp_Info.Items.Count;
                }
            }
            //随机播放
            else if (userControl_ALL_Button_Music_PANEL_BUTTOM.Button_Music_Order.Background == brush_Button_Music_Order_Random)
            {
                WMP_Song_Play_Ids_MV = rd_MV.Next(1, ListView_Temp_Info.Items.Count);//(生成1~10之间的随机数，不包括10)
            }
        }

        

        #endregion





        /// <summary>
        /// 桌面写真模式
        /// 应制作悬浮于桌面的画布，仅歌手图片切换动画，歌词同步悬浮于桌面且置于底层
        /// 桌面无法直接控制(取消焦点 Fousable = false)，桌面无响应操作
        /// </summary>
        /// <param name="uAction"></param>
        /// <param name="uParam"></param>
        /// <param name="lpvParam"></param>
        /// <param name="fuWinIni"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        private const int SPI_GETDESKWALLPAPER = 0x0073;
        #region 桌面写真模式

        //调用
        public static StringBuilder wallpaper_path = new StringBuilder();
        StringBuilder SingerPicPath = new StringBuilder();
        public bool Bool_Windows_Wallpaper;

        /// <summary>
        /// 桌面写真模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Open_Windows_Picture_Click(object sender, EventArgs e)
        {
            if (Bool_Button_MV_CLick == true && userControl_MV.MediaMent_MV.Source != null)//MV进度
            {

            }
            else
            {
                if (Bool_Windows_Wallpaper == false)
                {
                    //刷新Windows存储的背景图片信息
                    wallpaper_path.Clear();
                    //存储Windows背景图片
                    //定义存储缓冲区大小
                    StringBuilder s = new StringBuilder(300);
                    //获取Window 桌面背景图片地址，使用缓冲区
                    SystemParametersInfo(SPI_GETDESKWALLPAPER, 300, s, 0);
                    //缓冲区中字符进行转换
                    wallpaper_path.Append(s.ToString()); //系统桌面背景图片路径


                    Change_Windows_Background();

                    //打开桌面歌词
                    Window_Desk_krc.WindowStartupLocation = WindowStartupLocation.Manual;

                    double screeHeight = SystemParameters.FullPrimaryScreenHeight;
                    double screeWidth = SystemParameters.FullPrimaryScreenWidth;

                    Window_Desk_krc.Top = (screeHeight - Window_Desk_krc.Height);
                    Window_Desk_krc.Left = (screeWidth - Window_Desk_krc.Width) / 2;

                    Window_Desk_krc.Show();

                    Bool_Windows_Wallpaper = true;
                }
                else
                {
                    SystemParametersInfo(20, 1, wallpaper_path, 1);

                    Bool_Windows_Wallpaper = false;


                    //刷新Windows存储的背景图片信息
                    wallpaper_path.Clear();
                    //存储Windows背景图片
                    //定义存储缓冲区大小
                    StringBuilder s = new StringBuilder(300);
                    //获取Window 桌面背景图片地址，使用缓冲区
                    SystemParametersInfo(SPI_GETDESKWALLPAPER, 300, s, 0);
                    //缓冲区中字符进行转换
                    wallpaper_path.Append(s.ToString()); //系统桌面背景图片路径
                }
            }
        }
        public void Change_Windows_Background()
        {
            SingerPicPath.Clear();

            //如果歌手图片存在
            if (File.Exists(Singer_Image_Url))
                SingerPicPath.Append(Singer_Image_Url);//获取歌手图片所在路径  


            SystemParametersInfo(20, 1, SingerPicPath, 1);

            Bool_Windows_Wallpaper = true;
        }







        #endregion


        #region 内存清理

        /// <summary>
        /// 清理当前程序占用的UI缓存
        /// </summary>
        public void Clear_ALL_Cemory()
        {

        }

        /// <summary>
        /// 重构当前的UI缓存占用，使其恢复至启动前的性能占用
        /// </summary>
        public void Create_ALL_Cemory()
        {

        }

        #endregion


        private void Windows_FrmMain_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)//最大化按钮
            {              
                userControl_ALL_Button_Music_PANEL_TOP.Button_Max.Background = brush_MaxNormal;
            }
            else//最小化按钮
            {
                userControl_ALL_Button_Music_PANEL_TOP.Button_Max.Background = brush_Max;
            }
        }


        #region 歌单的读取

        string[] singer_Name;
        string[] song_Name;
        string[] album_Name;
        string[] song_Url;
        int[] song_No;

        private ListView_Item_Bing_ALL listView_Item_Bing_ALL = ListView_Item_Bing_ALL.Retuen_This();

        private void dataGridView_List_ALL_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox_Select_ListView.Items.Add("本地音乐_listView_Temp_Info_End_ALL");
            ListBox_Select_ListView.Items.Add("我的收藏_listView_Temp_Info_End_Love");
            ListBox_Select_ListView.Items.Add("默认列表_listView_Temp_Info_End_Auto");

            //加载歌单歌曲信息，多线程
            //解析mp3文件绝对路径，通过引用Shell32_Class包读取mp3文件流中的专辑信息
            //解析时占用线程资源较多，需要多线程
            /*var t1 = new Thread(Load_Data_ALL_D_Grid_View_1);//读取歌曲文件信息
            t1.Start();
            var t2 = new Thread(Load_Data_ALL_D_Grid_View_2);//读取歌曲文件信息
            t2.Start();
            var t3 = new Thread(Load_Data_ALL_D_Grid_View_3);//读取歌曲文件信息
            t3.Start();*/

            //先读取我的收藏歌单里的歌曲信息
            Load_Data_ALL_D_Grid_View_2();

            Load_Data_ALL_D_Grid_View_1();
            
            Load_Data_ALL_D_Grid_View_3();
        }


        /// <summary>
        /// 读取歌单
        /// </summary>
        public void Load_Data_ALL_D_Grid_View_1()
        {
            string temp = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\资源\歌单列表\本地音乐.ini");
            FileStream FS_List_Save = new FileStream(temp, FileMode.Open);
            SR_List = new StreamReader(FS_List_Save);
            Load_Data_ALL(listView_Item_Bing_ALL.listView_Temp_Info_End_ALL, FS_List_Save, SR_List, 1);

            SR_List = null;
        }
        /// <summary>
        /// 读取歌单
        /// </summary>
        public void Load_Data_ALL_D_Grid_View_2()
        {
            string temp = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\资源\歌单列表\我的收藏.ini");
            FileStream FS_List_Save = new FileStream(temp, FileMode.Open);
            SR_List = new StreamReader(FS_List_Save);
            Load_Data_ALL(listView_Item_Bing_ALL.listView_Temp_Info_End_Love, FS_List_Save, SR_List, 2);

            SR_List = null;
        }
        /// <summary>
        /// 读取歌单
        /// </summary>
        public void Load_Data_ALL_D_Grid_View_3()
        {
            string temp = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\资源\歌单列表\默认列表.ini");
            FileStream FS_List_Save = new FileStream(temp, FileMode.Open);
            SR_List = new StreamReader(FS_List_Save);
            Load_Data_ALL(listView_Item_Bing_ALL.listView_Temp_Info_End_Auto, FS_List_Save, SR_List, 3);

            SR_List = null;
        }

        private void Load_Data_ALL(List<ListView_Item_Bing> Save_Load_List_Name, FileStream FS_List_Save, StreamReader SR_List, int num)
        {
            singer_Name = new string[9999];
            song_Name = new string[9999];
            album_Name = new string[9999];
            song_Url = new string[9999];
            song_No = new int[9999];

            try//防止读取不到值
            {
                int RowCount = int.Parse(SR_List.ReadLine());

                for (int i = 0; i <= RowCount - 1; i++)
                {
                    singer_Name[i] = SR_List.ReadLine();
                    song_Name[i] = SR_List.ReadLine();
                    album_Name[i] = SR_List.ReadLine();
                    song_Url[i] = SR_List.ReadLine();
                    song_No[i] = Convert.ToInt32(SR_List.ReadLine());
                }

                SR_List.Close();
                FS_List_Save.Close();


                int count = 0;

                ListView_Item_Bing[] listView_Temp_Infos = new ListView_Item_Bing[9999];

                for (int i = 0; i < singer_Name.Length; i++)
                {
                    if (listView_Temp_Infos[i] == null)
                    {
                        if (singer_Name[i] != null)
                        {
                            if (song_Name[i] != null)
                            {
                                if (song_Url[i] != null)
                                {
                                    if (album_Name[i] != null)
                                    {
                                        ListView_Item_Bing temp = new ListView_Item_Bing();
                                        temp.Singer_Name = singer_Name[i];
                                        temp.Song_Name = song_Name[i];
                                        temp.Song_Url = song_Url[i];
                                        temp.Song_No = song_No[i];
                                        temp.Album_Name = album_Name[i];
                                        if (num == 2)
                                        {
                                            temp.Song_Image = new ImageBrush(new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\c_like_1.png")));
                                            temp.Song_Like = 1;
                                        }
                                        else
                                        {
                                            if (listView_Item_Bing_ALL.listView_Temp_Info_End_Love != null)
                                            {
                                                foreach (ListView_Item_Bing _Item_Bing in listView_Item_Bing_ALL.listView_Temp_Info_End_Love)
                                                {
                                                    if(_Item_Bing != null)
                                                        if (_Item_Bing.Song_Url.Equals(temp.Song_Url))
                                                        {
                                                            temp.Song_Image = new ImageBrush(new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\c_like_1.png")));
                                                            temp.Song_Like = 1;
                                                        }
                                                }
                                                if (temp.Song_Like != 1)
                                                {
                                                    temp.Song_Image = new ImageBrush(new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\c_like-2.png")));
                                                    temp.Song_Like = 0;
                                                }
                                            }
                                            else
                                            {
                                                if (temp.Song_Like != 1)
                                                {
                                                    temp.Song_Image = new ImageBrush(new BitmapImage(new Uri(Path_App + @"\图片资源\按键图片\c_like-2.png")));
                                                    temp.Song_Like = 0;
                                                }
                                            }
                                        }

                                        listView_Temp_Infos[i] = temp;

                                        count++;
                                    }
                                }
                            }
                        }
                    }
                }

                if (num == 1)
                {
                    listView_Item_Bing_ALL.listView_Temp_Info_End_ALL = new List<ListView_Item_Bing>();
                    for (int i = 0; i < count; i++)
                    {
                        if (listView_Temp_Infos[i] != null)
                        {
                            

                            listView_Item_Bing_ALL.listView_Temp_Info_End_ALL.Add(listView_Temp_Infos[i]);
                        }
                        else
                        {
                            //listView_Item_Bing_ALL.listView_Temp_Info_End_ALL = null;
                            break;
                        }
                    }
                }
                else if (num == 2)
                {
                    listView_Item_Bing_ALL.listView_Temp_Info_End_Love = new List<ListView_Item_Bing>();
                    for (int i = 0; i < count; i++)
                    {
                        if (listView_Temp_Infos[i] != null)
                        {
                            listView_Item_Bing_ALL.listView_Temp_Info_End_Love.Add(listView_Temp_Infos[i]);
                        }
                        else
                        {
                            //listView_Item_Bing_ALL.listView_Temp_Info_End_Love = null;
                            break;
                        }
                    }
                }
                else if (num == 3)
                {
                    listView_Item_Bing_ALL.listView_Temp_Info_End_Auto = new List<ListView_Item_Bing>();
                    for (int i = 0; i < count; i++)
                    {
                        if (listView_Temp_Infos[i] != null)
                        {
                            listView_Item_Bing_ALL.listView_Temp_Info_End_Auto.Add(listView_Temp_Infos[i]);
                        }
                        else
                        {
                            //listView_Item_Bing_ALL.listView_Temp_Info_End_Auto = null;
                            break;
                        }
                    }
                }
            }
            catch
            {
                Save_Load_List_Name = null;

                if (num == 1)
                    listView_Item_Bing_ALL.listView_Temp_Info_End_ALL = null;
                else if (num == 2)
                    listView_Item_Bing_ALL.listView_Temp_Info_End_Love = null;
                else if (num == 3)
                    listView_Item_Bing_ALL.listView_Temp_Info_End_Auto = null;

            }

        }

        #endregion

        #region 歌单的保存

        /// <summary>
        /// 保存歌单歌曲信息
        /// </summary>
        public void Save_ALL_SongListInfo()
        {
            //歌单歌曲排序
            userControl_主界面_FrmMain.Sort_SongList();
            Save_DataGridView();
        }

        //实例化一个文件流--->与写入文件相关联
        //静态读取资源文件会一直占用，导致只能写入不能导出，出现文件内容清空
        private FileStream FS_List_Save = null;
        private StreamWriter SW_List = null;//写入 
        private StreamReader SR_List = null;//读取

        public void Save_DataGridView()
        {
            Save_Data_ALL_List();

            FS_List_Save = null;

            SW_List = null;

        }
        public void Save_Data_ALL_List()
        {
            string temp = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\资源\歌单列表\本地音乐.ini");
            Clear_File_Info(temp);

            FS_List_Save = new FileStream(temp, FileMode.Create);
            SW_List = new StreamWriter(FS_List_Save);//无法静态
            Write_Song_Info(listView_Item_Bing_ALL.listView_Temp_Info_End_ALL, FS_List_Save);


            temp = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\资源\歌单列表\我的收藏.ini");
            Clear_File_Info(temp);

            FS_List_Save = new FileStream(temp, FileMode.Create);
            SW_List = new StreamWriter(FS_List_Save);//无法静态
            Write_Song_Info(listView_Item_Bing_ALL.listView_Temp_Info_End_Love, FS_List_Save);


            temp = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\资源\歌单列表\默认列表.ini");
            Clear_File_Info(temp);

            FS_List_Save = new FileStream(temp, FileMode.Create);
            SW_List = new StreamWriter(FS_List_Save);//无法静态
            Write_Song_Info(listView_Item_Bing_ALL.listView_Temp_Info_End_Auto, FS_List_Save);
        }
        public void Clear_File_Info(string FullName)
        {
            try
            {
                //先清空指定文件内所有信息
                FileStream fs = new FileStream(FullName, FileMode.Create);//清空此文件的数据
                fs.Flush();
                fs.Close();
            }
            catch
            {
                MessageBox.Show(FullName+"文件被占用");//一直报错则检查文件头部内容是否存在数字，不存在则加0
            }
        }

        private void Write_Song_Info(List<ListView_Item_Bing> Save_Load_List_Name, FileStream FS_List)
        {

            //开始写入
            if (Save_Load_List_Name != null && Save_Load_List_Name.Count > 0) //if有新的行可以插入
            {
                //因为DataGridView最后一行为空，所以减一
                SW_List.WriteLine(Save_Load_List_Name.Count);
                for (int i = 0; i < Save_Load_List_Name.Count; i++)
                {
                    //如果某一列数据为空，就写入""，因为空对象不能调用tostring()；
                    if (Save_Load_List_Name[i] != null)
                    {
                        if (Save_Load_List_Name[i].Singer_Name != null)
                            SW_List.WriteLine(Save_Load_List_Name[i].Singer_Name);

                        if (Save_Load_List_Name[i].Song_Name != null)
                            SW_List.WriteLine(Save_Load_List_Name[i].Song_Name);

                        if (Save_Load_List_Name[i].Album_Name != null)
                            SW_List.WriteLine(Save_Load_List_Name[i].Album_Name);

                        if (Save_Load_List_Name[i].Song_Url != null)
                            SW_List.WriteLine(Save_Load_List_Name[i].Song_Url);

                        if (Save_Load_List_Name[i].Song_No != 0)
                            SW_List.WriteLine(Save_Load_List_Name[i].Song_No);

                    }

                }
                //清空缓冲区
                //关闭流
                SW_List.Flush();
                SW_List.Close();

                //FS_List.Flush();
                FS_List.Close();
            }
        }

        #endregion

        #region 歌单CRUD操作
        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userControl_主界面_FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            //绑定删除选定项事件
            userControl_主界面_FrmMain.Button_Delete.Click += Button_Delete_Click;



        }

        /// <summary>
        /// 添加此歌曲到我的收藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Love_ListView_Song_Click(object sender, RoutedEventArgs e)
        {
            //歌单歌曲排序
            userControl_主界面_FrmMain.Sort_SongList();

            Button ck_Selected = sender as Button;

            if (Convert.ToInt32(ck_Selected.MinHeight) == 0)//初始为0，代表未添加至我的收藏
            {
                ck_Selected.MinHeight = 1;
                ck_Selected.Background = userControl_主界面_FrmMain.brush_LoveEnter;

            }
            else
            {
                ck_Selected.MinHeight = 0;
                ck_Selected.Background = userControl_主界面_FrmMain.brush_LoveNormal;

            }

        }
        /// <summary>
        /// 删除选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            //歌单歌曲排序
            userControl_主界面_FrmMain.Sort_SongList();

            int nums_select = 0;

            //删除选中项，一次删除操作可能漏掉部分要删除的项
            while (this.userControl_主界面_FrmMain.Song_Info_Temp.Count > 0)
            {
                for (int i = 0; i < this.userControl_主界面_FrmMain.Song_Info_Temp.Count; i++)
                {
                    //检测删除了多少列
                    nums_select++;

                    if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_ALL_Clear == true)
                    {
                        ListView_Item_Bing temp = listView_Item_Bing_ALL.listView_Temp_Info_End_ALL.Find(delegate (ListView_Item_Bing x) { return x.Song_No == Convert.ToInt32(((ListView_Item_Bing)userControl_主界面_FrmMain.Song_Info_Temp[i]).Song_No); });

                        listView_Item_Bing_ALL.listView_Temp_Info_End_ALL.Remove(temp);
                    }
                    else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Love_Clear == true)
                    {
                        ListView_Item_Bing temp = listView_Item_Bing_ALL.listView_Temp_Info_End_Love.Find(delegate (ListView_Item_Bing x) { return x.Song_No == Convert.ToInt32(((ListView_Item_Bing)userControl_主界面_FrmMain.Song_Info_Temp[i]).Song_No); });

                        listView_Item_Bing_ALL.listView_Temp_Info_End_Love.Remove(temp);
                    }
                    else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Auto_Clear == true)
                    {
                        ListView_Item_Bing temp = listView_Item_Bing_ALL.listView_Temp_Info_End_Auto.Find(delegate (ListView_Item_Bing x) { return x.Song_No == Convert.ToInt32(((ListView_Item_Bing)userControl_主界面_FrmMain.Song_Info_Temp[i]).Song_No); });

                        listView_Item_Bing_ALL.listView_Temp_Info_End_Auto.Remove(temp);
                    }

                    
                    this.userControl_主界面_FrmMain.Song_Info_Temp.Remove(this.userControl_主界面_FrmMain.Song_Info_Temp[i]);
                }
            }


            if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_ALL_Clear == true)
            {
                //歌曲序号重构
                for (int i = 0; i < listView_Item_Bing_ALL.listView_Temp_Info_End_ALL.Count; i++)
                {
                    listView_Item_Bing_ALL.listView_Temp_Info_End_ALL.ElementAt(i).Song_No = i + 1;
                }

            }
            else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Love_Clear == true)
            {
                //歌曲序号重构
                for (int i = 0; i < listView_Item_Bing_ALL.listView_Temp_Info_End_Love.Count; i++)
                {
                    listView_Item_Bing_ALL.listView_Temp_Info_End_Love.ElementAt(i).Song_No = i + 1;
                }
            }
            else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Auto_Clear == true)
            {
                //歌曲序号重构
                for (int i = 0; i < listView_Item_Bing_ALL.listView_Temp_Info_End_Auto.Count; i++)
                {
                    listView_Item_Bing_ALL.listView_Temp_Info_End_Auto.ElementAt(i).Song_No = i + 1;
                }
            }


            //切换歌曲播放列表
            if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_ALL_Clear == true)
            {
                ListView_Temp_Info.ItemsSource = null;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;

                ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_ALL;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_ALL;
                ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_ALL";
            }
            else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Love_Clear == true)
            {
                ListView_Temp_Info.ItemsSource = null;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;

                ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_Love;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_Love;
                ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_Love";
            }
            else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Auto_Clear == true)
            {
                ListView_Temp_Info.ItemsSource = null;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;

                ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_Auto;
                userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = listView_Item_Bing_ALL.listView_Temp_Info_End_Auto;
                ListView_Temp_Info_ItemSource_Name = "listView_Item_Bing_ALL.listView_Temp_Info_End_Auto";
            }


            //歌单歌曲排序
            userControl_主界面_FrmMain.Sort_SongList();
            //保存歌单信息
            Save_DataGridView();


        }
        /// <summary>
        /// 添加歌曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            //歌单歌曲排序
            userControl_主界面_FrmMain.Sort_SongList();


        }

        /// <summary>
        /// listview_Check选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_ListView_Song_Click(object sender, RoutedEventArgs e)
        {
            CheckBox ck_Selected = sender as CheckBox;

            if (ck_Selected.IsChecked == true)
            {
                //this.ListView_Temp_Info.SelectedItems.Add(this.ListView_Temp_Info.Items[Convert.ToInt32(ck_Selected.Tag) - 1]);
                //userControl_主界面_FrmMain.Song_Info_Temp.Add(this.ListView_Temp_Info.Items[Convert.ToInt32(ck_Selected.Tag) - 1]);
            }
            else if (ck_Selected.IsChecked == false)
            {
                //this.ListView_Temp_Info.SelectedItems.Remove(this.ListView_Temp_Info.Items[Convert.ToInt32(ck_Selected.Tag) - 1]);
                //userControl_主界面_FrmMain.Song_Info_Temp.Remove(this.ListView_Temp_Info.Items[Convert.ToInt32(ck_Selected.Tag) - 1]);
            }
        }



        #region 清空指定歌单所有音乐

        /// <summary>
        /// 右键快捷菜单删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_List_All_Click(object sender, EventArgs e)
        {
            listView_Item_Bing_ALL.listView_Temp_Info_End_ALL = null;
            Clear_Bool_listView_Temp_Info_End();
            MessageBox.Show("已清空本地音乐", "提示");
        }
        private void Delete_List_Love_Click(object sender, EventArgs e)
        {
            listView_Item_Bing_ALL.listView_Temp_Info_End_Love = null;
            Clear_Bool_listView_Temp_Info_End();
            MessageBox.Show("已清空我的收藏", "提示");
        }
        private void Delete_List_Auto_Click(object sender, EventArgs e)
        {
            listView_Item_Bing_ALL.listView_Temp_Info_End_Auto = null;
            Clear_Bool_listView_Temp_Info_End();
            MessageBox.Show("已清空默认列表", "提示");
        }

        public void Clear_Bool_listView_Temp_Info_End()
        {
            if(bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_ALL_Clear == true)
            {
                ListView_Temp_Info.Items.Clear();
                userControl_主界面_FrmMain.ListView_Temp_Info.Items.Clear();
                //ListView_Temp_Info.ItemsSource = null;
                //userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;

                ListView_Temp_Info_ItemSource_Name = "";
                bool_ListView_Temp_Info_End_Clear.userControl_主界面_FrmMain_ListView_Temp_Info_ItemSource_Name = "";
            }
            else if(bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Love_Clear == true)
            {
                ListView_Temp_Info.Items.Clear();
                userControl_主界面_FrmMain.ListView_Temp_Info.Items.Clear();
                //ListView_Temp_Info.ItemsSource = null;
                //userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;
                bool_ListView_Temp_Info_End_Clear.userControl_主界面_FrmMain_ListView_Temp_Info_ItemSource_Name = "";

                ListView_Temp_Info_ItemSource_Name = "";
            }
            else if (bool_ListView_Temp_Info_End_Clear.Bool_listView_Temp_Info_End_Auto_Clear == true)
            {
                ListView_Temp_Info.Items.Clear();
                userControl_主界面_FrmMain.ListView_Temp_Info.Items.Clear();
                //ListView_Temp_Info.ItemsSource = null;
                //userControl_主界面_FrmMain.ListView_Temp_Info.ItemsSource = null;
                bool_ListView_Temp_Info_End_Clear.userControl_主界面_FrmMain_ListView_Temp_Info_ItemSource_Name = "";

                ListView_Temp_Info_ItemSource_Name = "";
            }
        }

        #endregion

        #endregion

        #region 关闭应用

        //关闭程序
        private void Windows_FrmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //关闭桌面写真
            if (Bool_Windows_Wallpaper == true)
            {
                SystemParametersInfo(20, 1, wallpaper_path, 1);
            }

            //保存歌单歌曲信息
            Save_ALL_SongListInfo();

        }


        #endregion

       
    }
}
