namespace ShUtilities.Text
{
    public delegate bool Parser<TOutput>(string input, out TOutput output);

    public delegate bool Parser<TInput, TOutput>(TInput input, out TOutput output);
}
