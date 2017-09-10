using System;

namespace WAT.Modules
{
    public static class ErrorHandler
    {
        const string ErrorLogsFile = "errors.log";

        public static bool Handle(this Exception ex)
        {
            FileRW.Write(
                ErrorLogsFile,
                $"Time:{DateTime.Now}{Environment.NewLine}{ex}{Environment.NewLine}============================================={Environment.NewLine}");

            return false;
        }
    }
}
