using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using static System.String;
using Qoden.Util;

namespace Qoden.Validation
{
    public interface IValidator : INotifyDataErrorInfo
    {
        bool IsValid { get; }

        void Add(Error error);

        void Clear();

        void Clear(string key);

        IEnumerable<Error> Errors { get; }

        bool ThrowImmediately { get; set; }

        IEnumerable<Error> ErrorsForKey(string key);
    }

    public class Validator : IValidator
    {
        private List<Error> _errors = new List<Error>();

        public bool IsValid => _errors.Count == 0;

        public bool HasErrors => _errors.Count > 0;

        public void Add(Error error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            if (ThrowImmediately)
            {
                throw new ErrorException(error);
            }
            _errors.Add(error);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(error.Key));
        }

        public void Clear()
        {
            var keys = _errors.Select(e => e.Key).Distinct();
            _errors.Clear();
            if (ErrorsChanged != null)
            {
                foreach (var key in keys)
                {
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(key));
                }
            }
        }

        public void Clear(string key)
        {
            key = key ?? Empty;
            _errors = _errors.FindAll(e => e.Key != key);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(key));
        }

        public IEnumerable<Error> Errors => _errors.AsReadOnly();

        public IEnumerable<Error> ErrorsForKey(string key)
        {
            key = key ?? Empty;
            return _errors.FindAll(e => e.Key == key);
        }

        public bool ThrowImmediately { get; set; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string key)
        {
            return IsNullOrEmpty(key) ? Errors : ErrorsForKey(key);
        }
    }

    public static class DataErrorsChangedEventArgsExtensions
    {
        public static bool IsForKey<T>(this DataErrorsChangedEventArgs args, Expression<Func<T>> prop)
        {
            var key = PropertySupport.ExtractPropertyName(prop);
            return args.PropertyName == key;
        }

        public static bool IsForKey(this DataErrorsChangedEventArgs args, string key)
        {
            return args.PropertyName == key;
        }
    }
}