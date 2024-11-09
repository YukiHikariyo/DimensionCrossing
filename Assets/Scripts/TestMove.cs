using DG.Tweening;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    void Start()
    {
        transform.DOMove(new Vector3(1, 1, 1), 3);
    }
    
    void Update()
    {
        
    }
}
