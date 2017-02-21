using System;

namespace Qoden.Validation
{
    public class OperationValidator : NonCollectingValidator
    {
        public static readonly OperationValidator Instance = new OperationValidator();

        public override void Add(Error error)
        {
            throw new InvalidOperationException(error.Message);
        }
    }
}