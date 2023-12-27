using System.Windows.Forms;
namespace BMHelper
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {          
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textbox_info = new System.Windows.Forms.TextBox();
            this.label_text_length = new System.Windows.Forms.Label();
            this.bn_start = new System.Windows.Forms.Button();
            this.bn_stop = new System.Windows.Forms.Button();
            this.bn_find_wnd = new System.Windows.Forms.Button();
            this.cb_playername = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Bing_UnBing_change = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.Bn_TF_CT = new System.Windows.Forms.Button();
            this.Cb_map_name = new System.Windows.Forms.ComboBox();
            this.Bn_fly_and_hit = new System.Windows.Forms.Button();
            this.Cb_auto_fly = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Bn_auto_fight = new System.Windows.Forms.Button();
            this.Bn_start_auto_fb = new System.Windows.Forms.Button();
            this.Cb_fb_name = new System.Windows.Forms.ComboBox();
            this.label_wash_item = new System.Windows.Forms.Label();
            this.checkBox_wash_item = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_sorting_item = new System.Windows.Forms.CheckBox();
            this.label_sorting_item = new System.Windows.Forms.Label();
            this.checkBox_fuc_2 = new System.Windows.Forms.CheckBox();
            this.label_fuc_2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bn_ini_item = new System.Windows.Forms.Button();
            this.item_bar_4 = new System.Windows.Forms.ComboBox();
            this.item_bar_3 = new System.Windows.Forms.ComboBox();
            this.item_bar_2 = new System.Windows.Forms.ComboBox();
            this.item_bar_1 = new System.Windows.Forms.ComboBox();
            this.cb_check_pet = new System.Windows.Forms.CheckBox();
            this.Bn_set_top = new System.Windows.Forms.Button();
            this.Bn_set_untop = new System.Windows.Forms.Button();
            this.bn_move_side = new System.Windows.Forms.Button();
            this.bn_move_center = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_check_exp = new System.Windows.Forms.CheckBox();
            this.cb_auto_sg = new System.Windows.Forms.CheckBox();
            this.bn_test = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Bn_select_file = new System.Windows.Forms.Button();
            this.File_path = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.Reload = new System.Windows.Forms.Button();
            this.Bn_test_1 = new System.Windows.Forms.Button();
            this.itemInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // textbox_info
            // 
            this.textbox_info.Location = new System.Drawing.Point(6, 13);
            this.textbox_info.Multiline = true;
            this.textbox_info.Name = "textbox_info";
            this.textbox_info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textbox_info.Size = new System.Drawing.Size(344, 201);
            this.textbox_info.TabIndex = 0;
            this.textbox_info.TextChanged += new System.EventHandler(this.textbox_info_TextChanged);
            // 
            // label_text_length
            // 
            this.label_text_length.AutoSize = true;
            this.label_text_length.Location = new System.Drawing.Point(333, 435);
            this.label_text_length.Name = "label_text_length";
            this.label_text_length.Size = new System.Drawing.Size(17, 12);
            this.label_text_length.TabIndex = 1;
            this.label_text_length.Text = "00";
            // 
            // bn_start
            // 
            this.bn_start.Location = new System.Drawing.Point(535, 416);
            this.bn_start.Name = "bn_start";
            this.bn_start.Size = new System.Drawing.Size(70, 22);
            this.bn_start.TabIndex = 2;
            this.bn_start.Text = "开始检测";
            this.bn_start.UseVisualStyleBackColor = true;
            this.bn_start.Click += new System.EventHandler(this.bn_start_Click);
            // 
            // bn_stop
            // 
            this.bn_stop.Location = new System.Drawing.Point(611, 416);
            this.bn_stop.Name = "bn_stop";
            this.bn_stop.Size = new System.Drawing.Size(70, 22);
            this.bn_stop.TabIndex = 3;
            this.bn_stop.Text = "停止检测";
            this.bn_stop.UseVisualStyleBackColor = true;
            this.bn_stop.Click += new System.EventHandler(this.bn_stop_Click);
            // 
            // bn_find_wnd
            // 
            this.bn_find_wnd.Location = new System.Drawing.Point(94, 43);
            this.bn_find_wnd.Name = "bn_find_wnd";
            this.bn_find_wnd.Size = new System.Drawing.Size(70, 22);
            this.bn_find_wnd.TabIndex = 4;
            this.bn_find_wnd.Text = "查找窗口";
            this.bn_find_wnd.UseVisualStyleBackColor = true;
            this.bn_find_wnd.Click += new System.EventHandler(this.bn_find_wnd_Click);
            // 
            // cb_playername
            // 
            this.cb_playername.FormattingEnabled = true;
            this.cb_playername.Location = new System.Drawing.Point(6, 20);
            this.cb_playername.Name = "cb_playername";
            this.cb_playername.Size = new System.Drawing.Size(158, 20);
            this.cb_playername.TabIndex = 5;
            this.cb_playername.SelectedIndexChanged += new System.EventHandler(this.cb_playername_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Bing_UnBing_change);
            this.groupBox1.Controls.Add(this.bn_find_wnd);
            this.groupBox1.Controls.Add(this.cb_playername);
            this.groupBox1.Location = new System.Drawing.Point(364, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 71);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "初始化";
            // 
            // Bing_UnBing_change
            // 
            this.Bing_UnBing_change.AutoSize = true;
            this.Bing_UnBing_change.Location = new System.Drawing.Point(6, 46);
            this.Bing_UnBing_change.Name = "Bing_UnBing_change";
            this.Bing_UnBing_change.Size = new System.Drawing.Size(72, 16);
            this.Bing_UnBing_change.TabIndex = 6;
            this.Bing_UnBing_change.Text = "绑定窗口";
            this.Bing_UnBing_change.UseVisualStyleBackColor = true;
            this.Bing_UnBing_change.CheckedChanged += new System.EventHandler(this.Bing_UnBing_change_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox8);
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Controls.Add(this.label_text_length);
            this.groupBox2.Controls.Add(this.textbox_info);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Location = new System.Drawing.Point(2, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 446);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "日志";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.Bn_TF_CT);
            this.groupBox8.Controls.Add(this.Cb_map_name);
            this.groupBox8.Controls.Add(this.Bn_fly_and_hit);
            this.groupBox8.Controls.Add(this.Cb_auto_fly);
            this.groupBox8.Location = new System.Drawing.Point(143, 338);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(119, 97);
            this.groupBox8.TabIndex = 24;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "自动飞怪";
            // 
            // Bn_TF_CT
            // 
            this.Bn_TF_CT.Location = new System.Drawing.Point(2, 67);
            this.Bn_TF_CT.Name = "Bn_TF_CT";
            this.Bn_TF_CT.Size = new System.Drawing.Size(66, 26);
            this.Bn_TF_CT.TabIndex = 26;
            this.Bn_TF_CT.Text = "真CT";
            this.Bn_TF_CT.UseVisualStyleBackColor = true;
            this.Bn_TF_CT.Click += new System.EventHandler(this.Bn_TF_CT_Click);
            // 
            // Cb_map_name
            // 
            this.Cb_map_name.FormattingEnabled = true;
            this.Cb_map_name.Location = new System.Drawing.Point(2, 46);
            this.Cb_map_name.Name = "Cb_map_name";
            this.Cb_map_name.Size = new System.Drawing.Size(66, 20);
            this.Cb_map_name.TabIndex = 25;
            this.Cb_map_name.SelectedIndexChanged += new System.EventHandler(this.Cb_map_name_SelectedIndexChanged);
            // 
            // Bn_fly_and_hit
            // 
            this.Bn_fly_and_hit.Location = new System.Drawing.Point(74, 49);
            this.Bn_fly_and_hit.Name = "Bn_fly_and_hit";
            this.Bn_fly_and_hit.Size = new System.Drawing.Size(39, 42);
            this.Bn_fly_and_hit.TabIndex = 4;
            this.Bn_fly_and_hit.Text = "自动飞怪";
            this.Bn_fly_and_hit.UseVisualStyleBackColor = true;
            this.Bn_fly_and_hit.Click += new System.EventHandler(this.Bn_fly_and_hit_Click);
            // 
            // Cb_auto_fly
            // 
            this.Cb_auto_fly.FormattingEnabled = true;
            this.Cb_auto_fly.Location = new System.Drawing.Point(2, 20);
            this.Cb_auto_fly.Name = "Cb_auto_fly";
            this.Cb_auto_fly.Size = new System.Drawing.Size(111, 20);
            this.Cb_auto_fly.TabIndex = 24;
            this.Cb_auto_fly.SelectedIndexChanged += new System.EventHandler(this.Cb_auto_fly_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.nameColumn,
            this.quantityColumn});
            this.dataGridView1.Location = new System.Drawing.Point(14, 224);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(248, 94);
            this.dataGridView1.TabIndex = 2;
            // 
            // idColumn
            // 
            this.idColumn.DataPropertyName = "ItemID";
            this.idColumn.HeaderText = "物品ID";
            this.idColumn.Name = "idColumn";
            this.idColumn.Width = 65;
            // 
            // nameColumn
            // 
            this.nameColumn.DataPropertyName = "Name";
            this.nameColumn.HeaderText = "物品名称";
            this.nameColumn.Name = "nameColumn";
            // 
            // quantityColumn
            // 
            this.quantityColumn.DataPropertyName = "Quantity";
            this.quantityColumn.HeaderText = "物品数量";
            this.quantityColumn.Name = "quantityColumn";
            this.quantityColumn.Width = 80;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.Bn_auto_fight);
            this.groupBox6.Controls.Add(this.Bn_start_auto_fb);
            this.groupBox6.Controls.Add(this.Cb_fb_name);
            this.groupBox6.Location = new System.Drawing.Point(14, 338);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(119, 97);
            this.groupBox6.TabIndex = 23;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "自动副本";
            // 
            // Bn_auto_fight
            // 
            this.Bn_auto_fight.Location = new System.Drawing.Point(76, 49);
            this.Bn_auto_fight.Name = "Bn_auto_fight";
            this.Bn_auto_fight.Size = new System.Drawing.Size(39, 42);
            this.Bn_auto_fight.TabIndex = 3;
            this.Bn_auto_fight.Text = "自动打怪";
            this.Bn_auto_fight.UseVisualStyleBackColor = true;
            this.Bn_auto_fight.Click += new System.EventHandler(this.Bn_auto_fight_Click);
            // 
            // Bn_start_auto_fb
            // 
            this.Bn_start_auto_fb.Location = new System.Drawing.Point(6, 49);
            this.Bn_start_auto_fb.Name = "Bn_start_auto_fb";
            this.Bn_start_auto_fb.Size = new System.Drawing.Size(39, 42);
            this.Bn_start_auto_fb.TabIndex = 1;
            this.Bn_start_auto_fb.Text = "开始副本";
            this.Bn_start_auto_fb.UseVisualStyleBackColor = true;
            this.Bn_start_auto_fb.Click += new System.EventHandler(this.Bn_start_auto_fb_Click);
            // 
            // Cb_fb_name
            // 
            this.Cb_fb_name.FormattingEnabled = true;
            this.Cb_fb_name.Items.AddRange(new object[] {
            "藏宝图通刷",
            "幽冥坛",
            "玲珑仙岛",
            "巨人巷",
            "伏魔令(赤)",
            "伏魔令(狐)",
            "伏魔令(月)",
            "伏魔令(雪)",
            "伏魔令(阎)",
            "伏魔令(破)"});
            this.Cb_fb_name.Location = new System.Drawing.Point(6, 20);
            this.Cb_fb_name.Name = "Cb_fb_name";
            this.Cb_fb_name.Size = new System.Drawing.Size(107, 20);
            this.Cb_fb_name.TabIndex = 0;
            this.Cb_fb_name.SelectedIndexChanged += new System.EventHandler(this.Cb_fb_name_SelectedIndexChanged);
            // 
            // label_wash_item
            // 
            this.label_wash_item.AutoSize = true;
            this.label_wash_item.Location = new System.Drawing.Point(10, 17);
            this.label_wash_item.Name = "label_wash_item";
            this.label_wash_item.Size = new System.Drawing.Size(143, 12);
            this.label_wash_item.TabIndex = 9;
            this.label_wash_item.Text = "洗练装备快捷键Ctrl+Num1";
            // 
            // checkBox_wash_item
            // 
            this.checkBox_wash_item.AutoSize = true;
            this.checkBox_wash_item.Location = new System.Drawing.Point(161, 17);
            this.checkBox_wash_item.Name = "checkBox_wash_item";
            this.checkBox_wash_item.Size = new System.Drawing.Size(15, 14);
            this.checkBox_wash_item.TabIndex = 10;
            this.checkBox_wash_item.UseVisualStyleBackColor = true;
            this.checkBox_wash_item.CheckedChanged += new System.EventHandler(this.checkBox_wash_item_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox_sorting_item);
            this.groupBox3.Controls.Add(this.label_sorting_item);
            this.groupBox3.Controls.Add(this.checkBox_fuc_2);
            this.groupBox3.Controls.Add(this.label_fuc_2);
            this.groupBox3.Controls.Add(this.label_wash_item);
            this.groupBox3.Controls.Add(this.checkBox_wash_item);
            this.groupBox3.Location = new System.Drawing.Point(364, 89);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(182, 93);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "勾选生效";
            // 
            // checkBox_sorting_item
            // 
            this.checkBox_sorting_item.AutoSize = true;
            this.checkBox_sorting_item.Location = new System.Drawing.Point(161, 66);
            this.checkBox_sorting_item.Name = "checkBox_sorting_item";
            this.checkBox_sorting_item.Size = new System.Drawing.Size(15, 14);
            this.checkBox_sorting_item.TabIndex = 14;
            this.checkBox_sorting_item.UseVisualStyleBackColor = true;
            this.checkBox_sorting_item.CheckedChanged += new System.EventHandler(this.checkBox_sorting_item_CheckedChanged);
            // 
            // label_sorting_item
            // 
            this.label_sorting_item.AutoSize = true;
            this.label_sorting_item.Location = new System.Drawing.Point(10, 66);
            this.label_sorting_item.Name = "label_sorting_item";
            this.label_sorting_item.Size = new System.Drawing.Size(143, 12);
            this.label_sorting_item.TabIndex = 13;
            this.label_sorting_item.Text = "理经验卷快捷键Ctrl+Num3";
            // 
            // checkBox_fuc_2
            // 
            this.checkBox_fuc_2.AutoSize = true;
            this.checkBox_fuc_2.Location = new System.Drawing.Point(161, 41);
            this.checkBox_fuc_2.Name = "checkBox_fuc_2";
            this.checkBox_fuc_2.Size = new System.Drawing.Size(15, 14);
            this.checkBox_fuc_2.TabIndex = 12;
            this.checkBox_fuc_2.UseVisualStyleBackColor = true;
            // 
            // label_fuc_2
            // 
            this.label_fuc_2.AutoSize = true;
            this.label_fuc_2.Location = new System.Drawing.Point(10, 41);
            this.label_fuc_2.Name = "label_fuc_2";
            this.label_fuc_2.Size = new System.Drawing.Size(143, 12);
            this.label_fuc_2.TabIndex = 11;
            this.label_fuc_2.Text = "快速寻怪快捷键Ctrl+Num2";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.bn_ini_item);
            this.groupBox4.Controls.Add(this.item_bar_4);
            this.groupBox4.Controls.Add(this.item_bar_3);
            this.groupBox4.Controls.Add(this.item_bar_2);
            this.groupBox4.Controls.Add(this.item_bar_1);
            this.groupBox4.Location = new System.Drawing.Point(559, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(122, 170);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "设置快捷栏";
            // 
            // bn_ini_item
            // 
            this.bn_ini_item.Location = new System.Drawing.Point(6, 124);
            this.bn_ini_item.Name = "bn_ini_item";
            this.bn_ini_item.Size = new System.Drawing.Size(110, 22);
            this.bn_ini_item.TabIndex = 4;
            this.bn_ini_item.Text = "初始化快捷栏";
            this.bn_ini_item.UseVisualStyleBackColor = true;
            this.bn_ini_item.Click += new System.EventHandler(this.bn_ini_item_Click);
            // 
            // item_bar_4
            // 
            this.item_bar_4.FormattingEnabled = true;
            this.item_bar_4.Items.AddRange(new object[] {
            "太阳水",
            "强效太阳水",
            "万年雪霜"});
            this.item_bar_4.Location = new System.Drawing.Point(6, 98);
            this.item_bar_4.Name = "item_bar_4";
            this.item_bar_4.Size = new System.Drawing.Size(110, 20);
            this.item_bar_4.TabIndex = 3;
            this.item_bar_4.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // item_bar_3
            // 
            this.item_bar_3.FormattingEnabled = true;
            this.item_bar_3.Items.AddRange(new object[] {
            "强效魔法药",
            "特效魔法药",
            "魔力药"});
            this.item_bar_3.Location = new System.Drawing.Point(6, 72);
            this.item_bar_3.Name = "item_bar_3";
            this.item_bar_3.Size = new System.Drawing.Size(110, 20);
            this.item_bar_3.TabIndex = 2;
            this.item_bar_3.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // item_bar_2
            // 
            this.item_bar_2.FormattingEnabled = true;
            this.item_bar_2.Items.AddRange(new object[] {
            "强效金创药",
            "特效金创药",
            "疗伤药"});
            this.item_bar_2.Location = new System.Drawing.Point(6, 46);
            this.item_bar_2.Name = "item_bar_2";
            this.item_bar_2.Size = new System.Drawing.Size(110, 20);
            this.item_bar_2.TabIndex = 1;
            this.item_bar_2.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // item_bar_1
            // 
            this.item_bar_1.FormattingEnabled = true;
            this.item_bar_1.Items.AddRange(new object[] {
            "大补丸（小）",
            "大补丸（中）",
            "大补丸（大）"});
            this.item_bar_1.Location = new System.Drawing.Point(6, 20);
            this.item_bar_1.Name = "item_bar_1";
            this.item_bar_1.Size = new System.Drawing.Size(110, 20);
            this.item_bar_1.TabIndex = 0;
            this.item_bar_1.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // cb_check_pet
            // 
            this.cb_check_pet.AutoSize = true;
            this.cb_check_pet.Location = new System.Drawing.Point(370, 188);
            this.cb_check_pet.Name = "cb_check_pet";
            this.cb_check_pet.Size = new System.Drawing.Size(120, 16);
            this.cb_check_pet.TabIndex = 13;
            this.cb_check_pet.Text = "道士自动召唤宝宝";
            this.cb_check_pet.UseVisualStyleBackColor = true;
            this.cb_check_pet.CheckedChanged += new System.EventHandler(this.cb_check_pet_CheckedChanged);
            // 
            // Bn_set_top
            // 
            this.Bn_set_top.Location = new System.Drawing.Point(6, 20);
            this.Bn_set_top.Name = "Bn_set_top";
            this.Bn_set_top.Size = new System.Drawing.Size(75, 23);
            this.Bn_set_top.TabIndex = 14;
            this.Bn_set_top.Text = "窗口置顶";
            this.Bn_set_top.UseVisualStyleBackColor = true;
            this.Bn_set_top.Click += new System.EventHandler(this.Bn_set_top_Click);
            // 
            // Bn_set_untop
            // 
            this.Bn_set_untop.Location = new System.Drawing.Point(87, 20);
            this.Bn_set_untop.Name = "Bn_set_untop";
            this.Bn_set_untop.Size = new System.Drawing.Size(75, 23);
            this.Bn_set_untop.TabIndex = 15;
            this.Bn_set_untop.Text = "取消置顶";
            this.Bn_set_untop.UseVisualStyleBackColor = true;
            this.Bn_set_untop.Click += new System.EventHandler(this.Bn_set_untop_Click);
            // 
            // bn_move_side
            // 
            this.bn_move_side.Location = new System.Drawing.Point(6, 49);
            this.bn_move_side.Name = "bn_move_side";
            this.bn_move_side.Size = new System.Drawing.Size(75, 23);
            this.bn_move_side.TabIndex = 16;
            this.bn_move_side.Text = "窗口贴边";
            this.bn_move_side.UseVisualStyleBackColor = true;
            this.bn_move_side.Click += new System.EventHandler(this.bn_move_side_Click);
            // 
            // bn_move_center
            // 
            this.bn_move_center.Location = new System.Drawing.Point(87, 49);
            this.bn_move_center.Name = "bn_move_center";
            this.bn_move_center.Size = new System.Drawing.Size(75, 23);
            this.bn_move_center.TabIndex = 17;
            this.bn_move_center.Text = "窗口居中";
            this.bn_move_center.UseVisualStyleBackColor = true;
            this.bn_move_center.Click += new System.EventHandler(this.bn_move_center_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Bn_set_top);
            this.groupBox5.Controls.Add(this.bn_move_center);
            this.groupBox5.Controls.Add(this.bn_move_side);
            this.groupBox5.Controls.Add(this.Bn_set_untop);
            this.groupBox5.Location = new System.Drawing.Point(359, 367);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(170, 80);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "游戏窗口设置";
            // 
            // cb_check_exp
            // 
            this.cb_check_exp.AutoSize = true;
            this.cb_check_exp.Location = new System.Drawing.Point(370, 210);
            this.cb_check_exp.Name = "cb_check_exp";
            this.cb_check_exp.Size = new System.Drawing.Size(132, 16);
            this.cb_check_exp.TabIndex = 19;
            this.cb_check_exp.Text = "挂机自动合并经验卷";
            this.cb_check_exp.UseVisualStyleBackColor = true;
            this.cb_check_exp.CheckedChanged += new System.EventHandler(this.cb_check_exp_CheckedChanged);
            // 
            // cb_auto_sg
            // 
            this.cb_auto_sg.AutoSize = true;
            this.cb_auto_sg.Location = new System.Drawing.Point(370, 232);
            this.cb_auto_sg.Name = "cb_auto_sg";
            this.cb_auto_sg.Size = new System.Drawing.Size(96, 16);
            this.cb_auto_sg.TabIndex = 20;
            this.cb_auto_sg.Text = "挂机自动刷怪";
            this.cb_auto_sg.UseVisualStyleBackColor = true;
            this.cb_auto_sg.CheckedChanged += new System.EventHandler(this.cb_auto_sg_CheckedChanged);
            // 
            // bn_test
            // 
            this.bn_test.Location = new System.Drawing.Point(559, 357);
            this.bn_test.Name = "bn_test";
            this.bn_test.Size = new System.Drawing.Size(46, 53);
            this.bn_test.TabIndex = 21;
            this.bn_test.Text = "测试";
            this.bn_test.UseVisualStyleBackColor = true;
            this.bn_test.Click += new System.EventHandler(this.bn_test_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(565, 330);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(95, 21);
            this.textBox1.TabIndex = 22;
            // 
            // Bn_select_file
            // 
            this.Bn_select_file.Location = new System.Drawing.Point(6, 32);
            this.Bn_select_file.Name = "Bn_select_file";
            this.Bn_select_file.Size = new System.Drawing.Size(45, 27);
            this.Bn_select_file.TabIndex = 24;
            this.Bn_select_file.Text = "加载";
            this.Bn_select_file.UseVisualStyleBackColor = true;
            this.Bn_select_file.Click += new System.EventHandler(this.Bn_select_file_Click);
            // 
            // File_path
            // 
            this.File_path.AutoSize = true;
            this.File_path.Location = new System.Drawing.Point(8, 17);
            this.File_path.Name = "File_path";
            this.File_path.Size = new System.Drawing.Size(77, 12);
            this.File_path.TabIndex = 25;
            this.File_path.Text = "杂物配置文件";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.Reload);
            this.groupBox7.Controls.Add(this.File_path);
            this.groupBox7.Controls.Add(this.Bn_select_file);
            this.groupBox7.Location = new System.Drawing.Point(559, 188);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(122, 60);
            this.groupBox7.TabIndex = 26;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "杂物配置文件";
            // 
            // Reload
            // 
            this.Reload.Location = new System.Drawing.Point(52, 32);
            this.Reload.Name = "Reload";
            this.Reload.Size = new System.Drawing.Size(45, 27);
            this.Reload.TabIndex = 26;
            this.Reload.Text = "刷新";
            this.Reload.UseVisualStyleBackColor = true;
            this.Reload.Click += new System.EventHandler(this.Reload_Click);
            // 
            // Bn_test_1
            // 
            this.Bn_test_1.Location = new System.Drawing.Point(620, 357);
            this.Bn_test_1.Name = "Bn_test_1";
            this.Bn_test_1.Size = new System.Drawing.Size(40, 53);
            this.Bn_test_1.TabIndex = 27;
            this.Bn_test_1.Text = "开始";
            this.Bn_test_1.UseVisualStyleBackColor = true;
            this.Bn_test_1.Click += new System.EventHandler(this.button1_Click);
            // 
            // itemInfoBindingSource
            // 
            this.itemInfoBindingSource.DataSource = typeof(BMHelper.Form1.ItemInfo);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 450);
            this.Controls.Add(this.Bn_test_1);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bn_test);
            this.Controls.Add(this.cb_auto_sg);
            this.Controls.Add(this.cb_check_exp);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.cb_check_pet);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bn_stop);
            this.Controls.Add(this.bn_start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Leave += new System.EventHandler(this.Form1_Leave);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textbox_info;
        private System.Windows.Forms.Label label_text_length;
        private System.Windows.Forms.Button bn_start;
        private System.Windows.Forms.Button bn_stop;
        private System.Windows.Forms.Button bn_find_wnd;
        private System.Windows.Forms.ComboBox cb_playername;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_wash_item;
        private System.Windows.Forms.CheckBox checkBox_wash_item;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_sorting_item;
        private System.Windows.Forms.CheckBox checkBox_fuc_2;
        private System.Windows.Forms.Label label_fuc_2;
        private System.Windows.Forms.CheckBox checkBox_sorting_item;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox item_bar_3;
        private System.Windows.Forms.ComboBox item_bar_2;
        private System.Windows.Forms.ComboBox item_bar_1;
        private System.Windows.Forms.ComboBox item_bar_4;
        private System.Windows.Forms.Button bn_ini_item;
        private System.Windows.Forms.CheckBox cb_check_pet;
        private System.Windows.Forms.CheckBox Bing_UnBing_change;
        private System.Windows.Forms.Button Bn_set_top;
        private System.Windows.Forms.Button Bn_set_untop;
        private System.Windows.Forms.Button bn_move_side;
        private System.Windows.Forms.Button bn_move_center;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cb_check_exp;
        private System.Windows.Forms.CheckBox cb_auto_sg;
        private System.Windows.Forms.Button bn_test;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button Bn_start_auto_fb;
        private System.Windows.Forms.ComboBox Cb_fb_name;
        private System.Windows.Forms.Button Bn_auto_fight;
        private System.Windows.Forms.Button Bn_select_file;
        private System.Windows.Forms.Label File_path;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button Reload;
        private Button Bn_fly_and_hit;
        private Button Bn_test_1;
        private ComboBox Cb_auto_fly;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn idColumn;
        private DataGridViewTextBoxColumn nameColumn;
        private DataGridViewTextBoxColumn quantityColumn;
        private GroupBox groupBox8;
        private ComboBox Cb_map_name;
        private Button Bn_TF_CT;
        private BindingSource itemInfoBindingSource;
    }
}

