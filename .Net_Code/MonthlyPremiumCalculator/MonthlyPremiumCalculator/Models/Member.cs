namespace MonthlyPremiumCalculator.Models
{
    public class Member
    {
        public string Name { get; set; }
        public int AgeNextBirthday { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Occupation { get; set; }
        public decimal DeathSumInsured { get; set; }
    }
}
