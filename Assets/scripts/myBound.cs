using UnityEngine;

public class myBound
{
    private Vector3 min;
    private Vector3 max;
    private Vector3 center;
    private Vector3 size;

    public myBound()
    {
        min = Vector3.zero;
        max = Vector3.zero;
        UpdateCenterAndSize();
    }

    public void SetMinMax(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
        UpdateCenterAndSize();
    }

    private void UpdateCenterAndSize()
    {
        center = (min + max) / 2;
        size = max - min;
    }

    public Vector3 GetMin() => min;
    public Vector3 GetMax() => max;
    public Vector3 GetCenter() => center;
    public Vector3 GetSize() => size;
}