namespace Notification.Email.Interfaces
{
    public interface IPlaceholderReplacer
    {
        string Replace(string input, object obj);
    }
}
