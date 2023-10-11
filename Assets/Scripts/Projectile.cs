using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Unit
{
    public float moveSpeed;
    public float lifeDuration;
    protected Timer lifeTimer;

    new protected void Awake()
    {
        base.Awake();
        lifeTimer = new Timer();
    }

    new protected void FixedUpdate()
    {
        ApplyVelocity();
    }

    public void Fire(Vector2 direction)
    {
        lifeTimer.SetTime(lifeDuration);
        velocity = direction.normalized * moveSpeed;
        isFacingRight = direction.x > 0;
    }
}
