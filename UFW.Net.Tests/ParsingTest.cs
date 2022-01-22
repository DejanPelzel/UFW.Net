using NUnit.Framework;

namespace UFW.Net.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestParseType()
        {
            var rule = UfwRule.TryParse("[ 5] 1245/tcp                   ALLOW IN    15.59.22.255      ");
            Assert.AreEqual(RuleType.AllowIn, rule.Type);

            rule = UfwRule.TryParse("[15] 1245/tcp                   DENY OUT    15.59.22.255      ");
            Assert.AreEqual(RuleType.DenyOut, rule.Type);

            rule = UfwRule.TryParse("[ 1] Anywhere                   DENY IN     175.111.212.145 ");
            Assert.AreEqual(RuleType.DenyIn, rule.Type);

            rule = UfwRule.TryParse("[ 8] 33 (v6)                ALLOW IN    Anywhere (v6) ");
            Assert.AreEqual(RuleType.AllowIn, rule.Type);

            Assert.Pass();
        }

        [Test]
        public void TestParseIndex()
        {
            var rule = UfwRule.TryParse("[ 5] 8852/tcp                   ALLOW IN    15.59.22.255      ");
            Assert.AreEqual(5, rule.RuleIndex);

            rule = UfwRule.TryParse("[15] 663/tcp       [ g ]            ALLOW IN    15.59.22.255      ");
            Assert.AreEqual(15, rule.RuleIndex);

            rule = UfwRule.TryParse("[1 ] 6631/tcp  [                 ALLOW IN    175.111.212.145      ");
            Assert.AreEqual(1, rule.RuleIndex);

            rule = UfwRule.TryParse("[ 8] 33 (v6)                ALLOW IN    Anywhere (v6) ");
            Assert.AreEqual(8, rule.RuleIndex);

            Assert.Pass();
        }

        [Test]
        public void TestParsePort()
        {
            var rule = UfwRule.TryParse("[ 2] 80/tcp                     ALLOW IN    Anywhere             ");
            Assert.AreEqual("80", rule.Port);

            rule = UfwRule.TryParse("[ 5] 7440/udp                   ALLOW IN    112.112.227.15       ");
            Assert.AreEqual("7440", rule.Port);

            rule = UfwRule.TryParse("[ 7] 311                       ALLOW IN    59.119.22.22     ");
            Assert.AreEqual("311", rule.Port);

            rule = UfwRule.TryParse("[ 7] Anywhere                       ALLOW IN    121.219.12.118     ");
            Assert.AreEqual(string.Empty, rule.Port);

            rule = UfwRule.TryParse("[ 8] 22/tcp (v6)                ALLOW IN    Anywhere (v6) ");
            Assert.AreEqual("22", rule.Port);

            rule = UfwRule.TryParse("[ 8] 33 (v6)                ALLOW IN    Anywhere (v6) ");
            Assert.AreEqual("33", rule.Port);

            Assert.Pass();
        }

        [Test]
        public void TestParseProtocol()
        {
            var rule = UfwRule.TryParse("[ 2] 80/tcp                     ALLOW IN    Anywhere             ");
            Assert.AreEqual(RuleProtocol.TCP, rule.Protocol);

            rule = UfwRule.TryParse("[ 5] 443/udp                   ALLOW IN    255.12.111.56       ");
            Assert.AreEqual(RuleProtocol.UDP, rule.Protocol);

            rule = UfwRule.TryParse("[ 7] 8080                       ALLOW IN    19.19.12.212     ");
            Assert.AreEqual(RuleProtocol.Any, rule.Protocol);

            rule = UfwRule.TryParse("[ 8] 22/tcp (v6)                ALLOW IN    Anywhere (v6) ");
            Assert.AreEqual(RuleProtocol.TCP, rule.Protocol);

            Assert.Pass();
        }

        [Test]
        public void TestParseSource()
        {
            var rule = UfwRule.TryParse("[ 2] 80/tcp                     ALLOW IN    Anywhere             ");
            Assert.AreEqual("Anywhere", rule.Source);

            rule = UfwRule.TryParse("[ 5] 1222/udp                   ALLOW IN    11.1.12.1       ");
            Assert.AreEqual("11.1.12.1", rule.Source);

            rule = UfwRule.TryParse("[ 7] 1222                       ALLOW IN    119.1.2.2     ");
            Assert.AreEqual("119.1.2.2", rule.Source);

            rule = UfwRule.TryParse("[ 8] 22/tcp (v6)                ALLOW IN    Anywhere (v6) ");
            Assert.AreEqual("Anywhere (v6)", rule.Source);

            Assert.Pass();
        }

        [Test]
        public void TestParseSourceType()
        {
            var rule = UfwRule.TryParse("[ 2] 80/tcp                     ALLOW IN    Anywhere             ");
            Assert.AreEqual(SourceType.Anywhere, rule.SourceType);

            rule = UfwRule.TryParse("[ 5] 1222/udp                   ALLOW IN    52.12.242.155       ");
            Assert.AreEqual(SourceType.Address, rule.SourceType);

            rule = UfwRule.TryParse("[ 7] 1111                       ALLOW IN    129.19.252.22     ");
            Assert.AreEqual(SourceType.Address, rule.SourceType);

            rule = UfwRule.TryParse("[ 8] 22/tcp (v6)                ALLOW IN    Anywhere (v6) ");
            Assert.AreEqual(SourceType.Anywhere, rule.SourceType);

            Assert.Pass();
        }
    }
}