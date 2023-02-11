namespace LibUpdater.Tests;

internal static class FluentExtension
{
    public static T Do<T>(this T obj, Action<T> action)
    {
        action(obj);
        return obj;
    }
}