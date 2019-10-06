﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UFW.Net
{
    public class Ufw
    {
        /// <summary>
        /// Get all the rules currently configured with ufw
        /// </summary>
        /// <returns></returns>
        public List<UfwRule> GetRules()
        {
            var result = new List<UfwRule>();
            var ufwResult = LocalCommand.Execute("ufw status numbered");
            foreach(var line in ufwResult)
            {
                var rule = UfwRule.TryParse(line); ;
                if(rule != null)
                {
                    result.Add(rule);
                }
            }

            return result;
        }

        /// <summary>
        /// Delete the rule on the given index
        /// </summary>
        /// <param name="rule"></param>
        public void DeleteRule(UfwRule rule)
        {
            LocalCommand.Execute($"ufw delete {rule.RuleIndex}");
        }

        /// <summary>
        /// Allow inbound connections from the specific port
        /// </summary>
        /// <param name="fromIP"></param>
        /// <param name="port"></param>
        public void AllowInbound(int port)
        {
            LocalCommand.Execute($"ufw allow {port}");
        }

        /// <summary>
        /// Allow inbound connection from 
        /// </summary>
        /// <param name="fromIP"></param>
        /// <param name="port"></param>
        public void AllowInbound(string fromIP, int port)
        {
            LocalCommand.Execute($"ufw allow from {fromIP} to any port {port}");
        }

        /// <summary>
        /// Deny connections from the given IP
        /// </summary>
        /// <param name="fromIP"></param>
        public void DenyInbound(string fromIP)
        {
            LocalCommand.Execute($"ufw deny from {fromIP}");
        }


        /// <summary>
        /// Enables the ufw firewall
        /// </summary>
        public void Enable()
        {
            LocalCommand.Execute("ufw enable");
        }

        /// <summary>
        /// Enables the ufw firewall
        /// </summary>
        public void Disable()
        {
            LocalCommand.Execute("ufw enable");
        }
    }
}