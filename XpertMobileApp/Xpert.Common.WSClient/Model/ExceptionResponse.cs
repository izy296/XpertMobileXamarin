using System;
using System.Collections.Generic;
using System.Text;

namespace Xpert.Common.WSClient.Model
{
    public class ExceptionResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public ExceptionResponse InnerException { get; set; }
    }
}
