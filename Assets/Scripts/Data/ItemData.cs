using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemData : ScriptableObject
{
    public List<itemValues> itemValues;

    public Dictionary<string, itemValues> itemStats = new Dictionary<string, itemValues>();

    private void Awake()
    {
        for (int numItem = 0; numItem < itemValues.Count; numItem++)
        {
            itemStats.Add(itemValues[numItem].name, itemValues[numItem]);
        }
    }
}

[System.Serializable]
public struct itemValues
{
    public string name;

    public enum ItemType { Weapon, Consumable };
    public ItemType itemType;

    public Texture itemTexture;
    public GameObject itemGameobject;

    public float atk;
    public float def;
}