using System;
using System.Diagnostics;

namespace Storage
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