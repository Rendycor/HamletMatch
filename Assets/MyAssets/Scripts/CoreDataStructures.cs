using System.Collections.Generic;
using UnityEngine;
namespace HamletMatchCoreDataStructures
{
        public enum TileType
    {
        Empty,
        Money,
        Wood,
        Food,
        Metal,
        Stone,
        TypeBomb,
        LineBomb
    }

    public enum MatchOrientation
    {
        Horizontal,
        Vertical
    }

    public class TileData
    {
        public TileType Type;
        public Vector2Int Position;
    }

    public class MatchGroup
    {
        public TileType Type;
        public MatchOrientation Orientation;
        public readonly HashSet<Vector2Int> Positions = new();
        public int Length => Positions.Count;
    }

}
