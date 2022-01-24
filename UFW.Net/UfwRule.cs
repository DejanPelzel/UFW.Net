using System;

namespace UFW.Net
{
    public enum SourceType
    {
        Anywhere = 0,
        Address = 1
    }

    public enum RuleType
    {
        Allow = 0,
        AllowIn = 1,
        AllowOut = 2,
        Deny = 3,
        DenyIn = 4,
        DenyOut = 5
    }

    public enum RuleProtocol
    {
        Any = 0,
        TCP = 1,
        UDP = 2
    }

    public class UfwRule
    {
        /// <summary>
        /// Gets the ordered UFW list index for this rule
        /// </summary>
        public int RuleIndex { get; private set; }

        /// <summary>
        /// Gets or sets the type of the rule
        /// </summary>
        public RuleType Type { get; set; }

        /// <summary>
        /// Gets or sets the protocol that the rule covers
        /// </summary>
        public RuleProtocol Protocol { get; set; }

        /// <summary>
        /// Gets or sets the port that the rule covers
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Gets or sety the source type of the rule
        /// </summary>
        public SourceType SourceType { get; set; }

        /// <summary>
        /// Gets or sets the source of the rule
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the comment of the rule
        /// </summary>
        public string Comment { get; set; }
        
        /// <summary>
        /// Create a new instance of the UfwRule class linked to the given collection
        /// </summary>
        /// <param name="masterCollection"></param>
        internal UfwRule()
        {
        }

        /// <summary>
        /// Attempt to parse the rule from the Ufw response line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static UfwRule TryParse(string line)
        {
            try
            {
                line = line.Trim();
                if (!line.StartsWith('['))
                {
                    return null;
                }

                while (line.Contains("   "))
                    line = line.Replace("   ", "  ");

                var data = line.Split("  ");



                var rule = new UfwRule();

                // Parse the index
                var ruleIndex = line.Remove(0, 1);
                ruleIndex = ruleIndex.Remove(ruleIndex.IndexOf(']')).Trim();
                rule.RuleIndex = int.Parse(ruleIndex);

                // Parse the port & protocol
                var portAndProtocol = data[0];
                portAndProtocol = portAndProtocol.Remove(0, portAndProtocol.IndexOf(']') + 1);
                portAndProtocol = portAndProtocol.Trim();
                if (portAndProtocol.IndexOf(' ') != -1)
                {
                    portAndProtocol = portAndProtocol.Remove(portAndProtocol.IndexOf(' '));
                }
                if (portAndProtocol.Equals("anywhere", StringComparison.OrdinalIgnoreCase))
                {
                    rule.Port = string.Empty;
                    rule.Protocol = RuleProtocol.Any;
                }
                else if (portAndProtocol.Contains('/'))
                {
                    var portAndProtocolData = portAndProtocol.Split('/');
                    rule.Port = portAndProtocolData[0];
                    switch (portAndProtocolData[1])
                    {
                        case "tcp":
                            rule.Protocol = RuleProtocol.TCP;
                            break;
                        case "udp":
                            rule.Protocol = RuleProtocol.UDP;
                            break;
                        default:
                            rule.Protocol = RuleProtocol.Any;
                            break;
                    }
                }
                else
                {
                    var portData = portAndProtocol.Split(' ');
                    rule.Port = portData[0];
                    rule.Protocol = RuleProtocol.Any;
                }

                // Parse the type
                var type = data[1];
                type = type.Trim();

                switch (type)
                {
                    case "ALLOW":
                        rule.Type = RuleType.Allow;
                        break;
                    case "ALLOW IN":
                        rule.Type = RuleType.AllowIn;
                        break;
                    case "ALLOW OUT":
                        rule.Type = RuleType.AllowOut;
                        break;
                    case "DENY":
                        rule.Type = RuleType.Deny;
                        break;
                    case "DENY IN":
                        rule.Type = RuleType.DenyIn;
                        break;
                    case "DENY OUT":
                        rule.Type = RuleType.DenyOut;
                        break;
                }


                // Parse the source
                var source = data[2];
                if (source.ToLower().StartsWith("anywhere"))
                {
                    rule.Source = source;
                    rule.SourceType = SourceType.Anywhere;
                }
                else
                {
                    rule.SourceType = SourceType.Address;
                    rule.Source = source;
                }

                // Parse the comment
                if(data.Length > 3)
                {
                    var comment = data[3].TrimStart('#').Trim();
                    rule.Comment = comment;
                }


                return rule;
            }
            catch {
                return null;
            }
        }
    }
}
