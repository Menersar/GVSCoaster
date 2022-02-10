using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greiferArm : MonoBehaviour
{
    public bool openArm;
    public Vector3 zu;
    public Vector3 offen;
    private float speed = 10f;
    public float currentRotaion;

    void Start()
    {
        zu = transform.eulerAngles;
        offen = transform.eulerAngles;
        offen.z = offen.z - 10;
        currentRotaion = transform.eulerAngles.z;

    }

    void Update()
    {
        if (openArm && currentRotaion > offen.z)
        {
            transform.Rotate(new Vector3(0, 0, -Time.deltaTime * speed));
            currentRotaion = transform.eulerAngles.z;
        }
        else if (!openArm && currentRotaion < zu.z)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * speed));
            currentRotaion = transform.eulerAngles.z;
        }

    }

    public void toggleOpenTheArm()
    {
        openArm = !openArm;
    }
}
