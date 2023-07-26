using System;

namespace FileNetIntegration
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                FileNetFunctions fn;
                LFFunctions lf;

                if (args.Length > 1)
                {
                    if (args[0].Equals("CreateDocument"))
                    {
                        fn = new FileNetFunctions();
                        Console.WriteLine(fn.CreateDocument(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), args[3], args[4], args[5], /*metadata*/ args[6], args[7], args[8], args[9], args[10], args[11]));
                    }
                    else if (args[0].Equals("CreateFolder"))
                    {
                        fn = new FileNetFunctions();
                        Console.WriteLine(fn.CreateFolder(args[1], args[2]));
                    }
                    else if (args[0].Equals("DownloadDocument"))
                    {
                        fn = new FileNetFunctions();
                        fn.DocToWriteContentToFile(args[1], args[2]);
                    }
                    else if (args[0].Equals("LFDownloadPdf"))
                    {
                        lf = new LFFunctions();
                        lf.LFDownloadPdf(args[1], args[2]);
                    }
                    else if (args[0].Equals("RotateImage"))
                    {
                        lf = new LFFunctions();
                        lf.RotatePage(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), Convert.ToInt32(args[3]));
                    }
                    else if (args[0].Equals("MovePageTo"))
                    {
                        lf = new LFFunctions();
                        lf.MovePageTo(args[1], Convert.ToInt32(args[2]), Convert.ToInt32(args[3]));
                    }
                    else if (args[0].Equals("SplitDocument"))
                    {
                        lf = new LFFunctions();
                        lf.SplitDocument(args[1], args[2], Convert.ToInt32(args[3]), Convert.ToInt32(args[4]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
