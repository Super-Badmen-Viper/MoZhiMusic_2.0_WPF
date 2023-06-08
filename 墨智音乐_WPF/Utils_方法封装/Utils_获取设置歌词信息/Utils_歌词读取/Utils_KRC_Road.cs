using System;
using System.IO;

namespace 墨智音乐_WPF.Utils.Utils_歌词信息处理.Utils_歌词读取
{
    class Utils_KRC_Road
    {

        /// <summary>
        /// 定时器方法，调用该方法读取歌词，仅需调用一次，当返回值为true时，关闭歌词读取，开启歌词显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool Show_Lrc_Text_Tick()
        {
            return player_lrc_Save_Text();
        }





        public int Time_1_Middle;	//int 	("," 的位置)
        public int Time_1_End;		//int 	("]" 的位置)
        public int Time_2_Start;	//int 	("<" 的位置)
        public int TIme_2_Middle_1; //int 	("," 的位置)
        public int TIme_2_Middle_2; //int 	("," 的位置)
        public int Time_2_End;      //int 	(">" 的位置)
        public int Start_Time;      //int	开始的时间
        public int Nums_Time;       //int	持续的时间

        public int[] LRC_Nums_Times;  //int数组     存储单个歌词持续的时间
        public string[] LRC_Text_Times;

        public string[] LRC_Time = new string[500];
        public string[] LRC_Text = new string[500];

        public StreamReader Song_Lrc_StreamReader;//将当前的歌词文件转化临时文件流
        public String A_String_Read;//传递临时生成的歌词时间

        public static string LRC_Text_Temp;//要返回的歌词Text  

        public static int Selected_KRC;//当前同步显示歌词的选定项
        public double Start_First_Time_1 = 0;
        public double End_Last_Time_1 = 0;



        /// <summary>
        /// 一行一行while 的读取歌词文件流中的信息并保存
        /// </summary>
        public bool player_lrc_Save_Text()
        {
            //if (WMP1.playState == WMPLib.WMPPlayState.wmppsPlaying)//关闭检查可在暂停音乐时同步歌词
            //{
            try
            {

                if (Song_Lrc_StreamReader.EndOfStream == false)//指示当前流位置是否在结尾
                {
                    while ((A_String_Read = Song_Lrc_StreamReader.ReadLine()) != null)
                    {
                        if (A_String_Read.ToString().Length < 10)//跳过空格
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("id"))//跳过offset标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("ar"))//跳过ar标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("ti"))//跳过ti标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("by"))//跳过by标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("ha"))//跳过by标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("al"))//跳过by标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("si"))//跳过by标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("qq"))//跳过by标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("to"))//跳过by标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("of"))//跳过offset标签
                            continue;
                        if (A_String_Read.ToString().Substring(1, 2).Equals("la"))//跳过by标签
                            continue;

                        int nums_temp_1 = A_String_Read.IndexOf("]") + 1;
                        int nums_temp_2 = A_String_Read.IndexOf("[") + 1;

                        if (A_String_Read.Length <= nums_temp_1)//过滤歌词内容为空的行
                        {
                            continue;
                        }

                        //A_String_Read = "[" + Start_Time + "]" + Show_Song_KRC_Text(A_String_Read);
                        A_String_Read = Show_Song_KRC_Text(A_String_Read);

                        for (int i = 5; i < 500; i++)
                        {//获取krc文件歌词内容                 
                            if (LRC_Text[i] == null)
                                if (LRC_Text_Temp != null)
                                {
                                    LRC_Text[i] = A_String_Read;
                                    if (LRC_Time[i] == null)
                                    {
                                        LRC_Time[i] = Start_Time.ToString();
                                        break;
                                    }
                                }
                        }


                    }
                }
                else
                {
                    //关闭对歌词文件的读取定时
                    //Show_Lrc_Text.Stop();

                    ////生成ComBox中的歌词文件
                    //Clear_And_Create_SongKrc_LRC_Time_To_ComBox();


                    ///生成这首歌词第一句歌词开始的时间和最后一句歌词开始的时间
                    for (int j = 0; j < LRC_Time.Length; j++)//输出数组一开始的两个时间
                        if (LRC_Time[j] != null)
                        {
                            string temp_5 = LRC_Time[j];//753 
                            Start_First_Time_1 = Convert.ToInt64(temp_5) / 1000;
                            break;
                        }
                    for (int j = LRC_Time.Length - 1; j >= 0; j--)//输出数组最后的两个时间
                        if (LRC_Time[j] != null)
                        {
                            string temp_5 = LRC_Time[j];//201538
                            End_Last_Time_1 = Convert.ToInt64(temp_5) / 1000;//毫秒数 / 100
                            break;
                        }



                    return true;

                }
            }
            catch
            {

            }

