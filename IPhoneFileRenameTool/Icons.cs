using System.Drawing;
using System.Drawing.Drawing2D;

namespace IPhoneFileRenameTool
{
    internal static class Icons
    {
        /// <summary>16×16 yellow folder bitmap for the browse button.</summary>
        public static Bitmap FolderSmall()
        {
            var bmp = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var gold      = Color.FromArgb(255, 185, 10);
            var lightGold = Color.FromArgb(255, 220, 80);
            var darkGold  = Color.FromArgb(170, 120,  0);

            using var body   = new SolidBrush(gold);
            using var light  = new SolidBrush(lightGold);
            using var border = new Pen(darkGold, 1f);

            // Folder tab (top-left)
            g.FillRectangle(body, 0, 3, 7, 3);
            // Folder body
            g.FillRectangle(body, 0, 5, 16, 10);
            // Light highlight on top edge of body
            g.FillRectangle(light, 0, 5, 16, 2);
            // Borders
            g.DrawRectangle(border, 0, 3, 6,  2);
            g.DrawRectangle(border, 0, 5, 15, 9);

            return bmp;
        }

        /// <summary>16×16 green play-triangle for the Rename button.</summary>
        public static Bitmap PlaySmall()
        {
            var bmp = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            PointF[] pts = { new(3, 2), new(14, 8), new(3, 14) };
            using var b = new SolidBrush(Color.FromArgb(30, 160, 50));
            g.FillPolygon(b, pts);

            return bmp;
        }

        /// <summary>16×16 red X for the Cancel button.</summary>
        public static Bitmap StopSmall()
        {
            var bmp = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var p = new Pen(Color.FromArgb(210, 40, 40), 2.5f);
            p.StartCap = LineCap.Round;
            p.EndCap   = LineCap.Round;
            g.DrawLine(p, 3,  3, 13, 13);
            g.DrawLine(p, 13, 3,  3, 13);

            return bmp;
        }
    }
}
