namespace Models.Templates.Asbtract
{
    public interface ISimpleTemplate : ITemplate
    {
        string TableName { get; }
        DataSetRow CustomData { get; }
        DataSetRow DefaultData { get; }
        DataSetRow MergedData { get; }
    }
}
