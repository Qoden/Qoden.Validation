namespace Qoden.Validation
{
    public class ImmediateValidator : NonCollectingValidator
    {
        public static readonly ImmediateValidator Instance = new ImmediateValidator(false);
        public static readonly ImmediateValidator SkipNullInstance = new ImmediateValidator(true);

        private ImmediateValidator(bool skipNulls) : base(skipNulls)
        {
        }

        public override void Add(Error error)
        {
            throw new ErrorException(error);
        }
    }
}