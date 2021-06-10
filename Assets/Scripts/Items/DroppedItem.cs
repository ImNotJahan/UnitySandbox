using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour
{
    public string itemName = "undefined";
    public int id = 0;

    Text alertText;

    public void Start()
    {
        alertText = GameObject.Find("/Canvas/AlertText").GetComponent<Text>();
    }

    public void Pickup()
    {
        bool openslot = false;

        for(int k = 0; k < Inventory.instance.itemsInInv.Length; k++)
        {
            if(Inventory.instance.itemsInInv[k] == 0)
            {
                openslot = true;
                break;
            }
        }

        if(openslot == true)
        {
            Inventory.instance.CreateDraggableItem(id);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(ShowText("No free spaces in backpack"));
        }
    }

    public IEnumerator ShowText(string text)
    {
        alertText.enabled = true;
        alertText.text = text;

        yield return new WaitForSeconds(1);
        alertText.enabled = false;
    }
}
