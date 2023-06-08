using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 墨智音乐_WPF.Dao_类封装.歌曲信息类
{
    class Song_Info
    {
        //标题
        public string Title { get; set; }
        //艺术家
        public string Singer { get; set; }
        //专辑
        public string Album { get; set; }
        //年份
        public int Year { get; set; }
        //音轨号
        public string Track_number { get; set; }
        //碟号
        public string Disc_number { get; set; }
        //风格
        public enum Style 
        {
            classical, 
            baroque, 
            romantic, 
            impressionist, 
            expressionist, 
            country, 
            jazz, 
            rock, 
            heavy_metal, 
            punk, 
            electronic, 
            soul, 
            R_B, 
            British_rock, 
            divine_dance, 
            Gang_rap, 
            gothic, 
            etc
            //"古典主义音乐","巴洛克音乐","浪漫主义音乐","印象主义音乐","表现主义音乐","乡村音乐","爵士","摇滚","重金属音乐","朋克","电子音乐","灵魂音乐","R&B","英伦摇滚","神游舞曲","匪帮说唱","哥特式音乐"
        }
        //专辑艺术家
        public string Album_Artist { get; set; }
        //作曲家
        public string Composer { get; set; }
        //作词家
        public string Song_Writer { get; set; }
        //注释
        public string Song_Note { get; set; }
        //歌词
        public StringBuilder Song_Krc { get; set; }
        //歌曲图片
        public Image Song_Like_Image { get; set; }
    }
}
