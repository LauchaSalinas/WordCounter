using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounterBase.Models
{
    internal record PdfProcessingResult
    {
        public int WordCount;
        public int PageCount;
    }
}
