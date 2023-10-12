namespace Notification.Email.Interfaces
{
    public interface IPlaceholderReplacer
    {
        string Replace(string value, object obj);
    }
}
