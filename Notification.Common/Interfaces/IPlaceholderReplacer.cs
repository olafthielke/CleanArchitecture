namespace Notification.Common.Interfaces
{
    public interface IPlaceholderReplacer
    {
        string Replace(string input, object obj);
    }
}
