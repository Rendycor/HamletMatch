using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleUnits
{
    public enum BattleUnitsEnum
    {
        Archer,
        Spearmen,
        Cavalry
    }

    public abstract class BattleUnit : MonoBehaviour
    {
        [Header("Base Info")]
        [SerializeField] protected float movementSpeed;
        [SerializeField] private float health;
        [SerializeField] protected int damage;
        [SerializeField] protected int team;

        [Header("Animation")]
        [SerializeField] protected string walk;
        [SerializeField] protected string attack;
        [SerializeField] protected string die;
        [SerializeField] protected string idle;
        [SerializeField] protected float attackCooldown = 1f;
        [SerializeField] float unblockDelay = 1f;
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private float unblockTimer = 0f;
        private Coroutine flashRoutine;
        protected Animator animator;
        protected RectTransform rectTransform;
        protected new Rigidbody2D rigidbody;
        protected new BoxCollider2D collider;
        protected GameObject parentUnitlist;
        protected GameObject parentOpponentUnitlist;
        protected GameObject deathParent;

        protected enum UnitState { Moving, Attacking, Dead, Idle }
        protected UnitState currentState = UnitState.Moving;

        protected float attackTimer = 0f;

        protected RectTransform targetTransform;
        protected BattleUnit targetUnit;

        public float Health => health;

        public abstract BattleUnitsEnum StrongVS { get; }
        public abstract BattleUnitsEnum WeakVS { get; }
        public abstract BattleUnitsEnum UnitType { get; }

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            rectTransform = GetComponent<RectTransform>();
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<BoxCollider2D>();
            deathParent = GameObject.FindWithTag("Graveyard");
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;

            if (team == 0)
            {
                parentOpponentUnitlist = GameObject.FindWithTag("AiTeam");
                parentUnitlist = GameObject.FindWithTag("PlayerTeam");
            }
            else
            {
                parentOpponentUnitlist = GameObject.FindWithTag("PlayerTeam");
                parentUnitlist = GameObject.FindWithTag("AiTeam");
            }

            animator.Play(walk);
        }

        protected virtual void Update()
        {
            if (health <= 0 && currentState != UnitState.Dead)
            {
                Die();
                return;
            }

            if (currentState == UnitState.Dead) return;

            attackTimer -= Time.deltaTime;

            switch (currentState)
            {
                case UnitState.Moving:
                    HandleMoving();
                    break;

                case UnitState.Attacking:
                    HandleAttacking();
                    break;
                case UnitState.Idle:
                    HandleIdle();
                    break;
            }
        }

        protected bool IsBlocked()
        {
            foreach (Transform child in parentUnitlist.transform)
            {
                if (child == transform) continue;
                RectTransform unit = child.GetComponent<RectTransform>();
                float dx = unit.position.x - rectTransform.position.x;

                if (team == 0 && dx <= 0) continue;
                if (team == 1 && dx >= 0) continue;

                float dist = Mathf.Abs(dx);

                if(dist < 1f)
                {
                    return true;
                }
            }
            return false;
        }
        protected void EnterIdleState()
        {
            currentState = UnitState.Idle;
            animator.Play(idle);
        }
        void HandleIdle()
        {
            if (targetTransform != null && IsTargetInRange())
            {
                EnterAttackState();
                return;
            }
            if (!IsBlocked())
            {
                unblockTimer += Time.deltaTime;
                if (unblockTimer >= unblockDelay)
                {
                    currentState = UnitState.Moving;
                    unblockTimer = 0f;
                }
            }else
            {
                unblockTimer = 0f;
            }
        }
        protected virtual void HandleMoving()
        {
            FindTarget();

            if (targetTransform != null && IsTargetInRange())
            {
                EnterAttackState();
                return;
            }
            if (IsBlocked())
            {
                EnterIdleState();
                return;
            }
            MoveThroughBattlefield();
        }

        protected virtual void HandleAttacking()
        {
            if (!IsTargetValid())
            {
                ExitAttackState();
                return;
            }

            if (!IsTargetInRange())
            {
                ExitAttackState();
                return;
            }

            if (attackTimer <= 0f)
            {
                attackTimer = attackCooldown;
                animator.Play(attack);
            }
        }

        protected void EnterAttackState()
        {
            currentState = UnitState.Attacking;
        }

        protected void ExitAttackState()
        {
            currentState = UnitState.Moving;
            targetTransform = null;
            targetUnit = null;
        }

        protected bool IsTargetValid()
        {
            return targetUnit != null && targetUnit.health > 0;
        }

        protected virtual bool IsTargetInRange()
        {
            if (targetTransform == null) return false;

            float dist = Mathf.Abs(targetTransform.position.x - rectTransform.position.x);
            return dist <= GetRange();
        }

        protected void MoveThroughBattlefield()
        {
            animator.Play(walk);

            Vector2 pos = rectTransform.anchoredPosition;

            if (team == 0)
                pos.x += movementSpeed * Time.deltaTime;
            else
                pos.x -= movementSpeed * Time.deltaTime;

            rectTransform.anchoredPosition = pos;
        }

        public int CalculateDamage(BattleUnitsEnum opposingUnit)
        {
            if (opposingUnit == WeakVS) return damage / 2;
            if (opposingUnit == StrongVS) return damage * 2;
            return damage;
        }

        public void TakeDamageOrDie(int dmg)
        {
            if (currentState == UnitState.Dead) return;
            health -= dmg;
            if (health > 0)
            {
                if (flashRoutine != null)
                    StopCoroutine(flashRoutine);

                flashRoutine = StartCoroutine(FlashRedRoutine());
            }
            else
            {
                Die();
            }
        }
        IEnumerator FlashRedRoutine()
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.2f);

            if (currentState != UnitState.Dead)
            {
                spriteRenderer.color = originalColor;
            }
        }

        void Die()
        {
            currentState = UnitState.Dead;

            if (rigidbody != null) Destroy(rigidbody);
            if (collider != null) Destroy(collider);
            if(spriteRenderer.color == Color.red)
            {
                spriteRenderer.color = originalColor;
            }

            if (deathParent != null)
            {
                transform.SetParent(deathParent.transform, true);
            }

            animator.Play(die);

            StartCoroutine(FadeOutRoutine());
        }
        IEnumerator FadeOutRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            float time = 0f;
            Color startColor = spriteRenderer.color;

            while (time < 3f)
            {
                float alpha = Mathf.Lerp(startColor.a, 0f, time / 3f);

                spriteRenderer.color = new Color(
                    startColor.r,
                    startColor.g,
                    startColor.b,
                    alpha
                );

                time += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                0f
            );

            Destroy(gameObject);
        }

        protected abstract float GetRange();
        protected abstract void FindTarget();
        public abstract void OnAttackAnimationFinished();
    }
    #region RangedUnit
    public abstract class RangedUnit : BattleUnit
    {
        [Header("Ranged")]
        [SerializeField] float range;
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;

        protected override float GetRange() => range;

        protected override void FindTarget()
        {
            float myX = rectTransform.position.x;
            float closest = Mathf.Infinity;

            targetTransform = null;

            foreach (Transform child in parentOpponentUnitlist.transform)
            {
                RectTransform unit = child.GetComponent<RectTransform>();
                float dx = unit.position.x - myX;

                if (team == 0 && dx <= 0) continue;
                if (team == 1 && dx >= 0) continue;

                float dist = Mathf.Abs(dx);

                if (dist < closest && dist <= range)
                {
                    closest = dist;
                    targetTransform = unit;
                }
            }

            if (targetTransform != null)
                targetUnit = targetTransform.GetComponent<BattleUnit>();
        }

        public override void OnAttackAnimationFinished()
        {
            if (!IsTargetValid()) return;

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity, transform.parent);

            var script = proj.GetComponent<ProjectileScript>();
            script.SetTarget(targetTransform);
            script.SetDamage(CalculateDamage(targetUnit.UnitType));
        }
    }
    #endregion
    #region MeleeUnit

    public abstract class MeleeUnit : BattleUnit
    {
        [SerializeField] float range = 1f;

        protected override float GetRange() => range;

        protected override void FindTarget()
        {
            float myX = rectTransform.position.x;
            float closest = Mathf.Infinity;

            targetTransform = null;

            foreach (Transform child in parentOpponentUnitlist.transform)
            {
                RectTransform unit = child.GetComponent<RectTransform>();
                BattleUnit other = child.GetComponent<BattleUnit>();

                if (other == null || other.Health <= 0) continue;

                float dx = unit.position.x - myX;

                // only check forward
                if (team == 0 && dx <= 0) continue;
                if (team == 1 && dx >= 0) continue;

                float dist = Mathf.Abs(dx);

                if (dist < closest)
                {
                    closest = dist;
                    targetTransform = unit;
                    targetUnit = other;
                }
            }
        }
        public override void OnAttackAnimationFinished()
        {
            if (!IsTargetValid()) return;

            targetUnit.TakeDamageOrDie(CalculateDamage(targetUnit.UnitType));
        }
    }
    #endregion
}