using System;

namespace Models.Factories.Abstract
{
    /// <summary>
    /// The interface for UtcNow factories, to retrieve a value for UtcNow depending on the factory logic
    /// </summary>
    public interface IDateUtcNowFactory
    {
        /// <summary>
        /// Gets the value of UtcNow, according to the factory logic
        /// </summary>
        DateTime UtcNow { get; }
    }
}
