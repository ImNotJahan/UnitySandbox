using UnityEngine;

public class MoveLight : MonoBehaviour
{
    public float rotation;

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, 0, 0);
    }
}
