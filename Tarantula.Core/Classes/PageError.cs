using System;
using System.Collections.Generic;
using System.Text;

namespace Tarantula.Core.Classes
{
    public class PageError
    {
        public string Url { get; set; } = string.Empty;
        public string errMsg { get; set; } = string.Empty;

        public PageError(string url, string errMsg) { this.Url = url; this.errMsg = errMsg; }
    }
}
