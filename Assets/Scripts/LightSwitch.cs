using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : MonoBehaviour
{
    [Header("교실 조명")]
    [SerializeField] private Light2D globalLight;

    [Header("밝기")]
    [SerializeField] private float lightOnIntensity = 0.5f;
    [SerializeField] private float lightOffIntensity = 0.1f;

    private bool isLightOn = true;


    public void Interact()
    {
        isLightOn = !isLightOn;

        if (isLightOn)
        {
            globalLight.intensity = lightOnIntensity;
        }
        else
        {
            globalLight.intensity = lightOffIntensity;
        }
    }
}