using System;

namespace DoomFire
{
    public class Singleton<T>
    {
        static readonly Lazy<T> instance = new Lazy<T>();
        public static T Instance => instance.Value;
    }
}
