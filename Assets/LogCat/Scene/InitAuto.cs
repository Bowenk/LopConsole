//=======================================================
// 作者：bowenk
// 描述：测试添加log日志
//=======================================================
using UnityEngine;
using System.Collections;

namespace Babybus.Uno.LogCat
{
    public class InitAuto : MonoBehaviour
    {
        private int index = 0;

        void Start()
        {
            var i = 0;
            while (i < 20)
            {
                if (i % 5 == 0 && AutoAddLogMessage.Instance)
                    AutoAddLogMessage.Instance.AddLog("15659797541+" + i, "", (LogType)5);
                else if (i % 7 == 0)
                    Debug.LogError("this is error "+i);
                else
                    Debug.Log("I don't know who!" + i);
                i++;
            }
        }


        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Log"))
            {
                Debug.Log("Bowenk " + index);
                index++;
            }

            if (GUI.Button(new Rect(10, 80, 100, 50), "Error"))
            {
                Debug.LogError("LinjinLin " + index);
                index++;
            }

            if (GUI.Button(new Rect(10, 150, 100, 50), "Warning"))
            {
                Debug.LogWarning("XiLin " + index);
                index++;
            }
        }
    }
}
