//=======================================================
// 作者：bowenk
// 描述：自定义log类，重写ToString方法
//=======================================================
using UnityEngine;
using System.Collections;
using System;

namespace Babybus.Uno.LogCat
{
    public class AutoLog 
    {
        public string LogString;
        public string StackTrace;
        public LogType type;
        public DateTime time;
        public Color color;

        public override string ToString()
        {
            return time+"\n"+LogString+"\n"+StackTrace;
        }
    }
}
