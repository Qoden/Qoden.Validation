namespace Qoden.Validation
{
    public class ImmediateValidator : NonCollectingValidator
    {
        public static readonly ImmediateValidator Instance = new ImmediateValidator();

        public override void Add(Error error)
        {
            throw new ErrorException(error);
        }
    }
}