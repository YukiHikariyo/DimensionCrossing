public interface IDimensional
{
    void UpdateDimension(int currentDimension);
    
    void Register()   => DimensionManager.Instance.dimensionalList.Add(this);
    void Unregister() => DimensionManager.Instance.dimensionalList.Remove(this);
}