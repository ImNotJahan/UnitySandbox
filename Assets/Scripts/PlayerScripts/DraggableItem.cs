using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Collider2D collision;
    Vector3 startPos;
    Inventory inventory;

    int wasCraftingItem = 0;

    public int id = 0;

    public int currentInvSlot = 0;

    private void Awake()
    {
        inventory = Inventory.instance;

        gameObject.GetComponent<RawImage>().texture = inventory.itemData.itemValues[id].itemTexture;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        if(wasCraftingItem != 0)
        {
            inventory.craftingItems[wasCraftingItem - 1] = 0;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(collision != null && collision.tag != "InvItem")
        {
            transform.position = collision.transform.position;
            inventory.itemsInInv[currentInvSlot] = 0;
            if(collision.tag == "recipieSlot1")
            {
                inventory.craftingItems[0] = id;
                wasCraftingItem = 1;
            } else if(collision.tag == "recipieSlot2")
            {
                inventory.craftingItems[1] = id;
                wasCraftingItem = 2;
            }
            else
            {
                currentInvSlot = int.Parse(collision.name);
            }
        }
        else
        {
            transform.position = startPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collision = other;
    }
}
