using System.Globalization;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace ScheduleICSGenerator;

public partial class Form1 : Form
{
    // 用于存储特殊日期的类
    private class SpecialDay
    {
        public DateTime Date { get; set; }
        public bool IsRestDay { get; set; } // true表示休息日，false表示工作日

        public override string ToString()
        {
            string dayType = IsRestDay ? "[休]" : "[班]";
            return $"{dayType} {Date:yyyy-MM-dd (ddd)}";
        }
    }

    private List<SpecialDay> specialDays = new List<SpecialDay>();

    public Form1()
    {
        InitializeComponent();
        
        // 设置日期选择器的初始值
        startDatePicker.Value = DateTime.Today;
        endDatePicker.Value = DateTime.Today.AddDays(30); // 默认一个月的排班周期
        
        // 初始化CheckedListBox
        UpdateSpecialDaysListBox();
        
        // 初始化大小周界面
        groupBoxShiftType.Enabled = checkBoxUseShiftSystem.Checked;
    }

    private void UpdateSpecialDaysListBox()
    {
        restDaysCheckedListBox.Items.Clear();
        foreach (var specialDay in specialDays.OrderBy(d => d.Date))
        {
            restDaysCheckedListBox.Items.Add(specialDay, true);
        }
    }
    
    private void checkBoxUseShiftSystem_CheckedChanged(object sender, EventArgs e)
    {
        // 启用或禁用岗位选择框
        groupBoxShiftType.Enabled = checkBoxUseShiftSystem.Checked;
    }

    private void addRestDayButton_Click(object sender, EventArgs e)
    {
        AddSpecialDay(true);
    }
    
    private void addWorkDayButton_Click(object sender, EventArgs e)
    {
        AddSpecialDay(false);
    }
    
    private void AddSpecialDay(bool isRestDay)
    {
        // 创建日期选择对话框
        using (var picker = new DateTimePicker())
        {
            string title = isRestDay ? "选择特殊休息日" : "选择特殊工作日";
            
            var dialog = new Form
            {
                Width = 300,
                Height = 170,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                Text = title,
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = false
            };

            picker.Format = DateTimePickerFormat.Short;
            picker.Value = DateTime.Today;
            picker.Location = new Point(20, 20);
            picker.Size = new Size(250, 40);

            var okButton = new Button
            {
                Text = "确定",
                DialogResult = DialogResult.OK,
                Location = new Point(110, 80),
                Size = new Size(70, 30)
            };

            dialog.Controls.Add(picker);
            dialog.Controls.Add(okButton);
            dialog.AcceptButton = okButton;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var selectedDate = picker.Value.Date;
                
                // 检查选择的日期是否在开始和结束日期之间
                if (selectedDate < startDatePicker.Value.Date || selectedDate > endDatePicker.Value.Date)
                {
                    MessageBox.Show("请选择在开始日期和结束日期之间的日期。", "日期范围错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 检查日期是否已经添加
                var existingDay = specialDays.FirstOrDefault(d => d.Date == selectedDate);
                if (existingDay != null)
                {
                    // 如果日期已存在但类型不同，询问是否更改
                    if (existingDay.IsRestDay != isRestDay)
                    {
                        string message = isRestDay 
                            ? $"{selectedDate:yyyy-MM-dd} 已被设置为特殊工作日，是否改为特殊休息日？" 
                            : $"{selectedDate:yyyy-MM-dd} 已被设置为特殊休息日，是否改为特殊工作日？";
                        
                        if (MessageBox.Show(message, "更改日期类型", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            existingDay.IsRestDay = isRestDay;
                            UpdateSpecialDaysListBox();
                            resultLabel.Text = $"已将 {selectedDate:yyyy-MM-dd} 改为{(isRestDay ? "特殊休息日" : "特殊工作日")}";
                        }
                    }
                    else
                    {
                        resultLabel.Text = $"该日期已添加为{(isRestDay ? "特殊休息日" : "特殊工作日")}";
                    }
                }
                else
                {
                    // 添加新日期
                    specialDays.Add(new SpecialDay { Date = selectedDate, IsRestDay = isRestDay });
                    UpdateSpecialDaysListBox();
                    resultLabel.Text = $"已添加{(isRestDay ? "特殊休息日" : "特殊工作日")}: {selectedDate:yyyy-MM-dd}";
                }
            }
        }
    }
    
    private void restDaysCheckedListBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete && restDaysCheckedListBox.SelectedItem != null)
        {
            var selectedDay = restDaysCheckedListBox.SelectedItem as SpecialDay;
            if (selectedDay != null)
            {
                specialDays.Remove(selectedDay);
                UpdateSpecialDaysListBox();
                resultLabel.Text = $"已删除日期: {selectedDay.Date:yyyy-MM-dd}";
            }
        }
    }

