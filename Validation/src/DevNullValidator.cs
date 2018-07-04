namespace Qoden.Validation
{
    public class DevNullValidator : NonCollectingValidator
    {
        public static readonly DevNullValidator Instance = new DevNullValidator();

        private DevNullValidator() : base(false)
        {
        }

        public override void Add(Error error)
        {
            //ignore            
        }
    }
}