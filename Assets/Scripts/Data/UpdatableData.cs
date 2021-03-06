using UnityEngine;

public class UpdatableData : ScriptableObject
{
    public event System.Action OnValuesUpdated;
    public bool autoUpdate;

    #if UNITY_EDITOR

    protected virtual void OnValidate()
    {
        if (autoUpdate)
        {
            UnityEditor.EditorApplication.update += NotifyUpdatedValues;
        }
    }

    public void NotifyUpdatedValues()
    {
        UnityEditor.EditorApplication.update -= NotifyUpdatedValues;
        OnValuesUpdated?.Invoke();
    }

    #endif
}
