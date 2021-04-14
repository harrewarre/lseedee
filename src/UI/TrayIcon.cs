using System.Drawing;
using System.Windows.Forms;

namespace LSeeDee
{
    public class TrayIcon
    {
        public static void Create()
        {
            var icon = new NotifyIcon();
            icon.Icon = SystemIcons.Information;
            icon.Visible = true;
            icon.Text = "LSeeDee";
        }
    }
}