namespace DBConfirm.TemplateGeneration.SQLServer.Extensions
{
    public static class StringExtensions
    {
        public static string UppercaseFirstCharacter(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            if (char.IsLower(text[0]))
            {
                return $"{char.ToUpper(text[0])}{text[1..]}";
            }

            return text;
        }
    }
}
