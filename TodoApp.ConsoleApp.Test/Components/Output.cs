﻿using System;

namespace TodoApp.ConsoleApp.Test.Components
{
    internal static class Output
    {
        public static void WriteTitle(string title)
        {
            Console.WriteLine("-- {0} --", title);
            Console.WriteLine();
        }

        public static void WriteLabel(string lbl)
        {
            Console.Write("{0}: ", lbl);
        }

        public static void WriteField(string name, object value)
        {
            WriteLabel(name);
            Console.WriteLine(value);
        }

        public static void WriteParagraph(string p)
        {
            Console.WriteLine(p);
        }

        public static void WriteWarning(string warn)
        {
            Console.WriteLine("[ {0} ]", warn);
        }
    }
}
