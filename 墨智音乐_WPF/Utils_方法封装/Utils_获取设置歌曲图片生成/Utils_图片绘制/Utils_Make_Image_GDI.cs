using System;
using System.Drawing;
using System.Windows.Forms;

namespace 墨智音乐_WPF.Utils.Utils_歌曲图片生成.Utils_图片绘制
{
    class Utils_Make_Image_GDI
    {

        public double numsDeg = 0;//旋转度数
        //public Bitmap image;//矩阵位图
        //public Bitmap backgroung;
        Graphics graphics;//绘制矩阵画面
        Rectangle rect;//矩阵的位置和大小
        PointF center;//矩阵的位置
        RectangleF picRect;//新矩阵相对于原矩阵的位置和大小
        PointF Pcenter;//新矩阵相对于原矩阵的位置

        /// <summary>
        /// 使用背景图片进行绘制旋转，图片的缩放易控，更美观，但性能不如使用Bitmap，且受其它代码进程影响较多
        /// </summary>
        /// <param name="Temp_Image">背景图片二次生成的Bitmap,二次生成是通过背景图片可控样式生成指定比例大小的Bitmap</param>
        public void picDraw_BackGround(Bitmap Temp_Image, PictureBox pictureBox1)
        {
            //可以不重新绘制，但是Timer的运行速度取决于CPU的调度，如果不重新绘制，图片绘制的速度并不会受到控制，而是越来越快
            //是否重新绘制，目前对性能的影响微乎其微
            graphics = pictureBox1.CreateGraphics();//创建绘画区域            

            rect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);//创建矩阵区域初始位置及大小  //要显示到Form的矩形区域
            center = new PointF(rect.Width / 2, rect.Height / 2);//制定矩阵区域中心位置

            float offsetX = 0;
            float offsetY = 0;
            offsetX = center.X - pictureBox1.Width / 2;
            offsetY = center.Y - pictureBox1.Height / 2;//计算创建绘图变量

            picRect = new RectangleF(offsetX, offsetY, pictureBox1.Width, pictureBox1.Height);//指定位置与大小来初始化RectangleF类

            Pcenter = new PointF(picRect.X + picRect.Width / 2, picRect.Y + picRect.Height / 2);//指定坐标初始化PointF类

            graphics.TranslateTransform(Pcenter.X, Pcenter.Y);//为绘制的画面指定图片坐标的原点（初始点）

            graphics.RotateTransform(Convert.ToSingle(numsDeg));//将指定旋转度数numsDeg  应用于graphics对象的变换矩阵

            graphics.TranslateTransform(-Pcenter.X, -Pcenter.Y);//为绘制的画面指定图片坐标的原点（结束点）

            //所要旋转的图片信息源
            graphics.DrawImage(Temp_Image, picRect);//在指定的picRect对象的位置绘制指定大小的Bitmap对象image

            numsDeg += 0.3;//只能进行++，不变则无法旋转，每次旋转都是相对于原位置进行旋转，而不是相对于旋转后新的位置
            //所以只能利用timer控件，不断的递增旋转的度数以达到图片旋转的视觉效果
        }

    }
}
