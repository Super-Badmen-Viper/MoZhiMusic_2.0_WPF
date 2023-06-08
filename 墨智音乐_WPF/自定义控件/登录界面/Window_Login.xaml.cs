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
using System.Windows.Shapes;

namespace 墨智音乐_WPF.自定义控件.登录界面
{
    /// <summary>
    /// Window_Login.xaml 的交互逻辑
    /// </summary>
    public partial class Window_Login : Window
    {
        public Window_Login()
        {
            InitializeComponent();

            //显示位置在屏幕中心
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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

        墨智音乐 Get_FrmMain = new 墨智音乐();


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_User_Login_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (TextBox_User_Account.Text.ToString().Trim().Equals("snake"))
            {
                Get_FrmMain.Show();
                this.Close();
            }
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
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();//关闭
        }

        bool Bool_Login_QR_Code;
        /// <summary>
        /// 二维码登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Login_QR_Code_Click(object sender, RoutedEventArgs e)
        {
            if (Bool_Login_QR_Code == false)
            {
                StackPanel_Account_Header_Image.Visibility = Visibility.Hidden;
                StackPanel_Login_Info.Visibility = Visibility.Hidden;

                Button_Login_QR_Code.HorizontalAlignment = HorizontalAlignment.Center;
                Button_Login_QR_Code.VerticalAlignment = VerticalAlignment.Center;

                Button_Login_QR_Code.Width = 211;
                Button_Login_QR_Code.Height = 211;

                Bool_Login_QR_Code = true;
            }
            else
            {
                StackPanel_Account_Header_Image.Visibility = Visibility.Visible;
                StackPanel_Login_Info.Visibility = Visibility.Visible;

                Button_Login_QR_Code.HorizontalAlignment = HorizontalAlignment.Right;
                Button_Login_QR_Code.VerticalAlignment = VerticalAlignment.Bottom;

                Button_Login_QR_Code.Width = 40;
                Button_Login_QR_Code.Height = 40;

                Bool_Login_QR_Code = false;
            }

        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Register_An_Account_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Retrieve_Password_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
