using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BMHelper_fuc;
using System.Globalization;
using System.Runtime.InteropServices;



namespace BMHelper
{
    public partial class Form1 : Form
    {
        // 定义一个全局dm对象保持
        public dmsoft m_dm;

        //各种全局变量+
        //public System.Windows.Forms.ComboBox item_bar_5;
        public string path = @"G:\BackMir_3.11.01\Bin\过滤\自动副本专用.txt"; // 修改为你的TXT文件路径
        public string[] p_garbage_items;
        public string data_dir = "";
        public string configFilePath;
        public string scripver = "0.6";
        public int thread_count = 1;
        public bool needstop = false;
        public bool need_check_pet = false;
        public bool need_check_exp = false;
        public bool need_auto_run_fb = false;
        public bool need_auto_sg = false;
        public bool need_auto_fight = false;
        public bool need_fly_and_hit = false;
        public bool need_update_stop = false;
        public bool CT_type = true;
        public int p_fb_select;
        public string player_state = "脚本结束";
        public string p_hwnd_game;
        public string p_hwnd_helper;
        public string p_fly_name;
        public string p_fly_code;
        public HotKey HotKey;
        public PlayerState PS;
        public PlayerState.MapState MapState;
        public PlayerState.AttackState AttackState;
        public string wnd_pos = "center";
        public string[] p_item_name =
            {
                "大补丸（小）",
                "大补丸（中）",
                "大补丸（大）",
                "强效金创药",
                "特效金创药",
                "疗伤药",
                "强效魔法药",
                "特效魔法药",
                "魔力药",
                "太阳水",
                "强效太阳水",
                "万年雪霜",
                "回城卷",
                "随机传送卷"
            };
        public long[] p_item_id =
            {
                1231,
                1232,
                1233,
                28,
                813,
                15,
                29,
                814,
                17,
                16,
                312,
                260,
                402,
                404
            };
        public List<string> locations = new List<string>
        {
            "比奇",
            "祖玛一层",
            "石墓",
            "蛮荒废墟",
            "死亡深渊"
            // 添加更多位置...
        };
        public string fly_map_name;
        public CancellationTokenSource cancellationTokenSource;
        Dictionary<int, ItemInfo> Zd_iteminfo;
        List<InventoryItem> Bagdetail;
        public ArrayList Ini_item_bar_id_arr;
        public ArrayList Ini_item_data_name_arr;
        public ArrayList Ini_item_data_id_arr;

        //public int p_fly_and_hit_map;
        private System.Threading.Timer dataRefreshTimer;
        private static System.Threading.Timer NoCT_Timer;
        public static object lockObject;
        public static bool isCallbackExecuting = false;


        public Form1()
        {
            InitializeComponent(); 
        }
  

        //简单说明一下：
        //“public static extern bool RegisterHotKey()”这个函数用于注册热键。由于这个函数需要引用user32.dll动态链接库后才能使用，并且
        //user32.dll是非托管代码，不能用命名空间的方式直接引用，所以需要用“DllImport”进行引入后才能使用。于是在函数前面需要加上
        //“[DllImport("user32.dll", SetLastError = true)]”这行语句。
        //“public static extern bool UnregisterHotKey()”这个函数用于注销热键，同理也需要用DllImport引用user32.dll后才能使用。
        //“public enum KeyModifiers{}”定义了一组枚举，将辅助键的数字代码直接表示为文字，以方便使用。这样在调用时我们不必记住每一个辅
        //助键的代码而只需直接选择其名称即可。
        //（2）以窗体FormA为例，介绍HotKey类的使用
        //在FormA的Activate事件中注册热键，本例中注册Shift+S，Ctrl+Z，Alt+D这三个热键。这里的Id号可任意设置，但要保证不被重复。
        public void Form1_Activated(object sender, EventArgs e)
        {            
            //注册热键Ctrl+小键盘1，Id号为100。HotKey.KeyModifiers.Shift也可以直接使用数字4来表示。
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.Ctrl, Keys.NumPad1);
            //注册热键Ctrl+小键盘2，Id号为101。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.Ctrl, Keys.NumPad2);
            //注册热键Ctrl+小键盘3，Id号为102。HotKey.KeyModifiers.Alt也可以直接使用数字1来表示。
            HotKey.RegisterHotKey(Handle, 102, HotKey.KeyModifiers.Ctrl, Keys.NumPad3);
        }
        //在FormA的Leave事件中注销热键。
        public void Form1_Leave(object sender, EventArgs e)
        {
            //注销Id号为100的热键设定
            HotKey.UnregisterHotKey(Handle, 100);
            //注销Id号为101的热键设定
            HotKey.UnregisterHotKey(Handle, 101);
            //注销Id号为102的热键设定
            HotKey.UnregisterHotKey(Handle, 102);
        }

