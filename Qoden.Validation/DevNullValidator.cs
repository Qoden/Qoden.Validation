namespace Qoden.Validation
{
    public class DevNullValidator : NonCollectingValidator
    {
        public static readonly DevNullValidator Instance = new DevNullValidator();
        public override void Add(Error error)
        {
            //ignore            
        }
    }
}