using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCart : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] wayPoints;
    private int index = 0;

    public float speed = 10f;
    public float defaultSpeed = 10f;

    private float speedImpactOfAngle = 10f; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

        float angleX = transform.rotation.x;
        float angleZ = transform.rotation.z;
        speed = defaultSpeed + speedImpactOfAngle * angleX;
        if (speed <= 0)
        {
            speed = .01f;
        }
       float distance = Vector3.Distance(transform.position, wayPoints[index].transform.position);

        if (distance < 0.5f)
        {
            if (index < wayPoints.Length -1)
            {
                index++;
            } else
            {
                index = 0;
            }
        }

        //transform.LookAt(wayPoints[index].transform.position);

        var targetRotation = Quaternion.LookRotation(wayPoints[index].transform.position - transform.position);
     //   Quaternion.
            
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
       // transform.rotation = Quaternion.Slerp(transform.rotation, wayPoints[index].transform.rotation, speed * Time.deltaTime);

        
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[index].transform.position, speed * Time.deltaTime);
    }
}
