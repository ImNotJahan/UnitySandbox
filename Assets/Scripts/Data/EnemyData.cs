using UnityEngine;

[CreateAssetMenu()]
public class EnemyData : ScriptableObject
{
    public enum atkMode { Passive, Neutral, Agressive };

    public float health;
    public float atk;
    public float baseDefense;
    public float speed;

    public string[] items;

    public string[] drops;
    public float[] dropChances;

    public int lineOfSight;

    public string[] defenses;

    public atkMode mode;
}