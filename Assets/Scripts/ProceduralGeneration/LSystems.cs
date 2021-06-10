using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
}

public class LSystems : MonoBehaviour
{
    public float minLength = 5f;
    public float maxLength = 10f;

    public float angle = 30f;
    public int iterations = 4;
    public float scale = 1f;

    public float variance;

    public int maxRandoms;

    private GameObject branch;
    private GameObject leaf;

    private const string axiom = "X";

    public string axiomValue = "F+[[X]-X]-F[-FX]+X";

    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private string currentString = string.Empty;

    private float[] randomRotations;

    public void Start()
    {
        randomRotations = new float[1000];

        if(axiomValue == "")
        {
            for(var k = 0; k < maxRandoms; k++)
            {
                switch(Mathf.RoundToInt(UnityEngine.Random.Range(0, 3)))
                {
                    case 0:
                        axiomValue += "F";
                        break;

                    case 1:
                        axiomValue += "X";
                        break;

                    case 2:
                        axiomValue += "+";
                        break;

                    case 3:
                        axiomValue += "-";
                        break;
                }
            }

            if (!axiomValue.Contains("F"))
            {
                axiomValue = "F" + axiomValue;
            }
        }

        for(int i = 0; i < randomRotations.Length; i++)
        {
            randomRotations[i] = UnityEngine.Random.Range(-1f, 1f);
        }

        branch = Instantiate(Resources.Load("Prefabs/Branch", typeof(GameObject))) as GameObject;
        branch.transform.SetParent(transform);

        transformStack = new Stack<TransformInfo>();

        rules = new Dictionary<char, string>
        {
            {'X', axiomValue},
            {'F', "FF"}
        };

        GeneratePlant();
    }

    private void GeneratePlant()
    {
        currentString = axiom;

        StringBuilder sb = new StringBuilder();

        for (int k = 0; k < iterations; k++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }

            currentString = sb.ToString();
            sb = new StringBuilder();
        }

        for(int k = 0; k < currentString.Length; k++)
        {
            switch (currentString[k])
            {
                case 'F':
                    Vector3 initPosition = transform.position;
                    transform.Translate(Vector3.up * UnityEngine.Random.Range(minLength, maxLength));

                    GameObject treeSegment = Instantiate(branch);

                    treeSegment.GetComponent<LineRenderer>().SetPosition(0, initPosition);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    treeSegment.transform.SetParent(transform);
                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.forward * angle * (1f + variance / 100f * randomRotations[k % randomRotations.Length]));
                    break;

                case '-':
                    transform.Rotate(Vector3.back * angle * (1f + variance / 100f * randomRotations[k % randomRotations.Length]));
                    break;

                case '*':
                    transform.Rotate(Vector3.back * 120 * (1f + variance / 100f * randomRotations[k % randomRotations.Length]));
                    break;

                case '/':
                    transform.Rotate(Vector3.back * angle * (1f + variance / 100f * randomRotations[k % randomRotations.Length]));
                    break;

                case '[':
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;

                default:
                    throw new InvalidOperationException("invalid l-tree operation");
            }
        }

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        transform.localScale *= scale;
    }
}