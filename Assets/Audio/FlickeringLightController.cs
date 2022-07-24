using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLightController : MonoBehaviour
{
    protected AudioSource flickeringAudio;
    protected Light light;
    Renderer renderer;

    protected float initialLightIntensity;
    protected float lightIntensity;
    protected float intensityIncrement;
    protected float intensityMaximum;

    protected int step;

    // Start is called before the first frame update
    void Start()
    {
        flickeringAudio = transform.Find("AudioFlicker").GetComponent<AudioSource>();
        light = transform.GetComponentInChildren<Light>();
        initialLightIntensity = light.intensity;
        lightIntensity = light.intensity;
        intensityMaximum = initialLightIntensity + 1f;
        step = 0;
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartFlicker() {
        flickeringAudio.Play();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(0.2f);

        for (float i = initialLightIntensity; i <= intensityMaximum; i += 0.04f)
        {
            light.intensity = i;
            yield return new WaitForSeconds(0.001f);
        }

        lightIntensity = light.intensity;
        for (float i = lightIntensity; i >= initialLightIntensity - 0.5f; i -= 0.03f)
        {
            light.intensity = i;
            yield return new WaitForSeconds(0.001f);
        }
        yield return 0.3f;
        for (float i = initialLightIntensity - 0.5f; i <= intensityMaximum; i += 0.04f)
        {
            light.intensity = i;
            yield return new WaitForSeconds(0.001f);
        }

        lightIntensity = light.intensity;
        for (float i = lightIntensity; i >= initialLightIntensity; i -= 0.03f)
        {
            light.intensity = i;
            yield return new WaitForSeconds(0.001f);
        }
        renderer.material.DisableKeyword("_EMISSION");
        light.enabled = false;
        yield return new WaitForSeconds(1.7f);
        light.enabled = true;
        renderer.material.EnableKeyword("_EMISSION");

        yield break;
    }

    private void FadeLightIntensity()
    {

    }
}