public static class PersistentCombatStats
{
    // These variables will survive scene loads!
    public static float totalFighterDamage = 0f;
    public static float totalRangerDamage = 0f;

    // reset them when starting a new run
    public static void ResetStats()
    {
        totalFighterDamage = 0f;
        totalRangerDamage = 0f;
    }
}