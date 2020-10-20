using Models.Factories.Abstract;
using System;

namespace Models.Factories
{
    /// <summary>
    /// The default UtcNow factory, using <see cref="DateTime.UtcNow"/>
    /// </summary>
    public class DateUtcNowFactory : IDateUtcNowFactory
    {
        /// <summary>
        /// Gets the value of UtcNow, using <see cref="DateTime.UtcNow"/>
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
