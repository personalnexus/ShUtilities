using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShUtilities.Net;
using System.Net;

namespace ShUtilitiesTest.Tests
{
    [TestClass]
    public class NetTests
    {
        [TestMethod]
        public void Proxy()
        {
            using var webClient = new WebClientEx();
            Assert.IsNull(webClient.ProxyUri);
            Assert.IsNull(webClient.ProxyAddress);

            webClient.ProxyAddress = "http://myproxy:1234";
            Assert.IsNotNull(webClient.ProxyUri);
            Assert.IsNotNull(webClient.Proxy);

            var proxy = webClient.Proxy as WebProxy;
            Assert.IsNotNull(proxy);
            Assert.AreEqual(1234, proxy.Address.Port);
            Assert.AreEqual("myproxy", proxy.Address.Host);
            Assert.IsTrue(proxy.UseDefaultCredentials);
        }
    }
}