    private void generateICSButton_Click(object sender, EventArgs e)
    {
        try
        {
            var startDate = startDatePicker.Value.Date;
            var endDate = endDatePicker.Value.Date;

            if (endDate < startDate)
            {
                MessageBox.Show("结束日期必须晚于开始日期", "日期范围错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取用户选择的特殊日期
            var checkedSpecialDays = new List<SpecialDay>();
            for (int i = 0; i < restDaysCheckedListBox.CheckedItems.Count; i++)
            {
                if (restDaysCheckedListBox.CheckedItems[i] is SpecialDay specialDay)
                {
                    checkedSpecialDays.Add(specialDay);
                }
            }

            // 生成ICS文件
            var calendar = new Ical.Net.Calendar();
            
            // 设置日历属性
            calendar.ProductId = "-//排班ICS生成器//CN";
            calendar.Scale = "GREGORIAN";
            
            // 添加工作日事件 - 确保包含开始日期到结束日期的所有天
            var currentDate = startDate;
            // 使用小于等于确保包含结束日期
            while (currentDate <= endDate)
            {
                bool isRestDay = false;
                
                // 检查是否为特殊日期（优先级最高）
                var specialDay = checkedSpecialDays.FirstOrDefault(d => d.Date == currentDate);
                if (specialDay != null)
                {
                    // 使用特殊日期的设置
                    isRestDay = specialDay.IsRestDay;
                }
                else if (checkBoxUseShiftSystem.Checked)
                {
                    // 使用大小周排班制度
                    bool isShiftA = radioButtonShiftA.Checked;
                    
                    // 判断周六是否为休息日
                    if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        // 获取日期的号数
                        int dayOfMonth = currentDate.Day;
                        
                        // A岗: 周六偶数日期休息，单数日期上班
                        // B岗: 周六单数日期休息，偶数日期上班
                        if (isShiftA)
                        {
                            isRestDay = dayOfMonth % 2 == 0; // A岗偶数休息
                        }
                        else
                        {
                            isRestDay = dayOfMonth % 2 != 0; // B岗单数休息
                        }
                    }
                    else if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        // 周日都休息
                        isRestDay = true;
                    }
                    else
                    {
                        // 工作日
                        isRestDay = false;
                    }
                }
                else
                {
                    // 传统的周末休息制度
                    isRestDay = currentDate.DayOfWeek == DayOfWeek.Saturday || 
                                currentDate.DayOfWeek == DayOfWeek.Sunday;
                }

                if (isRestDay)
                {
                    // 添加休息日事件
                    var restEvent = new CalendarEvent
                    {
                        Start = new CalDateTime(currentDate, "Asia/Shanghai"),
                        End = new CalDateTime(currentDate.AddDays(1), "Asia/Shanghai"),
                        Summary = "休息日",
                        Description = "排班休息日",
                        IsAllDay = true,
                    };
                    calendar.Events.Add(restEvent);
                }
                else
                {
                    // 添加工作日事件
                    var workEvent = new CalendarEvent
                    {
                        Start = new CalDateTime(currentDate, "Asia/Shanghai"),
                        End = new CalDateTime(currentDate.AddDays(1), "Asia/Shanghai"),
                        Summary = "工作日",
                        Description = "排班工作日",
                        IsAllDay = true,
                    };
                    calendar.Events.Add(workEvent);
                }

                // 增加一天，继续循环
                currentDate = currentDate.AddDays(1);
            }

            // 序列化日历到ICS文件
            var serializer = new CalendarSerializer();
            var icsContent = serializer.SerializeToString(calendar);

            // 保存ICS文件
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "ICS文件|*.ics";
                saveFileDialog.Title = "保存排班ICS文件";
                
                // 在文件名中添加岗位信息
                string shiftInfo = checkBoxUseShiftSystem.Checked 
                    ? (radioButtonShiftA.Checked ? "_A岗" : "_B岗") 
                    : "";
                saveFileDialog.FileName = $"排班{shiftInfo}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.ics";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, icsContent);
                    resultLabel.Text = $"已生成{startDate:yyyy-MM-dd}至{endDate:yyyy-MM-dd}的排班ICS文件";
                    MessageBox.Show($"ICS文件已保存到: {saveFileDialog.FileName}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        catch (Exception ex)
        {
            resultLabel.Text = $"生成ICS文件失败: {ex.Message}";
            MessageBox.Show($"生成ICS文件出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
