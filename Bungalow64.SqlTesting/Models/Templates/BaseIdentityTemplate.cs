using Models.Templates.Abstract;
using Models.Runners.Abstract;

namespace Models.Templates
{
    /// <summary>
    /// The abstract template class used as the base for simple templates for tables that have an identity column
    /// </summary>
    /// <typeparam name="T">The type of the current template object</typeparam>
    public abstract class BaseIdentityTemplate<T> : BaseSimpleTemplate<T>
        where T : BaseIdentityTemplate<T>
    {
        /// <summary>
        /// Gets the name of the identity column
        /// </summary>
        public abstract string IdentityColumnName { get; }

        /// <summary>
        /// Gets the current value of the identity column, either explicitly set or set during <see cref="ITemplate.InsertAsync(ITestRunner)"/>
        /// </summary>
        public int Identity => (int)this[IdentityColumnName];

        /// <summary>
        /// Gets the resolver object, which, when resolved, returns the value of the identity column
        /// </summary>
        /// <remarks>This can be used to reference the identity value before <see cref="ITemplate.InsertAsync(ITestRunner)"/> has been executed</remarks>
        public Resolver<int> IdentityResolver => new Resolver<int>(() => Identity);
    }
}
