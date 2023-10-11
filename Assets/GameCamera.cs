using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public static GameCamera Instance = null;

    [SerializeField] GameObject target = null;
    [SerializeField] Vector2 targetBoxSize = new Vector2(8, 4);
    [SerializeField] float rubberBandFactor = 0.1f;
    Vector3 destination;

    void Awake()
    {
        Instance = this;
        destination = transform.position;
    }

    void FixedUpdate()
    {
        SetDestination();
        MoveTowardsDestination();
    }

    void SetDestination()
    {
        if (target.transform.position.x < transform.position.x - targetBoxSize.x/2) {
            destination.x = target.transform.position.x + targetBoxSize.x/2;
        }
        else if (target.transform.position.x > transform.position.x + targetBoxSize.x/2) {
            destination.x = target.transform.position.x - targetBoxSize.x/2;
        }

        if (target.transform.position.y < transform.position.y - targetBoxSize.y/2) {
            destination.y = target.transform.position.y + targetBoxSize.y/2;
        }
        else if (target.transform.position.y > transform.position.y + targetBoxSize.y/2) {
            destination.y = target.transform.position.y - targetBoxSize.y/2;
        }
    }

    void MoveTowardsDestination()
    {
        Vector3 delta = destination - transform.position;
        transform.position = transform.position + delta * rubberBandFactor;
    }
}
