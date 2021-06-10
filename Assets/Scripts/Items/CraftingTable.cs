using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    public enum CraftingTableType { Workbench, Cooking, Bottle };
    public CraftingTableType craftingTableType;

    public UIData uiData;

    public string customName = "";

    private void Start()
    {
        uiData.craftingTableUIContainer.gameObject.SetActive(true);

        for(int k = 0; k < uiData.craftingTableUIContainer.childCount; k++)
        {
            uiData.craftingTableUIContainer.GetChild(k).gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        switch (craftingTableType)
        {
            case CraftingTableType.Workbench:
                uiData.workbenchUI.SetActive(true);
                break;
            case CraftingTableType.Cooking:
                uiData.cookingUI.SetActive(true);
                break;
        }
    }
}