            return false;
        }


        /// <summary>
        /// 读取歌词文件中的逐字信息处理并保存在数组中
        /// </summary>
        /// <param name="KRC_One_Text">传入文件流读取的一整行KRC</param>
        /// <returns>返回一句完全由字符串拼接的string整个字符串</returns>
        public string Show_Song_KRC_Text(string KRC_One_Text)
        {
            //创建中间值存储
            string Temp_1 = KRC_One_Text;
            string Temp_2 = KRC_One_Text;
            string Temp_3 = KRC_One_Text;
            string Temp_4 = KRC_One_Text;

            LRC_Text_Temp = "";//要返回的歌词Text

            /*try
            {*/
                Time_1_Middle = Temp_1.IndexOf(",");//[81751,2131]  下标从0开始
                //MessageBox.Show(KRC_One_Text + "        , 的位置        " + Time_1_Middle);

                Time_1_End = Temp_1.IndexOf("]");
                //MessageBox.Show(KRC_One_Text + "        ] 的位置   " + Time_1_End);

                Start_Time = Convert.ToInt32(Temp_1.Substring(1, Time_1_Middle - 1));
                //MessageBox.Show(KRC_One_Text + "        开始的时间：    " + Start_Time);

                Nums_Time = Convert.ToInt32(Temp_2.Substring(Time_1_Middle + 1, Time_1_End - Time_1_Middle - 1));
                //MessageBox.Show(KRC_One_Text + "        持续的时间：    " + Nums_Time);


                //存储这一行歌词开始与持续的时间
                LRC_Start_And_Middle_Time(Start_Time, Nums_Time);



                Temp_3 = Temp_3.Substring(Time_1_End + 1, Temp_3.Length - Time_1_End - 1);//<0,255,0>胡<255,204,0>歌 <459,253,0>- <712,253,0>六<965,153,0>月<1118,203,0>的<1321,201,0>雨
                Temp_4 = Temp_3;

                LRC_Nums_Times = new int[30];
                LRC_Text_Times = new string[30];


                for (int i = 0; i < LRC_Nums_Times.Length; i++)
                {

                    Temp_3 = Temp_4;

                    if (LRC_Nums_Times[i] == 0 && LRC_Text_Times[i] == null)
                    {
                        /*try
                        {*/
                            //LRC_Nums_Times[i]  保存每个歌词的开始时间
                            string tempss = Temp_3;
                            int middle_nums_start_1 = tempss.IndexOf("<") + 1;//从0开始
                            int middle_nums_start_2 = tempss.IndexOf(",");//从0开始
                            int nums_1 = Convert.ToInt32(tempss.Substring(middle_nums_start_1, middle_nums_start_2 - middle_nums_start_1));//从0开始，选3个-> 255,



                            int middle_nums_1 = Temp_3.IndexOf(",");//<0,255,0>胡<255,204,0>歌 <459,253,0>- <712,253,0>六<965,153,0>月<1118,203,0>的<1321,201,0>雨
                            Temp_3 = Temp_3.Substring(middle_nums_1 + 1, Temp_3.Length - middle_nums_1 - 1);//255,0>胡<255,204,0>歌 <459,253,0>- <712,253,0>六<965,153,0>月<1118,203,0>的<1321,201,0>雨



                            //LRC_Text_Times   保存每个歌词的内容
                            string temp = Temp_3;//保存读取的单个歌词
                            Time_2_End = temp.IndexOf(">") + 1;
                            Time_2_Start = temp.IndexOf("<");
                            if (Time_2_Start > 0)
                            {
                                temp = temp.Substring(Time_2_End, Time_2_Start - Time_2_End);
                            }
                            else
                            {
                                temp = temp.Substring(Time_2_End, temp.Length - Time_2_End);
                            }
                            LRC_Text_Times[i] = temp;


                            string temps = Temp_3;//保存下一次substring的变量
                            Time_2_Start = temps.IndexOf("<");
                            if (Time_2_Start > 0)
                                Temp_4 = temps.Substring(Time_2_Start, temps.Length - Time_2_Start);//<255,204,0>歌 <459,253,0>- <712,253,0>六<965,153,0>月<1118,203,0>的<1321,201,0>雨



                            //LRC_Nums_Times[i]  保存每个歌词的持续时间
                            int middle_nums_2 = Temp_3.IndexOf(",");//从0开始
                            Temp_3 = Temp_3.Substring(0, middle_nums_2);//从0开始，选3个-> 255,
                            LRC_Nums_Times[i] = Convert.ToInt32(Temp_3);
                            int nums_2 = Convert.ToInt32(Temp_3);


                            //nums_1 + Start_Time    总时间制
                            KRC_Start_And_Middle_And_Text(nums_1, nums_2, temp, nums_1 + Start_Time);

                            if (Time_2_Start < 0)
                                break;

                       /* }
                        catch
                        {
                            break;
                        }*/

                    }
                }
           /* }
            catch
            {
                //MessageBox.Show("        错误！    ");
            }*/

            for (int i = 0; i < LRC_Text_Times.Length; i++)//字符串(char)集合
            {
                if (LRC_Text_Times[i] != null)
                {
                    LRC_Text_Temp += LRC_Text_Times[i];
                    if (LRC_Text_Times[i + 1] == null)
                    {
                        break;
                    }
                }
            }

            return LRC_Text_Temp;//整个字符串
        }


