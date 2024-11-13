using System;

public class Singleton<T> where T : class
{
    private static readonly Lazy<T> _lazy = new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);
    public static T Instance => _lazy.Value;
    protected Singleton() { }
}