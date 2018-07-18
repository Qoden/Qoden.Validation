using System;
using System.Collections;
using System.Collections.Generic;
using Qoden.Util;

namespace Qoden.Validation
{
    public class Error : IEnumerable<KeyValuePair<string, object>>
    {
        private string _key;
        private Dictionary<string, object> _info;
        private string _messageFormat;

        public Error() : this(string.Empty, string.Empty)
        {
        }

        public Error(string key, string messageFormat)
        {
            MessageFormat = messageFormat;
            Key = key;
        }

        public Error(string messageFormat) : this(string.Empty, messageFormat)
        {
        }

        public string Key
        {
            get { return _key; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _key = value;
            }
        }

        public string MessageFormat
        {
            get { return _messageFormat; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _messageFormat = value;
            }
        }

        public string Message
        {
            get
            {
                if (MessageFormat == null)
                    return string.Empty;
                string message = MessageFormat;
                if (_info != null)
                {
                    if (!_info.ContainsKey("Key"))
                    {
                        _info["Key"] = Key;
                    }
                    return message.FormatWithObject(_info);
                }
                else
                {
                    return message.FormatWithObject(this);
                }
            }
        }

        public Dictionary<string, object> Info
        {
            get
            {
                if (_info == null) _info = new Dictionary<string, object>();
                return _info;
            }
        }

        public override string ToString()
        {
            return Message;
        }

        public bool TryGetValue(string key, out object value)
        {
            value = null;
            return _info?.TryGetValue(key, out value) ?? false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Info.GetEnumerator();
        }

        public void Add(string key, object value)
        {
            Info[key] = value;
        }

        public bool ContainsKey(string value)
        {
            return _info?.ContainsKey(value) ?? false;
        }

        public object this[string key]
        {
            get
            {
                if (_info == null) throw new KeyNotFoundException(key);
                return _info[key];
            }
        }

		public override bool Equals(object obj)
		{
            if (obj == null || GetType() != obj.GetType()) return false;
            Error err = (Error)obj;

            if (!_key.Equals(err._key)) return false;
            if (!_messageFormat.Equals(err._messageFormat)) return false;
            if (!_info.Equals(err._info)) return false;
            return true;
		}
	}

    public class ErrorException : Exception
    {
        private Error _error;

        public ErrorException(Error error) : base(CheckError(error).Message)
        {
            Error = error;
        }

        public ErrorException(string message, Error error) : base(message)
        {
            Error = error;
        }

        public ErrorException(string message, Exception innerException, Error error) : base(message, innerException)
        {
            Error = error;
        }

        public Error Error
        {
            get { return _error; }
            set
            {
                Assert.Property(value).NotNull();
                _error = value;
                foreach (var kv in _error.Info)
                {
                    Data.Add(kv.Key, kv.Value);
                }
            }
        }

        private static Error CheckError(Error error)
        {
            Assert.Argument(error, nameof(error)).NotNull();
            return error;
        }
    }

    public class MultipleErrorsException : ErrorException
    {
        public MultipleErrorsException(IList<Error> errors) : base(CheckErrors(errors))
        {
            Errors = errors;
        }

        public MultipleErrorsException(string message, IList<Error> errors) : base(message, CheckErrors(errors))
        {
            Errors = errors;
        }

        public MultipleErrorsException(string message, Exception innerException, IList<Error> errors) : base(message, innerException, CheckErrors(errors))
        {
            Errors = errors;
        }

        public IList<Error> Errors { get; }

        private static Error CheckErrors(IList<Error> errors)
        {
            Assert.Argument((IList) errors, nameof(errors)).MinLength(1);
            return errors[0];
        }
    }
}