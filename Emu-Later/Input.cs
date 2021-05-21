using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmuLater
{
    //TODO refactor to be event based
    class Input
    {
        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] keystate);

        private static byte[] keys = new byte[256];
        private static bool[] priorPoll = new bool[256];

        public static void Poll()
        {
            for (int i = 0; i < priorPoll.Length; i++)
            {
                priorPoll[i] = GetKey(i);
            }
            GetKeyboardState(keys);
        }


        private static bool GetKey(int key)
        {
            if ((keys[key] & 128) == 128)
                return true;
            return false;
        }

        public static bool GetKey(Keys key)
        {
            return GetKey((int)key);
        }

        public static bool GetKeyDown(Keys key)
        {
            int i = (int)key;
            return GetKey(i) && !priorPoll[i];
        }

        public static bool AnyKeyDown()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (GetKeyDown((Keys)i))
                    return true;
            }
            return false;
        }
    }
}
