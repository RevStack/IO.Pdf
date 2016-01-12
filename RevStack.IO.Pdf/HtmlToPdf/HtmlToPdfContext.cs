using System;
using System.IO;

namespace RevStack.IO.Pdf
{
    public class HtmlToPdfContext
    {
        public string ExecutablePath { get; set; } 
        public string FileName { get; set; }
        public HtmlToPdfContext(string executablePath)
        {
            ExecutablePath = executablePath;
            FileName= DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".pdf";
        }
    }
}
