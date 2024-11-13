using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private void Start()
    {
        //TEST: 临时初始化地图
        DimensionManager.Instance.InitDimension(1, 0);
    }
}