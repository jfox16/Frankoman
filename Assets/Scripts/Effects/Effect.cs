using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    protected Animator animator;
    protected Transform spriteTransform;

    public bool isFacingRight {
        get {
            return Mathf.Sign(spriteTransform.localScale.x) > 0;
        }
        set {
            Vector3 newLocalScale = Vector3.zero + spriteTransform.localScale;
            newLocalScale.x = value ? 1 : -1;
            spriteTransform.localScale = newLocalScale;
        }
    }
    public float facingDirection {
        get {
            return Mathf.Sign(spriteTransform.localScale.x);
        }
        set {
            Vector3 newLocalScale = Vector3.zero + spriteTransform.localScale;
            newLocalScale.x = Mathf.Sign(value);
            spriteTransform.localScale = newLocalScale;
        }
    }

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        spriteTransform = transform.Find("Sprite");
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
