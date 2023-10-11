using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frank : Unit
{
    public float moveAcceleration = 3;
    public float maxMoveSpeed = 12.0f;
    public float jumpSpeed = 18.0f;
    public float airDrift = 0.5f;
    public float attackDrift = 0.1f;
    public int maxJumpHoldFrames = 8;
    public float punchKnockback;

    public Collider2D punch1Collider;

    protected int jumpHoldFramesLeft = 0;

    protected bool isPunch1 = false;

    protected bool isAttacking {
        get {
            return animator.GetCurrentAnimatorStateInfo(2).IsTag("attack");
        }
    }

    protected new void Awake()
    {
        base.Awake();
        punch1Collider = spriteTransform.Find("Punch1Collider").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        CheckIsGrounded();
        StopAtGround();

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        bool jumpPressed = Input.GetButton("Jump");
        bool attackPressed = Input.GetButton("Attack");
        bool specialPressed = Input.GetButton("Special");
        
        // Jump
        Jump(jumpPressed);

        // Attack
        if (!isAttacking && attackPressed) {
            if (!isPunch1) {
                animator.Play("Attack.frank-punch1");
            }
            else {
                animator.Play("Attack.frank-punch2");
            }
            isPunch1 = !isPunch1;
        }

        if (!isAttacking) {
            Flip(xInput);
        }

        // Move
        float xMove = Move(xInput);

        if (xMove == 0) {
            ApplyDrag();
        }
        ApplyGravity();
        ApplyVelocity();

        Animate(xMove);
    }

    bool CheckIsAttacking()
    {
        return animator.GetCurrentAnimatorStateInfo(2).IsTag("attack");
    }

    void Animate(float xMove)
    {
        animator.SetFloat("Walk Speed", xMove * (isFacingRight ? 1 : -1));
        animator.SetFloat("Vertical Velocity", velocity.y);
        animator.SetLayerWeight(1, !isGrounded ? 1.0f : 0.0f);
        animator.SetLayerWeight(2, isAttacking ? 1.0f : 0.0f);
    }

    float Move(float xInput)
    {
        float xMove = 0;
        if (xInput != 0) {
            float accelFactor = 1;
            if (isAttacking) {
                accelFactor *= attackDrift;
            }
            if (!isGrounded) {
                accelFactor *= airDrift;
            }
            float xAcceleration = moveAcceleration * accelFactor;
            xMove = Mathf.Clamp(velocity.x + xInput * xAcceleration, -maxMoveSpeed, maxMoveSpeed);
            velocity.x = xMove;
        }
        return xMove;
    }

    bool Jump(bool jumpPressed)
    {
        if (isGrounded && jumpPressed) {
            velocity.y = jumpSpeed;
            jumpHoldFramesLeft = maxJumpHoldFrames;
            return true;
        }
        else if (jumpPressed && jumpHoldFramesLeft > 0) {
            velocity.y = jumpSpeed;
            jumpHoldFramesLeft--;
            return true;
        }
        else if (jumpHoldFramesLeft > 0) {
            jumpHoldFramesLeft = 0;
        }

        return false;
    }

    void Flip(float xInput)
    {
        bool flipped = false;

        if (isFacingRight) {
            if (xInput < 0) {
                isFacingRight = false;
                flipped = true;
            }
        }
        else {
            if (xInput > 0) {
                isFacingRight = true;
                flipped = true;
            }
        }

        if (flipped && isGrounded) {
            velocity.x = 0;
        }
    }

    public void Punch1()
    {
        Collider2D[] punchedColliders = new Collider2D[10];
        int numPunchedColliders = FindOverlappingHurtboxes(punch1Collider, punchedColliders);

        Dictionary<Unit, Hurtbox> unitHurtboxMap = new Dictionary<Unit, Hurtbox>();
        Unit[] punchedUnits = new Unit[10];
        int numPunchedUnits = 0;

        Debug.Log("- PUNCHED -");
        Debug.Log("numPunchedColliders " + numPunchedColliders);

        for (int i = 0; i < numPunchedColliders; i++)
        {
            Collider2D targetColl = punchedColliders[i];
            Hurtbox targetHurtbox = targetColl.GetComponent<Hurtbox>();
            Unit targetUnit = targetHurtbox.parentUnit;

            if (UnitIsMyEnemy(targetUnit)) {
                if (unitHurtboxMap.ContainsKey(targetUnit)) {
                    if (targetHurtbox.isVulnerable) {
                        unitHurtboxMap[targetUnit] = targetHurtbox;
                    }
                }
                else {
                    unitHurtboxMap[targetUnit] = targetHurtbox;
                    punchedUnits[numPunchedUnits] = targetUnit;
                    numPunchedUnits++;
                }
            }
        }

        if (numPunchedUnits > 0) {
            Game.FreezeFrame(Game.FREEZE_FRAME_S);
        }

        Debug.Log("numPunchedUnits " + numPunchedUnits);

        for (int i = 0; i < numPunchedUnits; i++)
        {
            Unit targetUnit = punchedUnits[i];
            Hurtbox targetHurtbox = unitHurtboxMap[targetUnit];
            Collider2D targetColl = targetHurtbox.collider;

            targetUnit.Hurt(1, targetHurtbox.isVulnerable);

            float targetDirection = Mathf.Sign(targetUnit.transform.position.x - transform.position.x);

            Vector2 targetKnockback = new Vector2(
                targetDirection * punchKnockback,
                punchKnockback * 2
            );
            targetUnit.velocity = targetKnockback * targetUnit.knockbackCo;

            Vector2 selfKnockback = Vector2.right * -targetDirection * punchKnockback * knockbackCo;
            velocity = selfKnockback;

            Vector3 hitEffectPoint = punch1Collider.ClosestPoint(targetColl.bounds.center);
            if (targetHurtbox.isVulnerable) {
                Game.InstantiateVulnerableHitspark(hitEffectPoint, Quaternion.identity, targetDirection);
            }
            else {
                Game.InstantiateHitspark(hitEffectPoint, Quaternion.identity, targetDirection);
            }
        }
    }
}
