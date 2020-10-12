namespace Models.Templates
{
    public abstract class BaseIdentityTemplate : BaseTemplate
    {
        public abstract string IdentityColumnName { get; }

        public int Identity => (int)this[IdentityColumnName];

        public Resolver<int> IdentityResolver => new Resolver<int>(() => Identity);
    }
}
