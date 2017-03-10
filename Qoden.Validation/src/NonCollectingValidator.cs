using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Qoden.Validation
{
    public abstract class NonCollectingValidator : IValidator
    {
#pragma warning disable 0067
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }
#pragma warning restore 0067
        public abstract void Add(Error error);

        public void Clear()
        {
        }

        public void Clear(string key)
        {
        }

        public IEnumerable<Error> ErrorsForKey(string key)
        {
            return Enumerable.Empty<Error>();
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return Enumerable.Empty<Error>();
        }

        public bool IsValid => true;

        public IEnumerable<Error> Errors => Enumerable.Empty<Error>();

        public bool HasErrors => false;

        public bool ThrowImmediately
        {
            get { return true; }
            set { throw new InvalidOperationException(); }
        }
    }
}