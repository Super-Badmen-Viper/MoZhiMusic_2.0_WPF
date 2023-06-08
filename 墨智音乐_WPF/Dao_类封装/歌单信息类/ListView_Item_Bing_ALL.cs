using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 墨智音乐_WPF.Dao_类封装.歌单信息类
{
    public class ListView_Item_Bing_ALL
    {
        public List<ListView_Item_Bing> listView_Temp_Info_End_ALL = new List<ListView_Item_Bing>();
        public List<ListView_Item_Bing> listView_Temp_Info_End_Love = new List<ListView_Item_Bing>();
        public List<ListView_Item_Bing> listView_Temp_Info_End_Auto = new List<ListView_Item_Bing>();


        private static ListView_Item_Bing_ALL listView_Item_Bing_ALL;

        public static ListView_Item_Bing_ALL Retuen_This()
        {
            listView_Item_Bing_ALL = Return_This_listView_Item_Bing_ALL();

            return listView_Item_Bing_ALL;
        }
        public static ListView_Item_Bing_ALL Return_This_listView_Item_Bing_ALL()
        {
            if (listView_Item_Bing_ALL == null)
            {
                listView_Item_Bing_ALL = new ListView_Item_Bing_ALL();
            }

            return listView_Item_Bing_ALL;
        }

    }
}
