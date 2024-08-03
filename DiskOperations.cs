using System;
using System.Diagnostics;
using System.IO;

namespace LinHexShell
{
    public static class DiskOperations
    {
        public static void ListDisks()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "lsblk",
                        Arguments = "-o NAME,SIZE,TYPE,MOUNTPOINT",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"�޷��г�������Ϣ: {ex.Message}");
            }
        }

        public static void ListPartitions(string disk)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "lsblk",
                        Arguments = $"-o NAME,SIZE,TYPE,MOUNTPOINT {disk}",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"�޷��г�������Ϣ: {ex.Message}");
            }
        }
    }
}
