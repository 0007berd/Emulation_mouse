using System;
using System.Collections.Generic;
using System.Text;

namespace KeyboardHook
{
    [Serializable]
    public class macros
    {
        public String name { get; set; }
        public List<MouseDate> mousearray=new List<MouseDate>();
    }

}
