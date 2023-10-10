namespace Notification.Email.Interfaces
{
    public interface IPlaceholderReplacer
    {
        string Replace(string placeholderString, object replacement);
    }
}
