using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDivide : MonoBehaviour
{
    public Camera maincam;
    public Camera subCam1;
    public Camera subCam2;
    public Camera subCam3;


    void Start()
    {
        //maincam = Camera.main;
    }


    void Update()
    {
        if (Input.GetKey("1"))
        {
            maincam.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            subCam1.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
            subCam2.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            subCam3.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        }
        if (Input.GetKey("2"))
        {
            maincam.rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
            subCam1.rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
            subCam2.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            subCam3.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        }
        if (Input.GetKey("3"))
        {
            maincam.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            subCam1.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
            subCam2.rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
            subCam3.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        }
        if (Input.GetKey("4"))
        {
            maincam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            subCam1.rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
            subCam2.rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
            subCam3.rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        }



        MoveAnimation moveAnimation = GameObject.Find("Player_col").GetComponent<MoveAnimation>();
        
      

    }
}
