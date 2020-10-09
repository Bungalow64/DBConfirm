namespace Models.States.Abstract
{
    public interface IState
    {
        void AssertState(object value, string message);
    }
}
