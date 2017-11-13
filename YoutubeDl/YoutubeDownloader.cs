using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDl
{
    public class YoutubeDownloader
    {
        private string _program = @"youtube-dl.exe";
        private Process _ytProcess;
        private MainWindowVm vm;
        public YoutubeDownloader(MainWindowVm vmodel)
        {
            vm = vmodel;
            _ytProcess  = new Process();
            _ytProcess.StartInfo.FileName = _program;
            _ytProcess.StartInfo.RedirectStandardOutput = true;
            _ytProcess.StartInfo.RedirectStandardError = true;
            _ytProcess.StartInfo.UseShellExecute = false;
            _ytProcess.StartInfo.CreateNoWindow = true;
            if(!string.IsNullOrWhiteSpace(vm.SavePath)) _ytProcess.StartInfo.WorkingDirectory = vm.SavePath;
            vm.LogText += $"{nameof(YoutubeDownloader)} started\r\n";
        }

        public void GetVideo(string url, int formatNum)
        {

            _ytProcess.StartInfo.Arguments = $"-f {formatNum} \"{url}\"";

            Task.Run(() =>
            {
                _ytProcess.Start();
                string s;
                while ((s = _ytProcess.StandardOutput.ReadLine()) != null)
                {
                    vm.LogText += $"{s}\r\n";

                }
                string e;
                if (!string.IsNullOrWhiteSpace(e = _ytProcess.StandardError.ReadToEnd()))
                {
                    vm.LogText += $"{e}\r\n";
                }
                vm.LogText += $"Finished\r\n";
            });
        }

        public YoutubeInfo ParseYoutubeInfo(string s)
        {
            var split = s.IndexOf(',');
            var mainInfo = s.Substring(0, split);
            var note = s.Substring(++split, s.Length - split);
            var fn = mainInfo.Substring(0, 13);
            var ext = mainInfo.Substring(13, 11);
            var desc = mainInfo.Substring(24, mainInfo.Length - 24);
            return new YoutubeInfo()
            {
                FormatNum = int.Parse(fn.Trim())
                ,Extension= ext.Trim()
                ,Desc = desc
                ,Note = note
            };
        }

        public Task GetVideoFormats(string url)
        {
            _ytProcess.StartInfo.Arguments = $"-F \"{url}\"";
            var sb = new StringBuilder();
            var formatList = new List<YoutubeInfo>();
            return Task.Run(() =>
            {
                _ytProcess.Start();
                string s;
                bool tableBegin = false;
                while ((s = _ytProcess.StandardOutput.ReadLine()) != null)
                {

                    vm.LogText += $"{s}\r\n";

                    if (tableBegin)
                    {
                        formatList.Add(ParseYoutubeInfo(s));
                        continue;
                        
                    }
                    if (!tableBegin && s.Contains("format code  extension  resolution note")) tableBegin = true;
                    
                }
                vm.Formats = formatList;
                string e;
                if (!string.IsNullOrWhiteSpace(e = _ytProcess.StandardError.ReadToEnd()))
                {
                    vm.LogText += $"{e}\r\n";
                }
                vm.LogText += $"Finished\r\n";
            });
        }
    }

    public class YoutubeInfo
    {
        public int FormatNum { get; set; }
        public string Extension { get; set; }
        public string Desc { get; set; }
        public string Note { get; set; }
    }
}