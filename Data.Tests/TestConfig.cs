using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seznam.Data;

namespace Data.Tests
{
    class TestConfig : IConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
