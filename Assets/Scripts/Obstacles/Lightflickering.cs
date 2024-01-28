using System.Collections;
using UnityEngine;

public class Lightflickering : MonoBehaviour
{
    [SerializeField]
    private float minIntensity = 1.0f;    

    [SerializeField]
    private float maxIntensity = 2.0f;     

    [SerializeField]
    private float flickerSpeed = 1.0f;   
    private Light pointLight;
    private float originalIntensity;

    private void Start()
    {
        pointLight = GetComponent<Light>();

        originalIntensity = pointLight.intensity;

        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            float randomIntensity = Random.Range(minIntensity, maxIntensity);

            pointLight.intensity = randomIntensity;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));

            pointLight.intensity = originalIntensity;

            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }
}
