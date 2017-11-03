//=======================================================
// 作者：bowenk
// 描述：自动滑动 滑块控制
//=======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Babybus.Uno.LogCat
{
    public enum ItemState { None, MoveUp, MoveDown }

    public class AutoScrollController : MonoBehaviour
    {
        [HideInInspector]
        public List<RectTransform> item = new List<RectTransform>();
        [HideInInspector]
        public int CurrentIndex = -1;
        [HideInInspector]
        public RectTransform rectTransform;
        [HideInInspector]
        public int CurrentShowCount = 0;
        [HideInInspector]
        public float DefaultItemX = 0;

        private float _leftMargin = 10;
        private float _logHeight = 0;
        public AutoAddLogMessage AutoAddLogMessage { get; private set; }

        void Awake()
        {
            AutoAddLogMessage = transform.parent.GetComponent<AutoAddLogMessage>();
            rectTransform = TransToRectTrans(transform);
            HideAllChild();
        }

        private void HideAllChild()
        {
            foreach (Transform child in transform)
            {
                item.Add(TransToRectTrans(child));
                child.gameObject.SetActive(false);
            }
            DefaultItemX = item[0].anchoredPosition.x;
        }

        public void SetAllItemDefaultX()
        {
            foreach (var child in item)
            {
                if (child.anchoredPosition.x != DefaultItemX)
                    child.anchoredPosition = new Vector2(DefaultItemX, child.anchoredPosition.y);
            }
        }

        public void AddLog(AutoLog aLog)
        {
            CurrentIndex++;
            if (CurrentShowCount < 9)
                AddLogDirect(aLog);
            else
                AddLogFinly(aLog);
        }

        private void AddLogDirect(AutoLog aLog)  //直接添加显示子物体
        {
            var trans = item[CurrentShowCount].transform;
            trans.gameObject.SetActive(true);
            SetItemText(aLog, trans);
            CurrentShowCount++;
        }

        private void AddLogFinly(AutoLog aLog) //超过子物体个数，显示最后的几个
        {
            SetItemText(aLog, item[0].transform);
        }

        private void SetItemText(AutoLog aLog, Transform trans) //设置item大小
        {
            var height = SetTextForItemChild(CurrentIndex, aLog, trans);
            ResetTransformSize(trans, height);
            _logHeight += height;
            if ((_logHeight - rectTransform.rect.height) > 0)
            {
                MoveItem(_logHeight - rectTransform.rect.height, true);
                _logHeight = rectTransform.rect.height;
            }
        }

        public void ResetTransformSize(Transform trans, float height)
        {
            TransToRectTrans(trans).sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
            if (CurrentIndex <= 0)
                TransToRectTrans(trans).anchoredPosition = new Vector2(rectTransform.rect.width / 2.0f, -height / 2.0f);
            else
            {
                var index = CurrentIndex - 1;
                if (CurrentIndex > item.Count - 1)
                    index = item.Count - 1;
                TransToRectTrans(trans).anchoredPosition = new Vector2(rectTransform.rect.width / 2.0f, item[index].anchoredPosition.y - item[index].rect.height / 2.0f - height / 2.0f);
            }
        }

        public float SetTextForItemChild(int index, AutoLog aLog, Transform trans)
        {
            var text = trans.Find("Text").GetComponent<Text>();
            text.color = aLog.color;
            text.text ="No."+(index+1).ToString()+"  "+ aLog.ToString();
            var textR = TransToRectTrans(text.transform);
            textR.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);
            textR.anchoredPosition = new Vector2(textR.sizeDelta.x / 2.0f + _leftMargin, -textR.sizeDelta.y / 2.0f);
            return text.preferredHeight;
        }

        public void MoveItem(float distance, bool autoMove = false)
        {
            CheckMoveAllChild(distance);

            if (CurrentIndex >= item.Count - 1)
                CheckItemPosition(autoMove);
        }

        private void CheckMoveAllChild(float distance)
        {
            var index = transform.childCount - 1;
            if (CurrentShowCount < item.Count - 1)
                index = CurrentShowCount;
            if ((item[index].anchoredPosition.y + rectTransform.rect.height) > item[index].rect.height / 2.0f)
                return;

            foreach (Transform child in transform)
                child.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, distance);
        }

        private void CheckItemPosition(bool autoMove)
        {
            if (IsCanToMoveBottom())
                MoveToBottom(autoMove);
        }

        private bool IsCanToMoveBottom()
        {
            if (CurrentIndex > AutoAddLogMessage.Instance.GetAllLogs.Count - 1 || CurrentIndex < (item.Count - 1))
                return false;
            if ((GetChildRectTForIndex(transform.childCount - 1).anchoredPosition.y + rectTransform.rect.height)
                > GetChildRectTForIndex(transform.childCount - 1).rect.height / 2.0f)
                return true;
            return false;
        }

        public void MoveToTop()
        {
            item[item.Count - 1].SetAsFirstSibling();
            CurrentIndex--;
            ScrollReSetText(CurrentIndex - 8, item[item.Count - 1]);
            item[item.Count - 1].anchoredPosition =
                new Vector2(item[item.Count - 1].anchoredPosition.x, item[0].anchoredPosition.y
                + item[0].rect.height / 2.0f + item[item.Count - 1].rect.height / 2.0f);
            UpdateItemList();
        }

        public void MoveToBottom(bool autoMove)
        {
            item[0].SetAsLastSibling();
            if (!autoMove)
                CurrentIndex++;
            ScrollReSetText(CurrentIndex, item[0]);
            item[0].anchoredPosition =
                new Vector2(item[0].anchoredPosition.x, item[item.Count - 1].anchoredPosition.y
                - item[item.Count - 1].rect.height / 2.0f - item[0].rect.height / 2.0f);
            UpdateItemList();
        }

        private void ScrollReSetText(int index, Transform trans)
        {
            if (index < 0 || index > AutoAddLogMessage.Instance.GetAllLogs.Count - 1)
                return;
            var height = SetTextForItemChild(index, AutoAddLogMessage.Instance.GetAllLogs[index], trans);
            ResetTransformSize(trans, height);
        }

        private void UpdateItemList()
        {
            item.Clear();
            foreach (Transform child in transform)
                item.Add(child.GetComponent<RectTransform>());
        }

        private RectTransform GetChildRectTForIndex(int index)
        {
            return transform.GetChild(index).GetComponent<RectTransform>();
        }

        private RectTransform TransToRectTrans(Transform trans)
        {
            return trans.GetComponent<RectTransform>();
        }

        public void ReSetRectTransformSize(float width, float height)
        {
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.anchoredPosition = new Vector2(-width / 2.0f, height / 2.0f);

            for (var i = 0; i < item.Count; i++)
            {
                item[i].sizeDelta = new Vector2(width, item[i].sizeDelta.y);
                item[i].anchoredPosition = new Vector2(width / 2, item[i].anchoredPosition.y);
            }
            DefaultItemX = item[0].anchoredPosition.x;

            RecoveryAutoScroll();
        }

        /// <summary>
        /// 显示指定日志信息
        /// </summary>
        public void RecoveryAutoScroll()
        {
            ClearData();
            for (var i = 0; i < AutoAddLogMessage.Instance.GetAllLogs.Count; i++)
                AddLog(AutoAddLogMessage.Instance.GetAllLogs[i]);
        }

        /// <summary>
        /// 清楚所有的记录
        /// </summary>
        public void ClearData()
        {
            ManualScrollController.IsManualMove = false;
            CurrentIndex = -1;
            CurrentShowCount = 0;
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }

    }
}
