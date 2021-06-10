using UnityEngine;

[CreateAssetMenu]
public class TerrainData : UpdatableData
{
    public float meshHeightMultiplier;
    public AnimationCurve[] meshHeightCurve;

    public bool isFlatShaded;

    public float uniformScale;

    public float reducingSize;

    public float riverMapMax = 0.2f;
    public float riverMapMultiplier = 5;

    public float minHeight
    {
        get
        {
            return uniformScale * meshHeightMultiplier * meshHeightCurve[0].Evaluate(0);
        }
    }

    public float maxHeight
    {
        get
        {
            return uniformScale * meshHeightMultiplier * meshHeightCurve[0].Evaluate(1);
        }
    }
}
