using UnityEngine;
using BattleUnits;
using System.Collections.Generic;

public class Spawner2D : MonoBehaviour
{
    [SerializeField] GameObject archer;
    [SerializeField] GameObject cavalry;
    [SerializeField] GameObject spearMan;
    [SerializeField] RectTransform spawnPoint;
    [SerializeField] float spawnCooldown = 0.5f;
    [SerializeField] RectTransform TroopParent;
    public LayerMask blockingLayers;
    private Queue<GameObject> spawnQueue = new Queue<GameObject>();
    
    protected float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (TryProcessQueue())
            {
                timer = spawnCooldown;
            }
        }
    }

    public void EnqueueUnit(BattleUnitsEnum unitType)
    {
        switch (unitType)
        {
            case BattleUnitsEnum.Archer:
            spawnQueue.Enqueue(archer);
            break;
            case BattleUnitsEnum.Cavalry:
            spawnQueue.Enqueue(cavalry);
            break;
            case BattleUnitsEnum.Spearmen:
            spawnQueue.Enqueue(spearMan);
            break;
        }
    }
    
    bool TryProcessQueue()
    {
        if (spawnQueue.Count == 0)
            return false;

        GameObject nextPrefab = spawnQueue.Peek();

        Vector2 size = GetPrefabSize(nextPrefab);

        Collider2D hit = Physics2D.OverlapBox(spawnPoint.position,size,0f,blockingLayers);

        if (hit != null)
            return false;

        Instantiate(spawnQueue.Dequeue(), spawnPoint.position, spawnPoint.rotation, TroopParent);
        return true;
    }
    Vector2 GetPrefabSize(GameObject prefab)
    {
        Collider2D col = prefab.GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogWarning(prefab.name + " has no Collider2D! Using fallback size.");
            return Vector2.one;
        }

        return col.bounds.size;
    }
}

