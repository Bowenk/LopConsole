//=======================================================
// 作者：bowenk
// 描述：处理添加 log日志信息
//=======================================================
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Babybus.Uno.LogCat
{
    public enum LogStyle { None, Umeng, Error }
    public class AutoAddLogMessage : MonoBehaviour
    {
        public static AutoAddLogMessage Instance;
        public AutoScrollController AutoScrollController { get; private set; }
        public ManualScrollController ManualScrollController { get; private set; }
        private List<AutoLog> allLogs;
        public LogStyle LogStyle { get; set; }

        public List<AutoLog> GetAllLogs
        {
            get
            {
                if (LogStyle == LogStyle.Error)
                    return GetLogsForType(LogStyle.Error);
                else if (LogStyle == LogStyle.Umeng)
                    return GetLogsForType(LogStyle.Umeng);
                else
                    return GetLogsForType(LogStyle.None);
            }
        }

        void Awake()
        {
            Instance = this;
            Init();
            AutoScrollController = transform.GetChild(0).GetComponent<AutoScrollController>();
            ManualScrollController = transform.GetChild(0).GetComponent<ManualScrollController>();
            Application.logMessageReceivedThreaded += AddLog;
        }

        public void Init()
        {
            LogStyle = LogStyle.None;
            allLogs = new List<AutoLog>();
        }

        public void AddLog(string logString, string stackTrace, LogType type)
        {
            AddLogOnMainThread(logString, stackTrace, type);
        }

        private void AddLogOnMainThread(string logString, string stackTrace, LogType type)
        {
            AddLogToConsole(logString, stackTrace, type);
        }

        private void AddLogToConsole(string logString, string stackTrace, LogType type)
        {
            var color = GetLogColor(type);
            var log = new AutoLog()
            {
                LogString = logString,
                StackTrace = stackTrace,
                type = type,
                time = DateTime.Now,
                color = color
            };
            allLogs.Add(log);
            if (ManualScrollController.IsManualMove)
                ManualScrollController.AddLog(log);
            else
                AutoScrollController.AddLog(log);
        }

        private Color GetLogColor(LogType type)
        {
            var color = Color.white;
            switch (type)
            {
                case LogType.Log:
                    color = Color.white;
                    break;
                case LogType.Warning:
                    color = Color.yellow;
                    break;
                case (LogType)5:
                    color = Color.green;
                    break;
                default:
                    color = Color.red;
                    break;
            }
            return color;
        }

        public List<AutoLog> GetLogsForType(LogStyle style)
        {
            var logs = new List<AutoLog>();
            if (style == LogStyle.Umeng)
            {
                for (var i = 0; i < allLogs.Count; i++)
                {
                    if (allLogs[i].type == (LogType)5)
                        logs.Add(allLogs[i]);
                }
            }
            else if (style == LogStyle.Error)
            {
                for (var i = 0; i < allLogs.Count; i++)
                {
                    if (allLogs[i].type == LogType.Error)
                        logs.Add(allLogs[i]);
                }
            }
            else
                logs = allLogs;
            return logs;
        }
    }
}
