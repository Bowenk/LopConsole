//=======================================================
// 作者：bowenk
// 描述：
//=======================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Babybus.Uno.LogCat
{
    public class LogProcessController : MonoBehaviour
    {
        public RectTransform ViewRectTransform;
        public RectTransform SmallWindow;
        public RectTransform BigWindow;
        public AutoScrollController AutoScroll;
        public Button[] ButtonGroup;
        public Dropdown commandDropdown;

        public void FullScreenButton(Text text)
        {
            var fullScreen = text.text == "全屏";
            CopyFrom(ViewRectTransform, fullScreen ? BigWindow : SmallWindow);
            text.text = fullScreen ? "窗口" : "全屏";
            AutoScroll.ReSetRectTransformSize(ViewRectTransform.rect.width, ViewRectTransform.rect.height);
        }

        public void ShowLogConsole(Text text)
        {
            var show = text.text == "显示";
            ViewRectTransform.gameObject.SetActive(show);
            for (var i = 0; i < ButtonGroup.Length; i++)
                ButtonGroup[i].gameObject.SetActive(show);
            text.text = show ? "隐藏" : "显示";
            if (show == false)
                AutoScroll.RecoveryAutoScroll();
        }

        public void PauseOrRecoveryGame(Text text)
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            text.text = Time.timeScale == 0 ? "恢复" : "暂停";
        }

        public void CommandDropdown()
        {
            Debug.LogWarning("命令这个功能，暂时关闭");
        }

        public void ClearAllLogs()
        {
            AutoScroll.ClearData();
            if (AutoAddLogMessage.Instance)
                AutoAddLogMessage.Instance.Init();

        }

        public void ShowUmengLogsOrAllLogs(Text text)
        {
            switch (text.text)
            {
                case "友盟":
                    AutoAddLogMessage.Instance.LogStyle = LogStyle.Umeng;
                    text.text = "错误";
                    break;
                case "错误":
                    AutoAddLogMessage.Instance.LogStyle = LogStyle.Error;
                    text.text = "全部";
                    break;
                default:
                    AutoAddLogMessage.Instance.LogStyle = LogStyle.None;
                    text.text = "友盟";
                    break;
            }
            AutoScroll.RecoveryAutoScroll();
        }

        public void CopyFrom(RectTransform rectTransform, RectTransform other)
        {
            rectTransform.anchoredPosition = other.anchoredPosition;
            rectTransform.anchoredPosition3D = other.anchoredPosition3D;
            rectTransform.anchorMax = other.anchorMax;
            rectTransform.anchorMin = other.anchorMin;
            rectTransform.offsetMax = other.offsetMax;
            rectTransform.offsetMin = other.offsetMin;
            rectTransform.pivot = other.pivot;
            rectTransform.localScale = other.localScale;
            rectTransform.sizeDelta = other.sizeDelta;
        }
    }
}
