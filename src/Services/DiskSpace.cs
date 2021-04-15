using System;
using System.IO;
using System.Linq;
using System.Timers;
using LSeeDee.Services;

namespace LSeeDee
{
    public class DiskSpace
    {
        private readonly Display _display;

        public DiskSpace(Display display)
        {
            _display = display;

            var timer = new Timer(60 * 1000);

            timer.Elapsed += (sender, args) =>
            {
                WriteDriveInfo();
            };

            timer.Enabled = true;
            timer.AutoReset = true;

            WriteDriveInfo();
        }

        private void WriteDriveInfo()
        {
            _display.WriteText(3, 0, GetDriveInfo());
        }

        private string GetDriveInfo()
        {
            var drive = DriveInfo.GetDrives().First();
            var percentUsed = Math.Abs((100 * (double)drive.AvailableFreeSpace / drive.TotalSize) - 100);

            return $"C:  : {percentUsed:F02}% used";
        }
    }

}