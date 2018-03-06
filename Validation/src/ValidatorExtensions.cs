using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Qoden.Util;
#pragma warning disable CS1701 // Assuming assembly reference matches identity

namespace Qoden.Validation
{
    public static class ValidatorExtensions
    {
        public static IEnumerable<Error> ErrorsForKey<T>(this IValidator list, Expression<Func<T>> expr)
        {
			var key = PropertySupport.ExtractPropertyName(expr);
			return list.ErrorsForKey(key);
        }

        public static bool HasErrorsForKey(this IValidator list, string key = null)
        {
            key = key ?? string.Empty;
            return list.Errors.Any(e => e.Key == key);
        }

        public static bool HasErrorsForProperty(this IValidator list, [CallerMemberName] string key = null)
        {
            return list.HasErrorsForKey(key);
        }

        public static bool HasErrorsForKey<T>(this IValidator list, Expression<Func<T>> expr)
        {
            var key = PropertySupport.ExtractPropertyName(expr);
            return list.HasErrorsForKey(key);
        }

        public static Check<T> CheckProperty<T>(this IValidator result, T value, Action<Error> onError = null, [CallerMemberName] string key = null, bool clear = true)
        {
            return result.CheckValue(value, key, onError, clear);
        }

        public static Error ErrorForKey(this IValidator errors, string v)
        {
            return errors.ErrorsForKey(v).LastOrDefault();
        }

        public static void Throw(this IValidator errors)
        {
            if (errors.HasErrors)
            {
                throw new MultipleErrorsException(errors.Errors.ToList());
            }
        }
        
        public static IDisposable Scope(this IValidator validator, string prefix)
        {
            return new ValidatorScope(validator, prefix);
        }
    }
}