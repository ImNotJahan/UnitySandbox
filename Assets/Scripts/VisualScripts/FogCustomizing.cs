using UnityEngine;

public class FogCustomizing : MonoBehaviour
{
    public static void WaterSettings(bool inWater)
    {
        if (inWater)
        {
            RenderSettings.fogColor = new Color(0, 0.78f, 1);
            RenderSettings.fogStartDistance = 10;
            RenderSettings.fogEndDistance = 30;
        }
        else
        {
            RenderSettings.fogColor = Color.gray;
            RenderSettings.fogStartDistance = 100;
            RenderSettings.fogEndDistance = 300;
        }
    }
}
