namespace CommonProject
{
    public class Formula
    {
        public static int dailyCalorie;
        public static int bmr;

        public const double MassGainProteinPercent = 0.3;
        public const double MassGainCarbonPercent = 0.5;
        public const double MassGainFatPercent = 0.2;

        public const double FatLossProteinPercent = 0.45;
        public const double FatLossCarbonPercent = 0.2;
        public const double FatLossFatPercent = 0.35;

        public const double MaintenanceProteinPercent = 0.3;
        public const double MaintenanceCarbonPercent = 0.4;
        public const double MaintenanceFatPercent = 0.3;

        public const byte ProteinGrammToKcal = 4;
        public const byte CarbonGrammToKcal = 4;
        public const byte FatGrammToKcal = 9;

        public static void BmrCount(Gender gender, double weight, int height, int age)
        {
            switch (gender)
            {
                case Gender.Male:
                    bmr = (int)(66 + (13.7 * weight) + (5 * height) - (6.8 * age));
                    break;
                case Gender.Female:
                    bmr = (int)(655 + (9.6 * weight) + (1.8 * height) - (4.7 * age));
                    break;
            }
        }
        public static void DailyCalorie(ActivityPerWeek activity)
        {
            switch (activity)
            {
                case ActivityPerWeek.None:
                    dailyCalorie = (int)(bmr * 1.2);
                    break;
                case ActivityPerWeek.Low:
                    dailyCalorie = (int)(bmr * 1.375);
                    break;
                case ActivityPerWeek.Medium:
                    dailyCalorie = (int)(bmr * 1.55);
                    break;
                case ActivityPerWeek.High:
                    dailyCalorie = (int)(bmr * 1.725);
                    break;
                case ActivityPerWeek.Extreme:
                    dailyCalorie = (int)(bmr * 1.9);
                    break;
            }
        }
        
    }
}
