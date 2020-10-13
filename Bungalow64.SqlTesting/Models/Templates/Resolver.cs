using Models.Templates.Asbtract;
using System;

namespace Models.Templates
{
    public class Resolver<T> : IResolver
    {
        public Func<T> Function { get; }

        public Resolver(Func<T> function)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
        }

        public object Resolve() => Function();
    }
}
