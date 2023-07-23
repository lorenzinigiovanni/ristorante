using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace RistoranteDigitaleClient.Utils
{
    public class AutoClosingMessageBox
    {
        private const int wmClose = 0x0010;
        private readonly string caption;
        private readonly Timer timeoutTimer;

        private AutoClosingMessageBox(string text, string caption, int timeout)
        {
            this.caption = caption;
            timeoutTimer = new Timer(OnTimerElapsed, null, timeout, Timeout.Infinite);
            using (timeoutTimer)
            {
                MessageBox.Show(text, caption);
            }
        }

        public static void Show(string text, string caption, int timeout = 1000)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }

        private void OnTimerElapsed(object state)
        {
            var mbWnd = FindWindow("#32770", caption);
            if (mbWnd != IntPtr.Zero)
            {
                SendMessage(mbWnd, wmClose, IntPtr.Zero, IntPtr.Zero);
            }
            timeoutTimer.Dispose();
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}
