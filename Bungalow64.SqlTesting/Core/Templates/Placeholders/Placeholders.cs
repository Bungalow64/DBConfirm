namespace SQLConfirm.Core.Templates.Placeholders
{
    /// <summary>
    /// Facade to build placeholder objects, used to configure how specific columns should behave
    /// </summary>
    public static class Placeholders
    {
        /// <summary>
        /// Returns a <see cref="RequiredPlaceholder"/> to indicate that the column needs to have a value populated before the template can be executed
        /// </summary>
        /// <returns></returns>
        public static RequiredPlaceholder IsRequired() => new RequiredPlaceholder();
    }
}
