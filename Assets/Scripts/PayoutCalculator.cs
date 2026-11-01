
public static class PayoutCalculator
{
    public static int CalculatePayout(
        SlotSymbolType first,
        SlotSymbolType second,
        SlotSymbolType third,
        int bet)
    {
        // All three symbols must match.
        if (first != second || second != third)
        {
            return 0;
        }

        int multiplier = GetMultiplier(first);

        return bet * multiplier;
    }

    private static int GetMultiplier(SlotSymbolType symbol)
    {
        switch (symbol)
        {
            case SlotSymbolType.Seven:
                return 10;

            case SlotSymbolType.Bar:
                return 7;

            case SlotSymbolType.Bell:
                return 5;

            case SlotSymbolType.Cherries:
                return 3;

            default:
                return 0;
        }
    }
}
