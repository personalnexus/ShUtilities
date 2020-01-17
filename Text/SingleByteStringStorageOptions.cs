using System.Text;

namespace ShUtilities.Text
{
    public class SingleByteStringStorageOptions
    {

        public SingleByteStringStorageOptions(): this(Encoding.GetEncoding("ISO-8859-1"))
        {
        }

        public SingleByteStringStorageOptions(Encoding encoding)
        {
            Encoding = encoding;
            GrowthIncrement = 85000;
        }

        public Encoding Encoding { get; }

        public int GrowthIncrement { get; set; }
    }
}
