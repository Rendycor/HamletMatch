using UnityEngine;
using HamletMatchCoreDataStructures;

public static class TileUtils
{
    private static readonly TileType[] NormalTileTypes = new[]
    {
        TileType.Money,
        TileType.Wood,
        TileType.Food,
        TileType.Metal,
        TileType.Stone
    };  
    public static TileType GetRandomTileType()
    {
        int index = UnityEngine.Random.Range(0, NormalTileTypes.Length); // Unity-friendly
        return NormalTileTypes[index];
    }
}
