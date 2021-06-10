using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFallofMap(int size)
    {
        float[,] map = new float[size, size];

        for(int i = 0; i < size; i++)
        {
            for(int k = 0; k < size; k++)
            {
                float x = i / (float)size * 2 - 1;
                float y = k / (float)size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, k] = Evaluate(value);
            }
        }

        return map;
    }

    static float Evaluate(float value)
    {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}