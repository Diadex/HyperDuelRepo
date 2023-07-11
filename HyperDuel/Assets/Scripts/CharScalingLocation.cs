using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharScalingLocation : MonoBehaviour
{
    private float size;
    public CharSize charScale;
    public float scaleFrom = 300;
    public float distanceFromBottom = 0.15f;
    private float squarePictureSize = 1.2f;
    void Start()
    {
        size = charScale.size;
        float scaleFactor = scaleFrom / size;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        transform.position += new Vector3( (-1) * (scaleFactor - 1) * squarePictureSize / 2, (scaleFactor - 1) * squarePictureSize / 2 - distanceFromBottom, 0);
    }

}
