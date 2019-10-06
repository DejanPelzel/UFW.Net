using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace UFW.Net
{
    public class LocalCommand
    {
        /// <summary>
        /// Execute the command locally and return the response
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static string[] Execute(string commandText)
        {
            var commandData = commandText.Split(' ').ToList();
            if (commandData.Count < 1 || string.IsNullOrWhiteSpace(commandData[0]))
            {
                return null;
            }

            var command = commandData[0];
            commandData.RemoveAt(0);

            var arguments = "";
            if (commandData.Count > 0)
            {
                arguments = string.Join(" ", commandData);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = command;
            if (!string.IsNullOrWhiteSpace(arguments))
            {
                startInfo.Arguments = arguments;
            }
            using (Process exeProcess = Process.Start(startInfo))
            {
                using (StreamReader reader = exeProcess.StandardOutput)
                {
                    string output = reader.ReadToEnd();
                    exeProcess.WaitForExit();

                    return output.Split("\n");
                }
            }
        }
    }
}
