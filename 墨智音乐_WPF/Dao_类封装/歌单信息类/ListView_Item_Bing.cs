using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace 墨智音乐_WPF.Dao_类封装.歌单信息类
{
    public class ListView_Item_Bing
    {
        public string Singer_Name { get; set; }
        public string Song_Name { get; set; }
        public string Album_Name { get; set; }
        public string Song_Url { get; set; }
        public int Song_No { get; set; }
        public int Song_Like { get; set; }
        public ImageBrush Song_Image { get; set; }
       
    }
}
