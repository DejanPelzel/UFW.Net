using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UFW.Net
{
    public class Ufw
    {
        /// <summary>
        /// Get all the rules currently configured with ufw
        /// </summary>
        /// <returns></returns>
        public static List<UfwRule> GetRules()
        {
            var result = new List<UfwRule>();
            var ufwResult = LocalCommand.Execute("ufw status numbered");
            foreach (var line in ufwResult)
            {
                var rule = UfwRule.TryParse(line); ;
                if (rule != null)
                {
                    result.Add(rule);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns true UFW is currently enabled
        /// </summary>
        /// <returns></returns>
        public static bool IsEnabled()
        {
            var ufwResult = LocalCommand.Execute("ufw status numbered");
            return ufwResult.Where(e => e.Contains("Status: active")).FirstOrDefault() != null;
        }

        /// <summary>
        /// Delete the rule on the given index
        /// </summary>
        /// <param name="rule"></param>
        public static void DeleteRule(UfwRule rule)
        {
            LocalCommand.Execute($"ufw --force delete {rule.RuleIndex}");
        }

        /// <summary>
        /// Allow inbound connections from the specific port
        /// </summary>
        /// <param name="port"></param>
        /// <param name="proto"></param>
        public static void AllowInbound(string port, RuleProtocol proto, string comment = "")
        {
            string command = $"ufw allow {port}";

            switch (proto)
            {
                case RuleProtocol.Any:
                    if (port.Contains(',') && !port.Contains(':'))
                    {
                        AllowInbound(port, RuleProtocol.TCP, comment);
                        AllowInbound(port, RuleProtocol.UDP, comment);
                    }
                    break;
                case RuleProtocol.TCP:
                    command += "/tcp";
                    break;
                case RuleProtocol.UDP:
                    command += "/udp";
                    break;
            }

            if (!string.IsNullOrEmpty(comment))
            {
                command += $" comment {comment}";
            }

            string[] message = LocalCommand.Execute(command);

            if (message.Length > 0 && message[0].StartsWith("ERROR:"))
            {
                throw new UfwException(message[0][6..]);
            }
        }

        /// <summary>
        /// Allow inbound connection from 
        /// </summary>
        /// <param name="fromIP"></param>
        /// <param name="port"></param>
        public static void AllowInbound(string fromIP, string port, RuleProtocol proto, string comment = "")
        {
            string command = $"ufw allow from {fromIP} to any port {port}";

            switch (proto)
            {
                case RuleProtocol.TCP:
                    command += $" proto tcp";
                    break;
                case RuleProtocol.UDP:
                    command += $" proto udp";
                    break;
            }

            if (!string.IsNullOrEmpty(comment))
            {
                command += $" comment {comment}";
            }

            string[] message = LocalCommand.Execute(command);

            if (message.Length > 0 && message[0].StartsWith("ERROR:"))
            {
                throw new UfwException(message[0][6..]);
            }
        }

        /// <summary>
        /// Allow service profile
        /// </summary>
        /// <param name="profile"></param>
        public static void AllowService(string service)
        {
            LocalCommand.Execute($"ufw allow {service}");
        }

        /// <summary>
        /// Deny connections from the given IP
        /// </summary>
        /// <param name="fromIP"></param>
        public static void DenyInbound(string fromIP)
        {
            LocalCommand.Execute($"ufw deny from {fromIP}");
        }


        /// <summary>
        /// Enables the ufw firewall
        /// </summary>
        public static void Enable()
        {
            LocalCommand.Execute("ufw enable");
        }

        /// <summary>
        /// Enables the ufw firewall
        /// </summary>
        public static void Disable()
        {
            LocalCommand.Execute("ufw disable");
        }

        /// <summary>
        /// Reset the ufw firewall
        /// </summary>
        public static void Reset()
        {
            LocalCommand.Execute("ufw --force reset");
        }

        /// <summary>
        /// Shutdown the ufw logging
        /// </summary>
        public static void ShutdownLogging()
        {
            LocalCommand.Execute("ufw logging off");
        }

        /// <summary>
        /// Disable the ufw IPv6 support
        /// </summary>
        public static void DisableIPv6()
        {
            string file = "/etc/default/ufw";
            string[] content = File.ReadAllLines(file);

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i].StartsWith("IPV6=yes"))
                {
                    content[i] = $"IPV6=no";
                    File.WriteAllLines(file, content);
                    return;
                }
            }
        }

        /// <summary>
        /// Import the ufw config 
        /// </summary>
        public static void Import(string content, bool ipv6)
        {
            string file = $"/etc/ufw/{(ipv6 ? "user6" : "user")}.rules";
            File.WriteAllText(file, content);
        }

        /// <summary>
        /// Export the ufw config 
        /// </summary>
        public static string Export(bool ipv6)
        {
            string file = $"/etc/ufw/{(ipv6 ? "user6" : "user")}.rules";
            string content = File.ReadAllText(file);
            return content;
        }
    }
}
