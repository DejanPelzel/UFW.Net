using System.Diagnostics;
using System.IO;
using System.Linq;

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
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = command;
            if (!string.IsNullOrWhiteSpace(arguments))
            {
                startInfo.Arguments = arguments;
            }
            using (Process exeProcess = Process.Start(startInfo))
            {
                string output;

                using (StreamReader reader = exeProcess.StandardError)
                {
                    output = reader.ReadToEnd();
                }

                if (string.IsNullOrEmpty(output))
                {
                    using (StreamReader reader = exeProcess.StandardOutput)
                    {
                        output = reader.ReadToEnd();
                    }
                }

                exeProcess.WaitForExit();

                return output.Split("\n");
            }
        }
    }
}
