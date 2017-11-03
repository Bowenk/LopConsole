//=======================================================
// 作者：bowenk
// 描述：自定义单个滑块
//=======================================================
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Babybus.Uno.LogCat
{
    public class AutoItemController : MonoBehaviour
    {
        private Vector3 _curMPPosition;
        public ManualScrollController ManualScrollController { get; private set; }
        public AutoScrollController AutoScrollController { get; private set; }

        void Awake()
        {
            ManualScrollController = transform.parent.GetComponent<ManualScrollController>();
            AutoScrollController = transform.parent.GetComponent<AutoScrollController>();
        }

        public void OnItemDrag()
        {
            Vector3 off = Input.mousePosition - _curMPPosition;
            _curMPPosition = Input.mousePosition;

            if (Math.Abs(off.x) > Math.Abs(off.y))
                SlideItem(off.x);
            else
                MoveItem(off.y);
        }

        public void OnItemPointDown()
        {
            _curMPPosition = Input.mousePosition;
        }

        private void SlideItem(float distance)
        {
            var x = transform.GetComponent<RectTransform>().anchoredPosition.x;
            if ((distance < 0 && (x < AutoScrollController.DefaultItemX - ComputeItemX())) || (distance >= 0 && x > AutoScrollController.DefaultItemX))
                return;
            transform.GetComponent<RectTransform>().anchoredPosition += new Vector2(distance, 0);
        }

        private void MoveItem(float distance)
        {
            if (Math.Abs(distance) > 8)
                AutoScrollController.SetAllItemDefaultX();
            if (!ManualScrollController.IsManualMove)
                ManualScrollController.IsManualMove = true;
            ManualScrollController.ManualMoveItem(distance);
        }

        /// <summary>
        /// 计算滑块可移动的X值范围
        /// </summary>
        private float ComputeItemX()
        {
            var H = transform.FindChild("Text").GetComponent<RectTransform>().rect.width;
            var h = transform.GetComponent<RectTransform>().rect.width;
            return H - h + 20;
        }
    }
}
