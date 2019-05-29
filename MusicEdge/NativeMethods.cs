using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicEdge
{
    internal static class NativeMethods
    {
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbmp, DWM_SIT dwSitFlags);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWA dwAttribute, ref int pvAttribute, int cbAttribute);

        [DllImport("dwmapi.dll")]
        public static extern int DwmInvalidateIconicBitmaps(IntPtr hwnd);


        [DllImport("dwmapi.dll")]
        internal static extern int DwmSetIconicLivePreviewBitmap(
            IntPtr hwnd,
            IntPtr hbitmap,
            ref NativePoint ptClient,
        uint flags);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbmp, IntPtr pptClient, DWM_SIT dwSitFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct NativePoint
        {
            public NativePoint(int x, int y) : this()
            {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
            public static bool operator ==(NativePoint first, NativePoint second)
            {
                return first.X == second.X && first.Y == second.Y;
            }
            public static bool operator !=(NativePoint first, NativePoint second)
            {
                return !(first == second);
            }
            public override bool Equals(object obj)
            {
                return (obj != null && obj is NativePoint) ? this == (NativePoint)obj : false;
            }

            public override int GetHashCode()
            {
                int hash = X.GetHashCode();
                hash = hash * 31 + Y.GetHashCode();
                return hash;
            }
        }

        public enum DWM_SIT
        {
            None, 
            DISPLAYFRAME = 1
        }

        public enum DWMWA : uint
        {
            NCRENDERING_ENABLED = 1,
            NCRENDERING_POLICY,
            TRANSITIONS_FORCEDISABLED,
            ALLOW_NCPAINT,
            cAPTION_BUTTON_BOUNDS,
            NONCLIENT_RTL_LAYOUT,
            FORCE_ICONIC_REPRESENTATION,
            FLIP3D_POLICY,
            EXTENDED_FRAME_BOUNDS,
            HAS_ICONIC_BITMAP,
            DISALLOW_PEEK,
            EXCLUDED_FROM_PEEK,
            CLOAK,
            CLOAKED,
            FREEZE_REPRESENTATION,
            LAST
        }

        public const uint TRUE = 1;

        public const uint WM_DWMSENDICONICTHUMBNAIL = 0x0323;
        public const uint WM_DWMSENDICONICLIVEPREVIEWBITMAP = 0x0323;
    }
}
