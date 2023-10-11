using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumbotron : Unit
{
    [SerializeField] GameObject buzzsawPrefab;
    [SerializeField] Transform buzzsawSpawnPoint;
    [SerializeField] Collider2D visionArea;

    protected Unit targetUnit;

    protected bool isIdle {
        get {
            return animator.GetCurrentAnimatorStateInfo(0).IsTag("idle");
        }
    }

    new void FixedUpdate() {
        CheckIsGrounded();
        StopAtGround();

        DetectEnemyTarget();
        if (isIdle) {
            if (targetUnit != null) {
                Attack();
            }
        }
        ApplyDrag();
        ApplyGravity();
        ApplyVelocity();
    }

    

    protected void Attack()
    {
        animator.SetTrigger("Attack");
    }

    protected void DetectEnemyTarget()
    {
        // Collider2D[] detectedColliders = new Collider2D[10];
        // int numDetectedColliders = FindOverlappingHurtboxes(visionArea, detectedColliders);

        // targetUnit = null;

        // for (int i = 0; i < numDetectedColliders; i++)
        // {
        //     Collider2D _coll = detectedColliders[i];
        //     Unit detectedUnit = _coll.GetComponentInParent<Unit>();
            
        //     if (UnitIsMyEnemy(detectedUnit)) {
        //         targetUnit = detectedUnit;
        //     }
        // }
    }

    public void FireBuzzsaw()
    {
        GameObject _gameObject = Instantiate(buzzsawPrefab, buzzsawSpawnPoint.position, Quaternion.identity);
        Projectile projectile = _gameObject.GetComponent<Projectile>();
        Vector3 direction = isFacingRight ? Vector3.right : Vector3.left;
        projectile.Fire(direction);
    }

    public override void Hurt(int damage)
    {
        animator.SetTrigger("Hurt");
    }

    public override void Hurt(int damage, bool isVulnerable)
    {
        if (isVulnerable) {
            animator.SetTrigger("Hurt2");
        }
        else {
            Hurt(damage);
        }
    }
}
