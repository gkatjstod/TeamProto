using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking_angleTrack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;        //오브젝트지정
    public float dist = 4f;         //거리
    public float height = 3f;       //높이

    public float rotate = 3f;       //회전
    public float xSpeed = 220.0f;   //카메라 회전속도
    public float ySpeed = 100.0f;   //
    

    float ClampAngle(float angle, float min, float max)     //각도범위제한
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }


    private float x, y = 0.0f;      //카메라 초기위치


    private Transform ctrk;

    // Start is called before the first frame update
    void Start()
    {
        ctrk = GetComponent<Transform>();

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
    }

    // Update is called once per frame
    void Update()
    {
        ctrk.position = target.position - (1 * Vector3.forward * dist) + (Vector3.up * height);
        ctrk.LookAt(target);
        float AngleRotate = Mathf.LerpAngle(ctrk.eulerAngles.y, target.eulerAngles.y, rotate * Time.deltaTime);
        
        if (target)
        {
            dist -= 0.5f * Input.mouseScrollDelta.y;    //스크롤과 타겟의 거리계산
            if (dist < 0.5)
            {
                dist = 1;
            }

            if (dist >= 100)                             //타겟거리 최대최소범위
            {
                dist = 10;
            }

            if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                x += 90.0f;
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                x -= 90.0f;
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);        //카메라 위치변화 계산
            Vector3 position = rotation * new Vector3(0, 0.0f, -dist) + target.position + new Vector3(0.0f, 0, 0.0f);

            transform.rotation = rotation;
            transform.position = position;

        }
        

    }
}
