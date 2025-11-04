namespace PremiumCalculator.Exceptions
{
    public class OccupationNotFoundException : Exception
    {
        public OccupationNotFoundException() { }
        public OccupationNotFoundException(string message) : base(message) { }
        public OccupationNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}