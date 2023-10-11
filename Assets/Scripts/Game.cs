using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public static float gravity = -1.8f;
    public static LayerMask DEFAULT_LAYER_MASK;
    public static LayerMask UNIT_LAYER_MASK;
    public static LayerMask HURTBOX_LAYER_MASK;

    public static float FREEZE_FRAME_S = 0.10f;
    public static float FREEZE_FRAME_M = 0.15f;

    public GameObject hitsparkPrefab;
    public GameObject vulnerableHitsparkPrefab;

    void Awake() {
        Instance = this;

        Application.targetFrameRate = 60;

        DEFAULT_LAYER_MASK = LayerMask.GetMask("Default");
        UNIT_LAYER_MASK = LayerMask.GetMask("Unit");
        HURTBOX_LAYER_MASK = LayerMask.GetMask("Hurtbox");
    }

    public static void FreezeFrame(float time)
    {
        Instance.StartCoroutine(Instance.PauseForSeconds(time));
    }

    public static void InstantiateHitspark(Vector3 position, Quaternion rotation, float facingDirection)
    {
        GameObject hitsparkGo = Instantiate(Instance.hitsparkPrefab, position, rotation);
        Effect hitspark = hitsparkGo.GetComponent<Effect>();
        hitspark.facingDirection = facingDirection;
    }

    public static void InstantiateVulnerableHitspark(Vector3 position, Quaternion rotation, float facingDirection)
    {
        GameObject hitsparkGo = Instantiate(Instance.vulnerableHitsparkPrefab, position, rotation);
        Effect hitspark = hitsparkGo.GetComponent<Effect>();
        hitspark.facingDirection = facingDirection;
    }

    private IEnumerator PauseForSeconds(float time)
    {
        Time.timeScale = 0.0f;
        float pauseEndTime = Time.unscaledTime + time;
        while (Time.unscaledTime < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1.0f;
    }
}
