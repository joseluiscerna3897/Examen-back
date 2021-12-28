using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Entity
{
    public class Response
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
