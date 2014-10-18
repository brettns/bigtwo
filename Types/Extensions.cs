using System;

namespace BigTwo.Types
{
    public static class Extensions
    {
        public static void Raise<T>(this EventHandler<T> eventHandler, object sender, T args) where T : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, args);
            }
        }
    }
}
