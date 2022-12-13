using UnityEngine;
using System.Collections;
 
public class BlinkingLight : MonoBehaviour
{
 
    public float totalSeconds;     // The total of seconds the flash wil last
    public float maxIntensity;     // The maximum intensity the flash will reach
    public Light myLight;        // Your light

    void Start() {
        StartCoroutine(FlashNow());
    }

    public IEnumerator FlashNow()
    {
        float waitTime = totalSeconds / 2;                        
        // Get half of the seconds (One half to get brighter and one to get darker)
        while (myLight.intensity < maxIntensity) {
            myLight.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (myLight.intensity > 0) {
            myLight.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(FlashNow());
    }
 }