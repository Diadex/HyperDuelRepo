using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] GameObject gObject;
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - gObject.transform.position);
        
    }
}
