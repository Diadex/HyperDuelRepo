using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    
    private float length, startpos;
    public CharScalingLocation scaling;
    public GameObject lookObj;
    public float parallaxEffect;

    void Start()
    {
        
        startpos = transform.position.x;
        length = 2.4f * scaling.scaleFrom / (scaling.charScale.size);
        

    }

    void FixedUpdate()
    {
        float temp = (lookObj.transform.position.x * (1 - parallaxEffect));
        float dist = (lookObj.transform.position.x * parallaxEffect);

        transform.position = new Vector3 (startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