        //监测热键
        /// 
        /// 监视Windows消息
        /// 重载WndProc方法，用于实现热键响应
        /// 
        /// 
        protected override void WndProc(ref Message m)
        {

            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Ctrl+Num.1
                                     //此处填写快捷键响应代码 
                            if (checkBox_wash_item.Checked == true)
                            {
                                Echo_info("开始洗练...");
                                Wash_item();

                            }
                            else
                            {
                                Echo_info("未勾选功能模块，请先勾选");
                            }
                            Application.DoEvents();

                            break;
                        case 101:    //按下的是Ctrl+Num.2
                                     //此处填写快捷键响应代码
                            if (checkBox_fuc_2.Checked == true)
                            {
                                //Echo_info("按下的是Ctrl+Num.2");
                                StartLongRunningTask();
                            }
                            else
                            {
                                Echo_info("未勾选功能模块，请先勾选");
                            }
                            Application.DoEvents();
                            break;
                        case 102:    //按下的是Ctrl+Num.3
                                     //此处填写快捷键响应代码
                            if (checkBox_sorting_item.Checked == true)
                            {
                                Echo_info("开始整理物品");
                                Sorting_item();

                            }
                            else
                            {
                                Echo_info("未勾选功能模块，请先勾选");
                            }
                            Application.DoEvents();
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        //查询当前地图所有怪物、玩家、NPC
        public List<string> Find_obj_by_id(int id)
        {
            string result;
            string[] results;
            List<string> ls = new List<string>();
            switch (id)
            {
                case -1:
                    result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "E8 40 72 00", 4, 1, 1);//当前地图玩家地址合集字符串
                    try
                    {
                        results = result.Split('|');
                        foreach (string s in results)
                        {
                            int addr = Convert.ToInt32(s, 16) + 0x6c;
                            string player_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr.ToString("X"), 0, 0);
                            ls.Add(player_name);
                        }
                    }
                    catch (Exception)
                    {

                        return ls;
                    } 
                    return ls;
                case 0:                    
                    result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "F4 1B 72 00", 4, 1, 1);//当前地图npc地址合集字符串                    
                    try
                    {
                        results = result.Split('|');
                        foreach (string s in results)
                        {
                            int addr = Convert.ToInt32(s, 16) + 0x6c;
                            string npc_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr.ToString("X"), 0, 0);
                            ls.Add(npc_name);
                        }
                    }
                    catch (Exception)
                    {

                        return ls;
                    }                    
                    return ls;
                case 17:
                case 18:
                case 31:
                case 34:
                case 94:
                case 130:                
                    result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "C8 E2 71 00", 4, 1, 1);//当前地图npc地址合集字符串                    
                    try
                    {
                        results = result.Split('|');
                        List<string> ls_id = new List<string>();
                        List<string> ls_name = new List<string>();
                        List<string> ls_x = new List<string>();
                        List<string> ls_y = new List<string>();
                        foreach (string s in results)
                        {
                            int addr_id = Convert.ToInt32(s, 16) + 0x68;
                            int addr_x = Convert.ToInt32(s, 16) + 0x4c;
                            int addr_y = Convert.ToInt32(s, 16) + 0x50;
                            int addr_name = Convert.ToInt32(s, 16) + 0x6C;
                            long monster_id = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_id.ToString("X"), 5);
                            string monster_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0);
                            long monster_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_x.ToString("X"), 4);
                            long monster_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_y.ToString("X"), 4);
                            ls_id.Add(monster_id.ToString());
                            ls_name.Add($"{monster_name}");
                            ls_x.Add(monster_x.ToString());
                            ls_y.Add(monster_y.ToString());
                        }
                        for (int i = 0; i < ls_id.Count; i++)
                        {
                            if (Convert.ToInt32(ls_id[i]) == id)
                            {
                                string str = $"{ls_x[i]},{ls_y[i]}";
                                ls.Add(str);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        return ls;
                    }
                    return ls;
                case 72:
                case 162:
                    result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "20 2A 71 00", 4, 1, 1);//当前地图npc地址合集字符串                    
                    try
                    {
                        results = result.Split('|');
                        List<string> ls_id = new List<string>();
                        List<string> ls_name = new List<string>();
                        List<string> ls_x = new List<string>();
                        List<string> ls_y = new List<string>();
                        foreach (string s in results)
                        {
                            int addr_id = Convert.ToInt32(s, 16) + 0x68;
                            int addr_x = Convert.ToInt32(s, 16) + 0x4c;
                            int addr_y = Convert.ToInt32(s, 16) + 0x50;
                            int addr_name = Convert.ToInt32(s, 16) + 0x6C;
                            long monster_id = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_id.ToString("X"), 5);
                            string monster_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0);
                            long monster_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_x.ToString("X"), 4);
                            long monster_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_y.ToString("X"), 4);
                            ls_id.Add(monster_id.ToString());
                            ls_name.Add($"{monster_name}");
                            ls_x.Add(monster_x.ToString());
                            ls_y.Add(monster_y.ToString());
                        }
                        for (int i = 0; i < ls_id.Count; i++)
                        {
                            if (Convert.ToInt32(ls_id[i]) == id)
                            {
                                string str = $"{ls_x[i]},{ls_y[i]}";
                                ls.Add(str);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        return ls;
                    }
                    return ls;
                case 19:
                case 131:
                    result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "20 0F 6E 00", 4, 1, 1);//当前地图npc地址合集字符串                    
                    try
                    {
                        results = result.Split('|');
                        List<string> ls_id = new List<string>();
                        List<string> ls_name = new List<string>();
                        List<string> ls_x = new List<string>();
                        List<string> ls_y = new List<string>();
                        foreach (string s in results)
                        {
                            int addr_id = Convert.ToInt32(s, 16) + 0x68;
                            int addr_x = Convert.ToInt32(s, 16) + 0x4c;
                            int addr_y = Convert.ToInt32(s, 16) + 0x50;
                            int addr_name = Convert.ToInt32(s, 16) + 0x6C;
                            long monster_id = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_id.ToString("X"), 5);
                            string monster_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0);
                            long monster_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_x.ToString("X"), 4);
                            long monster_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_y.ToString("X"), 4);
                            ls_id.Add(monster_id.ToString());
                            ls_name.Add($"{monster_name}");
                            ls_x.Add(monster_x.ToString());
                            ls_y.Add(monster_y.ToString());
                        }
                        for (int i = 0; i < ls_id.Count; i++)
                        {
                            if (Convert.ToInt32(ls_id[i]) == id)
                            {
                                string str = $"{ls_x[i]},{ls_y[i]}";
                                ls.Add(str);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        return ls;
                    }
                    return ls;
                case 46:
                case 47:
                case 85:
                    result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "40 AE 71 00", 4, 1, 1);//当前地图npc地址合集字符串                    
                    try
                    {
                        results = result.Split('|');
                        List<string> ls_id = new List<string>();
                        List<string> ls_name = new List<string>();
                        List<string> ls_x = new List<string>();
                        List<string> ls_y = new List<string>();
                        foreach (string s in results)
                        {
                            int addr_id = Convert.ToInt32(s, 16) + 0x68;
                            int addr_x = Convert.ToInt32(s, 16) + 0x4c;
                            int addr_y = Convert.ToInt32(s, 16) + 0x50;
                            int addr_name = Convert.ToInt32(s, 16) + 0x6C;
                            long monster_id = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_id.ToString("X"), 5);
                            string monster_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0);
                            long monster_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_x.ToString("X"), 4);
                            long monster_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_y.ToString("X"), 4);
                            ls_id.Add(monster_id.ToString());
                            ls_name.Add($"{monster_name}");
                            ls_x.Add(monster_x.ToString());
                            ls_y.Add(monster_y.ToString());
                        }
                        for (int i = 0; i < ls_id.Count; i++)
                        {
                            if (Convert.ToInt32(ls_id[i]) == id)
                            {
                                string str = $"{ls_x[i]},{ls_y[i]}";
                                ls.Add(str);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        return ls;
                    }
                    return ls;
                case 86:
                case 140:
                    result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "18 22 70 00", 4, 1, 1);//当前地图npc地址合集字符串                    
                    try
                    {
                        results = result.Split('|');
                        List<string> ls_id = new List<string>();
                        List<string> ls_name = new List<string>();
                        List<string> ls_x = new List<string>();
                        List<string> ls_y = new List<string>();
                        foreach (string s in results)
                        {
                            int addr_id = Convert.ToInt32(s, 16) + 0x68;
                            int addr_x = Convert.ToInt32(s, 16) + 0x4c;
                            int addr_y = Convert.ToInt32(s, 16) + 0x50;
                            int addr_name = Convert.ToInt32(s, 16) + 0x6C;
                            long monster_id = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_id.ToString("X"), 5);
                            string monster_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0);
                            long monster_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_x.ToString("X"), 4);
                            long monster_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_y.ToString("X"), 4);
                            ls_id.Add(monster_id.ToString());
                            ls_name.Add($"{monster_name}");
                            ls_x.Add(monster_x.ToString());
                            ls_y.Add(monster_y.ToString());
                        }
                        for (int i = 0; i < ls_id.Count; i++)
                        {
                            if (Convert.ToInt32(ls_id[i]) == id)
                            {
                                string str = $"{ls_x[i]},{ls_y[i]}";
                                ls.Add(str);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        return ls;
                    }
                    return ls;                
                default:
                    string[] key_code = { "C8 E2 71 00","20 2A 71 00","20 0F 6E 00","40 AE 71 00","FC 8D 72 00",
                        "EC 8C 72 00","18 22 70 00","84 AE 6E 00","18 5F 6E 00","30 79 71 00","50 71 70 00","60 C0 70 00",
                        "D8 8B 70 00","C8 18 6F 00","10 DB 70 00","98 F5 70 00","C8 93 6E 00","90 07 70 00","08 2A 6E 00",
                        "08 C9 6E 00","90 E3 6E 00","40 FE 6E 00","C8 56 70 00"};
                    result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "98 0E 6E 00", 4, 1, 1);//当前地图怪物地址合集字符串
                    foreach (var item in key_code)
                    {
                        string result1 = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", item, 4, 1, 1);
                        if (result1.Length > 0)
                        {
                            result = $"{result}|{result1}";
                        }                       
                    }         
                    try
                    {
                        results = result.Split('|');
                        List<string> ls_base = new List<string>();
                        List<string> ls_id = new List<string>();
                        List<string> ls_name = new List<string>();
                        List<string> ls_x = new List<string>();
                        List<string> ls_y = new List<string>();
                        foreach (string s in results)
                        {
                            int addr_id = Convert.ToInt32(s, 16) + 0x68;
                            int addr_x = Convert.ToInt32(s, 16) + 0x4c;
                            int addr_y = Convert.ToInt32(s, 16) + 0x50;
                            int addr_name = Convert.ToInt32(s, 16) + 0x6C;                            
                            long monster_id = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_id.ToString("X"), 5);
                            string monster_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0);
                            long monster_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_x.ToString("X"), 4);
                            long monster_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_y.ToString("X"), 4);
                            ls_base.Add(s);
                            ls_id.Add(monster_id.ToString());
                            ls_name.Add($"{monster_name}");
                            ls_x.Add(monster_x.ToString());
                            ls_y.Add(monster_y.ToString());                            

                        }
                        for (int i = 0; i < ls_id.Count; i++)
                        {
                            if (Convert.ToInt32(ls_id[i]) == id)
                            {
                                string str = $"{ls_x[i]},{ls_y[i]}";
                                ls.Add(str);
                            }
                        }                        
                        if (id == 999) 
                        {
                            ///去重代码
                            HashSet<string> uniqueData = new HashSet<string>(ls_name);
                            foreach (string str in uniqueData) 
                            {
                                ls.Add(str);
                                //Echo_info(str);
                            }
                            return ls;
                        }
                        if (id == 998)
                        {
                            
                            return ls_base;
                        }

                    }
                    catch (Exception)
                    {

                        return ls;
                    }
                    
                    return ls;
            }
        }
        //包裹内物品信息字典
        public class ItemInfo
        {
            public long ItemID { get; set; }
            public string Name { get; set; }            
            public long Quantity { get; set; }

            public ItemInfo(long itemID , string name, long quantity)
            {
                ItemID = itemID;
                Name = name;                
                Quantity = 0; // 初始数量为0
            }
        }
        //包裹信息结构体
        public struct InventoryItem
        {
            public int GridNumber;  // 格子编号
            public long ItemID;      // 物品ID
            public string ItemName; // 物品名称
            public long Quantity;    // 物品数量

            public InventoryItem(int gridNumber, long itemID, string itemName, long quantity)
            {
                GridNumber = gridNumber;
                ItemID = itemID;
                ItemName = itemName;
                Quantity = quantity;
            }
        }
        //更新结构体
        public void UpdateInventoryItem()
        {
            Bagdetail = new List<InventoryItem>();
            for (int y = 0; y < 10; y++) // 10行
            {
                for (int x = 0; x < 8; x++) // 8列
                {
                    int gridNumber = y * 8 + x; // 计算格子编号，从0开始
                    int dynamicAddress = gridNumber * 0x60; // 动态的地址部分，根据列数变化

                    // 读取内存中的值
                    long itemID = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(dynamicAddress).ToString("X")}", 4);

                    // 读取数量
                    long itemCount = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(dynamicAddress + 0x1C).ToString("X")}", 6);
                    // 读取物品名称
                    string itemName = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(dynamicAddress + 0x4).ToString("X")}", 0, 0);

                    // 处理读取到的值，可以将它存储起来或进行其他操作
                    // Echo_info($"格子 {itemID}: {itemCount} 个, 物品名称: {itemName}");

                    // 将每一条 InventoryItem 都添加到 Bagdetail 列表中，即使 ItemID 为 0
                    Bagdetail.Add(new InventoryItem(gridNumber, (int)itemID, itemName, itemCount));
                }
            }
        }
        //统计结构体
        public void ProcessBagDetails(List<InventoryItem> Bagdetail)
        {
            // 获取 Bagdetail 中存在的 Ini_item_data_id_arr 中的 ItemID
            var existingItemIDs = Bagdetail.Select(item => item.ItemID).ToList();

            // 获取 Ini_item_data_id_arr 中存在于 Bagdetail 中的 ItemID
            var commonItemIDs = Ini_item_data_id_arr.Cast<long>().Intersect(existingItemIDs).ToList();

            // 获取 Ini_item_data_id_arr 中存在于 Bagdetail 中的 ItemID 对应的小合集
            var smallCollection = Bagdetail
                .Where(item => commonItemIDs.Contains(item.ItemID))
                .ToList();

            // 获取 Bagdetail 中不存在于 Ini_item_data_id_arr 中的 ItemID
            var missingItemIDs = Ini_item_data_id_arr.Cast<long>().Except(existingItemIDs).ToList();

            // 对于每个缺失的 ItemID，添加一条信息到 smallCollection
            foreach (var missingItemID in missingItemIDs)
            {
                // 获取缺失 ItemID 对应的名称
                int index = Ini_item_data_id_arr.IndexOf(missingItemID);
                string missingItemName = index != -1 ? Ini_item_data_name_arr[index].ToString() : "Unknown"; // 使用 Unknown 或其他默认名称

                // 添加缺失信息到 smallCollection
                smallCollection.Add(new InventoryItem(0, missingItemID, missingItemName, 0));
            }

            // 使用 LINQ 将 smallCollection 按照 ItemID 分组，并计算每个 ItemID 的总数量
            var itemCounts = smallCollection
                .GroupBy(item => item.ItemID)
                .Select(group => new
                {
                    ItemID = group.Key,
                    TotalQuantity = group.Sum(item => item.Quantity),
                    ItemName = group.First().ItemName
                })
                .ToList();

            // 打印每个 ItemID 的总数量
            foreach (var item in itemCounts)
            {
                Echo_info($"ItemID: {item.ItemID}, 名称：{item.ItemName}, Total Quantity: {item.TotalQuantity}");
            }
        }
        //更新读取包裹物品字典
        public void WR_zd_info() 
        {
            /*
            foreach (var itemInfo in Zd_iteminfo.Values)
            {
                itemInfo.Quantity = 0; // 将每个 ItemInfo 的 Quantity 置为 0
            }
            */
            Zd_iteminfo = new Dictionary<int, ItemInfo>();
            // 使用 Zip 方法同时遍历两个集合
            var combinedList = Ini_item_data_id_arr.Cast<long>().Zip(Ini_item_data_name_arr.Cast<string>(), (item1, item2) => (item1, item2));
            // 遍历合并后的集合
            foreach (var (item1, item2) in combinedList)
            {
                Zd_iteminfo[(int)item1] = new ItemInfo(item1, item2, 0);
            }
            // 遍历包裹格子
            for (int y = 0; y < 10; y++) // 10行
            {
                for (int x = 0; x < 8; x++) // 8列
                {
                    int gridNumber = y * 8 + x; // 计算格子编号，从0开始
                    int dynamicAddress = gridNumber * 0x60; // 动态的地址部分，根据列数变化

                    // 读取内存中的值
                    long itemID = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(dynamicAddress).ToString("X")}", 4);
                    // 读取数量
                    long itemCount = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(dynamicAddress + 0x1C).ToString("X")}", 6);
                    // 读取物品名称
                    string itemName = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(dynamicAddress + 0x4).ToString("X")}", 0, 0);

                    // 检查当前的 itemID 是否在 Ini_item_bar_id_arr 中
                    if (!Ini_item_data_id_arr.Contains(itemID))
                    {
                        continue; // 如果不在，跳过当前格子
                    }

                    // 处理读取到的值，可以将它存储起来或进行其他操作
                    // Echo_info($"格子 {itemID}: {itemCount} 个, 物品名称: {itemName}");

                    // 如果字典中没有该物品，则添加到字典，并初始化 Quantity 为 0
                    if (!Zd_iteminfo.ContainsKey((int)itemID))
                    {
                        Zd_iteminfo[(int)itemID] = new ItemInfo(itemID, itemName, 0);
                    }

                    // 累加数量
                    Zd_iteminfo[(int)itemID].Quantity += itemCount;
                }
            }
        }
        //刷新字典到表格        
        private void RefreshData(object state)
        {
            // 在此处更新数据源
            WR_zd_info();

            // 更新DataGridView的数据源
            if (InvokeRequired)
            {
                // 如果在不同的线程上调用，通过Invoke方法在UI线程上执行
                Invoke(new Action(() => dataGridView1.DataSource = new List<ItemInfo>(Zd_iteminfo.Values)));
            }
            else
            {
                // 如果已经在UI线程上，直接执行
                dataGridView1.DataSource = new List<ItemInfo>(Zd_iteminfo.Values);
            }            
        }        
        //人物状态设置
        public class PlayerState
        {
            public dmsoft dm;
            public Form1 Form1;
            public PlayerState()
            {
                dm = new dmsoft();
                Form1 = new Form1();
            }
            public enum MapState
            {
                Unknown,
                InCity,
                InField,
                InDungeon
                // 添加更多地图状态...
            }

            public enum AttackState
            {
                Unknown,
                Stand,
                Moving,
                Attacking
                // 添加更多攻击状态...
            }


            

            public MapState CurrentMapState { get; private set; }
            public AttackState CurrentAttackState { get; private set; }

            public readonly long[] cityMapIds = { 0, 27 };
            public readonly long[] dungeonMapIds = { 17, 18, 19, 51, 52, 53, 54, 60, 61, 62, 63 };
            // 添加更多地图和攻击状态的映射关系...

            public void UpdateStates(long playerMapId, long playerAttackState)
            {
                UpdateMapState(playerMapId);
                UpdateAttackState(playerAttackState);
            }

            public void UpdateMapState(long playerMapId)
            {
                if (cityMapIds.Contains(playerMapId))
                {
                    CurrentMapState = MapState.InCity;
                }
                else if (dungeonMapIds.Contains(playerMapId))
                {
                    CurrentMapState = MapState.InDungeon;
                }
                else
                {
                    CurrentMapState = MapState.Unknown;
                }
            }

            public readonly long[] standIds = { 1 };
            public readonly long[] moveIds = { 2, 3 };
            public readonly long[] attackIds = { 4, 5, 11, 12 };

            public void UpdateAttackState(long playerAttackState)
            {
                if (standIds.Contains(playerAttackState))
                {
                    CurrentAttackState = AttackState.Stand;
                }
                else if (moveIds.Contains(playerAttackState))
                {
                    CurrentAttackState = AttackState.Moving;
                }
                else if (attackIds.Contains(playerAttackState))
                {
                    CurrentAttackState = AttackState.Attacking;
                }
                // 可以根据需要继续添加更多状态的判断
            }
        }
        //计算两坐标之间距离
        public static long Js_jl(long x1, long y1, long x2, long y2)
        {
            double squaredDistance = Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2);
            return (long)Math.Sqrt(squaredDistance);
        }
        //怪物特征码字典
        public static Dictionary<string, string> entityFeatureCodes = new Dictionary<string, string>
        {            
            { "沃玛教主", "0071E2C8" },
            { "祖玛教主", "0071E2C8" },
            { "虹魔蝎卫", "0071E2C8" },
            { "暗之虹魔教主", "0071E2C8" },            
            { "触龙神", "006E0F20" },
            { "暗之触龙神", "006E0F20" },          
            { "暗之弓箭手", "0071AE40" },
            { "赤月恶魔", "00728CEC" },
            { "魔龙树妖", "00728CEC" },
            { "魔龙教主", "00702218" },
            { "破天教主", "00702218" },
            { "火龙神", "006EAE84" },
            { "月氏镰神将", "006E0E98" },
            { "月氏飞行神像", "006F18C8" },
            { "月氏战魔", "0070DB10" },
            { "怨恶巨兽", "006E2A08" },
            { "寒冰魔王", "006F6860" },
            { "蛮荒恶魔", "006E0E98" },
            { "白野猪","006E0E98"}
            
             
            // 添加更多实体...
        };
        public static string Get_fly_code(string entityName)
        {
            // 查找实体并返回特征码
            return entityFeatureCodes.TryGetValue(entityName, out string featureCode) ? featureCode : "未找到特征码";
        }
        //特征码改造
        public static string SplitAndReverse(string input, int groupSize)
        {
            // 检查输入长度是否符合要求
            if (input.Length % groupSize != 0)
            {
                throw new ArgumentException("Input length must be a multiple of group size");
            }

            // 将字符串分割成指定大小的组
            string[] groups = new string[input.Length / groupSize];
            for (int i = 0; i < input.Length; i += groupSize)
            {
                groups[i / groupSize] = input.Substring(i, groupSize);
            }

            // 倒序整个分割后的字符串数组
            Array.Reverse(groups);

            // 用空格拼接每个组
            string result = string.Join(" ", groups);

            return result;
        }
        //异步执行
        public async Task StartLongRunningTask()
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // 在异步任务中执行长时间运行的操作
                await Task.Run(() =>
                {
                fly_and_hit1(lockObject);
                }, cancellationTokenSource.Token);

                Echo_info("飞怪完成...");
            }
            catch (OperationCanceledException)
            {
                Echo_info("Status: Task canceled");
            }
        }

        //不打断CT飞怪
        public void fly_and_hit1(object state)
        {
            bool lockAcquired = false;            
            try
            {
                // 检查是否有回调正在执行
                if (Monitor.TryEnter(lockObject))
                {
                    lockAcquired = true;
                    if (!isCallbackExecuting)
                    {
                        isCallbackExecuting = true;

                        string result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", p_fly_code, 4, 1, 1);//当前地图怪物地址合集字符串            
                        if (result.Length > 0)
                        {
                            string[] results = result.Split('|');
                            foreach (var s in results)
                            {
                                int times = 0;
                                int addr_name = Convert.ToInt32(s, 16) + 0x6C;
                                int addr_hp = Convert.ToInt32(s, 16) + 0xA8;
                                if (m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0) == p_fly_name)
                                {
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_hp.ToString("X"), 4) != 0)//血量不等于0
                                    {
                                        int addr_x = Convert.ToInt32(s, 16) + 0x4c;
                                        int addr_y = Convert.ToInt32(s, 16) + 0x50;
                                        long monster_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_x.ToString("X"), 4);
                                        long monster_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_y.ToString("X"), 4);
                                        m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 0, Convert.ToInt32(s, 16));
                                        m_dm.KeyPressChar("enter");
                                        Thread.Sleep(500);
                                        m_dm.SendString(Convert.ToInt32(p_hwnd_game), $"@move {monster_x},{monster_y + 1}");
                                        Thread.Sleep(50);
                                        m_dm.KeyPressChar("enter");
                                        m_dm.MoveTo(45, 686);
                                        m_dm.LeftClick();//点击血量区域 用于关闭聊天窗口
                                        m_dm.MoveTo(890, 10);//用于激活自动技能
                                        Thread.Sleep(100);
                                        Echo_info($"找到怪物[{p_fly_name}]坐标[{monster_x},{monster_y}]血量[{m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_hp.ToString("X"), 4)}]");
                                        Echo_info($"当前传送戒指耐久[{m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+4B6", 5)}]");
                                        Thread.Sleep(1000);
                                        while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_hp.ToString("X"), 4) != 0)
                                        {
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 4) != Convert.ToInt32(s, 16))
                                            {
                                                break;
                                            }
                                            if (times > 55)
                                            {
                                                break;
                                            }
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 1)//如果人物在站立状态则+1
                                            {
                                                times++;
                                            }
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 2)//如果人物在走动状态则+1
                                            {
                                                times++;
                                            }
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 3)//如果人物在跑动状态则+1
                                            {
                                                times++;
                                            }
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 4)//如果人物在攻击状态则清0
                                            {
                                                times = 0;
                                            }
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 5)//如果人物在攻击状态则清0
                                            {
                                                times = 0;
                                            }
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+74]+4", 6) == 1)//如果复活窗口打开
                                            {
                                                Echo_info("人物死亡...");
                                                //need_fly_and_hit = false;
                                                //Bn_fly_and_hit.Invoke(new Action(() => { this.Bn_fly_and_hit_Click(null, null); }));
                                                Thread.Sleep(2000);
                                                m_dm.MoveTo(576, 269);
                                                m_dm.LeftClick();
                                                Thread.Sleep(2000);
                                                Fix_item();//修理
                                                Thread.Sleep(2000);
                                                Fz_fly(7);//辅助飞7号位（蛮荒）
                                                Thread.Sleep(8000);
                                                break;
                                            }
                                            Thread.Sleep(200);
                                        }
                                        Thread.Sleep(800);
                                        m_dm.KeyPressChar("space");
                                        Thread.Sleep(200);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Echo_info("没有怪物");
                            Thread.Sleep(1000);
                        }
                    }
                }
                
            }
            finally
            {
                if (lockAcquired)
                {
                    isCallbackExecuting = false;
                    Monitor.Exit(lockObject);
                }
            }



        }
        public void fly_and_hit() 
        {
            /*
            if (true)//如果不在地图
            {
                Goto_map(fly_map_name);
                Thread.Sleep(2000);
            }
            */
            while (need_fly_and_hit == true)
            {
                //绑定窗口
                if (!Bing_UnBing_change.Checked)
                {
                    Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
                }
                //下坐骑
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+214", 6) != 0)
                {
                    m_dm.KeyPressChar("a");
                }
                //真假CT
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 6) == 0)//如果挂机状态为0
                {
                    if (CT_type == true)
                    {
                        m_dm.KeyDownChar("ctrl");
                        Thread.Sleep(200);
                        m_dm.KeyDownChar("t");
                        Thread.Sleep(200);
                        m_dm.KeyUpChar("t");
                        Thread.Sleep(200);
                        m_dm.KeyUpChar("ctrl");
                    }
                    else
                    {
                        m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 2, 1);//开启假ct
                    }
                }
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+4B6", 5)<200)//传送戒指耐久少于500
                {
                    while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) != 1)
                    {
                        //上坐骑
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+214", 6) == 0)
                        {
                            m_dm.KeyPressChar("a");
                        }
                        Thread.Sleep(200);
                        //下坐骑
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+214", 6) != 0)
                        {
                            m_dm.KeyPressChar("a");
                        }
                        m_dm.MoveTo(453, 322);
                        m_dm.RightClick();
                        Thread.Sleep(1000);
                    }
                    while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+4B6", 5) < 1000)
                    {
                        Fz_fly(1);//辅助飞1号位（庄园）
                        Thread.Sleep(2000);
                        Zy_to_bq();//庄园去比奇
                        Thread.Sleep(2000);
                        Fix_item();//修理
                        Thread.Sleep(2000);
                        Buy_item();//购买
                        Thread.Sleep(2000);
                        //Fz_fly(7);//辅助飞7号位（蛮荒）
                        Goto_map(fly_map_name);
                        Thread.Sleep(8000);
                    }
                    continue;
                }
                string result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", p_fly_code, 4, 1, 1);//当前地图怪物地址合集字符串            
                if (result.Length > 0)
                {
                    string[] results = result.Split('|');
                    foreach (var s in results)
                    {
                        int times = 0;
                        int addr_name = Convert.ToInt32(s, 16) + 0x6C;
                        int addr_hp = Convert.ToInt32(s, 16) + 0xA8;
                        if (m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0) == p_fly_name)
                        {
                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_hp.ToString("X"), 4) != 0)//血量不等于0
                            {
                                int addr_x = Convert.ToInt32(s, 16) + 0x4c;
                                int addr_y = Convert.ToInt32(s, 16) + 0x50;
                                long monster_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_x.ToString("X"), 4);
                                long monster_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_y.ToString("X"), 4);
                                m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 0, Convert.ToInt32(s, 16));
                                m_dm.KeyPressChar("enter");
                                Thread.Sleep(500);
                                m_dm.SendString(Convert.ToInt32(p_hwnd_game), $"@move {monster_x},{monster_y}");
                                Thread.Sleep(50);
                                m_dm.KeyPressChar("enter");
                                m_dm.MoveTo(45, 686);
                                m_dm.LeftClick();//点击血量区域 用于关闭聊天窗口
                                m_dm.MoveTo(890, 10);//用于激活自动技能
                                Thread.Sleep(100);
                                if (m_dm.ReadString(Convert.ToInt32(p_hwnd_game), "[[[[<BackMir.exe>+00892364]+FC]+6C]+D0]+0", 0, 0) == "传送提示：野外禁止传送到非队友身边")
                                {
                                    continue;
                                }
                                
                                Echo_info($"找到怪物[{p_fly_name}]坐标[{monster_x},{monster_y}]血量[{m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_hp.ToString("X"), 4)}]");
                                Echo_info($"当前传送戒指耐久[{m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+4B6", 5)}]");
                                Thread.Sleep(1000);
                                while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_hp.ToString("X"), 4) != 0)
                                {
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 4) != Convert.ToInt32(s, 16))
                                    {
                                        break;
                                    }
                                    if (times > 55)
                                    {
                                        break;
                                    }
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 1)//如果人物在站立状态则+1
                                    {
                                        times++;
                                    }
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 2)//如果人物在走动状态则+1
                                    {
                                        times++;
                                    }
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 3)//如果人物在跑动状态则+1
                                    {
                                        times++;
                                    }
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 4)//如果人物在攻击状态则清0
                                    {
                                        times = 0;
                                    }
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 5)//如果人物在攻击状态则清0
                                    {
                                        times = 0;
                                    }
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+74]+4", 6) == 1)//如果复活窗口打开
                                    {
                                        Echo_info("人物死亡...");
                                        //need_fly_and_hit = false;
                                        //Bn_fly_and_hit.Invoke(new Action(() => { this.Bn_fly_and_hit_Click(null, null); }));
                                        Thread.Sleep(2000);
                                        m_dm.MoveTo(576, 269);
                                        m_dm.LeftClick();
                                        Thread.Sleep(2000);
                                        Fix_item();//修理
                                        Thread.Sleep(2000);
                                        Fz_fly(7);//辅助飞7号位（蛮荒）
                                        Thread.Sleep(8000);
                                        break;
                                    }
                                    Thread.Sleep(200);
                                }
                                if (CT_type == false)
                                {
                                    Thread.Sleep(800);
                                    m_dm.KeyPressChar("space");
                                    Thread.Sleep(200);
                                }
                                break;
                            }
                        }
                    }
                }
                else 
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(1);
            }
            
        }

        //副本挂机
        public void Auto_run_fb()
        {
            int i = 1;
            long prevExperience = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[<BackMir.exe>+00389260]+B8", 4); // 上一次的经验值
            long totalExperienceGain = 0; // 总的经验增长值
            player_state = "脚本结束";
            //绑定窗口                               
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
            }
            //如果快捷栏未初始化则初始化一下
            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+85C]+1E0", 4) !=402)
            {
                Bn_start_auto_fb.Invoke(new Action(() => { this.bn_ini_item_Click(null, null); }));
                //Thread.Sleep(5000);
            }
            else if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+85C]+180", 4) != 404)
            {
                Bn_start_auto_fb.Invoke(new Action(() => { this.bn_ini_item_Click(null, null); }));
            }
            //循环执行脚本
            while (need_auto_run_fb == true)
            {
                if (player_state == "脚本结束")
                {
                    Echo_info($"第{i}次副本开始...");
                    Fix_item();
                    Sorting_item();//整理物品
                    player_state = "脚本开始";
                    Thread.Sleep(1000);
                    Auto_run_fb_scrip();
                    long currentExperience = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[<BackMir.exe>+00389260]+B8", 4); // 模拟读取当前的经验值                                                                                                                            
                    long experienceGain = Math.Max((int)currentExperience - (int)prevExperience, 0);// 计算经验增长                                                                                                   
                    totalExperienceGain += experienceGain; // 累加总的经验增长值
                    prevExperience = currentExperience;// 更新上一次的经验值
                    i++;
                }
                Thread.Sleep(1000);
            }
            Echo_info("自动副本已停止...");
            Echo_info($"累计完成副本{i}次，获取经验{totalExperienceGain.ToString("N0")}左右...");
        }
        //从任何地方到任何地方
        public int Goto_map(string msg) 
        {
            switch (msg)
            {
                case "比奇":
                    Fz_fly(1);
                    Thread.Sleep(2000);
                    Zy_to_bq();
                    return 1;
                case "祖玛一层":
                    Fz_fly(4);
                    Thread.Sleep(2000);
                    m_dm.KeyPressChar("enter");
                    Thread.Sleep(500);
                    m_dm.SendString(Convert.ToInt32(p_hwnd_game), $"@move 21,87");
                    Thread.Sleep(50);
                    m_dm.KeyPressChar("enter");
                    Thread.Sleep(500);
                    m_dm.MoveTo(417, 272);
                    Thread.Sleep(500);
                    m_dm.LeftClick();//点击用于过图
                    Thread.Sleep(100);
                    return 1;
                case "石墓":
                    Fz_fly(4);
                    Thread.Sleep(2000);
                    m_dm.KeyPressChar("enter");
                    Thread.Sleep(500);
                    m_dm.SendString(Convert.ToInt32(p_hwnd_game), $"@move 36,60");
                    Thread.Sleep(50);
                    m_dm.KeyPressChar("enter");
                    Thread.Sleep(500);
                    m_dm.MoveTo(417, 272);
                    Thread.Sleep(500);
                    m_dm.LeftClick();//点击用于过图
                    Thread.Sleep(100);
                    return 1;
                case "蛮荒废墟":
                    Fz_fly(7);
                    return 1;
                case "死亡深渊":
                    Fz_fly(4);
                    Thread.Sleep(2000);
                    m_dm.MoveTo(509, 395);
                    Thread.Sleep(500);
                    m_dm.LeftClick();//点击用于过图
                    Thread.Sleep(500);
                    m_dm.LeftClick();//点击用于过图
                    return 1;

                default:
                    Echo_info("暂不支持！");
                    return 0;
            }
        }
        //使用辅助传送
        public void Fz_fly(int type) 
        {
            Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), false);
            Thread.Sleep(2000);
            long hwnd = m_dm.FindWindow("", "311群服辅助");
            m_dm.BindWindowEx((int)hwnd, "dx2", "windows3", "normal", "", 0);
            m_dm.MoveTo(265, 305);
            m_dm.LeftClick();
            Thread.Sleep(500);
            hwnd = m_dm.FindWindow("", "3服地图直传");
            m_dm.BindWindowEx((int)hwnd, "dx2", "windows3", "normal", "", 0);
            switch (type) 
            { 
                case 1:
                    m_dm.MoveTo(55, 55);
                    m_dm.LeftClick();
                    break;
                case 2:
                    m_dm.MoveTo(170, 55);
                    m_dm.LeftClick();
                    break;
                case 3:
                    m_dm.MoveTo(270, 55);
                    m_dm.LeftClick();
                    break;
                case 4:
                    m_dm.MoveTo(55, 110);
                    m_dm.LeftClick();
                    break;              
                case 5:
                    m_dm.MoveTo(170, 110);
                    m_dm.LeftClick();
                    break;
                case 6:
                    m_dm.MoveTo(270, 110);
                    m_dm.LeftClick();
                    break;
                case 7:
                    m_dm.MoveTo(55, 160);
                    m_dm.LeftClick();
                    break;
                case 8:
                    m_dm.MoveTo(170, 160);
                    m_dm.LeftClick();
                    break;
                case 9:
                    m_dm.MoveTo(270, 160);
                    m_dm.LeftClick();
                    break;
                default:
                    m_dm.MoveTo(170, 210);
                    m_dm.LeftClick();
                    break;
            }
            Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
        }
        //从庄园去比奇
        public void Zy_to_bq()
        {
            //绑定窗口                               
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
            }
            m_dm.MoveTo(321, 177);
            m_dm.LeftClick();
            Thread.Sleep(1000);
            m_dm.MoveTo(60, 295);
            m_dm.LeftClick();
        }
        //人物修理脚本
        public void Fix_item()
        {
            //绑定窗口                               
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
            }
            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+884", 5) != 0)//在比奇
            {
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+884", 5) != 27)//且不在雪原
                {
                    m_dm.KeyPressChar("6");//按回城
                }
            }
            Thread.Sleep(1000);
            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+884", 5) == 0)//在比奇
            {
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+4", 6) == 0)
                {
                    //打开地图窗口
                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+FC]+104", 6) == 1) //聊天输入框是否打开
                    {
                        m_dm.MoveTo(45, 686);
                        m_dm.LeftClick();//点击血量区域 用于关闭聊天窗口
                    }
                    m_dm.KeyPressChar("tab");
                    Thread.Sleep(500);
                }
                move_to_npc://goto 语句标记点
                //使用传送戒指的逻辑
                m_dm.KeyPressChar("enter");
                Thread.Sleep(500);
                m_dm.SendString(Convert.ToInt32(p_hwnd_game), "@move 325,283");
                m_dm.KeyPressChar("enter");
                m_dm.MoveTo(45, 686);
                m_dm.LeftClick();//点击血量区域 用于关闭聊天窗口

                //不使用传送戒指的逻辑
                /*
                long map_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+8", 0);
                long map_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+C", 0);
                m_dm.MoveTo((int)map_x + 243, (int)map_y + 249);
                Thread.Sleep(1000);
                m_dm.LeftClick();
                Thread.Sleep(500);
                m_dm.MoveTo(45, 686);//移动到血量区域
                */
                m_dm.KeyPressChar("tab");
                Thread.Sleep(2000);//走到目的地的时间                
                m_dm.MoveTo(511, 566);
                m_dm.LeftClick();//点击npc打开对话窗口
                Thread.Sleep(500);
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+4", 6) == 1)//npc对话窗口是否打开
                {
                    long x1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+8", 0);
                    long y1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+c", 0);
                    long x2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+10", 0);
                    long y2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+14", 0);
                    m_dm.FindStr((int)x1, (int)y1, (int)x2, (int)y2, "修|理", "ffff00-303030", 0.8, out int intX, out int intY);
                    m_dm.MoveTo(intX + 5, intY + 5);
                    m_dm.LeftClick();
                    Thread.Sleep(100);
                    m_dm.MoveTo(45, 686);//移动鼠标  别挡住选项
                    Thread.Sleep(500);
                    m_dm.FindStr((int)x1, (int)y1, (int)x2, (int)y2, "全|部", "ffff00-303030", 0.8, out intX, out intY);
                    m_dm.MoveTo(intX + 5, intY + 5);
                    m_dm.LeftClick();
                    Thread.Sleep(500);
                    m_dm.KeyPressChar("tab");
                    Thread.Sleep(500);
                    m_dm.LeftClick();
                    Thread.Sleep(500);
                    m_dm.KeyPressChar("tab");
                    m_dm.MoveTo((int)x1 + 262, (int)y1 + 47);
                    m_dm.LeftClick();//关闭窗口
                }
                else
                {
                    Thread.Sleep(30000);//暂停30秒确保传送戒指cd                    
                    goto move_to_npc;
                }
                //开始购物逻辑
                Buy_item();


                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+884", 5) == 27)//在雪域
                {
                    //修理逻辑                
                }

            }
        }
        //人物购买物品
        public void Buy_item() 
        {
            Thread.Sleep(500);
            Array itemIDsSnapshot = Zd_iteminfo.Keys.ToArray();
            foreach (int item in itemIDsSnapshot)
            {
                switch (item)
                {
                    case 1231:
                        if (Zd_iteminfo[item].Quantity < 255)
                        {
                            m_dm.MoveTo(990, 670);
                            m_dm.LeftClick();
                            Thread.Sleep(500);
                            int a = 0;
                            while (Zd_iteminfo[item].Quantity < 255)
                            {
                                m_dm.MoveTo(305, 303);
                                m_dm.LeftClick();
                                Thread.Sleep(500);
                                m_dm.MoveTo(454, 394);
                                m_dm.LeftClick();
                                Thread.Sleep(500);
                                if (a > 10)
                                {
                                    break;//兜底别死循环！痛~
                                }
                                a++;
                            }
                            m_dm.MoveTo(990, 670);
                            m_dm.LeftClick();
                            Thread.Sleep(500);
                        }                        
                        break;
                    case 16:
                    case 312:
                    case 402:
                    case 404:
                        break;
                    default:
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+B4]+4", 6) != 1)//商店窗口没有打开
                        {
                            m_dm.MoveTo(511, 566);
                            m_dm.LeftClick();//点击npc打开对话窗口
                            Thread.Sleep(500);
                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+4", 6) == 1)//npc对话窗口是否打开
                            {
                                long x1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+8", 0);
                                long y1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+c", 0);
                                long x2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+10", 0);
                                long y2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+14", 0);
                                m_dm.FindStr((int)x1, (int)y1, (int)x2, (int)y2, "要|买", "ffff00-303030", 0.8, out int intX, out int intY);
                                m_dm.MoveTo(intX + 5, intY + 5);
                                m_dm.LeftClick();
                                Thread.Sleep(100);//移动鼠标  别挡住选项
                                m_dm.MoveTo(45, 686);//移动鼠标  别挡住选项
                                Thread.Sleep(500);
                            }
                        }
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+B4]+4", 6) == 1)//商店窗口已经打开
                        {
                            for (int y = 0; y < 2; y++) //读取第一第二行
                            {
                                for (int x = 0; x < 6; x++)//读取第一到六列
                                {
                                    int gridNumber = y * 6 + x; // 计算格子编号，从0开始
                                    int dynamicAddress = gridNumber * 0x60 + 0xE8; // 动态的地址部分，根据列数变化                                        
                                    long item_id_in_shop = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00892364]+B4]+{(dynamicAddress).ToString("X")}", 4);// 读取内存中的值
                                    if (item_id_in_shop == item)
                                    {
                                        int mouse_x = 35 + 41 * x;
                                        int mouse_y = 97 + 41 * y;
                                        int a = 0;
                                        while (Zd_iteminfo[(int)item].Quantity < 255)
                                        {
                                            //Echo_info($"{Zd_iteminfo[(int)item].Quantity}");
                                            m_dm.MoveTo(mouse_x, mouse_y);
                                            m_dm.LeftClick();
                                            Thread.Sleep(500);
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+CC]+4", 6) == 1)//交易确认窗口已经打开
                                            {
                                                m_dm.MoveTo(454, 394);
                                                m_dm.LeftClick();
                                                Thread.Sleep(500);
                                            }
                                            if (a > 5)
                                            {
                                                break;//兜底别死循环！痛~
                                            }
                                            a++;
                                        }
                                        break;
                                    }
                                    //Thread.Sleep(100);
                                }
                            }
                        }
                        
                        
                        break;
                }
                Thread.Sleep (500);
            }
            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+B4]+4", 6) == 1)//商店窗口打开这
            {
                m_dm.MoveTo(247, 45);//关闭商店
                m_dm.LeftClick();
                Thread.Sleep(500);
                m_dm.MoveTo(45, 686);//移动到血条
                m_dm.KeyPressChar("b");
            }
        }
        //副本脚本
        public void Auto_run_fb_scrip() 
        {
            string[] fb_name = { "巨|人|巷|玲|珑|仙|岛|幽|冥|坛", "幽|冥|坛", "玲|珑|仙|岛", "巨|人|巷", "伏魔令赤|赤", "伏魔令狐", "伏魔令月|月", "伏魔令雪|雪", "伏魔令阎|阎", "伏魔令破|破" };
            long[] fb_map_id = { 17,18,19,51,52,53,54,60,61,62,63};
            //绑定窗口                               
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
            }
            if (need_auto_run_fb == true && player_state == "脚本开始")
            {
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+884", 5) != 39)//人物不在皇宫逻辑
                {
                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+884", 5) != 0)
                    {
                        m_dm.KeyPressChar("6");//不在比奇则按回城
                        Thread.Sleep(500);
                    }
                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+4C", 4) != 325)
                    {
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+4", 6) == 0)
                        {
                            //打开地图窗口
                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+FC]+104", 6) == 1) //聊天输入框是否打开
                            {
                                m_dm.MoveTo(45, 686);
                                m_dm.LeftClick();//点击血量区域 用于关闭聊天窗口
                            }
                            m_dm.KeyPressChar("tab");
                            Thread.Sleep(500);
                        }
                        //使用传送戒指的逻辑
                        
                        m_dm.KeyPressChar("enter");
                        Thread.Sleep(200);
                        m_dm.SendString(Convert.ToInt32(p_hwnd_game), "@move 325,283");
                        Thread.Sleep(200);
                        m_dm.KeyPressChar("enter");
                        m_dm.MoveTo(45, 686);
                        m_dm.LeftClick();//点击血量区域 用于关闭聊天窗口

                        //不使用传送戒指的逻辑
                        /*
                        long map_x = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+8", 0);
                        long map_y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+C", 0);
                        m_dm.MoveTo((int)map_x + 243, (int)map_y + 249);
                        Thread.Sleep(1000);
                        m_dm.LeftClick();
                        Thread.Sleep(500);                        
                        m_dm.MoveTo(45, 686);//移动到血量区域
                        */
                        m_dm.KeyPressChar("tab");
                        Thread.Sleep(2000);
                        
                    }
                    m_dm.MoveTo(656, 21);//点击试炼长者
                    m_dm.LeftClick();
                    Thread.Sleep(500);
                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+4", 6) == 1)//npc对话窗口是否打开
                    {
                        long x1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+8", 0);
                        long y1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+c", 0);
                        long x2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+10", 0);
                        long y2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+14", 0);
                        m_dm.FindStr((int)x1, (int)y1, (int)x2, (int)y2, "前|往|皇|宫", "ffff00-303030", 0.8, out int intX, out int intY);
                        m_dm.MoveTo(intX + 5, intY + 5);
                        m_dm.LeftClick();
                        Thread.Sleep(2000);
                        player_state = "在皇宫";
                    }
                }
                else 
                {
                    //人物已经在皇宫的逻辑
                    player_state = "在皇宫";
                }                
            }
            if (need_auto_run_fb == true && player_state == "在皇宫")
            {
                m_dm.MoveTo(696, 209);
                Thread.Sleep(500);
                m_dm.RightDown();
                Thread.Sleep(6000);//走到桥边
                m_dm.RightUp();
                Thread.Sleep(500);
                m_dm.MoveTo(220, 245);
                m_dm.LeftClick();//点击副本NPC
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+F8]+F8", 6) == 0) 
                {
                    m_dm.KeyPressChar("enter");
                    Thread.Sleep(200);
                    m_dm.SendString(Convert.ToInt32(p_hwnd_game), "@createteam");
                    Thread.Sleep(200);
                    m_dm.KeyPressChar("enter");
                }
                Thread.Sleep(500);
                m_dm.MoveTo(65, 325);
                m_dm.LeftClick();//点击伏魔选项
                Thread.Sleep(500);
                m_dm.MoveTo(45, 686);//移动到血量区域
                Thread.Sleep(1000);
                long x1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+8", 0);
                long y1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+c", 0);
                long x2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+10", 0);
                long y2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+14", 0);
                if(m_dm.FindStr((int)x1, (int)y1, (int)x2, (int)y2, fb_name[p_fb_select], "ffff00-303030", 0.8, out int intX, out int intY) == -1)
                {
                    Echo_info($"没有门票{fb_name[p_fb_select]}!");                    
                    Bn_start_auto_fb.Invoke(new Action(() =>{this.Bn_start_auto_fb_Click(null,null);}));
                }
                m_dm.MoveTo(intX + 5, intY + 5);
                m_dm.LeftClick();                
                Thread.Sleep(2000);//大延迟传送到副本
                int a = Array.IndexOf(fb_map_id, m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+884", 5));
                if (a == -1)//人物不在副本逻辑
                {
                    Echo_info("传送副本出错");
                    Thread.Sleep(20000);//大延迟等待传送cd
                    player_state = "脚本结束";
                }
                else 
                {
                    player_state = "在副本";
                }
            }
            if (need_auto_run_fb == true && player_state == "在副本")
            {
                m_dm.FindStr(0, 0, 1024, 768, "司|徒|秋|言", "5483f1-303030", 0.8, out int intX,out int  intY);
                m_dm.MoveTo(intX + 5, intY + 5);
                m_dm.LeftClick();
                Thread.Sleep(1000);
                long x1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+8", 0);
                long y1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+c", 0);
                long x2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+10", 0);
                long y2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+64]+14", 0);
                m_dm.FindStr((int)x1, (int)y1, (int)x2, (int)y2, "开|始|挑|战", "ffff00-303030", 0.8, out intX, out intY);
                m_dm.MoveTo(intX + 5, intY + 5);
                m_dm.LeftClick();
                Thread.Sleep(200);
                m_dm.MoveTo(45, 686);
                Thread.Sleep(1000);
                m_dm.FindStr((int)x1, (int)y1, (int)x2, (int)y2, "关|闭", "ffff00-303030", 0.8, out intX, out intY);
                m_dm.MoveTo(intX + 5, intY + 5);
                m_dm.LeftClick();
                Thread.Sleep(2000);                
                player_state = "开始打怪";
                Thread.Sleep(1000);
            }
            if (need_auto_run_fb == true && player_state == "开始打怪") 
            {
                need_auto_fight = true;
                Auto_ctrl_t();
            }
        }
        //自动打怪CT版
        public void Auto_ctrl_t()
        {
            long[] state = { 1, 2, 3 ,4 ,5};
            //绑定窗口                               
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
            }
            //下坐骑
            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+214", 6) != 0)
            {
                m_dm.KeyPressChar("a");
            }
            //循环打怪
            m_dm.MoveTo(45, 686);//移动到血量区域 用于防止卡怪
            need_auto_fight = true;
            int times = 0;           
            m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 2, 1);//开启CT
            while (need_auto_fight == true)
            {
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 6) == 0)
                {
                    m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 2, 1);//开启CT
                }
                if (m_dm.ReadString(Convert.ToInt32(p_hwnd_game), "[[[[<BackMir.exe>+00892364]+FC]+6C]+D0]+0", 0, 0) == "[系统]打开藏宝盒领取奖励")
                {
                    Echo_info("[聊天框]宝箱刷新");
                    string result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "98 0E 6E 00", 4, 1, 1);//当前地图怪物地址合集字符串            
                    if (result.Length > 0)
                    {
                        string[] results = result.Split('|');
                        foreach (var s in results)
                        {
                            int addr_name = Convert.ToInt32(s, 16) + 0x6C;
                            int addr_hp = Convert.ToInt32(s, 16) + 0xA8;
                            if (m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr_name.ToString("X"), 0, 0) == "藏宝盒")
                            {
                                while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_hp.ToString("X"), 4) != 0)//血量不等于0
                                {
                                    if (times > 25)
                                    {
                                        m_dm.KeyPressChar("5");
                                        times = 0;
                                    }
                                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMbir.exe>+00389260]+60", 4) == 1)//如果人物在站立状态则+1
                                    {
                                        times++;
                                    }
                                    Thread.Sleep(50);
                                }
                                Echo_info("[聊天框]副本结束");
                                m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 2, 0);//关CT
                                m_dm.KeyPressChar("a");
                                Thread.Sleep(200);
                                m_dm.KeyPressChar("a");
                                Thread.Sleep(1000);//等待屏蔽生效
                                m_dm.KeyPressChar("space");
                                Thread.Sleep(200);
                                need_auto_fight = false;
                                break;
                            }
                        }
                    }
                }
                if (m_dm.ReadString(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00389260]+880]+6c", 0, 0) == "藏宝盒")
                {
                    Echo_info("[目标]宝箱刷新");
                    while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 4) != 0)
                    {
                        if (times > 25)
                        {
                            m_dm.KeyPressChar("5");
                            times = 0;
                        }
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMbir.exe>+00389260]+60", 4) == 1)//如果人物在站立状态则+1
                        {
                            times++;
                        }
                        Thread.Sleep(50);
                    }
                    Echo_info("[目标]副本结束");
                    m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 2, 0);//关CT
                    m_dm.KeyPressChar("a");
                    Thread.Sleep(200);
                    m_dm.KeyPressChar("a");
                    Thread.Sleep(1000);//等待屏蔽生效
                    m_dm.KeyPressChar("space");
                    Thread.Sleep(200);
                    need_auto_fight = false;
                    break;
                }
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+A3C", 4) != 0)
                {
                    m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+A3C", 0, 0);//卡技能目标了  清空一下
                }
                if (times > 25)
                {                    
                    m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 2, 0);
                    Thread.Sleep(1000);
                    m_dm.KeyPressChar("5");
                    m_dm.KeyPressChar("a");
                    Thread.Sleep(1000);
                    m_dm.KeyPressChar("a");
                    Thread.Sleep(1000);
                    m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 2, 1);
                    times = 0;
                }
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 1)//如果人物在站立状态则+1
                {
                    times++;
                }
                while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 4) != 0)
                { 
                    Thread.Sleep(100); 
                }
                if (Array.IndexOf(state,m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4)) !=-1)//如果人物站立、移动就拾取
                {
                    Thread.Sleep(500);//等屏蔽生效
                    m_dm.KeyPressChar("space");
                }
                Thread.Sleep(50);
            }
            player_state = "脚本结束";
        }
        //自动打怪识别血条版
        public void Auto_fight() 
        {
            //绑定窗口                               
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
            }
            //循环打怪
            int times = 0;
            while (need_auto_fight == true)
            {
                long ret = Find_monster_on_scream();                
                if (ret == 0)
                {
                    //Echo_info("没有怪物");
                    m_dm.KeyPressChar("tab");
                    long x1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+8", 0);
                    long y1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+c", 0);
                    long x2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+10", 0);
                    long y2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+14", 0);
                    Thread.Sleep(1000);
                    string dm_ret = m_dm.FindPicEx((int)x1, (int)y1, (int)x2, (int)y2, "monster_point.bmp", "000000", 0.8, 0);//查询红点并前往
                    dm_ret = m_dm.FindNearestPos(dm_ret, 0, 497, 225);
                    if (dm_ret.Length > 0)
                    {
                        string[] pos = dm_ret.Split(',');
                        m_dm.MoveTo(Convert.ToInt32(pos[1])+2, Convert.ToInt32(pos[2])+2);
                        m_dm.LeftClick();
                        Thread.Sleep(200);
                        m_dm.KeyPressChar("tab");
                        m_dm.MoveTo(50, 685);
                        while (need_auto_fight == true && m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B08", 4)!=0)
                        {                            
                            Thread.Sleep(500);
                        }
                        if (times > 10)
                        {
                            m_dm.KeyPressChar("5");
                            times = 0;
                        }
                        times++;
                        Thread.Sleep(500);
                    }
                    else 
                    {
                        //没有红点就搜寻黄点
                        dm_ret = m_dm.FindPicEx((int)x1, (int)y1, (int)x2, (int)y2, "boss_point.bmp", "000000", 0.8, 0);//查询蓝点并前往
                        dm_ret = m_dm.FindNearestPos(dm_ret, 0, 497, 225);
                        if (dm_ret.Length > 0)
                        {
                            string[] pos = dm_ret.Split(',');
                            m_dm.MoveTo(Convert.ToInt32(pos[1]) + 1, Convert.ToInt32(pos[2]) + 1);
                            m_dm.LeftClick();
                            Thread.Sleep(200);
                            m_dm.KeyPressChar("tab");
                            m_dm.MoveTo(50, 685);
                            Thread.Sleep(500);                       
                        }
                        else
                        { 
                            //没有黄点表面当前波次已经打完去npc边等待刷宝箱或者刷新一波怪物
                            dm_ret = m_dm.FindPicEx((int)x1, (int)y1, (int)x2, (int)y2, "npc_point.bmp", "000000", 0.8, 0);//查询蓝点并前往
                            dm_ret = m_dm.FindNearestPos(dm_ret, 0, 497, 225);
                            if (dm_ret.Length > 0)
                            {
                                string[] pos = dm_ret.Split(',');
                                m_dm.MoveTo(Convert.ToInt32(pos[1])+3, Convert.ToInt32(pos[2])+5);
                                m_dm.LeftClick();
                                Thread.Sleep(200);
                                m_dm.KeyPressChar("tab");
                                m_dm.MoveTo(50, 685);
                                Thread.Sleep(6000);//大延迟 等刷宝箱或者新一波怪
                                times++;
                                if (times > 5)
                                {
                                    m_dm.KeyPressChar("5");
                                    //m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 0, 0);
                                    times = 0;
                                }
                            }
                        }
                    }
                }
                else 
                {            
                    Thread.Sleep(1000);
                    //times = 0;
                    while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 4) != 0)
                    {
                        if (m_dm.ReadString(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00389260]+880]+6c", 0, 0) == "藏宝盒")
                        {
                            string result = m_dm.FindDataEx(Convert.ToInt32(p_hwnd_game), "00000000-FFFFFFFF", "98 0E 6E 00", 4, 1, 1);//当前地图怪物地址合集字符串
                            if (result.Length > 0)
                            {
                                string[] results = result.Split('|');
                                foreach (string s in results)
                                {
                                    int addr = Convert.ToInt32(s, 16) + 0x6c;
                                    int addr_hp = Convert.ToInt32(s, 16) + 0xA8;
                                    result = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr.ToString("X"), 0, 0);
                                    //Echo_info(result);
                                    if (m_dm.ReadString(Convert.ToInt32(p_hwnd_game), addr.ToString("X"), 0, 0) == "藏宝盒")
                                    {
                                        Echo_info("找到宝箱");
                                        m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 0, Convert.ToInt32(s, 16));
                                        Thread.Sleep(1000);
                                        while (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), addr_hp.ToString("X"),4) != 0)
                                        {
                                            if (times > 5)
                                            {
                                                m_dm.KeyPressChar("5");
                                                //m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 0, 0);
                                                times = 0;
                                            }
                                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 1)//如果人物在站立状态则+1
                                            {
                                                times++;
                                            }
                                            Thread.Sleep(1000);
                                        }
                                        Thread.Sleep(800);//等待屏蔽生效
                                        m_dm.KeyPressChar("space");
                                        Thread.Sleep(200);
                                        need_auto_fight = false;
                                    }
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                        if (times > 5)
                        {
                            m_dm.KeyPressChar("5");
                            m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+880", 0, 0);
                            times = 0;
                        }
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4) == 1)//如果人物在站立状态则+1
                        {
                            times++;
                        }
                        
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(800);//等待屏蔽生效
                    m_dm.KeyPressChar("space");
                    Thread.Sleep(200);
                }                
            }
            //判定是否打完宝箱，拾取后回城
            need_auto_fight = false;
            player_state = "脚本结束";
        }
        //自动召唤宝宝线程
        public void Check_pet()
        {
            need_check_pet = true;
            //绑定窗口                               
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
            }
            while (need_check_pet == true)
            {
                //m_dm.MoveTo(1,1);
                m_dm.KeyDownChar("ctrl");
                Thread.Sleep(50);
                m_dm.KeyPressChar("z");
                Thread.Sleep(50);
                m_dm.KeyUpChar("ctrl");
                Thread.Sleep(50);
                m_dm.KeyDownChar("F8");
                Thread.Sleep(2000);
                m_dm.KeyUpChar("F8");
                Echo_info("召唤");
                Thread.Sleep(30000);//30秒后循环
            }
            need_check_pet = false;
        }       
        //查询某怪物坐标
        public long Find_monster_on_scream()
        {
            string ret = m_dm.FindPicEx(0, 28, 1026, 621, "monster_blood.bmp", "000000", 0.8, 0);
            ret = m_dm.FindNearestPos(ret, 0, 497, 225);
            if (ret.Length > 0)
            {                
                string[] pos = ret.Split(',');
                m_dm.MoveTo(Convert.ToInt32(pos[1]) + 10, Convert.ToInt32(pos[2]) + 35);
                Thread.Sleep(50);
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+0089364C]+64", 4) == 0)
                {
                    m_dm.MoveTo(Convert.ToInt32(pos[1]) + 10, Convert.ToInt32(pos[2]) + 55);
                    Thread.Sleep(50);
                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+0089364C]+64", 4) == 0)
                    {
                        m_dm.MoveTo(Convert.ToInt32(pos[1]) + 10, Convert.ToInt32(pos[2]) + 75);
                        Thread.Sleep(50);
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+0089364C]+64", 4) == 0)
                        {
                            m_dm.MoveTo(Convert.ToInt32(pos[1]) + 10, Convert.ToInt32(pos[2]) + 95);
                            Thread.Sleep(50);
                            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+0089364C]+64", 4) == 0)
                            {
                                m_dm.MoveTo(Convert.ToInt32(pos[1]) + 10, Convert.ToInt32(pos[2]) + 115);
                                Thread.Sleep(50);
                                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+0089364C]+64", 4) == 0)
                                {
                                    m_dm.MoveTo(Convert.ToInt32(pos[1]) -30, Convert.ToInt32(pos[2]) + 88);

                                }
                            }
                        }
                    }
                }
                m_dm.LeftClick();
                Thread.Sleep(200);
                m_dm.MoveTo(896, 29);
                Thread.Sleep(200);
                m_dm.MoveTo(50, 685);                
                m_dm.LeftClick();
                return 1;
            }
            else 
            {
                return 0;
            }           
        }
        //用修改自动寻路目的地方式寻路去指定坐标
        public int WalkTo(int x, int y)
        {
            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+4", 6) == 0)
            {
                m_dm.KeyPressChar("tab");
            }
            long x1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+8", 0);
            long y1 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+c", 0);
            long x2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+10", 0);
            long y2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+7C]+14", 0);
            Thread.Sleep(1000);
            string dm_ret = m_dm.FindPicEx((int)x1, (int)y1, (int)x2, (int)y2, $"self_point.bmp", "000000", 0.8, 0);//查询红点并前往
            m_dm.Capture((int)x1, (int)y1, (int)x2, (int)y2, "screen.bmp");
            dm_ret = m_dm.FindNearestPos(dm_ret, 0, 497, 225);
            Echo_info($"{dm_ret}|{x1}|{y1}|{m_dm.GetID()}|{m_dm.GetDmCount()}|{dm_ret}");
            if (dm_ret.Length > 0)
            {
                string[] pos = dm_ret.Split(',');
                m_dm.MoveTo(Convert.ToInt32(pos[1]) + 5, Convert.ToInt32(pos[2]) + 5);
                m_dm.LeftClick();
                Thread.Sleep(50);
                m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+AFC", 1, x);
                m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B00", 1, y);
                m_dm.KeyPressChar("tab");
                Thread.Sleep(500);
                return 1;
            }
            return 0;
        }
        //获取游戏窗口位置 在未绑定时修正坐标
        public void Get_pos_fix(out int fix_x, out int fix_y) 
        {
            if (Bing_UnBing_change.Checked)
            {
                fix_x = 0;
                fix_y = 0;                
            }
            else 
            {
                m_dm.GetWindowRect(Convert.ToInt32(p_hwnd_game), out int x1, out int y1, out int x2, out int y2);
                fix_x = x1; 
                fix_y = y1+27;
            }
        }       

        //检测经验卷轴
        public void Check_exp()
        {
            need_check_exp = true;
            long[] fb_map_id = { 17, 18, 19, 51, 52, 53, 54, 60, 61, 62, 63 };            
            while (need_check_exp == true)
            {
                int a = Array.IndexOf(fb_map_id, m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+884", 5));
                if (need_auto_run_fb ==true && a != -1)
                {
                    Sorting_item();//整理物品                   
                }
                else if (need_auto_run_fb == false)
                {
                    Sorting_item();//整理物品  
                }
                Thread.Sleep(60000);//60秒一检测

            }
            need_check_exp = false;
        }
        //自动刷怪
        public void Auto_sg() 
        {
            need_auto_sg = true;
            while (need_auto_sg == true)
            {
                //绑定窗口                               
                if (!Bing_UnBing_change.Checked)
                {
                    Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
                }
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+FC]+104", 6) == 0) //聊天输入框是否打开
                {
                    m_dm.KeyPressChar("enter");                    
                }                
                Thread.Sleep(500);
                m_dm.SendString(Convert.ToInt32(p_hwnd_game), "#sg");
                m_dm.KeyPressChar("enter");
                m_dm.MoveTo(45, 686);
                m_dm.LeftClick();//点击血量区域 用于关闭聊天窗口
                Echo_info("刷怪！");
                for (int i = 0; i < 300; i++) 
                {
                    if (need_auto_sg == false) 
                    {
                        break;
                    }
                    Thread.Sleep(1000);//1秒 1循环 5分钟
                    
                }
            }
            if (need_auto_sg == false) 
            { 
                Echo_info("自动刷怪线程已停止...");
            }
        }
        //合成、丢弃、使用、道具
        public void Sorting_item()
        {
            long[] ball_item_id = { 42, 48, 52,55, 56, 59, 101, 102, 104, 147, 157, 158, 182,183, 195, 196, 197, 208,209,214,226,229,234,239,240,244,336};//挫丸子 祖玛圣战装备
            long[] exp_item_id = { 262,263,264,265,279,287 };//经验券 奖券之类的直接使用
            string[] garbage_items_name = p_garbage_items;
            long bag_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+8", 0);
            long bag_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+C", 0);
            long box_X, box_Y;
            int i = 0x0, fix_x, fix_y;
            ArrayList x_list = new ArrayList();
            ArrayList y_list = new ArrayList();
            ArrayList ball_x_list = new ArrayList();
            ArrayList ball_y_list = new ArrayList();
            ArrayList garbage_x_list = new ArrayList();
            ArrayList garbage_y_list = new ArrayList();
            for (int y = 1; y< 11; y++) 
            {
                for (int x = 1; x< 9; x++)
                {
                    long item_id = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(i * 0x60).ToString("X")}", 4);
                    string item_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(i * 0x60 + 0x4).ToString("X")}", 0, 0);
                    if (exp_item_id.Contains(item_id))
                    {                       
                        x_list.Add(x);
                        y_list.Add(y);                 
                    }
                    if (ball_item_id.Contains(item_id))
                    {
                        ball_x_list.Add(x);
                        ball_y_list.Add(y);
                    }
                    if (garbage_items_name.Contains(item_name) && item_id != 0)
                    {
                        if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(i * 0x60+0x1E).ToString("X")}", 6) ==0)//不绑定不锁定就丢弃
                        {
                            garbage_x_list.Add(x);
                            garbage_y_list.Add(y);
                            //Echo_info($"{item_name}第{y}行，第{x}个");
                        }                        
                    }
                    i++;                    
                }
            }
            //右击经验卷
            if (x_list.Count > 0 )
            {                
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+4", 6) == 0)
                {
                    //打开背包窗口
                    m_dm.KeyPressChar("b");
                    Thread.Sleep(200);
                }
                Get_pos_fix(out  fix_x, out  fix_y);             
                for ( int a = 0; a < x_list.Count; a++)
                {                     
                    int x = Convert.ToInt32(x_list[a]);
                    int y = Convert.ToInt32(y_list[a]);
                    int addr_count_point = (x-1 + (y - 1) * 8)*96+28;
                    //int addr_name_point = (x-1 + (y - 1) * 8) * 96 + 4;
                    long item_count = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{addr_count_point.ToString("X")}", 6);
                    //string item_name = m_dm.ReadString(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{addr_name_point.ToString("X")}", 0, 0);
                    //Echo_info($"{item_name}有{item_count}个");
                    long mouse_x = bag_X + 38 + 41 * (x - 1);
                    long mouse_y = bag_Y + 100 + 41 * (y - 1);
                    m_dm.MoveTo((int)mouse_x + fix_x, (int)mouse_y + fix_y);
                    for ( i = 0; i < item_count; i++)
                    {
                        m_dm.RightClick();
                        Thread.Sleep(20);
                    }                                       
                    Thread.Sleep(100);
                }
            }
            //合成大补丸
            if (ball_x_list.Count > 0 )
            {
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+4", 6) == 0)
                {
                    //打开背包窗口
                    m_dm.KeyPressChar("b");
                    Thread.Sleep(200);
                }
                box_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+8", 0);
                box_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+C", 0);
                Get_pos_fix(out  fix_x, out  fix_y);
                //打开魔盒
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+4", 6) == 0) 
                { 
                    Find_item(858,out long box_pos_x,out long box_pos_y);                    
                    Thread.Sleep(500);
                    m_dm.MoveTo((int)box_pos_x + fix_x, (int)box_pos_y + fix_y);
                    Thread.Sleep(50);
                    m_dm.RightClick();
                }
                //移动魔盒窗口                
                box_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+8", 0);
                box_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+C", 0);
                Thread.Sleep(500);
                m_dm.MoveTo((int)box_X + 150 + fix_x, (int)box_Y + 25 +fix_y);
                Thread.Sleep(100);
                m_dm.LeftDown();
                Thread.Sleep(100);
                m_dm.MoveTo(500 + fix_x,55 + fix_y);
                Thread.Sleep(100);
                m_dm.LeftUp();
                Thread.Sleep(100);
                for (int a = 0; a < ball_x_list.Count; a++)
                {
                    box_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+8", 0);
                    box_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+C", 0);
                    int x = Convert.ToInt32(ball_x_list[a]);
                    int y = Convert.ToInt32(ball_y_list[a]);
                    long mouse_x = bag_X + 38 + 41 * (x - 1);
                    long mouse_y = bag_Y + 100 + 41 * (y - 1);
                    //取物品
                    m_dm.MoveTo((int)mouse_x + fix_x, (int)mouse_y + fix_y);
                    Thread.Sleep(100);
                    m_dm.LeftClick();
                    Thread.Sleep(100);
                    //放物品
                    m_dm.MoveTo((int)box_X+71+18 + fix_x, (int)box_Y+93+18 + fix_y);
                    Thread.Sleep(100);
                    m_dm.LeftClick();
                    Thread.Sleep(100);
                    //合成
                    m_dm.MoveTo((int)box_X + 150 + fix_x, (int)box_Y + 210 + fix_y);
                    Thread.Sleep(100);
                    m_dm.LeftClick();
                    Thread.Sleep(100);
                }
                //关闭魔盒
                Get_pos_fix(out fix_x, out fix_y);
                box_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+8", 0);
                box_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+C", 0);
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+90]+4", 6) == 1)
                {
                    m_dm.MoveTo((int)box_X + 272 + fix_x, (int)box_Y + 47 + fix_y);
                    Thread.Sleep(50);
                    m_dm.LeftClick();
                    Thread.Sleep(200);
                }
            }
            //丢垃圾
            if (garbage_x_list.Count >0 )
            {
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+4", 6) == 0)
                {
                    //打开背包窗口
                    m_dm.KeyPressChar("b");
                    Thread.Sleep(200);
                }
                Get_pos_fix(out fix_x, out fix_y);
                for (int a = 0; a < garbage_x_list.Count; a++)
                {
                    int x = Convert.ToInt32(garbage_x_list[a]);
                    int y = Convert.ToInt32(garbage_y_list[a]);
                    long mouse_x = bag_X + 38 + 41 * (x - 1);
                    long mouse_y = bag_Y + 100 + 41 * (y - 1);
                    m_dm.MoveTo((int)mouse_x + fix_x, (int)mouse_y + fix_y);                    
                    m_dm.LeftClick();
                    Thread.Sleep(100);
                    m_dm.MoveTo(900 + fix_x , 600 + fix_y);
                    m_dm.LeftClick();
                    Thread.Sleep(100);
                    if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+CC]+4", 6) == 1) 
                    {
                        m_dm.MoveTo(455 + fix_x, 395 + fix_y);
                        m_dm.LeftClick();
                    }
                    Thread.Sleep(100);
                }
            }
            
            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+4", 6) == 1)
            {
                //关闭包裹
                m_dm.KeyPressChar("b");
                Thread.Sleep(200);
            }
            m_dm.MoveTo(890, 10);
            Thread.Sleep(1000);
        }
        //查找item_id 对应在包裹的坐标
        public void Find_item(long in_item_id ,out long out_mouse_x, out long out_mouse_y)
        {
            out_mouse_x = -1;
            out_mouse_y = -1;
            long bag_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+8", 0);
            long bag_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+C", 0);
            int i = 0x0;
            for (int y = 1; y< 11; y++) 
            {
                for (int x = 1; x < 9; x++)
                {
                    long item_id = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+850]+{(i * 0x60).ToString("X")}", 4);
                    if (item_id == in_item_id )
                    {
                        long mouse_x = bag_X + 38 + 41 * (x-1);
                        long mouse_y = bag_Y + 100 + 41 * (y-1);
                        out_mouse_x = mouse_x;
                        out_mouse_y = mouse_y;                        
                    }                    
                    i++;
                    //Thread.Sleep(100);
                }
            }
            
        }
        //物品 名称 转 ID
        public long Name_to_id(string name) 
        {
            long id = -1;          
            string[] item_name = p_item_name;
            long[] item_id = p_item_id;            
            int pos = Array.IndexOf(item_name, name);
            if (pos != -1)//item_name存在
            {
                id = item_id[pos];                
            }
            return id;
        }
        //初始化快捷栏
        public void Initialize_item_bar(ArrayList arrayList)
        {
            //绑定窗口
            bool reset = true;
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Checked = true;
                reset = false;
            }
            long item_bar_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+9C]+8", 0);
            long item_bar_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+9C]+C", 0);
            long bag_state = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+4", 6);            
            long item_id;
            Get_pos_fix(out int fix_x, out int fix_y);
            //Name_to_id(this.item_bar_1.Text);//
            int i = 0;
            if (bag_state == 0)
            {
                //打开背包窗口
                m_dm.KeyPressChar("b");
                Thread.Sleep(200);
            }
            foreach (long c in arrayList)
            {
                //初始化第i格快捷栏
                if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), $"[[<BackMir.exe>+00389260]+85C]+{(i*0x60).ToString("X")}", 4) != c)
                {
                    item_id = c;
                    Find_item(item_id, out long mouse_x, out long mouse_y);
                    m_dm.MoveTo((int)mouse_x + fix_x, (int)mouse_y + fix_y);
                    m_dm.LeftClick();
                    Thread.Sleep(100);
                    mouse_x = item_bar_X + 29 + 36 * i;
                    mouse_y = item_bar_Y + 20;
                    m_dm.MoveTo((int)mouse_x + fix_x, (int)mouse_y + fix_y);
                    m_dm.LeftClick();
                }                
                i++;
                Thread.Sleep(100); 
                              
            }
            if (Bing_UnBing_change.Checked != reset)
            {
                Bing_UnBing_change.Checked = reset;
            }
            m_dm.KeyPressChar("b");
            m_dm.MoveTo(45, 686);
        }
        //人物挂机卡住脚本
        public void Reset_state() 
        {
            //绑定窗口                               
            if (!Bing_UnBing_change.Checked)
            {
                Bing_UnBing_change.Invoke(new Action<bool>(n => { Bing_UnBing_change.Checked = n; }), true);
            }
            while (true)
            {
                long player_autorun = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 6);
                //如果人物自动反击状态失效，则启动反击状态
                if (player_autorun == 0)
                {
                    m_dm.WriteInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18",2 ,1);
                    Thread.Sleep(1000);//延迟1秒
                }
                //如果人物反击状态没有失效，且人物还在站街，则上坐骑加随机强制结束反击状态进入下一次循环
                else
                {                    
                    m_dm.KeyPressChar("5");//使用随机      
                    break; 
                }                     
            }
        }
        //洗练装备
        public void Wash_item() 
        {
            long x, y;
            Get_pos_fix(out int fix_x, out int fix_y);
            long bag_state = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+4", 6);
            long bag_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+8", 0);
            long bag_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+8C]+C", 0);
            long wash_state = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+A8]+4", 6);
            long wash_state_2 = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+A8]+E8", 6);
            long wash_X = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+A8]+8", 0);
            long wash_Y = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[[<BackMir.exe>+00892364]+A8]+C", 0);
            if (bag_state == 0)
            {
                //打开背包窗口
                m_dm.KeyPressChar("b");
                Thread.Sleep(200);
            }
            if (wash_state == 0)
            {
                //打开洗练窗口
                m_dm.KeyPressChar("m");
                Thread.Sleep(200);
            }
            if (wash_state_2 == 0)
            {
                //选择洗练标签
                x = wash_X + 135;
                y = wash_Y + 66;
                m_dm.MoveTo((int)x + fix_x, (int)y + fix_y);
                m_dm.LeftClick();
                Thread.Sleep(200);
            }
            //取装备 
            x = bag_X + 35 + 36 * 8;
            y = bag_Y + 97 + 36 * 9;
            m_dm.MoveTo((int)x + fix_x, (int)y + fix_y);
            m_dm.LeftClick();
            Thread.Sleep(50);
            //放装备
            x = wash_X + 150;
            y = wash_Y + 280;
            m_dm.MoveTo((int)x + fix_x, (int)y + fix_y);
            m_dm.LeftClick();
            Thread.Sleep(50);
            //取洗练道具
            x = bag_X + 35 + 36 * 8;
            y = bag_Y + 97 + 36 * 8;
            m_dm.MoveTo((int)x + fix_x, (int)y + fix_y);
            m_dm.LeftClick();
            Thread.Sleep(50);
            //放洗练道具
            x = wash_X + 120;
            y = wash_Y + 220;
            m_dm.MoveTo((int)x + fix_x, (int)y + fix_y);
            m_dm.LeftClick();
            Thread.Sleep(50);
            //点击洗练
            x = wash_X + 150;
            y = wash_Y + 380;
            m_dm.MoveTo((int)x + fix_x, (int)y + fix_y);
            m_dm.LeftClick();
            Thread.Sleep(50);
            //回装备处
            x = bag_X + 35 + 36 * 8;
            y = bag_Y + 97 + 36 * 9;
            m_dm.MoveTo((int)x + fix_x, (int)y + fix_y);
            Thread.Sleep(50);
            Echo_info("洗练完成。");            
        }

        //获取时间差
        public static string GetTime(DateTime timeA)
        {
            /*
            timeA 表示需要计算
            ts.Days.ToString();//相差天数：
            ts.Hours.ToString();//相差小时：
            ts.Minutes.ToString();//相差分钟：
            ts.Seconds.ToString();//相差秒数：
            ts.TotalDays.ToString();//相差总天数：
            ts.TotalHours.ToString();//相差总小时：
            ts.TotalMinutes.ToString();//相差总分钟：
            ts.TotalSeconds.ToString();//相差总秒数：
            */
            DateTime timeB = DateTime.Now;//获取当前时间
            TimeSpan ts = timeB - timeA;//计算时间差
            string hr = ts.Hours.ToString();//将时间差转换为相差小时
            string min = ts.Minutes.ToString();//将时间差转换为相差分钟
            string sec = ts.Seconds.ToString();//将时间差转换为相差秒
            string time = $"已挂机{hr}小时{min}分钟{sec}秒";
            return time;
        }
        //循环检测线程
        public void Check_state()
        {
            //控制线程数量防止UI错乱
            thread_count -= 1;
            //初始化数据            
            long player_exp_begin = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B8", 4);
            DateTime time_begin = DateTime.Now;
            int stop_time = 1;           

            //循环检测
            while (needstop == false)
            {                
                //武器耐久2字节无符号
                long dur_weapon = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+276", 5);
                //左戒指耐久2字节无符号
                //long dur_ring = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+4B6", 5);
                //人物状态 4字节无符号 1.站立，2.走路，3.跑路，4.攻击后摇，5.普通攻击，10.技能攻击，12.死亡
                long player_state = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+60", 4);
                //人物经验 4字节无符号
                //long player_exp = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B8", 4);
                //坐骑状态 1字节无符号 0.无坐骑 12.有坐骑  12应该是编号 非零为有坐骑
                //long player_ride = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+214", 6);
                //人物反击状态 1字节无符号 0.未在反击状态   1.在反击状态
                //long player_autorun = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B18", 6);                
                if (player_state == 0)
                {
                    Echo_info("窗口异常，已停止检测...");
                    needstop = true;
                    break;
                }
                if (player_state == 12)
                {
                    Echo_info($"武器耐久=[{dur_weapon}],人物已经死亡[{player_state}]");
                    break;
                }
                if (stop_time == 30)
                {
                    Echo_info("人物卡主了...启动重置脚本...");
                    Reset_state();                    
                    stop_time = 1;
                    Thread.Sleep(2000);
                    continue;
                }
                if (player_state == 1) 
                {
                    Echo_info($"武器耐久=[{dur_weapon}],人物已经站街[{stop_time}]秒");
                    stop_time += 1;
                    Thread.Sleep(1000);
                    continue;
                }
                if (player_state == 2 | player_state == 3)
                {
                    stop_time = 1;
                    Echo_info($"武器耐久=[{dur_weapon}],人物正在移动[{player_state}]");                    
                    Thread.Sleep(1000);
                    continue;
                }
                if (player_state == 4 | player_state == 5 | player_state ==10)
                {
                    stop_time = 0;
                    Echo_info($"武器耐久=[{dur_weapon}],人物正在攻击[{player_state}]");
                    Thread.Sleep(1000);
                    continue;
                }                
                Thread.Sleep(1000);

            }
            long player_exp_end = m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+B8", 4);
            Echo_info($"检测已停止...\r\n{GetTime(time_begin)}\r\n本次挂机获得经验：{player_exp_end - player_exp_begin}");
            thread_count += 1;          
        }
        //更新UI  ui线程直接append，非ui线程走委托append
        public void Echo_info(string message) 
        {

            if (textbox_info.InvokeRequired)
            {
                // 如果需要调用（即当前线程不是UI线程），通过Invoke在UI线程上执行
                textbox_info.Invoke((Action)(() => Echo_info(message)));
            }
            else
            {
                // 如果是UI线程，直接调用Echo_info
                textbox_info.AppendText($"{DateTime.Now.ToString("T")} >>> {message}\r\n");
            }

        }
        //加载配置文件
        public void LoadConfig()
        {
            if (File.Exists(configFilePath))
            {
                string[] lines = File.ReadAllLines(configFilePath);

                if (lines.Length >= 5)
                {
                    this.item_bar_1.SelectedIndex = Convert.ToInt32(lines[0]);
                    this.item_bar_2.SelectedIndex = Convert.ToInt32(lines[1]);
                    this.item_bar_3.SelectedIndex = Convert.ToInt32(lines[2]);
                    this.item_bar_4.SelectedIndex = Convert.ToInt32(lines[3]);
                    this.Cb_fb_name.SelectedIndex = Convert.ToInt32(lines[4]);
                }
            }
            else
            {
                string[] defaultLines = new string[] { "0", "0", "0", "0", "0" };
                File.WriteAllLines(configFilePath, defaultLines);
                LoadConfig(); // 重新加载配置文件
            }
        }
        //更新配置文件
        public void UpdateConfig()
        {
            string[] lines = new string[]
            {
                this.item_bar_1.SelectedIndex.ToString(),
                this.item_bar_2.SelectedIndex.ToString(),
                this.item_bar_3.SelectedIndex.ToString(),
                this.item_bar_4.SelectedIndex.ToString(),
                this.Cb_fb_name.SelectedIndex.ToString()
            };
            File.WriteAllLines(configFilePath, lines);
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            //初始化窗口
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = $"BMHelper V.{scripver}";
            //自动飞快下拉框初始化
            Cb_auto_fly.Items.AddRange(entityFeatureCodes.Keys.ToArray());
            Cb_map_name.Items.AddRange(locations.ToArray());
            // 创建全局对象，此对象必须全程保持，不可释放.
            m_dm = new dmsoft();
            PS = new PlayerState();
            // 收费注册
            int dm_ret = m_dm.Reg("xf30557fc317f617eead33dfc8de3bdd4ab9043", "xh34e44xok7sgp7");
            if (dm_ret != 1)
            {
                Echo_info("收费注册失败,返回值:" + dm_ret.ToString());
            }
            else
            {
                Echo_info("收费注册成功,返回值:" + dm_ret.ToString());
                Echo_info("大漠插件版本："+m_dm.Ver());
            }
            //初始化基础路径 设定字库
            data_dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Tencent\\MiniBrowser\\Storage\\Legal\\Files\\Version\\Data";
            m_dm.SetPath(data_dir);
            m_dm.SetDict(0, "dm_soft.txt");
            
            //配置文件相关
            try
            {
                // 读取文件的所有行并存储在字符串数组中
                p_garbage_items = File.ReadAllLines(path);
                File_path.Text = Path.GetFileName(path);
            }
            catch (Exception)
            {
                // 处理可能的异常                
                Echo_info("杂物文件路径有错误，请从新加载");
            }
            //初始化各选项
            configFilePath = Path.Combine(data_dir, "config.txt");
            //软件启动自动查询窗口1次
            bn_find_wnd_Click(sender, e);
            LoadConfig();
            // 字典用于存储每个物品的信息
            //Zd_iteminfo = new Dictionary<int, ItemInfo>();
            Ini_item_data_name_arr = new ArrayList();
            Ini_item_data_id_arr = new ArrayList();
            Ini_item_bar_id_arr = new ArrayList();

            Ini_item_data_id_arr.Add(Name_to_id(this.item_bar_1.Text));
            Ini_item_data_id_arr.Add(Name_to_id(this.item_bar_2.Text));
            Ini_item_data_id_arr.Add(Name_to_id(this.item_bar_3.Text));
            Ini_item_data_name_arr.Add(this.item_bar_1.Text);
            Ini_item_data_name_arr.Add(this.item_bar_2.Text);
            Ini_item_data_name_arr.Add(this.item_bar_3.Text);
            // 初始化定时器，设置刷新间隔为1000毫秒（1秒）
            
            dataRefreshTimer = new System.Threading.Timer(RefreshData, null, 0, 1000);
        }

        public void textbox_info_TextChanged(object sender, EventArgs e)
        {
            this.label_text_length.Text = textbox_info.Lines.Length.ToString();
            if (textbox_info.Lines.Length > 99)
            {
                textbox_info.Lines = textbox_info.Lines.Skip(1).ToArray();
                textbox_info.Select(textbox_info.TextLength, 0);
                textbox_info.ScrollToCaret();
            }
        }

        public void bn_start_Click(object sender, EventArgs e)
        {            
            if (thread_count >0)
            {
                needstop = false;
                Thread th = new Thread(Check_state);
                th.IsBackground = true;
                th.Start();
            }
            else 
            {
                Echo_info("进程已经存在");
            }
            bn_start.Enabled = false;
            Application.DoEvents();
            bn_start.Enabled = true;
        }

        public void bn_stop_Click(object sender, EventArgs e)
        {
            //Test();
            needstop = true;
            bn_stop.Enabled = false;
            Application.DoEvents();
            bn_stop.Enabled = true;
        }

        public void bn_find_wnd_Click(object sender, EventArgs e)
        {            
            string hwnds = m_dm.EnumWindow(0, "", "HGE__WNDCLASS", 1 + 2 + 4 + 8 + 16);
            if (hwnds.Length == 0)
            {
                Echo_info("枚举窗口失败");
                return;
            }
            else
            {
                string[] hwnd_array = hwnds.Split(new char[] { ',' });
                ArrayList arrayList = new ArrayList();
                foreach (string i in hwnd_array)
                {
                    string title = m_dm.GetWindowTitle(Convert.ToInt32(i));
                    string[] arr = title.Split(new char[] { '-' });
                    if (arr.Length >2)
                    {                   
                        string name = arr[2];
                        arrayList.Add(name);                    
                        Echo_info("找到窗口："+i);
                        Echo_info ("窗口标题："+title);
                    }
                }
                string[] arrs = (string[])arrayList.ToArray(typeof(string));
                if (arrs.Length>0)
                {
                    cb_playername.Items.Clear();
                    cb_playername.Items.AddRange(arrs);
                    cb_playername.SelectedIndex = 0;
                }
                
            }
            hwnds = m_dm.EnumWindow(0, "", "AutoIt v3 GUI", 1 + 2 + 4 + 8 + 16);
            if (hwnds.Length == 0)
            {
                Echo_info("枚举窗口失败");
                return;
            }
            else
            {
                string[] hwnd_array = hwnds.Split(new char[] { ',' });
                ArrayList arrayList = new ArrayList();
                foreach (string i in hwnd_array)
                {
                    p_hwnd_helper = i;
                    Echo_info("找到辅助窗口：" + p_hwnd_helper);
                }
            }
        }


        public void cb_playername_SelectedIndexChanged(object sender, EventArgs e)
        {
            p_hwnd_game = m_dm.EnumWindow(0, cb_playername.Text, "HGE__WNDCLASS", 1 + 2 + 4 + 8 + 16);
            Echo_info($"已选择窗口: {cb_playername.Text}\r\n窗口句柄：{p_hwnd_game}");
        }

        public void bn_ini_item_Click(object sender, EventArgs e)
        {
            Ini_item_bar_id_arr.Clear();
            Ini_item_bar_id_arr.Add(Name_to_id(this.item_bar_1.Text));//添加第1个下拉框物品ID
            Ini_item_bar_id_arr.Add(Name_to_id(this.item_bar_2.Text));//添加第2个下拉框物品ID
            Ini_item_bar_id_arr.Add(Name_to_id(this.item_bar_3.Text));//添加第3个下拉框物品ID
            Ini_item_bar_id_arr.Add(Name_to_id(this.item_bar_4.Text));//添加第4个下拉框物品ID
            Ini_item_bar_id_arr.Add((long)404);//添加随机卷ID
            Ini_item_bar_id_arr.Add((long)402);//添加回城卷ID
            //long[] arrs = (long[])arrayList.ToArray(typeof(long));
            Initialize_item_bar(Ini_item_bar_id_arr);
        }

        public void checkBox_wash_item_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_wash_item.Checked)
            {
                Echo_info("洗练功能已打开。需要窗口内按快捷键");
                Echo_info("背包窗口和打造窗口不能重叠太多！！！");
            }
            else 
            {
                Echo_info("洗练功能已关闭...");
            }
        }

        public void cb_check_pet_CheckedChanged(object sender, EventArgs e)
        {
            if (m_dm.ReadInt(Convert.ToInt32(p_hwnd_game), "[<BackMir.exe>+00389260]+75E", 6) == 2) //道士专用召唤宝宝
            {                
                if (cb_check_pet.Checked)
                {
                    if (need_check_pet == false)
                    {
                        Echo_info("自动召唤宝宝已开启...");
                        Thread th = new Thread(Check_pet);
                        th.IsBackground = true;
                        th.Start();
                    }
                    else
                    {
                        Echo_info("进程已经存在");
                    }
                }
                else
                {
                    Echo_info("自动召唤宝宝已关闭...");
                    need_check_pet = false;
                }
            }
            else 
            {
                Echo_info("只有道士能使用这个功能...");
                cb_check_pet.Checked=false;
            }
        }

        public void Bing_UnBing_change_CheckedChanged(object sender, EventArgs e)
        {
            if (Bing_UnBing_change.Checked)
            {
                int wnd_bind = m_dm.BindWindowEx(Convert.ToInt32(p_hwnd_game), "dx2",
                    "dx.mouse.position.lock.api|dx.mouse.position.lock.message|dx.mouse.clip.lock.api|dx.mouse.input.lock.api|dx.mouse.state.api|dx.mouse.api|dx.mouse.cursor",
                    "dx.keypad.input.lock.api|dx.keypad.state.api|dx.keypad.api",
                    "dx.public.active.api|dx.public.active.message", 0);
                if (wnd_bind == 0)
                {
                    Echo_info("绑定窗口失败" + m_dm.GetLastError());
                    Bing_UnBing_change.Checked = false;
                }
                else
                {
                    Echo_info("窗口绑定成功");                    
                }
            }
            else
            {
                Echo_info("窗口解绑成功");
                m_dm.UnBindWindow();
            }
        }

        public void checkBox_sorting_item_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_sorting_item.Checked)
            {
                Echo_info("整理经验卷功能已打开。");
                Echo_info("会自动整理5W和10W两种经验卷。");
            }
            else
            {
                Echo_info("整理经验卷功能已关闭...");
            }
        }

        public void Bn_set_top_Click(object sender, EventArgs e)
        {
            m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 1);
            m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 8);
        }

        public void Bn_set_untop_Click(object sender, EventArgs e)
        {
            m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 1);
            m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 9);
        }

        public void bn_move_side_Click(object sender, EventArgs e)
        {

            int WSH = Screen.PrimaryScreen.WorkingArea.Height;//调试机器屏幕工作区高
            int WSW = Screen.PrimaryScreen.WorkingArea.Width;//调试机器屏幕工作区宽
            int SH = Screen.PrimaryScreen.Bounds.Height; //调试机器屏幕分辨率高1440
            int SW = Screen.PrimaryScreen.Bounds.Width; //调试机器屏幕分辨率宽2560
            int WH = 797;// 游戏窗口高
            int WW = 1030;//游戏窗口宽
            if (wnd_pos == "center" | wnd_pos == "rightbot") 
            {
                m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 1);
                m_dm.MoveWindow(Convert.ToInt32(p_hwnd_game), 0, 0);
                wnd_pos = "lefttop";
            }
            else if (wnd_pos == "lefttop")
            {
                m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 1);
                m_dm.MoveWindow(Convert.ToInt32(p_hwnd_game), WSW-WW , 0);
                wnd_pos = "righttop";
            }
            else if (wnd_pos == "righttop")
            {
                m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 1);
                m_dm.MoveWindow(Convert.ToInt32(p_hwnd_game), 0 , WSH-WH );
                wnd_pos = "leftbot";
            }
            else if (wnd_pos == "leftbot")
            {
                m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 1);
                m_dm.MoveWindow(Convert.ToInt32(p_hwnd_game), WSW - WW, WSH - WH);
                wnd_pos = "rightbot";
            }

        }

        public void bn_move_center_Click(object sender, EventArgs e)
        {
            int WSH = Screen.PrimaryScreen.WorkingArea.Height;//调试机器屏幕工作区高
            int WSW = Screen.PrimaryScreen.WorkingArea.Width;//调试机器屏幕工作区宽
            int SH = Screen.PrimaryScreen.Bounds.Height; //调试机器屏幕分辨率高1440
            int SW = Screen.PrimaryScreen.Bounds.Width; //调试机器屏幕分辨率宽2560
            int WH = 797;// 游戏窗口高
            int WW = 1030;//游戏窗口宽
            m_dm.SetWindowState(Convert.ToInt32(p_hwnd_game), 1);
            m_dm.MoveWindow(Convert.ToInt32(p_hwnd_game), (WSW - WW)/2, (WSH - WH)/2);
            wnd_pos = "center";
        }

        public void cb_check_exp_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_check_exp.Checked)
            {
                if (need_check_exp == false)
                {
                    Echo_info("自动合并经验卷已开启...");
                    Thread th = new Thread(Check_exp);
                    th.IsBackground = true;
                    th.Start();
                }
                else
                {
                    Echo_info("进程已经存在");
                }
            }
            else
            {
                Echo_info("自动合并经验卷已关闭...");
                need_check_exp = false;
            }
        }

        public void cb_auto_sg_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_auto_sg.Checked)
            {
                if (need_auto_sg == false)
                {
                    Echo_info("自动刷怪已开启...");
                    Thread th = new Thread(Auto_sg);
                    th.IsBackground = true;
                    th.Start();
                }
                else
                {
                    Echo_info("进程已经存在");
                }
            }
            else
            {
                Echo_info("自动刷怪已关闭...");
                need_auto_sg = false;
            }
        }
        //遍历包裹按照包裹格子建立包裹字典
        
        public void bn_test_Click(object sender, EventArgs e)
        {
            //fly_and_hit1(textBox1.Text);
            //Echo_info($"{WalkTo(300, 300)}");
            //开始购物逻辑

            Task.Run(() =>
            {
                UpdateInventoryItem();
                ProcessBagDetails(Bagdetail);
            });
            //Goto_map("石墓");
        }
        public void Bn_start_auto_fb_Click(object sender, EventArgs e)
        {
            Thread th_auto_fb = new Thread(Auto_run_fb);
            th_auto_fb.IsBackground = true;
            if (Bn_start_auto_fb.Text == "开始副本")
            {               
                need_auto_fight = true;
                need_auto_run_fb = true;
                th_auto_fb.Start();
                Bn_start_auto_fb.Enabled = false;
                Application.DoEvents();
                Bn_start_auto_fb.Text = "停止副本";
                Bn_start_auto_fb.Enabled = true;
                Bn_auto_fight.Enabled = false;
                Bn_fly_and_hit.Enabled = false;
            }
            else
            {
                need_auto_fight = false;
                need_auto_run_fb = false;
                th_auto_fb.Abort();
                Bn_start_auto_fb.Enabled = false;
                Application.DoEvents();
                Bn_start_auto_fb.Text = "开始副本";
                Bn_start_auto_fb.Enabled = true;
                Bn_auto_fight.Enabled = true;
                Bn_fly_and_hit.Enabled = true;
            }
        }

        public void Bn_auto_fight_Click(object sender, EventArgs e)
        {
            Thread th_auto_fight = new Thread(Auto_ctrl_t);
            th_auto_fight.IsBackground = true; 
            if (Bn_auto_fight.Text == "自动打怪")
            {
                need_auto_fight=true;
                th_auto_fight.Start();
                Bn_auto_fight.Enabled = false;
                Application.DoEvents();
                Bn_auto_fight.Text = "停止打怪";
                Bn_auto_fight.Enabled = true;
            }
            else 
            {
                need_auto_fight = false;
                th_auto_fight.Abort();
                Bn_auto_fight.Enabled = false;
                Application.DoEvents();
                Bn_auto_fight.Text = "自动打怪";
                Bn_auto_fight.Enabled = true;
            }
        }

        public void Cb_fb_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            p_fb_select = this.Cb_fb_name.SelectedIndex;
            Echo_info($"当前选择副本为：{Cb_fb_name.Text}");
            UpdateConfig();
        }

        public void Bn_select_file_Click(object sender, EventArgs e)
        {
            // 打开文件选择对话框
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                openFileDialog.Title = "选择一个杂物TXT配置文件";
                // 设置默认路径
                openFileDialog.InitialDirectory = @"G:\BackMir_3.11.01\Bin\过滤";
                // 如果用户点击了“确定”按钮
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取选择的文件路径
                    path = openFileDialog.FileName;

                    // 使用 Path.GetFileName 获取文件名并显示在文本框中
                    this.File_path.Text = Path.GetFileName(path);
                    p_garbage_items = File.ReadAllLines(path);
                }
            }
        }

        public void Reload_Click(object sender, EventArgs e)
        {
            if (File_path.Text != "杂物配置文件")
            {
                p_garbage_items = File.ReadAllLines(path);
            }
            else 
            {
                MessageBox.Show("配置文件有错误，请先加载配置文件", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Ini_item_data_id_arr.Clear();
            Ini_item_data_name_arr.Clear();
            Ini_item_data_id_arr.Add(Name_to_id(this.item_bar_1.Text));
            Ini_item_data_id_arr.Add(Name_to_id(this.item_bar_2.Text));
            Ini_item_data_id_arr.Add(Name_to_id(this.item_bar_3.Text));
            Ini_item_data_name_arr.Add(this.item_bar_1.Text);
            Ini_item_data_name_arr.Add(this.item_bar_2.Text);
            Ini_item_data_name_arr.Add(this.item_bar_3.Text);

        }

        public void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateConfig();
        }

        public void Bn_fly_and_hit_Click(object sender, EventArgs e)
        {
            Thread th_fly_and_hit = new Thread(fly_and_hit);
            th_fly_and_hit.IsBackground = true;
            if (Bn_fly_and_hit.Text == "自动飞怪")
            {
                need_fly_and_hit = true;
                th_fly_and_hit.Start();
                Bn_fly_and_hit.Enabled = false;
                Application.DoEvents();
                Bn_fly_and_hit.Text = "停止飞怪";
                Bn_start_auto_fb.Enabled = false;
                Bn_auto_fight.Enabled = false;
                Bn_fly_and_hit.Enabled = true;
            }
            else 
            {
                need_fly_and_hit = false;
                th_fly_and_hit.Abort();
                Bn_fly_and_hit.Enabled = false;
                Application.DoEvents();
                Bn_fly_and_hit.Text = "自动飞怪";
                Bn_start_auto_fb.Enabled = true;
                Bn_auto_fight.Enabled = true;
                Bn_fly_and_hit.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Bn_test_1.Text == "开始")
            {
                lockObject = new object();
                NoCT_Timer = new System.Threading.Timer(fly_and_hit1, null, 0, 3000);
                lock (lockObject)
                {
                    Bn_test_1.Text = "结束";
                }
            }
            else 
            {
                // 释放定时器资源
                NoCT_Timer.Dispose();
                Bn_test_1.Text = "开始";                
            }
                            
        }

        private void Cb_auto_fly_SelectedIndexChanged(object sender, EventArgs e)
        {
            p_fly_name = Cb_auto_fly.SelectedItem.ToString();
            p_fly_code = SplitAndReverse(Get_fly_code(p_fly_name),2);
            Echo_info($"{p_fly_name}|{p_fly_code}");
        }

        private void Cb_map_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            fly_map_name = Cb_map_name.SelectedItem.ToString();
        }

        private void Bn_TF_CT_Click(object sender, EventArgs e)
        {
            if (Bn_TF_CT.Text =="真CT")
            {
                CT_type = false;
                Bn_TF_CT.Text = "假CT";
                Bn_TF_CT.Enabled = false;
                Application.DoEvents();
                Bn_TF_CT.Enabled = true;
            }
            else
            {
                CT_type = true;
                Bn_TF_CT.Text = "真CT";
                Bn_TF_CT.Enabled = false;
                Application.DoEvents();
                Bn_TF_CT.Enabled = true;
            }
        }
    }
}
