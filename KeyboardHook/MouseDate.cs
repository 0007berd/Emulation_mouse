using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardHook
{
    [Serializable]
    public class MouseDate
    {
        [DllImport("User32.dll")]
        static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

        //для удобства использования создаем перечисление с необходимыми флагами (константами), которые определяют действия мыши: 
        [Flags]
        enum MouseFlags
        {
            Move = 0x0001, LeftDown = 0x0002, LeftUp = 0x0004, RightDown = 0x0008,
            RightUp = 0x0010, Absolute = 0x8000
        };

        public int position_x { get; set; }
        public int position_y { get; set; }
        public int type_click { get; set; }
        public int time { get; set; }
        //public static String coment = "";
        //public static int k = 100;

        public MouseDate(int position_x, int position_y, int type_click, int time)
        {
            this.position_x = position_x;
            this.position_y = position_y;
            this.type_click = type_click;
            this.time = time;
        }
        public MouseDate()
        {
            this.position_x = 0;
            this.position_y = 0;
            this.type_click = 0;
            this.time =0;
        }
        public static void mouseLeftDown(int x, int y)
        {
            double x1 = (x * 65536 / Screen.PrimaryScreen.Bounds.Width);
            double y1 = (y * 65536 / Screen.PrimaryScreen.Bounds.Height);
            x = (int)x1;
            y = (int)y1;
            mouse_event(MouseFlags.Absolute | MouseFlags.Move, x, y, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, x, y, 0, UIntPtr.Zero);
        }
        public static void mouseLeftUp(int x, int y)
        {
            double x1 = (x * 65536 / Screen.PrimaryScreen.Bounds.Width);
            double y1 = (y * 65536 / Screen.PrimaryScreen.Bounds.Height);
            x = (int)x1;
            y = (int)y1;
            mouse_event(MouseFlags.Absolute | MouseFlags.Move, x, y, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, x, y, 0, UIntPtr.Zero);
        }
        public static void mouseRightDown(int x, int y)
        {
            double x1 = (x * 65536 / Screen.PrimaryScreen.Bounds.Width);
            double y1 = (y * 65536 / Screen.PrimaryScreen.Bounds.Height);
            x = (int)x1;
            y = (int)y1;
            mouse_event(MouseFlags.Absolute | MouseFlags.Move, x, y, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.Absolute | MouseFlags.RightDown, x, y, 0, UIntPtr.Zero);
        }
        public static void mouseRightUp(int x, int y)
        {
            double x1 = (x * 65536 / Screen.PrimaryScreen.Bounds.Width);
            double y1 = (y * 65536 / Screen.PrimaryScreen.Bounds.Height);
            x = (int)x1;
            y = (int)y1;
            mouse_event(MouseFlags.Absolute | MouseFlags.Move, x, y, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.Absolute | MouseFlags.RightUp, x, y, 0, UIntPtr.Zero);
        }
        public static void mousemove(int x, int y)
        {
            double x1 = (x * 65536 / Screen.PrimaryScreen.Bounds.Width);
            double y1 = (y * 65536 / Screen.PrimaryScreen.Bounds.Height);
            x = (int)x1;
            y = (int)y1;
            mouse_event(MouseFlags.Absolute | MouseFlags.Move, x, y, 0, UIntPtr.Zero);
        }


    }
}
