using Models.Factories.Abstract;
using System;

namespace Models.Factories
{
    public class DateUtcNowFactory : IDateUtcNowFactory
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
