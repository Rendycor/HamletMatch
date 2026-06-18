using BattleUnits;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] float speed = 300f;
    private RectTransform rectTransform;
    private RectTransform target;
    private int damage;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetTarget(RectTransform newTarget)
    {
        target = newTarget;
    } 
    public void SetDamage(int damageToDeal)
    {
        damage = damageToDeal;
    }
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 current = rectTransform.anchoredPosition;
        Vector2 targetPos = target.anchoredPosition - new Vector2(0f,target.anchoredPosition.y-0.2f);
        Vector2 direction = (targetPos - current).normalized;

        rectTransform.anchoredPosition += direction * speed * Time.deltaTime;
        if(Vector2.Distance(current, targetPos) < 10f)
        {
            HitTarget();
        }

    }
    private BattleUnit GetTarget()
    {
        BattleUnit targetedUnit = target.GetComponent<BattleUnit>();
        return targetedUnit;
    }
    private void HitTarget()
    {
        BattleUnit targetedUnit = GetTarget();
        if (targetedUnit != null)
        {
            targetedUnit.TakeDamageOrDie(damage);
        }
        Destroy(gameObject);
    }
}
