using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestParameters
{
    public class QuizRequestParameters : RequestParameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public QuizRequestParameters() : this(1, 6)
        {

        }
        public QuizRequestParameters(int pageNumber = 1, int pageSize = 6)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}