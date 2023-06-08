using System.Drawing;

namespace 墨智音乐_WPF.资源处理
{
    class Utils_MakeThumbnailImage
    {

        //Utils_MakeThumbnailImage(image, 1980, 1080, image.Width, image.Height, 0, 0);         
        /// <summary>
        /// 裁剪图片并保存
        /// </summary>
        /// <param name="Image">图片信息</param>
        /// <param name="maxWidth">缩略图宽度</param>
        /// <param name="maxHeight">缩略图高度</param>
        /// <param name="cropWidth">裁剪宽度</param>
        /// <param name="cropHeight">裁剪高度</param>
        /// <param name="X">X轴</param>
        /// <param name="Y">Y轴</param>
        public static Bitmap MakeThumbnailImage(Image originalImage, int maxWidth, int maxHeight, int cropWidth, int cropHeight, int X, int Y)
        {
            Bitmap b = new Bitmap(cropWidth, cropHeight);
            try
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    //清空画布并以透明背景色填充
                    g.Clear(Color.Transparent);
                    //在指定位置并且按指定大小绘制原图片的指定部分
                    g.DrawImage(originalImage, new Rectangle(0, 0, cropWidth, cropHeight), X, Y, cropWidth, cropHeight, GraphicsUnit.Pixel);
                    Image displayImage = new Bitmap(b, maxWidth, maxHeight);
                    displayImage.Save(@"D:\墨智_毒蛇讯息（简单音乐播放器）1.0.1\singer_songPhoto\缩略图.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    Bitmap bit = new Bitmap(b, maxWidth, maxHeight);
                    return bit;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                //originalImage.Dispose();
                //b.Dispose();
            }
        }

    }
}
