using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 墨智音乐_WPF.Dao_类封装.歌词信息类
{
    class Dao_ListBox_Temp_Krc_OneLine
    {
        //歌词行开始的时间
        public int Krc_Line_Start_Time { get; set; }
        //歌词行持续的时间
        public int Krc_Line_Continue_Time { get; set; }
        //歌词行的内容
        public string Krc_Line_Text { get; set; }
        //单个歌词信息对象
        public Dao_ListBox_Temp_Krc_OneLine_MoreByte[] dao_listBox_temp_krc_oneLine_moreByte { get; set; }

    }
}
