using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Font font1;
    public Font font2;

    public ItemData itemData;

    public static Inventory instance;

    public int[] itemsInInv = new int[20];

    public int[] craftingItems = new int[2] { 0, 0 };
    Dictionary<string, int> recipies = new Dictionary<string, int>();

    private void Awake()
    {
        instance = this;

        for(int k = 0; k < itemsInInv.Length; k++)
        {
            itemsInInv[k] = 0;
        }

        DeactivateAllChildren();
        AddCodexEntries();

        GetRecipies();
    }

    private void Update()
    {
        if(craftingItems[0] != 0 && craftingItems[1] != 0)
        {
            if(recipies.ContainsKey(craftingItems[0].ToString() + "|" + craftingItems[1].ToString()))
            {
                transform.GetChild(2).GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(2).GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            transform.GetChild(2).GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(false);
        }

        if(itemsInInv[0] != 0)
        {
            MouseLook.instance.holding = itemData.itemValues[itemsInInv[0]].itemGameobject;
        }
    }

    public void DeactivateAllChildren()
    {
        for (var k = 1; k < transform.childCount; k++)
        {
            transform.GetChild(k).gameObject.SetActive(false);
        }
    }

    public void AddCodexEntries()
    {

        string text = Resources.Load<TextAsset>("Text/CodexEntries").text;
        string[] seperators = { ">" };
        string[] seperators2 = { Environment.NewLine };

        string[] seperatedText = text.Split(seperators, StringSplitOptions.None);

        for(var k = 1; k < seperatedText.Length; k++)
        {
            GameObject button = (GameObject)Instantiate(Resources.Load<UnityEngine.Object>("Prefabs/Button"));

            button.transform.SetParent(transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0));
            button.transform.GetChild(0).GetComponent<Text>().text = seperatedText[k].Split(seperators2, StringSplitOptions.None)[0];
            button.transform.localPosition = new Vector2(0, 320 - (k - 1) * 40);

            button.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, k * 40);

            string codexText = seperatedText[k];

            button.GetComponent<Button>().onClick.AddListener(() => { SetCodexEntryText(codexText); });
        }
    }

    private void GetRecipies()
    {
        string text = Resources.Load<TextAsset>("Text/Recipies").text;
        string[] seperators2 = { Environment.NewLine };

        string[] seperatedText = text.Split(seperators2, StringSplitOptions.None);

        for (var k = 0; k < seperatedText.Length; k += 2)
        {
            recipies.Add(seperatedText[k], Convert.ToInt32(seperatedText[k + 1]));
        }
    }

    private void SetCodexEntryText(string text)
    {
        transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = text;
    }

    bool toggled = false;
    public void ToggleFont(Text text)
    {
        if(!toggled)
        {
            text.font = font2;
            text.fontSize = 25;

            toggled = true;
        }
        else
        {
            text.font = font1;
            text.fontSize = 45;

            toggled = false;
        }
    }

    public GameObject draggableObject;
    public void CreateDraggableItem(int id)
    {
        GameObject createdItem = Instantiate(draggableObject);

        createdItem.GetComponent<DraggableItem>().id = id;
        createdItem.transform.parent = transform;

        for(int k = 0; k < itemsInInv.Length; k++)
        {
            if(itemsInInv[k] == 0)
            {
                itemsInInv[k] = 1;

                createdItem.GetComponent<DraggableItem>().currentInvSlot = k;
                createdItem.transform.localPosition = new Vector2(-465 + 230 * (k % 5), 290 - 150 * Mathf.FloorToInt(k / 5));
                break;
            }
        }

        createdItem.transform.SetParent(transform.GetChild(2));

        createdItem.SetActive(true);
    }
}
