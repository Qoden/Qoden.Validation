using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using static System.String;
using Qoden.Util;
using System.Runtime.CompilerServices;

namespace Qoden.Validation
{
    public interface IValidator : INotifyDataErrorInfo
    {
        bool IsValid { get; }

        void Add(Error error);

        void Clear(string key = null);

        IEnumerable<Error> Errors { get; }

        bool ThrowImmediately { get; }

        string ErrorKeyPrefix { get; set; }

        IEnumerable<Error> ErrorsForKey(string key);

        Check<T> CheckValue<T>(T value, string key = null, Action<Error> onError = null, bool clear = true);        

        void ReplaceErrorsForKey(string key, List<Error> errors);
    }

    public class ValidatorScope : IDisposable
    {
        private readonly IValidator _validator;
        private readonly string _oldPrefix;

        public ValidatorScope(IValidator validator, string prefix)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _oldPrefix = _validator.ErrorKeyPrefix;
            if (_oldPrefix != null)
            {
                prefix = $"{_oldPrefix}.{prefix}";
            }
            _validator.ErrorKeyPrefix = prefix;
        }

        public void Dispose()
        {
            _validator.ErrorKeyPrefix = _oldPrefix;
        }
    }

    public class PropertyValidator : Validator, IDisposable
    {
        private readonly IValidator _globalValidator;
        private readonly string _key;

        public PropertyValidator(IValidator globalValidator, [CallerMemberName] string key = null)
        {
            _globalValidator = globalValidator ?? throw new ArgumentNullException(nameof(globalValidator));
            _key = key;
        }

        public void Dispose()
        {
            var currentErrors = Errors.Select((e) => { e.Key = _key; return e; });
            var globalErrors = _globalValidator.ErrorsForKey(_key);

            if (globalErrors.All(currentErrors.Contains) && globalErrors.Count() == currentErrors.Count()) return;
            _globalValidator.ReplaceErrorsForKey(_key, currentErrors.ToList());
        }
    }

    public class Validator : IValidator
    {
        private List<Error> _errors = new List<Error>();

        public bool ThrowImmediately { get; }
        
        public bool SkipNulls { get; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool IsValid => _errors.Count == 0;

        public bool HasErrors => _errors.Count > 0;

        public string ErrorKeyPrefix { get; set; } = null;

        public Validator(bool throwImmediately = false, bool skipNulls = false)
        {
            ThrowImmediately = throwImmediately;
            SkipNulls = skipNulls;
        }

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

        public void ReplaceErrorsForKey(string key, List<Error> errors)
        {
            _errors.RemoveAll((e) => e.Key == key);
            _errors.AddRange(errors);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(key));
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

        public IEnumerable GetErrors(string key)
        {
            return IsNullOrEmpty(key) ? Errors : ErrorsForKey(key);
        }
        
        public Check<T> CheckValue<T>(T value, string key = null, Action<Error> onError = null, bool clear = true)
        {
            if (clear)
            {
                Clear(key);
            }

            IValidator validator = this;
            if (SkipNulls && value == null)
            {
                validator = DevNullValidator.Instance;                
            }
            
            return new Check<T>(value, key, validator).OnError(onError);
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