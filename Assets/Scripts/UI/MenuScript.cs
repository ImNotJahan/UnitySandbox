using System.Collections;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.LoadGame();
        }
    }

    public void ExitToMenu()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.LoadMenu();
        }
    }

    public void ToggleGameObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void SetKeybind(string keybind)
    {
        StartCoroutine(SetKeybindI(ParseEnum<Keybinds>(keybind)));
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)System.Enum.Parse(typeof(T), value, true);
    }

    public enum Keybinds { MoveForward, MoveBackwards, MoveLeft, MoveRight, OpenInv, OpenPauseMenu, Interact };
    public IEnumerator SetKeybindI(Keybinds keybind)
    {
        KeyCode newKeybind = 0;
        bool done = false;

        while(!done)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey) && vKey != KeyCode.Mouse0)
                {
                    newKeybind = vKey;
                    done = true;
                }
            }

            yield return null;
        }

        PlayerPrefs.SetInt(keybind.ToString(), (int)newKeybind);
    }
}
