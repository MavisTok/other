using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ScheduleICSGenerator.Properties;

namespace ScheduleICSGenerator.Helpers
{
    public class ICSFileHelper
    {
        /// <summary>
        /// 加载ICS文件内容
        /// </summary>
        /// <param name="filePath">ICS文件路径</param>
        /// <returns>文件内容字符串</returns>
        public static string LoadICSFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;

            try
            {
                // 读取文件内容
                string content = File.ReadAllText(filePath);
                
                // 更新上次加载的文件路径
                Settings.Default.LastLoadedFilePath = filePath;
                Settings.Default.Save();
                
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载ICS文件失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 解析字符串形式的日期列表
        /// </summary>
        /// <param name="datesString">逗号分隔的日期字符串(yyyy-MM-dd格式)</param>
        /// <returns>日期列表</returns>
        public static List<DateTime> ParseDatesString(string datesString)
        {
            List<DateTime> dates = new List<DateTime>();
            
            if (string.IsNullOrEmpty(datesString))
                return dates;
            
            string[] dateStrings = datesString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string dateStr in dateStrings)
            {
                if (DateTime.TryParseExact(dateStr.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out DateTime date))
                {
                    dates.Add(date);
                }
            }
            
            return dates;
        }

        /// <summary>
        /// 获取特殊工作日列表
        /// </summary>
        public static List<DateTime> GetSpecialWorkDays()
        {
            return ParseDatesString(Settings.Default.SpecialWorkDays);
        }

        /// <summary>
        /// 获取特殊休息日列表
        /// </summary>
        public static List<DateTime> GetSpecialRestDays()
        {
            return ParseDatesString(Settings.Default.SpecialRestDays);
        }

        /// <summary>
        /// 添加特殊工作日
        /// </summary>
        /// <param name="date">要添加的日期</param>
        /// <returns>是否添加成功</returns>
        public static bool AddSpecialWorkDay(DateTime date)
        {
            List<DateTime> workDays = GetSpecialWorkDays();
            
            // 如果日期已存在，不重复添加
            if (workDays.Any(d => d.Date == date.Date))
                return false;
                
            // 如果该日期在休息日列表中，先移除
            RemoveSpecialRestDay(date);
                
            // 格式化为yyyy-MM-dd
            string dateStr = date.ToString("yyyy-MM-dd");
            
            // 添加到设置
            string currentWorkDays = Settings.Default.SpecialWorkDays;
            if (string.IsNullOrEmpty(currentWorkDays))
                Settings.Default.SpecialWorkDays = dateStr;
            else
                Settings.Default.SpecialWorkDays += "," + dateStr;
                
            Settings.Default.Save();
            return true;
        }

        /// <summary>
        /// 添加特殊休息日
        /// </summary>
        /// <param name="date">要添加的日期</param>
        /// <returns>是否添加成功</returns>
        public static bool AddSpecialRestDay(DateTime date)
        {
            List<DateTime> restDays = GetSpecialRestDays();
            
            // 如果日期已存在，不重复添加
            if (restDays.Any(d => d.Date == date.Date))
                return false;
                
            // 如果该日期在工作日列表中，先移除
            RemoveSpecialWorkDay(date);
                
            // 格式化为yyyy-MM-dd
            string dateStr = date.ToString("yyyy-MM-dd");
            
            // 添加到设置
            string currentRestDays = Settings.Default.SpecialRestDays;
            if (string.IsNullOrEmpty(currentRestDays))
                Settings.Default.SpecialRestDays = dateStr;
            else
                Settings.Default.SpecialRestDays += "," + dateStr;
                
            Settings.Default.Save();
            return true;
        }

        /// <summary>
        /// 移除特殊工作日
        /// </summary>
        public static bool RemoveSpecialWorkDay(DateTime date)
        {
            List<DateTime> workDays = GetSpecialWorkDays();
            bool removed = false;
            
            // 移除匹配的日期
            workDays.RemoveAll(d => {
                if (d.Date == date.Date) {
                    removed = true;
                    return true;
                }
                return false;
            });
            
            if (removed)
            {
                // 重建特殊工作日字符串
                Settings.Default.SpecialWorkDays = string.Join(",", workDays.Select(d => d.ToString("yyyy-MM-dd")));
                Settings.Default.Save();
            }
            
            return removed;
        }

        /// <summary>
        /// 移除特殊休息日
        /// </summary>
        public static bool RemoveSpecialRestDay(DateTime date)
        {
            List<DateTime> restDays = GetSpecialRestDays();
            bool removed = false;
            
            // 移除匹配的日期
            restDays.RemoveAll(d => {
                if (d.Date == date.Date) {
                    removed = true;
                    return true;
                }
                return false;
            });
            
            if (removed)
            {
                // 重建特殊休息日字符串
                Settings.Default.SpecialRestDays = string.Join(",", restDays.Select(d => d.ToString("yyyy-MM-dd")));
                Settings.Default.Save();
            }
            
            return removed;
        }

        /// <summary>
        /// 检查当前日期是否为特殊工作日
        /// </summary>
        public static bool IsSpecialWorkDay(DateTime date)
        {
            List<DateTime> workDays = GetSpecialWorkDays();
            return workDays.Any(d => d.Date == date.Date);
        }

        /// <summary>
        /// 检查当前日期是否为特殊休息日
        /// </summary>
        public static bool IsSpecialRestDay(DateTime date)
        {
            List<DateTime> restDays = GetSpecialRestDays();
            return restDays.Any(d => d.Date == date.Date);
        }

        /// <summary>
        /// 自动加载上次打开的文件
        /// </summary>
        public static string AutoLoadLastFile()
        {
            if (Settings.Default.AutoLoadLastFile && 
                !string.IsNullOrEmpty(Settings.Default.LastLoadedFilePath) &&
                File.Exists(Settings.Default.LastLoadedFilePath))
            {
                return LoadICSFile(Settings.Default.LastLoadedFilePath);
            }
            
            return null;
        }
    }
}
