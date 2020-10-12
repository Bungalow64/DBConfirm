namespace Models.Templates.Asbtract
{
    public interface ITemplate
    {
        string TableName { get; }
        DataSetRow CustomData { get; }
        DataSetRow DefaultData { get; }
        DataSetRow MergedData { get; }
    }
}
