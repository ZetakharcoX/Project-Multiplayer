using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject points;
    
    void Awake()
    {
        points.SetActive(false);
    }
}
