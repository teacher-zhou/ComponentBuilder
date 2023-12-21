namespace ComponentBuilder.JSInterop;

internal static class Singleton<T> where T : class
{
    static object _locker = new();
    static T? _instance;

    public static T? Create(params object?[]? args)
    {
        if (_instance == null)
        {
            lock (_locker)
            {
                if (_instance is null)
                {
                    _instance = (T?)Activator.CreateInstance(typeof(T), args);
                }
            }
        }
        return _instance;
    }
}
