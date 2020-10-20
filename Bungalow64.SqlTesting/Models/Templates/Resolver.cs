using Models.Templates.Abstract;
using System;

namespace Models.Templates
{
    /// <summary>
    /// A deferred action, which only evaluates the result when the <see cref="Resolver{T}.Resolve"/> method is called
    /// </summary>
    /// <typeparam name="T">The type of the resolved object</typeparam>
    public class Resolver<T> : IResolver
    {
        /// <summary>
        /// Gets the function which is evaluated during <see cref="Resolver{T}.Resolve"/>
        /// </summary>
        public Func<T> Function { get; }

        /// <summary>
        /// Constructor, including the function to be evaluated during <see cref="Resolver{T}.Resolve"/>
        /// </summary>
        /// <param name="function">The function to be evaluated</param>
        public Resolver(Func<T> function)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
        }

        /// <inheritdoc/>
        public object Resolve() => Function();
    }
}
