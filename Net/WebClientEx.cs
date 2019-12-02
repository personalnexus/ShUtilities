using System;
using System.Net;

namespace ShUtilities.Net
{
    /// <summary>
    /// Derived from <see cref="WebClient"/> this class adds convient configuration of a proxy as well as other little niceties.
    /// </summary>
    /// <remarks>
    /// Use this class to fill the void in terms of features between WebClient and what <see cref="HttpWebRequest"/> offers.
    /// </remarks>
    public class WebClientEx: WebClient
    {
        /// <summary>
        /// The address from which a <see cref="WebProxy"/> is created using the default credentials.
        /// </summary>
        public string ProxyAddress
        {
            get => ProxyUri?.ToString();
            set => ProxyUri = new Uri(value);
        }

        /// <summary>
        /// The <see cref="Uri"/> from which a <see cref="WebProxy"/> is created using the default credentials.
        /// </summary>
        public Uri ProxyUri
        {
            get => (Proxy as WebProxy)?.Address;
            set
            {
                var proxy = new WebProxy
                {
                    Address = value,
                    UseDefaultCredentials = true
                };
                Proxy = proxy;
            }
        }

        /// <summary>
        /// If not zero, the timeout used for each <see cref="WebRequest"/>.
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (RequestTimeout != TimeSpan.Zero)
            {
                request.Timeout = (int)RequestTimeout.TotalMilliseconds;
            }
            ((HttpWebRequest)request).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request;
        }
    }
}
