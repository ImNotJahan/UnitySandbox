using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    [Range(50f, 1000f)]
    public float mouseSensitivity = 100;

    public Transform playerBody;

    public GameObject textDisplayBox;
    public Text displayText;

    public float interactDst = 5f;

    float xRotation = 0f;

    public static MouseLook instance;

    public GameObject holding;

    private void Start()
    {
        instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        textDisplayBox.transform.parent.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Time.timeScale == 1 && !Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * Time.timeScale;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * Time.timeScale;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerBody.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        bool interact = false;
        if (Input.GetKeyUp(KeyCode.E))
        {
            interact = true;
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactDst) && Time.timeScale == 1)
        {
            if (hit.transform.tag == "Pillar")
            {
                textDisplayBox.SetActive(true);
                displayText.text = "Press E to activate the pillar";

                if (interact)
                {
                    hit.transform.tag = "Untagged";
                    hit.transform.gameObject.GetComponent<PillarScript>().Interacted(this);
                }
            } else if(hit.transform.tag == "Item")
            {
                textDisplayBox.SetActive(true);

                DroppedItem droppedItem = hit.transform.gameObject.GetComponent<DroppedItem>();

                displayText.text = "Press E to pickup the " + droppedItem.itemName;

                if (interact)
                {
                    droppedItem.Pickup();
                }
            } else if(hit.transform.tag == "Crafting")
            {
                textDisplayBox.SetActive(true);

                CraftingTable craftingTable = hit.transform.gameObject.GetComponent<CraftingTable>();

                displayText.text = "Press E to use the " + craftingTable.craftingTableType.ToString().ToLower();

                if (interact)
                {
                    craftingTable.Interact();

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    Time.timeScale = 0.13f;
                }
            }
            else
            {
                textDisplayBox.SetActive(false);
            }
        }
        else
        {
            textDisplayBox.SetActive(false);
        }
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}