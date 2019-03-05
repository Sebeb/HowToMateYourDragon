using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] static bool currentlyShaking = false;
    [SerializeField] static float shakeDur = .25f;
    [SerializeField] static float shakeDurMul = 1;
    [SerializeField] static float shakeOffset = .6f;
    [SerializeField] static float shakeOffsetMul = 1;
    [SerializeField] static float shakeFreq = .05f;

    public static GameObject target;

    public static bool shouldShake;

    Vector2 originalPos;
    float shakeTimer;
    float velocity;

    // Use this for initialization
    void Start()
    {
        target = gameObject;
        //target = GetComponent<CameraFollow>().target;
    }

    public void Shake()
    {
        originalPos = transform.position;
        currentlyShaking = true;
        shakeTimer = 0;
    }

    public static void Shake(float intensity = 1)
    {
        shouldShake = true;
        shakeOffsetMul = intensity;
        shakeDurMul = intensity / 8;
    }

    void LateUpdate()
    {
        if (!currentlyShaking)
            if (shouldShake)
                Shake();
            else
                return;

        float remTime = shakeDur * shakeDurMul - shakeTimer;
        originalPos = target.transform.position;
        //originalPos = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        Vector2 shakePos = originalPos + shakeOffset * shakeOffsetMul * Random.insideUnitCircle;

        float x = Mathf.SmoothDamp(transform.position.x, shakePos.x, ref velocity, shakeFreq);
        float y = Mathf.SmoothDamp(transform.position.y, shakePos.y, ref velocity, shakeFreq);

        transform.position = new Vector3(x, y, transform.position.z);

        shakeTimer += Time.deltaTime;
        //EndShake(100000000 / Time.deltaTime / shakeDur / shakeDurMul);
        /*if (shakeTimer > shakeDur * shakeDurMul - 0.3f)
        {
            EndShake(100);
        }*/
        if (shakeTimer > shakeDur * shakeDurMul)
            EndShake();
    }

    private void EndShake()
    {
        currentlyShaking = false;
        shouldShake = false;
        transform.position -= transform.position - new Vector3(originalPos.x, originalPos.y, transform.position.z);
        shakeTimer = 0;
    }
}
