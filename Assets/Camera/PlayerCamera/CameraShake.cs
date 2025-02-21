using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public void SmallShake()
    {

        StartCoroutine(this.Shake(.1f, .1f));

    }

    public void MedShake()
    {

        StartCoroutine(this.Shake(.2f, .2f));
        
    }

    public void LargeShake()
    {

        StartCoroutine(this.Shake(.4f, .2f));

    }

    public IEnumerator Shake(float durantion, float magitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsedTime = 0.0f;

        while (elapsedTime < durantion)
        {
            float x = Random.Range(-1f, 1f) * magitude;
            float y = Random.Range(-1f, 1f) * magitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
