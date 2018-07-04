using System;

namespace Qoden.Validation
{
    public class OperationValidator : NonCollectingValidator
    {
        public static readonly OperationValidator Instance = new OperationValidator(false);
        public static readonly OperationValidator SkipNullsInstance = new OperationValidator(true);

        public override void Add(Error error)
        {
            throw new InvalidOperationException(error.Message);
        }

        private OperationValidator(bool skipNulls) : base(skipNulls)
        {
        }
    }
}