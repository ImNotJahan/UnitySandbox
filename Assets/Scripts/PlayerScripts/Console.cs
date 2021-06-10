using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public GameObject player;
    public CharacterController playerController;

    public Text consoleText;
    public Text logText;

    public void RunCommand()
    {
        if (!consoleText.text.Contains("`") || consoleText.text == "")
        {
            string[] command = consoleText.text.Split(' ');
            string output = "";

            output += "\n" + consoleText.text + "\n";

            switch (command[0])
            {
                case "help":
                    output += ">>\n" +
                        "help - shows all commands and their functions\n" +
                        "position - logs position\n" +
                        "tp || telport <x> <y> <z> - telports player to certain position\n" +
                        "rtp <range> - telports player to random pos\n" +
                        "clear || cls - clears the log\n" +
                        "speed <speed> - sets the players speed\n" +
                        ">>";
                    break;

                case "position":
                    output += "> " + player.transform.position.ToString();
                    break;

                case "telport":
                case "tp":
                    if(command.Length > 3)
                    {
                        int x = int.Parse(command[1]);
                        int y = int.Parse(command[2]);
                        int z = int.Parse(command[3]);

                        if (x >= 50000 || y >= 50000 || z >= 50000)
                        {
                            playerController.enabled = false;
                            player.transform.position = new Vector3(x, y, z);
                            playerController.enabled = true;
                        }
                        else
                        {
                            output += "> x y and z must each be under 50,000";
                        }
                    }
                    else
                    {
                        output += "> not enough parameters - " + command[0] + " <x> <y> <z> is the correct usage";
                    }
                    break;

                case "clear":
                case "cls":
                    logText.text = "";
                    break;

                case "rtp":
                    if (command.Length > 1)
                    {
                        int size = int.Parse(command[1]);

                        if(size <= 10000)
                        {
                            playerController.enabled = false;
                            player.transform.position = new Vector3(Random.Range(-size / 2, size / 2) + player.transform.position.x, 0, Random.Range(-size / 2, size / 2) + player.transform.position.z);
                            playerController.enabled = true;
                        }
                        else
                        {
                            output += "> the max range you can have is 10,000";
                        }
                        
                    }
                    else
                    {
                        output += "> not enough parameters - rtp <range> is the correct usage";
                    }
                    break;

                case "speed":
                    if (command.Length > 1)
                    {
                        int speed = int.Parse(command[1]);

                        if(speed <= 500)
                        {
                            player.GetComponent<PlayerMovementScript>().speed = speed;

                            output += "> set players speed to " + speed;
                        }
                        else
                        {
                            output += "> max you can set the speed to is 500";
                        }
                    }
                    else
                    {
                        output += "> not enough parameters - speed <speed> is the correct usage";
                    }

                    break;

                default:
                    output += "> command not found";
                    break;
            }

            logText.text += output;

            if(logText.text.Split('\n').Length > 25)
            {
                string[] lines = logText.text
                .Split('\n')
                .Skip(logText.text.Split('\n').Length - 25)
                .ToArray();

                logText.text = string.Join("\n", lines);
            }
        }
    }
}