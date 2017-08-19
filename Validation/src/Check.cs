﻿using System;

namespace Qoden.Validation
{
    /// <summary>
    /// Lightweight struct binds value to validate with <see cref="IValidator"/> instance.
    /// </summary>
    /// <typeparam name="T">TYpe of value to check</typeparam>
    public struct Check<T>
    {
        public Check(T value, string key, IValidator validator, Action<Error> onErrorAction)
        {
            Validator = validator;
            Value = value;
            Key = key ?? string.Empty;
            OnErrorAction = onErrorAction;
        }

        public Check(T value, string key, IValidator validator) : this(value, key, validator, null)
        {
        }

        public Check(T value, string key) : this(value, key, null, null)
        {
        }

        public Check(T value) : this(value, null, null, null)
        {
        }

        /// <summary>
        /// Value to check.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Key to identify errors generated by this validator. Often times it is name of a property or argument being validated.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Validator instance.
        /// </summary>
        public IValidator Validator { get; set; }


        /// <summary>
        /// Add error to <see cref="Validator"/>.
        /// </summary>
        /// <param name="error">Error to add</param>
        public void Fail(Error error)
        {
            Assert.Argument(error, "error").NotNull();
            error.Key = Key;
            OnErrorAction?.Invoke(error);
            if (Validator != null)
            {
                Validator.Add(error);
            }
            else
            {
                throw new ErrorException(error);
            }
        }

        public bool IsValid => Error == null;

        public bool HasError => Error != null;

        /// <summary>
        /// Returns last error registered in <see cref="Validator"/> with this checker <see cref="Key"/>.
        /// </summary>
        public Error Error => Validator?.ErrorForKey(Key);

        /// <summary>
        /// Post processor function to apply to error before it is added to <see cref="Validator"/>.
        /// </summary>
        public Action<Error> OnErrorAction { get; set; }
    }

    public static class Check
    {
        /// <summary>
        /// Creates checker which throw validation error immediately when error detected.
        /// </summary>
        public static Check<T> Value<T>(T value, string key = null, Action<Error> onError = null, IValidator validator = null)
        {
            return new Check<T>(value, key, validator).OnError(onError);
        }
    }
}