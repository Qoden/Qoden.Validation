using System;

namespace Qoden.Validation
{
    public static class ConversionValidation
    {
        public const string ConversionMessage = "{Key} cannot be converted to {TypeCode}";

        public static Check<T> ConvertTo<T>(this Check<string> check, string message = ConversionMessage,
            IFormatProvider format = null, Action<Error> onError = null)
        {
			var vv = default(T);
			TypeCode typeCode;
			if (vv is IConvertible)
			{
				typeCode = ((IConvertible)vv).GetTypeCode();
			}
			else 
			{
				typeCode = TypeCode.Object;
			}

            Exception ex;
            try
            {
                var value = Convert.ChangeType(check.Value, typeCode, format);
                return new Check<T>((T) value, check.Key, check.Validator, check.OnErrorAction);
            }
            catch (InvalidCastException e)
            {
                //This conversion is not supported
                ex = e;
            }
            catch (FormatException e)
            {
                //value is not in a format recognized by the typeCode type.
                ex = e;
            }
            catch (OverflowException e)
            {
                //value represents a number that is out of the range of the typeCode type.
                ex = e;
            }
            var error = new Error(message)
            {
                {"Exception", ex},
                {"Value", check.Value},
                {"TargetType", typeof(T)}
            };
            check.FailValidator(error, onError);
            return new Check<T>(default(T), check.Key, check.Validator, check.OnErrorAction);
        }
    }
}