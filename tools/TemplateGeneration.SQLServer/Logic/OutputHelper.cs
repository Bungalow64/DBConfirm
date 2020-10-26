using System;

namespace SQLConfirm.TemplateGeneration.Logic
{
    public static class OutputHelper
    {
        public static void WriteError(string text) => WriteLine(text, ConsoleColor.Red);
        public static void WriteSuccess(string text) => WriteLine(text, ConsoleColor.Green);
        public static void WriteLine(string text, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void WriteException(Exception ex)
        {
            WriteError(ex.Message);
            WriteError(ex.StackTrace);
            if (ex.InnerException != null)
            {
                WriteError("Inner Exception:");
                WriteException(ex.InnerException);
            }
        }
    }
}
