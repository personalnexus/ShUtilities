namespace ShUtilities.Text
{
    public delegate bool Parser<T>(string input, out T output);
}
