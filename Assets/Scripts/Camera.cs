using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public static Camera Instance;

    public GameObject target;

    void Awake()
    {
        Instance = this;
    }

    void LateUpdate()
    {
        if (target) {
            transform.position = new Vector3(
                target.transform.position.x,
                target.transform.position.y,
                transform.position.z
            );
        }
    }
}
