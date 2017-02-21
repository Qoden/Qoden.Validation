using System.Runtime.CompilerServices;

namespace Qoden.Validation
{
    public static class Assert
    {
        public static Check<T> Argument<T> (T value, string key = null)
        {			
            return new Check<T> (value, key, ArgumentValidator.Instance);
        }

        public static Check<T> State<T> (T value, string key = null)
        {			
            return new Check<T> (value, key, OperationValidator.Instance);
        }

        public static Check<T> Property<T>(T value, [CallerMemberName] string key = null)
        {
            return State(value, key);
        }
    }
}