using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RavenRole
{
    static class Log
    {
        internal static void Error(string error)
        {
            Trace.TraceError(error);
        }

        internal static void Error(string fmt, params object[] args)
        {
            Trace.TraceError(String.Format(fmt, args));
        }

        internal static void Info(string info)
        {
            Trace.TraceInformation(info);
        }

        internal static void Info(string fmt, params object[] args)
        {
            Trace.TraceInformation(String.Format(fmt, args));
        }
    }
}
