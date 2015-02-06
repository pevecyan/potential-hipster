using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hipster_DNS.Handlers
{
    class NetshHandler
    {
        public static List<string> GetAdapters()
        {
            List<string> adapters = new List<string>();

            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "netsh.exe";
            p.StartInfo.Arguments = "interface show interface";
            
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            //Console.WriteLine(output);
            string [] newLine = {"\r\n"};
            string[] lines = output.Split(newLine, 22, StringSplitOptions.RemoveEmptyEntries);

            char []spaces = {' '};
            for (int i = 2; i < lines.Length; i++)
            {
                adapters.Add(lines[i].Split(spaces, 4, StringSplitOptions.RemoveEmptyEntries)[3]);
            }

            return adapters;
        }

        public static void SetDns(int index, string adapterName, string ip)
        {
            // add dnsservers "Wired Ethernet Connection" 10.0.0.3 index=2
            string argument = "interface ipv4 add dnsservers \"" + adapterName + "\" " + ip + " index=" + index + " validate=no";

            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "netsh.exe";
            p.StartInfo.Arguments = argument;

            p.Start();
            Console.WriteLine(p.StandardOutput.ReadToEnd());
        }

        public static void ResetDNS(string adapterName)
        {
            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "netsh.exe";
            p.StartInfo.Arguments = "interface ipv4 delete dnsservers \"" + adapterName + "\" "+"all";

            p.Start();
            Console.WriteLine(p.StandardOutput.ReadToEnd());
        }
    }
}
