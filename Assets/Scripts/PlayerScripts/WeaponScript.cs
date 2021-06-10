using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float weaponSpeed = 70;
    public float zWeaponSpeed = 140;

    float xRotation = 0f;
    float zRotation = 0f;

    public Transform axis;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * weaponSpeed * Time.deltaTime * Time.timeScale;
            float mouseY = Input.GetAxis("Mouse Y") * weaponSpeed * Time.deltaTime * Time.timeScale;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 12f);

            axis.localEulerAngles = new Vector3(0, Mathf.Min(Mathf.Max(axis.localEulerAngles.y + mouseX, -5), 75), 0);

            zRotation += Input.mouseScrollDelta.y * zWeaponSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);
        }
    }
}
