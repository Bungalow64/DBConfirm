namespace Models.Templates
{
    public abstract class BaseIdentityTemplate<T> : BaseSimpleTemplate<T>
        where T : BaseIdentityTemplate<T>
    {
        public abstract string IdentityColumnName { get; }

        public int Identity => (int)this[IdentityColumnName];

        public Resolver<int> IdentityResolver => new Resolver<int>(() => Identity);
    }
}
