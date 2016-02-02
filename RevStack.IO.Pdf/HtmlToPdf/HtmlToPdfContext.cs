using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RevStack.IO.Pdf
{
    public class HtmlToPdfContext
    {
        public string ExecutablePath { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public HtmlToPdfContext(string executablePath)
        {
            ExecutablePath = executablePath;
            FileName = Guid.NewGuid().ToString() + ".pdf"; //random name for temp disk document
            FilePath = null;
        }

        public void GeneratePdf(string url)
        {
            string args = "rasterize.js " + url + " " + FileName;

            var p = new Process()
            {
                StartInfo =
                    {
                        FileName = ExecutablePath + @"\phantomjs",
                        Arguments = args,
                        UseShellExecute = false, // needs to be false in order to redirect output
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                        WorkingDirectory = ExecutablePath
                    }
            };

            p.Start();

            // read the output here...
            var output = p.StandardOutput.ReadToEnd();
            var errorOutput = p.StandardError.ReadToEnd();

            // ...then wait n milliseconds for exit (as after exit, it can't read the output)
            p.WaitForExit(60000);

            // read the exit code, close process
            int returnCode = p.ExitCode;
            p.Close();

            // if 0 or 2, it worked so return path of pdf
            if ((returnCode == 0) || (returnCode == 2))
                FilePath = Path.Combine(ExecutablePath, FileName);
            else
                throw new Exception(errorOutput);
        }

        public byte[] PdfStream()
        {
            if (FilePath == null)
            {
                throw new Exception("Pdf document error: Pdf stream requires a generated Pdf document.");
            }
            byte[] bytes = new byte[0];
            bool fail = true;

            while (fail)
            {
                try
                {
                    using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                    }

                    fail = false;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }

            File.Delete(FilePath);//delete the document on disk before returning
            return bytes;
        }
    }
}
