namespace Models.Strings.Abstract
{
    public interface IStringComparison
    {
        void AssertString(string value, string message);
    }
}
