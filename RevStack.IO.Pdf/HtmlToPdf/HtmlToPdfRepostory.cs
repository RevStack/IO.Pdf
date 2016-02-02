using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace RevStack.IO.Pdf
{
    public class HtmlToPdfRepository : IEntityStreamRepository
    {
        private HtmlToPdfContext _context;
        public HtmlToPdfRepository(HtmlToPdfContext context)
        {
            _context = context;
        }

        public byte[] Get(string id)
        {
            _context.GeneratePdf(id);
            return _context.PdfStream();
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

    }
}
