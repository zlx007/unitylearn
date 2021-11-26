using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string keyUp;
    public string keyDown;
    public string keyLeft;
    public string keyRight;
    
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;


    public bool inputEnable = true;

    private float TargetDup;
    private float TargetDright;
    private float velocityDup;
    private float velocityDright;
    void Update()
    {
        TargetDup = (Input.GetKey(keyUp)?1.0f:0) - (Input.GetKey(keyDown)?1.0f:0); 
        TargetDright= (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        if (inputEnable==false) {
            TargetDup = 0;
            TargetDright = 0;
        }
        Dup = Mathf.SmoothDamp(Dup,TargetDup,ref velocityDup,0.1f);
        Dright = Mathf.SmoothDamp(Dright,TargetDright,ref velocityDright,0.1f);
        Dmag = Mathf.Sqrt((Dup * Dup) + (Dright * Dright));
        Dvec = Dright * transform.right + Dup * transform.forward;
    }


}
