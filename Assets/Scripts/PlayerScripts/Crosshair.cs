using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public float crosshairScale = 1;
    void OnGUI()
    {
        if (Time.timeScale == 1)
        {
            if (crosshairTexture != null)
            {
                GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale) / 2, crosshairTexture.height * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
            }
        }
    }
}
