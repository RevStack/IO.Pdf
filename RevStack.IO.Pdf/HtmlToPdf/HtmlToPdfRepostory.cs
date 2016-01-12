using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using RevStack.IO;
using System.Diagnostics;
using System.Threading;

namespace RevStack.IO.Pdf
{
    public class HtmlToPdfRepostory : IEntityStreamRepository
    {
        private HtmlToPdfContext _context;
        public HtmlToPdfRepostory(HtmlToPdfContext context)
        {
            _context = context;
        }

        public byte[] Get(string id)
        {

            new Thread(new ParameterizedThreadStart(x =>
            {
                ExecuteCommand(string.Format("{0} rasterize.js {1} {2} \"A4\"", _context.ExecutablePath, id, _context.FileName));
              
            })).Start();

            var filePath = Path.Combine(_context.ExecutablePath, _context.FileName);

            var stream = new MemoryStream();
            byte[] bytes = DoWhile(filePath);

            return bytes;

        }

        public EntityStream Add(EntityStream entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(EntityStream entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<EntityStream> Find(Expression<Func<EntityStream, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EntityStream> Get()
        {
            throw new NotImplementedException();
        }

        public EntityStream Update(EntityStream entity)
        {
            throw new NotImplementedException();
        }

        private void ExecuteCommand(string Command)
        {
            try
            {
                ProcessStartInfo ProcessInfo;
                Process Process;

                ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);

                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;

                Process = Process.Start(ProcessInfo);
            }
            catch { }
        }

        private byte[] DoWhile(string filePath)
        {
            byte[] bytes = new byte[0];
            bool fail = true;

            while (fail)
            {
                try
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
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

            File.Delete(filePath);
            return bytes;
        }
    }
}
