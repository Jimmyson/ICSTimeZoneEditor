using System;
using System.IO;
using System.Collections.Generic;

namespace ICSTimeZoneEditor
{
    class Program
    {
        ///<summary>
        ///Open ICS and create a destination file for processing and dumping
        ///</summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            if (args.Length == 0) {
                throw new Exception("Program needs arguments to operate");
            }

            if (!args[0].ToLower().EndsWith(".ics")) {
                throw new FileLoadException("File is not an \".ics\" calendar file");
            }

            StreamReader original = File.OpenText(args[0]);

            StreamWriter output = (args.Length != 2) 
                ? File.CreateText("output.ics")
                : File.CreateText(args[1]);

            ProcessFile(original, output);

            original.Close();
            output.Close();
        }

        ///<summary>
        ///Extract event data in preperation for Data Manipulation
        ///</summary>
        public static void ProcessFile(StreamReader original, StreamWriter output) {
            IList<string> data = new List<string>();
            IList<string> edits = null;
            bool capture = false;
            string incoming;

            while (!original.EndOfStream) {
                incoming = original.ReadLine();
                if (incoming == "BEGIN:VEVENT")
                    capture = true;
                if (capture)
                    data.Add(incoming);
                
                if (incoming == "END:VEVENT") {
                    capture = false;
                    edits = ProcessEvent(data);
                }

                if (data.Count == 0) {
                    output.WriteLine(incoming);
                } else if (!capture && edits != null) {
                    foreach (string line in edits) {
                        output.WriteLine(line);
                    }

                    data = new List<string>();
                    edits = null;
                }
            }
        }

        ///<summary>
        ///Edit the event data
        ///</summary>
        public static IList<string> ProcessEvent(IList<string> data) {
            foreach (string line in data) {
                Console.WriteLine(line);
            }

            //Find DateTime values
            //Value Type #1: <KEY>:YYYYMMDDTHHMSSZ (UTC)
            //Valye Type #2: <KEY>;TZID=<VTIMEZONE-NAME>:YYYYMMDDTHHMMSS

            //Ask for an input to change into

            //Print output and ask for verification

            //Replace line with new data

            return data;
        }
    }
}
