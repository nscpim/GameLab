using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLights : MonoBehaviour
{

    public float rotationvalue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rotating the Volumetric Lights
       transform.Rotate(0, rotationvalue, 0, Space.World);
    }
}
