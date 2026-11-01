
using UnityEngine;

public static class RandomResultGenerator
{
    public static SlotSymbolType GenerateResult()
    {
        int randomIndex = Random.Range(0, 4);

        return (SlotSymbolType)randomIndex;
    }

    public static SlotSymbolType[] GenerateResults()
    {
        return new SlotSymbolType[]
        {
            GenerateResult(),
            GenerateResult(),
            GenerateResult()
        };
    }
}
