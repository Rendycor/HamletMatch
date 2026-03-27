using UnityEngine;
using HamletMatchCoreDataStructures;
using System.Collections.Generic;
using System;
using System.Linq;
using DG.Tweening;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private GameObject tileBackgroundPrefab;
    [SerializeField] private GameObject[] TypePrefabs;
    [SerializeField] private float tileSize = 1f;

    [Header("Tween Settings")]
    [SerializeField] private float swapDuration = 0.15f;
    [SerializeField] private Ease swapEase = Ease.InOutQuad;

    [Header("Fall Stagger")]
    [SerializeField] private float fallDuration = 0.12f;
    [SerializeField] private float fallDelayPerTile = 0.005f;
    [SerializeField] private float fallDelayPerColumn = 0.01f;
    [SerializeField] private Ease fallEase = Ease.OutQuad;
    [Header("Resource Counters")]
    [SerializeField] private int MoneyCounter = 0;
    [SerializeField] private int WoodCounter = 0;
    [SerializeField] private int FoodCounter = 0;
    [SerializeField] private int MetalCounter = 0;
    [SerializeField] private int StoneCounter = 0;


    private Dictionary<TileType, HashSet<Vector2Int>> typePositions = new();
    private GameObject[,] tileVisuals;
    private GameObject[,] typeVisuals;
    private TileData[,] Grid;

    public bool IsBusy { get; private set; }

    void Awake()
    {
        Grid = new TileData[width, height];
        tileVisuals = new GameObject[width, height];
        typeVisuals = new GameObject[width, height];

        InitializeBoard();
        CreateVisualBoard();
    }

    #region Grid & Visuals

    private void CreateVisualBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new(x, y);
                SpawnTypeVisual(pos, Grid[x, y].Type);
            }
        }
    }

    private void SpawnTypeVisual(Vector2Int pos, TileType type)
    {
        if (typeVisuals[pos.x, pos.y] != null)
            Destroy(typeVisuals[pos.x, pos.y]);

        if (type == TileType.Empty) return;

        GameObject prefab = GetTypePrefab(type);
        if (prefab == null) return;

        GameObject newType = Instantiate(prefab, new Vector3(pos.x,pos.y, 0f), Quaternion.identity, transform);
        TypeInput input = newType.AddComponent<TypeInput>();
        input.GridPos = pos;

        UpdateTypeGridPos(pos);
        typeVisuals[pos.x, pos.y] = newType;
    }

    private void UpdateTypeGridPos(Vector2Int pos)
    {
        if (typeVisuals[pos.x, pos.y] == null) return;
        TypeInput input = typeVisuals[pos.x, pos.y].GetComponent<TypeInput>();
        input.GridPos = pos;
    }

    private GameObject GetTypePrefab(TileType type)
    {
        return type switch
        {
            TileType.Money => TypePrefabs[0],
            TileType.Wood => TypePrefabs[1],
            TileType.Food => TypePrefabs[2],
            TileType.Metal => TypePrefabs[3],
            TileType.Stone => TypePrefabs[4],
            TileType.TypeBomb => TypePrefabs[5],
            TileType.LineBomb => TypePrefabs[6],
            _ => null
        };
    }

    #endregion

    #region Board Initialization

    void InitializeBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Grid[x, y] = new TileData
                {
                    Type = TileUtils.GetRandomTileType(),
                    Position = new Vector2Int(x, y)
                };
            }
        }

        InitializeTypePositions();
    }

    private void InitializeTypePositions()
    {
        typePositions.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileType t = Grid[x, y].Type;
                if (t == TileType.Empty) continue;

                if (!typePositions.ContainsKey(t))
                    typePositions[t] = new HashSet<Vector2Int>();

                typePositions[t].Add(new Vector2Int(x, y));
            }
        }
    }

    private void SetTileType(Vector2Int pos, TileType newType)
    {
        TileType oldType = GetTileType(pos);
        if (oldType != TileType.Empty && typePositions.ContainsKey(oldType))
            typePositions[oldType].Remove(pos);

        Grid[pos.x, pos.y].Type = newType;

        if (newType != TileType.Empty)
        {
            if (!typePositions.ContainsKey(newType))
                typePositions[newType] = new HashSet<Vector2Int>();

            typePositions[newType].Add(pos);
        }
    }
    private TileType GetTileType(Vector2Int pos)
    {
        return Grid[pos.x, pos.y].Type;
    }
    #endregion

    #region Swap & Resolve

    public void SwapAndResolve(Vector2Int posA, Vector2Int posB)
    {
        if (IsBusy) return;
        if (!IsInsideBoard(posA) || !IsInsideBoard(posB)) return;

        StartCoroutine(SwapAndResolveRoutine(posA, posB));
    }

    private IEnumerator SwapAndResolveRoutine(Vector2Int posA, Vector2Int posB)
    {
        if (!IsInsideBoard(posA) || !IsInsideBoard(posB))
            yield break;

        IsBusy = true;

        yield return AnimateSwapTween(posA, posB).WaitForCompletion();
        SwapTiles(posA, posB);


        bool hadMatch = CheckBoardHasMatches();

        if (!hadMatch)
        {
            yield return AnimateSwapTween(posA, posB).WaitForCompletion();
            SwapTiles(posA, posB);
            IsBusy = false;
            yield break;
        }

        yield return StartCoroutine(ResolveBoardAnimatedRoutine());
        IsBusy = false;
    }

    private Tween AnimateSwapTween(Vector2Int a, Vector2Int b)
    {
        GameObject objA = typeVisuals[a.x, a.y];
        GameObject objB = typeVisuals[b.x, b.y];

        Vector3 posA = new Vector3(a.x, a.y, 0f);
        Vector3 posB = new Vector3(b.x, b.y, 0f);

        Sequence seq = DOTween.Sequence();

        if (objA != null)
            seq.Join(objA.transform.DOMove(posB, swapDuration).SetEase(swapEase));

        if (objB != null)
            seq.Join(objB.transform.DOMove(posA, swapDuration).SetEase(swapEase));

        seq.OnComplete(() =>
        {
            typeVisuals[a.x, a.y] = objB;
            typeVisuals[b.x, b.y] = objA;

            if (typeVisuals[a.x, a.y] != null)
                typeVisuals[a.x, a.y].GetComponent<TypeInput>().GridPos = a;

            if (typeVisuals[b.x, b.y] != null)
                typeVisuals[b.x, b.y].GetComponent<TypeInput>().GridPos = b;
        });

        return seq;
    }

    private void SwapTiles(Vector2Int a, Vector2Int b)
    {
        TileType temp = Grid[a.x, a.y].Type;
        Grid[a.x, a.y].Type = Grid[b.x, b.y].Type;
        Grid[b.x, b.y].Type = temp;

        InitializeTypePositions();
    }

    #endregion

    #region Board Resolution

    private IEnumerator ResolveBoardAnimatedRoutine()
    {
        while (true)
        {
            var groups = new HashSet<MatchGroup>();
            CheckHorizontalMatches(groups);
            CheckVerticalMatches(groups);

            var matches = new HashSet<Vector2Int>();
            foreach (var g in groups)
                foreach (var p in g.Positions)
                    matches.Add(p);

            if (matches.Count == 0) yield break;

            yield return AnimateClear(matches).WaitForCompletion();

            foreach (var pos in matches)
            {
                UpCounterByType(GetTileType(pos));
                SetTileType(pos, TileType.Empty);
            }
                

            foreach (var pos in matches)
            {
                if (typeVisuals[pos.x, pos.y] != null)
                {
                    Destroy(typeVisuals[pos.x, pos.y]);
                    typeVisuals[pos.x, pos.y] = null;
                }
            }

            ApplyGravityVisuals();
            yield return AnimateAllToGrid().WaitForCompletion();
            yield return RefillAnimated().WaitForCompletion();
        }
    }

    private Tween AnimateClear(HashSet<Vector2Int> matches)
    {
        Sequence seq = DOTween.Sequence();

        foreach (var pos in matches)
        {
            GameObject obj = typeVisuals[pos.x, pos.y];
            if (obj == null) continue;
            seq.Join(obj.transform.DOScale(0f, 0.12f).SetEase(Ease.InBack));
        }

        return seq;
    }

    private void ApplyGravityVisuals()
    {
        for (int x = 0; x < width; x++)
        {
            int writeY = 0;
            for (int y = 0; y < height; y++)
            {
                if (Grid[x, y].Type == TileType.Empty) continue;

                if (writeY != y)
                {
                    Grid[x, writeY].Type = Grid[x, y].Type;
                    Grid[x, y].Type = TileType.Empty;

                    typeVisuals[x, writeY] = typeVisuals[x, y];
                    typeVisuals[x, y] = null;

                    if (typeVisuals[x, writeY] != null)
                        typeVisuals[x, writeY].GetComponent<TypeInput>().GridPos = new Vector2Int(x, writeY);
                }

                writeY++;
            }
        }

        InitializeTypePositions();
    }

    private Tween AnimateAllToGrid()
    {
        Sequence seq = DOTween.Sequence();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = typeVisuals[x, y];
                if (obj == null) continue;

                Vector3 target = new Vector3(x, y, 0f);
                float distance = Vector3.Distance(obj.transform.position, target);
                float delay = Mathf.Clamp(distance * 0.03f, 0f, 0.08f);
                float duration = Mathf.Clamp(distance * 0.08f, 0.08f, 0.22f);

                seq.Join(obj.transform.DOMove(target, duration).SetEase(Ease.OutQuad).SetDelay(delay));
            }
        }

        return seq;
    }

    private Tween RefillAnimated()
    {
        Sequence seq = DOTween.Sequence();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Grid[x, y].Type != TileType.Empty) continue;

                TileType newType = TileUtils.GetRandomTileType();
                Grid[x, y].Type = newType;

                GameObject prefab = GetTypePrefab(newType);
                if (prefab == null) continue;

                Vector2Int pos = new Vector2Int(x, y);
                Vector3 spawnPos = new Vector3(x, height +2, 0f);;
                Vector3 targetPos = new Vector3(pos.x, pos.y, 0f);

                GameObject type = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
                type.AddComponent<TypeInput>().GridPos = pos;
                typeVisuals[x, y] = type;

                float distance = Vector3.Distance(spawnPos, targetPos);
                float duration = Mathf.Clamp(distance * 0.07f, 0.12f, 0.28f);

                seq.Join(type.transform.DOMove(targetPos, duration).SetEase(Ease.OutQuad));

                Vector3 finalScale = prefab.transform.localScale;
                type.transform.localScale = Vector3.zero;
                seq.Join(type.transform.DOScale(finalScale, 0.12f).SetEase(Ease.OutBack));
            }
        }

        InitializeTypePositions();
        return seq;
    }

    #endregion

    #region Matching Logic

    private void ScanLine(int length, Func<int, TileType> getType, Func<int, Vector2Int> getPos, MatchOrientation orientation, HashSet<MatchGroup> results)
    {
        int runStart = 0;

        for (int i = 1; i <= length; i++)
        {
            bool endOfRun = i == length || getType(i) != getType(i - 1);
            if (!endOfRun) continue;

            int runLength = i - runStart;
            if (runLength >= 3)
            {
                var group = new MatchGroup
                {
                    Type = getType(runStart),
                    Orientation = orientation
                };

                for (int j = runStart; j < i; j++)
                    group.Positions.Add(getPos(j));

                results.Add(group);
            }

            runStart = i;
        }
    }

    private void CheckHorizontalMatches(HashSet<MatchGroup> results)
    {
        foreach (int y in Enumerable.Range(0, height))
            ScanLine(width, x => Grid[x, y].Type, x => new Vector2Int(x, y), MatchOrientation.Horizontal, results);
    }

    private void CheckVerticalMatches(HashSet<MatchGroup> results)
    {
        foreach (int x in Enumerable.Range(0, width))
            ScanLine(height, y => Grid[x, y].Type, y => new Vector2Int(x, y), MatchOrientation.Vertical, results);
    }

    public bool IsInsideBoard(Vector2Int pos) 
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }
    private bool CheckBoardHasMatches()
    {
        var groups = new HashSet<MatchGroup>();
        CheckHorizontalMatches(groups);
        CheckVerticalMatches(groups);
    
        return groups.Count > 0;
    }
    #endregion
    #region Counter Logic

    private void UpCounterByType(TileType type)
    {
        switch (type)
        {
            case TileType.Money:
                MoneyCounter++;
                break;
            case TileType.Wood:
                WoodCounter++;
                break;
            case TileType.Food:
                FoodCounter++;
                break;
            case TileType.Metal:
                MetalCounter++;
                break;
            case TileType.Stone:
                StoneCounter++;
                break;
            default:
                break;
        }
    }
    #endregion
}
