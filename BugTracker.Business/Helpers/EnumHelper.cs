namespace BugTracker.Business.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<TEnum> GetAllEnumValues<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