        public int Select_Open_KRC;//记录歌词逐字同步是否被打开  ,0:关闭  ，1：打开
        public static double Krc_Start_Times;//当前播放时间所同步的单个歌词的持续时间
        public int[] StartKrcTimes;//存储每个歌词开始的时间

        public int[] MiddleKrcTimes;//存储每个歌词持续的时间


        public string[] StartKrcTexts;//存储每个歌词的内容

        public double[] StartKrcTimes_All;//存储每个歌词开始的时间,总时间制

        public int[] StartTimes;//存储每一行歌词开始的时间
        public int[] MiddleTimes;//存储每一行歌词持续的时间

        /// <summary>
        /// 存储单行歌词开始与持续的时间
        /// </summary>
        /// <param name="StartTime">存储单行歌词开始的时间</param>
        /// <param name="MiddleTime">存储单行歌词持续的时间</param>
        public void LRC_Start_And_Middle_Time(int StartTime, int MiddleTime)
        {
            for (int i = 5; i < 500; i++)
            {
                if (StartTimes[i] == 0)
                {
                    if (MiddleTimes[i] == 0)
                    {
                        StartTimes[i] = StartTime;
                        MiddleTimes[i] = MiddleTime;
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// 存储每个歌词的内容
        /// </summary>
        /// <param name="StartTime">存储每个歌词开始的时间</param>
        /// <param name="MiddleTime">存储每个歌词持续的时间</param>
        /// <param name="KRC_Text">存储每个歌词的内容</param>
        public void KRC_Start_And_Middle_And_Text(int StartTime, int MiddleTime, String KRC_Text, int StartTime_ALL)
        {
            for (int i = 5; i < 1000; i++)
            {
                if (StartKrcTimes[i] == 0)
                {
                    if (MiddleKrcTimes[i] == 0)
                    {
                        if (StartKrcTexts[i] == null)
                        {
                            if (StartKrcTimes_All[i] == 0)
                            {
                                StartKrcTimes[i] = StartTime;
                                StartKrcTexts[i] = KRC_Text;
                                StartKrcTimes_All[i] = Convert.ToDouble(StartTime_ALL) / 1000;


                                MiddleKrcTimes[i] = MiddleTime;

                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
