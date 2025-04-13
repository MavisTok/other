using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ScheduleICSGenerator
{
    public class HolidayManager
    {
        // 节假日和调休日数据
        private List<DateTime> holidays = new List<DateTime>();
        private List<DateTime> workOnWeekends = new List<DateTime>();

        // 配置文件路径
        private readonly string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "holidays.json");
        //检查这个路径是否合法
        private readonly string configDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
        // 添加公开属性以访问配置文件路径
        public string ConfigFilePath => configFilePath;
        // 构造函数
        public HolidayManager()
        {
            // 检查配置文件目录是否存在，如果不存在则创建
            if (!Directory.Exists(configDirPath))
            {
                Directory.CreateDirectory(configDirPath);
            }
            // 确保初始化时配置文件存在
            if (!File.Exists(configFilePath))
            {
                // 配置文件不存在时，使用默认2025年数据
                InitDefaultHolidays();
                SaveHolidays();
            }
        }

        // 初始化默认的2025年节假日数据（如果没有配置文件）
        private void InitDefaultHolidays()
        {
            // 元旦：2025年1月1日(周三)放假1天
            holidays.Add(new DateTime(2025, 1, 1));
            
            // 春节：1月28日(农历除夕,周二)至2月4日(正月初七,周二)放假
            holidays.Add(new DateTime(2025, 1, 28));
            holidays.Add(new DateTime(2025, 1, 29));
            holidays.Add(new DateTime(2025, 1, 30));
            holidays.Add(new DateTime(2025, 1, 31));
            holidays.Add(new DateTime(2025, 2, 1));
            holidays.Add(new DateTime(2025, 2, 2));
            holidays.Add(new DateTime(2025, 2, 3));
            holidays.Add(new DateTime(2025, 2, 4));
            
            // 清明节：4月4日(周五)至6日(周日)放假
            holidays.Add(new DateTime(2025, 4, 4));
            holidays.Add(new DateTime(2025, 4, 5));
            holidays.Add(new DateTime(2025, 4, 6));
            
            // 劳动节：5月1日(周四)至5日(周一)放假
            holidays.Add(new DateTime(2025, 5, 1));
            holidays.Add(new DateTime(2025, 5, 2));
            holidays.Add(new DateTime(2025, 5, 3));
            holidays.Add(new DateTime(2025, 5, 4));
            holidays.Add(new DateTime(2025, 5, 5));
            
            // 端午节：5月31日(周六)至6月2日(周一)放假
            holidays.Add(new DateTime(2025, 5, 31));
            holidays.Add(new DateTime(2025, 6, 1));
            holidays.Add(new DateTime(2025, 6, 2));
            
            // 中秋节与国庆节：10月1日(周三)至8日(周三)放假
            holidays.Add(new DateTime(2025, 10, 1));
            holidays.Add(new DateTime(2025, 10, 2));
            holidays.Add(new DateTime(2025, 10, 3));
            holidays.Add(new DateTime(2025, 10, 4));
            holidays.Add(new DateTime(2025, 10, 5));
            holidays.Add(new DateTime(2025, 10, 6));
            holidays.Add(new DateTime(2025, 10, 7));
            holidays.Add(new DateTime(2025, 10, 8));

            // 调休上班日（周末需要上班的日期）
            // 春节调休
            workOnWeekends.Add(new DateTime(2025, 1, 26)); // 周日
            workOnWeekends.Add(new DateTime(2025, 2, 8));  // 周六
            
            // 劳动节调休
            workOnWeekends.Add(new DateTime(2025, 4, 27)); // 周日
            
            // 国庆节调休
            workOnWeekends.Add(new DateTime(2025, 9, 28)); // 周日
            workOnWeekends.Add(new DateTime(2025, 10, 11)); // 周六
        }

        // 从配置文件加载节假日
        public void LoadHolidays()
        {
            try
            {
                if (File.Exists(configFilePath))
                {
                    string json = File.ReadAllText(configFilePath);
                    var data = JsonSerializer.Deserialize<HolidayData>(json);
                    
                    holidays = data.Holidays?.Select(s => DateTime.Parse(s)).ToList() ?? new List<DateTime>();
                    workOnWeekends = data.WorkOnWeekends?.Select(s => DateTime.Parse(s)).ToList() ?? new List<DateTime>();
                }
                else
                {
                    // 如果配置文件不存在，使用默认数据
                    InitDefaultHolidays();
                    SaveHolidays();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载节假日配置时出错: {ex.Message}\n将使用默认数据。", 
                    "加载错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // 出错时使用默认数据
                InitDefaultHolidays();
            }
        }

        // 保存节假日到配置文件
        public void SaveHolidays()
        {
            try
            {
                var holidayData = new HolidayData
                {
                    Holidays = holidays.Select(d => d.ToString("yyyy-MM-dd")).ToList(),
                    WorkOnWeekends = workOnWeekends.Select(d => d.ToString("yyyy-MM-dd")).ToList()
                };

                string json = JsonSerializer.Serialize(holidayData, new JsonSerializerOptions
                {
                    WriteIndented = true // 格式化JSON以便于阅读
                });
                //在exe运行目录下创建配置文件
                // 这里使用AppDomain.CurrentDomain.BaseDirectory获取当前应用程序的目录
                // 也可以使用Environment.CurrentDirectory获取当前工作目录
                // 也可以使用Path.Combine来构建路径

                File.WriteAllText(configFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存节假日配置时出错: {ex.Message}", 
                    "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 从API更新节假日信息（这里使用示例API，实际使用时需要替换为可用的API）
        public async Task<bool> UpdateFromApiAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // 设置超时时间
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // 获取当前年份
                    int currentYear = DateTime.Now.Year;

                    // 使用timor.tech开放API获取中国法定节假日数据
                    string apiUrl = $"http://timor.tech/api/holiday/year/{currentYear}";

                    // 添加日志以便调试
                    Console.WriteLine($"正在请求API: {apiUrl}");

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // 输出状态码以便调试
                    Console.WriteLine($"API响应状态码: {(int)response.StatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        // 改用与UpdateHolidaysAsync一致的方式处理数据
                        using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                        {
                            JsonElement root = doc.RootElement;

                            // 检查API返回状态
                            if (root.TryGetProperty("code", out JsonElement codeElement) &&
                                codeElement.GetInt32() == 0)
                            {
                                // 清空现有数据
                                holidays.Clear();
                                workOnWeekends.Clear();

                                // 解析节假日数据
                                if (root.TryGetProperty("holiday", out JsonElement holidayElement))
                                {
                                    foreach (JsonProperty item in holidayElement.EnumerateObject())
                                    {
                                        string dateStr = item.Name; // 格式: yyyy-MM-dd

                                        if (DateTime.TryParse(dateStr, out DateTime date))
                                        {
                                            var details = item.Value;

                                            // 检查是否是节假日或调休工作日
                                            if (details.TryGetProperty("holiday", out JsonElement isHolidayElement))
                                            {
                                                bool isHoliday = isHolidayElement.GetBoolean();

                                                if (isHoliday)
                                                {
                                                    holidays.Add(date);
                                                }
                                                else if (details.TryGetProperty("workday", out JsonElement isWorkdayElement) &&
                                                        isWorkdayElement.GetBoolean())
                                                {
                                                    workOnWeekends.Add(date);
                                                }
                                            }
                                        }
                                    }

                                    // 保存更新后的数据
                                    SaveHolidays();
                                    return true;
                                }
                            }
                        }
                    }

                    MessageBox.Show($"从API获取节假日数据失败: HTTP {(int)response.StatusCode}",
                        "API错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // 捕获并显示详细的异常信息
                MessageBox.Show($"从API更新节假日时出错: {ex.Message}\n{ex.GetType().Name}\n{ex.StackTrace}",
                    "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        /// <summary>
        /// 异步更新节假日信息
        /// </summary>
        /// <returns>更新是否成功</returns>
        public async Task<bool> UpdateHolidaysAsync()
        {
            try
            {
                // 创建HTTP客户端
                using (HttpClient client = new HttpClient())
                {
                    // 设置超时时间
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // 获取当前年份和下一年
                    int currentYear = DateTime.Now.Year;
                    int nextYear = currentYear + 1;
                    
                    // 使用开放API获取中国法定节假日数据
                    string apiUrl = $"http://timor.tech/api/holiday/year/{currentYear}";
                    
                    // 发送GET请求
                    // 添加浏览器类型的User-Agent
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    
                    // 检查是否成功
                    if (response.IsSuccessStatusCode)
                    {
                        // 读取响应内容
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        
                        // 解析JSON
                        using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                        {
                            JsonElement root = doc.RootElement;
                            
                            // 检查API返回状态
                            if (root.TryGetProperty("code", out JsonElement codeElement) && 
                                codeElement.GetInt32() == 0)
                            {
                                // 清除现有节假日数据
                                holidays.Clear();
                                workOnWeekends.Clear();
                                
                                // 解析节假日数据
                                if (root.TryGetProperty("holiday", out JsonElement holidayElement))
                                {
                                    foreach (JsonProperty item in holidayElement.EnumerateObject())
                                    {
                                        string dateStr = item.Name; // 格式: yyyy-MM-dd
                                        
                                        if (DateTime.TryParse(dateStr, out DateTime date))
                                        {
                                            var details = item.Value;
                                            
                                            // 检查是否是节假日或调休工作日
                                            if (details.TryGetProperty("holiday", out JsonElement isHolidayElement))
                                            {
                                                bool isHoliday = isHolidayElement.GetBoolean();
                                                
                                                if (isHoliday)
                                                {
                                                    holidays.Add(date);
                                                }
                                                else if (details.TryGetProperty("workday", out JsonElement isWorkdayElement) && 
                                                        isWorkdayElement.GetBoolean())
                                                {
                                                    // 周末调休工作日
                                                    workOnWeekends.Add(date);
                                                }
                                            }
                                        }
                                    }
                                }
                                
                                // 同时获取下一年的数据
                                await GetNextYearHolidayData(client, nextYear);
                                
                                // 保存节假日信息
                                SaveHolidays();
                                return true;
                            }
                        }
                    }
                    
                    // 如果当前年份数据失败，仍然尝试获取下一年数据
                    bool nextYearSuccess = await GetNextYearHolidayData(client, nextYear);
                    if (nextYearSuccess)
                    {
                        SaveHolidays();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新节假日信息时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取下一年的节假日数据
        /// </summary>
        private async Task<bool> GetNextYearHolidayData(HttpClient client, int year)
        {
            try
            {
                string apiUrl = $"http://timor.tech/api/holiday/year/{year}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    
                    using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                    {
                        JsonElement root = doc.RootElement;
                        
                        if (root.TryGetProperty("code", out JsonElement codeElement) && 
                            codeElement.GetInt32() == 0 &&
                            root.TryGetProperty("holiday", out JsonElement holidayElement))
                        {
                            foreach (JsonProperty item in holidayElement.EnumerateObject())
                            {
                                string dateStr = item.Name;
                                
                                if (DateTime.TryParse(dateStr, out DateTime date))
                                {
                                    var details = item.Value;
                                    
                                    if (details.TryGetProperty("holiday", out JsonElement isHolidayElement))
                                    {
                                        bool isHoliday = isHolidayElement.GetBoolean();
                                        
                                        if (isHoliday)
                                        {
                                            holidays.Add(date);
                                        }
                                        else if (details.TryGetProperty("workday", out JsonElement isWorkdayElement) && 
                                                isWorkdayElement.GetBoolean())
                                        {
                                            workOnWeekends.Add(date);
                                        }
                                    }
                                }
                            }
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                // 忽略下一年数据获取的错误，不影响当前年份数据
                return false;
            }
        }

        // 检查指定日期是否为法定节假日
        public bool IsHoliday(DateTime date)
        {
            return holidays.Any(d => d.Date == date.Date);
        }

        // 检查指定日期是否为调休工作日（周末需要上班的日期）
        public bool IsWorkOnWeekend(DateTime date)
        {
            return workOnWeekends.Any(d => d.Date == date.Date);
        }

        // 获取所有节假日
        public List<DateTime> GetHolidays()
        {
            return new List<DateTime>(holidays);
        }

        // 获取所有调休工作日
        public List<DateTime> GetWorkOnWeekends()
        {
            return new List<DateTime>(workOnWeekends);
        }

        // 用于序列化/反序列化的数据类
        private class HolidayData
        {
            public List<string> Holidays { get; set; }
            public List<string> WorkOnWeekends { get; set; }
        }
        
        // API返回的JSON数据结构（根据实际API调整）
        private class ApiHolidayResponse
        {
            public bool Success { get; set; }
            public List<ApiHolidayItem> Data { get; set; }
        }
        
        private class ApiHolidayItem
        {
            public string Date { get; set; }
            public bool IsHoliday { get; set; }
            public bool IsWorkday { get; set; }
            public string Name { get; set; }
        }
    }
}
