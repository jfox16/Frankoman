using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzsaw : Projectile
{
    public float moveAcceleration = 0.5f;

    new protected void Awake()
    {
        base.Awake();
    }

    new protected void FixedUpdate()
    {
        if (isAlive && lifeTimer.isDone) {
            isAlive = false;
            Die();
        }
        
        CheckIsGrounded();
        StopAtGround();

        if (isAlive) {
            if (isGrounded) {
                float xAcceleration = (isFacingRight ? 1 : -1) * moveAcceleration;
                velocity.x += xAcceleration;
            }
        }
        
        ApplyDrag();
        ApplyGravity();
        ApplyVelocity();
    }
}
