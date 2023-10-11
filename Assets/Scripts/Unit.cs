using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Effect
{
    public enum UnitType { Unit, Projectile }
    public UnitType unitType = UnitType.Unit;

    public enum Team { Neutral, Player, Enemy }
    public Team team = Team.Neutral;

    public bool isAlive = true;
    public int health = 1;

    public float knockbackCo = 1.0f;
    public float frictionCo = 0.2f;
    public float airFrictionCo = 0.1f;

    protected new Rigidbody2D rigidbody;
    protected new Collider2D collider;

    protected bool isGrounded = false;
    public Vector2 velocity = Vector2.zero;

    protected new void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = transform.Find("Collider").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        CheckIsGrounded();
        StopAtGround();
        ApplyDrag();
        ApplyGravity();
        ApplyVelocity();
    }

    protected void CheckIsGrounded()
    {
        Collider2D colliderHit = null;

        BoxCollider2D boxCollider = collider as BoxCollider2D;
        CircleCollider2D circleCollider = collider as CircleCollider2D;

        if (boxCollider) {
            Vector2 unitBoxCenter = boxCollider.bounds.center;
            Vector2 unitBoxSize = boxCollider.bounds.size * transform.lossyScale.x;
            RaycastHit2D groundHit = Physics2D.BoxCast(
                unitBoxCenter,
                unitBoxSize,
                0,
                Vector2.down,
                0.05f,
                Game.DEFAULT_LAYER_MASK
            );
            colliderHit = groundHit.collider;
        }
        else if (circleCollider) {
            Vector2 unitCircleCenter = circleCollider.bounds.center;
            float unitCircleRadius = circleCollider.radius * transform.lossyScale.x;
            RaycastHit2D groundHit = Physics2D.CircleCast(
                unitCircleCenter,
                unitCircleRadius,
                Vector2.down,
                0.05f,
                Game.DEFAULT_LAYER_MASK
            );
            colliderHit = groundHit.collider;
        }

        isGrounded = colliderHit != null && velocity.y <= 0;

    }

    protected void StopAtGround()
    {
        if (isGrounded) {
            velocity.y = 0;
        }
    }

    protected void ApplyDrag()
    {
        float coefficient = isGrounded ? frictionCo : airFrictionCo;
        float friction = velocity.x * coefficient;
        velocity.x = velocity.x - friction;
        if (Mathf.Abs(velocity.x) < 0.01f) {
            velocity.x = 0;
        }
    }

    protected void ApplyGravity()
    {
        velocity.y += Game.gravity;
    }

    protected void ApplyVelocity()
    {
        rigidbody.velocity = velocity;
    }

    public virtual void Hurt(int damage) {}

    public virtual void Hurt(int damage, bool isVulnerable) {}

    public virtual void KnockBack(Vector2 velocity)
    {
        // rigidbody.velocity = velocity * knockbackCo;
        rigidbody.velocity = velocity;
    }

    public virtual void SetVelocity(Vector2 velocity)
    {

    }

    public int FindOverlappingHurtboxes(Collider2D detectionCollider, Collider2D[] overlappingColliders)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(Game.HURTBOX_LAYER_MASK);
        filter.useTriggers = true;
        Debug.Log("FindOverlappingHurtboxes");
        int colliderCount = detectionCollider.OverlapCollider(filter, overlappingColliders);
        Debug.Log("colliderCount: " + colliderCount);
        return colliderCount;
    }

    public bool UnitIsMyEnemy(Unit otherUnit)
    {
        bool isNotMe = otherUnit != this;
        bool isOtherTeam = otherUnit.team != team;

        return (
            otherUnit != null
            && isNotMe
            && isOtherTeam
        );
    }
}
