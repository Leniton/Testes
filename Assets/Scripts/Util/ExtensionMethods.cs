using System;
namespace Util
{
    public static class ExtensionMethods
    {
        public static bool OnTrue(this bool value, Action callback)
        {
            if (value) callback?.Invoke();
            return value;
        }
        
        public static bool OnFalse(this bool value, Action callback)
        {
            if (!value) callback?.Invoke();
            return value;
        }
    }
}
