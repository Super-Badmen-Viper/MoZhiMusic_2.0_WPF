﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 墨智音乐_WPF.Dao_类封装.歌单信息类
{
    public class Bool_listView_Temp_Info_End_Clear
    {

        public bool Bool_listView_Temp_Info_End_ALL_Clear;
        public bool Bool_listView_Temp_Info_End_Love_Clear;
        public bool Bool_listView_Temp_Info_End_Auto_Clear;
        public string userControl_主界面_FrmMain_ListView_Temp_Info_ItemSource_Name;


        private static Bool_listView_Temp_Info_End_Clear bool_ListView_Temp_Info_End_Clear;

        public static Bool_listView_Temp_Info_End_Clear Retuen_This()
        {
            bool_ListView_Temp_Info_End_Clear = Return_This_listView_Item_Bing_ALL();

            return bool_ListView_Temp_Info_End_Clear;
        }
        public static Bool_listView_Temp_Info_End_Clear Return_This_listView_Item_Bing_ALL()
        {
            if (bool_ListView_Temp_Info_End_Clear == null)
            {
                bool_ListView_Temp_Info_End_Clear = new Bool_listView_Temp_Info_End_Clear();
            }

            return bool_ListView_Temp_Info_End_Clear;
        }
    }
}
