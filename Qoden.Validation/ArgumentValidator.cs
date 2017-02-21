using System;

namespace Qoden.Validation
{
    public class ArgumentValidator : NonCollectingValidator
    {
        public static readonly ArgumentValidator Instance = new ArgumentValidator();

        public override void Add(Error error)
        {
            if (error.ContainsKey("Value") && error["Value"] == null)
            {
                throw new ArgumentNullException(error.Key);
            }
            throw new ArgumentException(error.Message, error.Key);
        }
    }
}