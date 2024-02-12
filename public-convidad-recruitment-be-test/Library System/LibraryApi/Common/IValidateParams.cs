namespace LibraryDatabase.Common
{
    public interface IValidateParams
    {
        IValidateParams Equals<T>(T ida, T idb);

        IValidateParams GreaterThanZero(int value);
    }
}
