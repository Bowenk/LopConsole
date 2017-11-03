//=======================================================
// 作者：bowenk
// 描述：手动滑动滑块 控制
//=======================================================
using UnityEngine;
using System.Collections;

namespace Babybus.Uno.LogCat
{
    public class ManualScrollController : MonoBehaviour
    {
        public static bool IsManualMove = false;
        public AutoScrollController AutoScrollController { get; private set; }
        public AutoAddLogMessage AutoAddLogMessage { get; private set; }

        void Awake()
        {
            AutoScrollController = GetComponent<AutoScrollController>();
            AutoAddLogMessage = transform.parent.GetComponent<AutoAddLogMessage>();
        }

        public void AddLog(AutoLog aLog)
        {
            if (AutoScrollController.CurrentShowCount < AutoScrollController.item.Count)
            {
                AutoScrollController.CurrentIndex++;
                var trans = AutoScrollController.item[AutoScrollController.CurrentShowCount].transform;
                trans.gameObject.SetActive(true);
                var height = AutoScrollController.SetTextForItemChild(AutoScrollController.CurrentIndex, aLog, trans);
                AutoScrollController.ResetTransformSize(trans, height);
                AutoScrollController.CurrentShowCount++;
            }
        }

        public void ManualMoveItem(float distance)
        {
            if (distance > 0)
            {
                MoveWhole(distance, ItemState.MoveUp);
                MoveSingleItem(ItemState.MoveUp);
            }
            else
            {
                MoveWhole(distance, ItemState.MoveDown);
                MoveSingleItem(ItemState.MoveDown);
            }
        }

        private void MoveWhole(float distance, ItemState manualState)
        {
            RectTransform rectTrans = null;
            if (manualState == ItemState.MoveUp)
            {
                if (AutoScrollController.CurrentShowCount < AutoScrollController.item.Count)
                {
                    rectTrans = transform.GetChild(AutoScrollController.CurrentShowCount - 1).GetComponent<RectTransform>();
                    if ((rectTrans.anchoredPosition.y + transform.GetComponent<RectTransform>().rect.height) > rectTrans.rect.height / 2.0f)
                        return;
                }
                else
                {
                    rectTrans = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
                    if ((rectTrans.anchoredPosition.y + AutoScrollController.rectTransform.rect.height) > rectTrans.rect.height / 2.0f)
                        return;
                }
            }
            else
            {
                if (CanToTop())
                    return;
            }
            foreach (Transform child in transform)
            {
                child.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, distance);
            }
        }

        private void MoveSingleItem(ItemState manualState)
        {
            if (manualState == ItemState.MoveUp)
            {
                if (CantoBottom() && AutoScrollController.CurrentIndex < AutoAddLogMessage.Instance.GetAllLogs.Count - 1)
                    MoveToBottom();
            }
            else
            {
                if (CanToTop() && AutoScrollController.CurrentIndex >= transform.childCount)
                    MoveToTop();
            }
        }

        private bool CanToTop()
        {
            var rectTrans = transform.GetChild(0).GetComponent<RectTransform>();
            if (rectTrans.anchoredPosition.y <= -rectTrans.rect.height / 2.0f)
                return true;
            return false;
        }

        private bool CantoBottom()
        {
            RectTransform rectTrans = null;
            if (AutoScrollController.CurrentShowCount < AutoScrollController.item.Count)
            {
                rectTrans = transform.GetChild(AutoScrollController.CurrentShowCount - 1).GetComponent<RectTransform>();
                if ((rectTrans.anchoredPosition.y + transform.GetComponent<RectTransform>().rect.height) < rectTrans.rect.height / 2.0f)
                    return false;
            }
            else
            {
                rectTrans = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
                if ((rectTrans.anchoredPosition.y + AutoScrollController.rectTransform.rect.height) < rectTrans.rect.height / 2.0f)
                    return false;
            }
            return true;
        }

        private void MoveToTop()
        {
            AutoScrollController.MoveToTop();
        }

        private void MoveToBottom()
        {
            AutoScrollController.MoveToBottom(false);
        }
    }
}
