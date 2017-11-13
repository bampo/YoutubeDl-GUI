using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeDl;

namespace YoutubeDlTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var s = @"139          m4a        audio only DASH audio   48k , m4a_dash container, mp4a.40.5@ 48k (22050Hz), 1.02MiB";
            var yd = new YoutubeDownloader(null);
            yd.ParseYoutubeInfo(s);
        }
    }
}
