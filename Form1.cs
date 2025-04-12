using System.Globalization;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System.Diagnostics;
using System.Net;
using System.Text;
using WebDav;
using ScheduleICSGenerator.Properties;
using System.Text.Json;

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

    // 使用HolidayManager管理节假日
    private readonly HolidayManager holidayManager = new HolidayManager();

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

        // 加载保存的 WebDAV 设置
        LoadWebDavSettings();
        
        // 加载节假日信息
        holidayManager.LoadHolidays();
    }

    private void LoadWebDavSettings()
    {
        textBoxWebDavUrl.Text = Properties.Settings.Default.WebDavUrl ?? "";
        textBoxWebDavUser.Text = Properties.Settings.Default.WebDavUser ?? "";
        textBoxWebDavPassword.Text = Properties.Settings.Default.WebDavPassword ?? "";

        bool webDavConfigured = !string.IsNullOrWhiteSpace(textBoxWebDavUrl.Text);
        checkBoxUploadWebDav.Checked = webDavConfigured;
        textBoxWebDavUrl.Enabled = webDavConfigured;
        textBoxWebDavUser.Enabled = webDavConfigured;
        textBoxWebDavPassword.Enabled = webDavConfigured;
    }

    private void SaveWebDavSettings()
    {
        Properties.Settings.Default.WebDavUrl = textBoxWebDavUrl.Text;
        Properties.Settings.Default.WebDavUser = textBoxWebDavUser.Text;
        Properties.Settings.Default.WebDavPassword = textBoxWebDavPassword.Text;
        Properties.Settings.Default.Save();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        SaveWebDavSettings();
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
                
                if (selectedDate < startDatePicker.Value.Date || selectedDate > endDatePicker.Value.Date)
                {
                    MessageBox.Show("请选择在开始日期和结束日期之间的日期。", "日期范围错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                var existingDay = specialDays.FirstOrDefault(d => d.Date == selectedDate);
                if (existingDay != null)
                {
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
                    specialDays.Add(new SpecialDay { Date = selectedDate, IsRestDay = isRestDay });
                    UpdateSpecialDaysListBox();
                    resultLabel.Text = $"已添加{(isRestDay ? "特殊休息日" : "特殊工作日")}: {selectedDate:yyyy-MM-dd}";
                }
            }
        }
    }

    private void checkBoxUploadWebDav_CheckedChanged(object sender, EventArgs e)
    {
        bool isChecked = checkBoxUploadWebDav.Checked;
        textBoxWebDavUrl.Enabled = isChecked;
        textBoxWebDavUser.Enabled = isChecked;
        textBoxWebDavPassword.Enabled = isChecked;
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

    private async void generateICSButton_Click(object sender, EventArgs e)
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

            var checkedSpecialDays = new List<SpecialDay>();
            for (int i = 0; i < restDaysCheckedListBox.CheckedItems.Count; i++)
            {
                if (restDaysCheckedListBox.CheckedItems[i] is SpecialDay specialDay)
                {
                    checkedSpecialDays.Add(specialDay);
                }
            }

            var calendar = new Ical.Net.Calendar();
            
            calendar.ProductId = "-//排班ICS生成器//CN";
            calendar.Scale = "GREGORIAN";
            
            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                bool isRestDay = false;
                
                var specialDay = checkedSpecialDays.FirstOrDefault(d => d.Date == currentDate);
                if (specialDay != null)
                {
                    isRestDay = specialDay.IsRestDay;
                }
                else
                {
                    if (holidayManager.IsHoliday(currentDate))
                    {
                        isRestDay = true;
                    }
                    else if (holidayManager.IsWorkOnWeekend(currentDate))
                    {
                        isRestDay = false;
                    }
                    else if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        isRestDay = true;
                    }
                    else if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        if (checkBoxUseShiftSystem.Checked)
                        {
                            bool isShiftA = radioButtonShiftA.Checked;
                            
                            int dayOfMonth = currentDate.Day;
                            bool isOddDay = dayOfMonth % 2 != 0;
                            
                            if (isShiftA)
                            {
                                isRestDay = !isOddDay;
                            }
                            else
                            {
                                isRestDay = isOddDay;
                            }
                        }
                        else
                        {
                            isRestDay = true;
                        }
                    }
                    else
                    {
                        isRestDay = false;
                    }
                }

                if (isRestDay)
                {
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

                currentDate = currentDate.AddDays(1);
            }

            var serializer = new CalendarSerializer();
            var icsContent = serializer.SerializeToString(calendar);

            string shiftInfo = checkBoxUseShiftSystem.Checked
                ? (radioButtonShiftA.Checked ? "_A岗" : "_B岗")
                : "";
            string baseFileName = $"排班{shiftInfo}.ics";
            string fixedDirectory = @"E:\.Net\ics";
            string localFullPath = Path.Combine(fixedDirectory, baseFileName);

            bool githubAttempted = checkBoxAutoUpload.Checked;
            string finalSavePath = "";

            if (githubAttempted)
            {
                resultLabel.Text = "正在保存并尝试上传到 GitHub...";
                try
                {
                    Directory.CreateDirectory(fixedDirectory);
                    File.WriteAllText(localFullPath, icsContent);
                    resultLabel.Text = $"文件已保存到本地: {localFullPath}。准备上传到 GitHub...";

                    string gitCommands = $"cd /d \"{fixedDirectory}\" && git add \"{baseFileName}\" && git commit -m \"Update schedule {DateTime.Now:yyyy-MM-dd HH:mm}\" && git push https://github.com/MavisTok/other HEAD";
                    ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", $"/c {gitCommands}")
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = fixedDirectory
                    };

                    using (Process process = new Process { StartInfo = procStartInfo })
                    {
                        process.Start();
                        string output = await process.StandardOutput.ReadToEndAsync();
                        string error = await process.StandardError.ReadToEndAsync();
                        await process.WaitForExitAsync();

                        if (process.ExitCode == 0)
                        {
                            finalSavePath = localFullPath;
                            resultLabel.Text = $"文件已保存并成功推送到 GitHub: {localFullPath}";
                        }
                        else
                        {
                            resultLabel.Text = $"文件已保存本地，但 GitHub 推送失败。错误: {error}";
                            MessageBox.Show($"文件已保存本地，但 GitHub 推送失败。\n错误信息:\n{error}\n\n输出:\n{output}", "GitHub上传失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception gitEx)
                {
                     resultLabel.Text = $"保存到本地或执行 Git 命令时出错: {gitEx.Message}";
                     MessageBox.Show($"保存到本地或执行 Git 命令时出错: {gitEx.Message}", "GitHub错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            bool webDavAttempted = checkBoxUploadWebDav.Checked;
            if (webDavAttempted)
            {
                resultLabel.Text = (resultLabel.Text.Length > 0 ? resultLabel.Text + Environment.NewLine : "") + "正在尝试上传到 WebDAV...";
                string webDavUrl = textBoxWebDavUrl.Text.Trim();
                string webDavUser = textBoxWebDavUser.Text.Trim();
                string webDavPassword = textBoxWebDavPassword.Text;

                if (string.IsNullOrWhiteSpace(webDavUrl))
                {
                    MessageBox.Show("请输入 WebDAV URL。", "WebDAV 错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    resultLabel.Text += " WebDAV URL 为空，上传取消。";
                }
                else
                {
                    if (!webDavUrl.EndsWith("/"))
                    {
                        webDavUrl += "/";
                    }
                    string webDavFilePath = webDavUrl + baseFileName;

                    try
                    {
                        var clientParams = new WebDavClientParams
                        {
                            BaseAddress = new Uri(webDavUrl),
                            Credentials = (!string.IsNullOrEmpty(webDavUser) || !string.IsNullOrEmpty(webDavPassword))
                                            ? new NetworkCredential(webDavUser, webDavPassword)
                                            : null
                        };

                        using (var client = new WebDavClient(clientParams))
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(icsContent)))
                        {
                            var response = await client.PutFile(webDavFilePath, stream);

                            if (response.IsSuccessful)
                            {
                                finalSavePath = webDavFilePath;
                                resultLabel.Text = (resultLabel.Text.Contains("GitHub") ? resultLabel.Text + Environment.NewLine : "") + $"文件已成功上传到 WebDAV: {webDavFilePath}";
                            }
                            else
                            {
                                resultLabel.Text += $" WebDAV 上传失败。状态码: {response.StatusCode}, 描述: {response.Description}";
                                MessageBox.Show($"WebDAV 上传失败。\nURL: {webDavFilePath}\n状态码: {response.StatusCode}\n描述: {response.Description}", "WebDAV 上传失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception webDavEx)
                    {
                        resultLabel.Text += $" WebDAV 上传出错: {webDavEx.Message}";
                        MessageBox.Show($"WebDAV 上传过程中出错: {webDavEx.Message}\n请检查 URL、用户名和密码是否正确，以及网络连接。", "WebDAV 错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (!githubAttempted && !webDavAttempted)
            {
                resultLabel.Text = "请选择保存位置...";
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "ICS文件|*.ics";
                    saveFileDialog.Title = "保存排班ICS文件";
                    saveFileDialog.FileName = baseFileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            File.WriteAllText(saveFileDialog.FileName, icsContent);
                            finalSavePath = saveFileDialog.FileName;
                            resultLabel.Text = $"文件已保存到: {finalSavePath}";
                            MessageBox.Show($"ICS文件已保存到: {finalSavePath}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception saveEx)
                        {
                            resultLabel.Text = $"保存文件时出错: {saveEx.Message}";
                            MessageBox.Show($"保存文件时出错: {saveEx.Message}", "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                         resultLabel.Text = "用户取消保存。";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            resultLabel.Text = $"生成ICS文件或处理过程中出错: {ex.Message}";
            MessageBox.Show($"生成ICS文件或处理过程中出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void updateHolidaysButton_Click(object sender, EventArgs e)
    {
        try
        {
            resultLabel.Text = "正在更新节假日信息...";
            bool success = await holidayManager.UpdateHolidaysAsync();
            
            if (success)
            {
                resultLabel.Text = $"节假日信息更新成功，最后更新时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                MessageBox.Show("节假日信息已成功更新！", "更新成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                resultLabel.Text = "节假日信息更新失败，请检查网络连接或稍后再试";
                MessageBox.Show("无法更新节假日信息，请检查网络连接后重试。", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            resultLabel.Text = $"更新节假日信息时出错: {ex.Message}";
            MessageBox.Show($"更新节假日信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void loadICSButton_Click(object sender, EventArgs e)
    {
        try
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "ICS文件|*.ics|所有文件|*.*";
                openFileDialog.Title = "选择ICS排班文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(filePath);
                    string icsContent = File.ReadAllText(filePath);
                    
                    bool isShiftA = fileName.Contains("A岗");
                    bool isShiftB = fileName.Contains("B岗");
                    bool useShiftSystem = isShiftA || isShiftB;
                    
                    checkBoxUseShiftSystem.Checked = useShiftSystem;
                    if (isShiftA)
                        radioButtonShiftA.Checked = true;
                    else if (isShiftB)
                        radioButtonShiftB.Checked = true;
                    
                    var calendar = Ical.Net.Calendar.Load(icsContent);
                    
                    specialDays.Clear();
                    
                    if (calendar.Events.Count > 0)
                    {
                        var allDates = calendar.Events.Select(e => e.Start.Date).OrderBy(d => d).ToList();
                        if (allDates.Any())
                        {
                            startDatePicker.Value = allDates.First();
                            endDatePicker.Value = allDates.Last();
                        }
                    }

                    foreach (var evt in calendar.Events)
                    {
                        if (evt.IsAllDay)
                        {
                            DateTime eventDate = evt.Start.Date;
                            bool isWorkEvent = evt.Summary == "工作日";
                            bool isRestEvent = evt.Summary == "休息日";
                            
                            if (isWorkEvent || isRestEvent)
                            {
                                bool isSpecialDay = false;
                                
                                if (eventDate.DayOfWeek == DayOfWeek.Sunday && isWorkEvent)
                                {
                                    if (!holidayManager.IsWorkOnWeekend(eventDate))
                                    {
                                        isSpecialDay = true;
                                    }
                                }
                                else if ((eventDate.DayOfWeek >= DayOfWeek.Monday && 
                                          eventDate.DayOfWeek <= DayOfWeek.Friday) && 
                                          isRestEvent)
                                {
                                    if (!holidayManager.IsHoliday(eventDate))
                                    {
                                        isSpecialDay = true;
                                    }
                                }
                                else if (eventDate.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    bool shouldWork = holidayManager.IsWorkOnWeekend(eventDate);
                                    
                                    if (!shouldWork && useShiftSystem)
                                    {
                                        int dayOfMonth = eventDate.Day;
                                        bool isOddDay = dayOfMonth % 2 != 0;
                                        
                                        bool shouldBeWorkDay = (isShiftA && isOddDay) || (isShiftB && !isOddDay);
                                        
                                        if ((shouldBeWorkDay && isRestEvent) || (!shouldBeWorkDay && isWorkEvent))
                                        {
                                            isSpecialDay = true;
                                        }
                                    }
                                    else
                                    {
                                        if ((shouldWork && isRestEvent) || (!shouldWork && isWorkEvent && !useShiftSystem))
                                        {
                                            isSpecialDay = true;
                                        }
                                    }
                                }
                                
                                if (isSpecialDay)
                                {
                                    specialDays.Add(new SpecialDay 
                                    { 
                                        Date = eventDate, 
                                        IsRestDay = isRestEvent 
                                    });
                                }
                            }
                        }
                    }

                    UpdateSpecialDaysListBox();
                    resultLabel.Text = $"已从ICS文件加载 {specialDays.Count} 个特殊日期";
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"读取ICS文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            resultLabel.Text = $"读取ICS文件失败: {ex.Message}";
        }
    }
}
