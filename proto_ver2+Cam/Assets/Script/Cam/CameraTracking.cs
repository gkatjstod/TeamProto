using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{

    //public GameObject target;

    public float offsetX = 0f;
    public float offsetY = 0f;
    public float offsetZ = 0f;
    public float followspeed = 2f;

    public Transform target;
    public 

    Vector3 cameraPosition;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Camera");

    }


    void  LateUpdate()
    {


        cameraPosition.x = target.transform.position.x + offsetX;
        cameraPosition.y = target.transform.position.y + offsetY;
        cameraPosition.z = target.transform.position.z + offsetZ;
        

        transform.position = Vector3.Lerp(transform.position, cameraPosition, followspeed * Time.deltaTime);
        


    }

}
