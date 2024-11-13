using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : Singleton<DimensionManager>
{
    public int maxDimension;
    public int currentDimension;
    
    public readonly List<IDimensional> dimensionalList = new List<IDimensional>();

    private void UpdateDimension()
    {
        foreach (var dimensional in dimensionalList)
            dimensional.UpdateDimension(currentDimension);
    }

    public void InitDimension(int max, int cur)
    {
        maxDimension = max;
        currentDimension = cur;
        UpdateDimension();
    }
    
    public void ChangeDimension()
    {
        currentDimension = currentDimension + 1 > maxDimension ? 0 : currentDimension + 1;
        UpdateDimension();
        Debug.Log(currentDimension);
    }
}