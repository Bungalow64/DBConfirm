using System;

namespace Models.Factories.Abstract
{
    public interface IDateUtcNowFactory
    {
        DateTime UtcNow { get; }
    }
}
