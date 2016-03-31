using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Reflection;
using System.Configuration;

namespace TraceSourceDemo
{
    class Program
    {
        // Initialize the trace source.
        static TraceSource ts = new TraceSource("TraceTest");
        [SwitchAttribute("SourceSwitch", typeof(SourceSwitch))]
        static void Main()
        {
            double count = 0;
            for (int i = 0; i < 40; i++)
            {
                count+=11/120*Math.Sqrt(i)-5.75*i+100;
            }

            Console.WriteLine(System.Configuration.ConfigurationManager.AppSettings["myconfig"]);
            try
            {
                // Initialize trace switches.
//#if(!ConfigFile)
//                SourceSwitch sourceSwitch = new SourceSwitch("SourceSwitch", "Verbose");
//                ts.Switch = sourceSwitch;
//                int idxConsole = ts.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
//                ts.Listeners[idxConsole].Name = "console";
//#endif
                //ts.Listeners.Add(new TextWriterTraceListener("log.log"));

                DisplayProperties(ts);

                ts.Listeners["console"].TraceOutputOptions |= TraceOptions.Callstack;
                ts.TraceEvent(TraceEventType.Warning, 1);
                ts.Listeners["console"].TraceOutputOptions = TraceOptions.DateTime;
                // Issue file not found message as a warning.
                ts.TraceEvent(TraceEventType.Warning, 2, "File Test not found");
                // Issue file not found message as a verbose event using a formatted string.
                ts.TraceEvent(TraceEventType.Verbose, 3, "File {0} not found.", "test");
                // Issue file not found message as information.
                ts.TraceInformation("File {0} not found.", "test");
                ts.Listeners["console"].TraceOutputOptions |= TraceOptions.LogicalOperationStack;
                // Issue file not found message as an error event.
                ts.TraceEvent(TraceEventType.Error, 4, "File {0} not found.", "test");
                // Test the filter on the ConsoleTraceListener.
                ts.Listeners["console"].Filter = new SourceFilter("No match");
                ts.TraceData(TraceEventType.Error, 5,
                    "SourceFilter should reject this message for the console trace listener.");
                ts.Listeners["console"].Filter = new SourceFilter("TraceTest");
                ts.TraceData(TraceEventType.Error, 6,
                    "SourceFilter should let this message through on the console trace listener.");
                ts.Listeners["console"].Filter = null;
                // Use the TraceData method. 
                ts.TraceData(TraceEventType.Warning, 7, new object());
                ts.TraceData(TraceEventType.Warning, 8, new object[] { "Message 1", "Message 2" });
                // Activity tests.
                ts.TraceEvent(TraceEventType.Start, 9, "Will not appear until the switch is changed.");
                ts.Switch.Level = SourceLevels.ActivityTracing | SourceLevels.Critical;
                ts.TraceEvent(TraceEventType.Suspend, 10, "Switch includes ActivityTracing, this should appear");
                ts.TraceEvent(TraceEventType.Critical, 11, "Switch includes Critical, this should appear");
                ts.Flush();
                ts.Close();
                Console.WriteLine("Press any key to exit.");
                Console.Read();
            }
            catch (Exception e)
            {
                // Catch any unexpected exception.
                Console.WriteLine("Unexpected exception: " + e.ToString());
                Console.Read();
            }
        }
        public static void DisplayProperties(TraceSource ts)
        {
            Console.WriteLine("TraceSource name = " + ts.Name);
            Console.WriteLine("TraceSource switch level = " + ts.Switch.Level);
            Console.WriteLine("TraceSource switch = " + ts.Switch.DisplayName);
            SwitchAttribute[] switches = SwitchAttribute.GetAll(Assembly.GetExecutingAssembly());
            for (int i = 0; i < switches.Length; i++)
            {
                Console.WriteLine("Switch name = " + switches[i].SwitchName);
                Console.WriteLine("Switch type = " + switches[i].SwitchType);
            }
#if(ConfigFile)
            // Get the custom attributes for the TraceSource.
            Console.WriteLine("Number of custom trace source attributes = "
                + ts.Attributes.Count);
            foreach (DictionaryEntry de in ts.Attributes)
                Console.WriteLine("Custom trace source attribute = "
                    + de.Key + "  " + de.Value);
            // Get the custom attributes for the trace source switch.
            foreach (DictionaryEntry de in ts.Switch.Attributes)
                Console.WriteLine("Custom switch attribute = "
                    + de.Key + "  " + de.Value);
#endif
            Console.WriteLine("Number of listeners = " + ts.Listeners.Count);
            foreach (TraceListener traceListener in ts.Listeners)
            {
                Console.Write("TraceListener: " + traceListener.Name + "\t");
                // The following output can be used to update the configuration file.
                Console.WriteLine("AssemblyQualifiedName = " +
                    (traceListener.GetType().AssemblyQualifiedName));
            }
        }
    }
}
