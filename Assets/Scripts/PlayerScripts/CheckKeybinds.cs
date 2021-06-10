using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void checkKeybinds()
    {

    }

    public int[] getKeybinds()
    {
        int[] keybinds = new int[System.Enum.GetNames(typeof(MenuScript.Keybinds)).Length];

        int k = 0;
        foreach (MenuScript.Keybinds vKey in System.Enum.GetValues(typeof(MenuScript.Keybinds)))
        {
            keybinds[k] = PlayerPrefs.GetInt(vKey.ToString());
            k++;
        }

        return keybinds;
    }
}
