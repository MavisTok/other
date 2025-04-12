using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ScheduleICSGenerator.Helpers; // 添加包含ICSFileHelper的命名空间

namespace ScheduleICSGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
            // 添加按钮的点击事件处理程序
            btnOpenFile.Click += BtnOpenFile_Click;
            btnGenerateICS.Click += BtnGenerateICS_Click;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 自动加载上次打开的文件
            string icsContent = ICSFileHelper.AutoLoadLastFile();
            if (!string.IsNullOrEmpty(icsContent))
            {
                // TODO: 处理加载的ICS文件内容
                // 例如：ParseAndDisplayICSContent(icsContent);
            }

            // 加载特殊工作日和休息日
            List<DateTime> specialWorkDays = ICSFileHelper.GetSpecialWorkDays();
            List<DateTime> specialRestDays = ICSFileHelper.GetSpecialRestDays();

            // TODO: 在界面上显示或处理这些特殊日期
            // 例如：UpdateCalendarWithSpecialDays(specialWorkDays, specialRestDays);
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "ICS 文件 (*.ics)|*.ics|所有文件 (*.*)|*.*",
                Title = "选择 ICS 文件"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                statusLabel.Text = $"已打开文件: {filePath}";
                
                // TODO: 处理打开的文件
            }
        }

        private void BtnGenerateICS_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "ICS 文件 (*.ics)|*.ics",
                Title = "保存 ICS 文件"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                statusLabel.Text = $"正在生成 ICS 文件: {filePath}";
                
                // TODO: 生成并保存 ICS 文件
                
                MessageBox.Show("ICS 文件生成完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                statusLabel.Text = $"ICS 文件已保存至: {filePath}";
            }
        }

        private void SomeMethod()
        {
            // 使用 ICSFileHelper 的示例
            var helper = new ICSFileHelper();
            // ...
        }
    }
}