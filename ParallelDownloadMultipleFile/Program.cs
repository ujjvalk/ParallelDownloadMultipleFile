using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ParallelDownloadMultipleFile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (CheckForInternetConnection())
            {
                var list = new[] 
            {
                "http://www.sample-videos.com/video/mp4/360/big_buck_bunny_360p_1mb.mp4",
                "http://www.sample-videos.com/video/flv/360/big_buck_bunny_360p_1mb.flv",
                "http://www.sample-videos.com/video/mkv/360/big_buck_bunny_360p_1mb.mkv",
                "http://www.sample-videos.com/video/3gp/144/big_buck_bunny_144p_1mb.3gp"
            };
                ServicePointManager.DefaultConnectionLimit = 200;
                Stopwatch t1 = new Stopwatch();
                t1.Start();
                #region Parallel.foreach Task
                var tasks = Parallel.ForEach(list,
                                s =>
                                {
                                    using (var client = new WebClient())
                                    {
                                        t1.Start();
                                        Uri uri = new Uri((string)s);
                                        string filename = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\" + System.IO.Path.GetFileName(uri.AbsolutePath);
                                        Console.WriteLine("starting to download {0} and Time is {1}.", s, t1.ElapsedMilliseconds);
                                        client.DownloadFile((string)s, filename);
                                        Console.WriteLine("finished downloading {0} and Time is {1}.", s, t1.ElapsedMilliseconds);
                                    }
                                });
                #endregion

                #region Without Parallel.foreach Task 
                //foreach (var s in list)
                //{
                //    using (var client = new WebClient())
                //    {
                //        Uri uri = new Uri((string)s);
                //        string filename = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\" + System.IO.Path.GetFileName(uri.AbsolutePath);
                //        client.DownloadFile((string)s, filename);
                //    }
                //}
                #endregion
                Console.Write("Download Complete on " + Convert.ToInt16(TimeSpan.FromMilliseconds(t1.ElapsedMilliseconds).TotalSeconds));
            }
            else
            {
                Console.WriteLine("Internet Connection problem, check your internet connection.");
            }

            Console.ReadLine();
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
