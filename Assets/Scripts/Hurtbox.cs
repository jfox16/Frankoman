using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public Unit parentUnit { get; private set; }
    public new Collider2D collider { get; private set; }
    public bool isVulnerable;

    void Awake()
    {
        parentUnit = GetComponentInParent<Unit>();
        collider = GetComponent<Collider2D>();
    }
}
