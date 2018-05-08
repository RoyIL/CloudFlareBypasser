using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
namespace CloudflareBypass
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
        static void Main(string[] args)
        {
            ShowWindow(ThisConsole, MAXIMIZE);
            new Thread(Listener).Start();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your site address: ");
            Console.ResetColor();
            var url = Console.ReadLine();
            if (!url.ToLower().Contains("http://")) url = "http://" + url;
            var ip = Dns.GetHostAddresses(new Uri(url).Host)[0];
            if (new WebClient().DownloadString("http://ip-api.com/json/" + ip.ToString()).ToLower().Contains("cloudflare"))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.crimeflare.org:82/cgi-bin/cfsearch.cgi");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII)) { writer.Write("cfS=" + site);}
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var str = reader.ReadToEnd();
                    str = str.Substring(str.IndexOf("<LI>") + 4).Replace(site.ToLower(), "");
                    str = str.Substring(str.IndexOf(" ") + 2);
                    str = str.Replace(str.Substring(str.IndexOf(" ")), "");
                    if (!str.Contains("TTP"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[Success]");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(site + " IP: " + str);
                    }
                    else { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Website isn't an Cloudflare site."); }
                }
            }
            else { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Website isn't an Cloudflare site."); }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new WebClient().DownloadString("https://pastebin.com/raw/sM2T3jTt"));
            Thread.Sleep(304324141);
        }

        static void Listener()
        {
            var on = true;
            var height = Console.WindowHeight;
            var wi = Console.WindowWidth;
            while (on)
            {
                if (height != Console.WindowHeight || wi != Console.WindowWidth)
                {
                    ShowWindow(ThisConsole, MAXIMIZE);
                }
            }
        }
    }
}
