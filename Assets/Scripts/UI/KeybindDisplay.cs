using UnityEngine;
using UnityEngine.UI;

public class KeybindDisplay : MonoBehaviour
{
    public MenuScript.Keybinds keybind;
    int assignedKey;

    Text label;

    private void Start()
    {
        label = GetComponent<Text>();
    }

    void Update()
    {
        assignedKey = PlayerPrefs.GetInt(keybind.ToString());
        
        label.text = keybind + " " + (KeyCode)assignedKey;
    }
}
