using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("=====key settings=====")]
    public string keyUp;
    public string keyDown;
    public string keyLeft;
    public string keyRight;
    
    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    public string keyJUp;
    public string keyJDown;
    public string keyJLeft;
    public string keyJRight;

    [Header("=====Output singals=====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    public float Jup;
    public float Jright;

    public bool run;
    public bool jump;
    private bool lastJump;
    public bool attack;
    private bool lastAttack;

    [Header("=====other======")]
    public bool inputEnable = true;

    private float TargetDup;
    private float TargetDright;
    private float velocityDup;
    private float velocityDright;

    void Update()
    {
        Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
        Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);
        TargetDup = (Input.GetKey(keyUp)?1.0f:0) - (Input.GetKey(keyDown)?1.0f:0); 
        //TargetDup = Input.GetAxis("Vertical");
        TargetDright= (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        //TargetDright = Input.GetAxis("Horizontal");
        if (inputEnable==false) {
            TargetDup = 0;
            TargetDright = 0;
        }
        Dup = Mathf.SmoothDamp(Dup,TargetDup,ref velocityDup,0.1f);
        Dright = Mathf.SmoothDamp(Dright,TargetDright,ref velocityDright,0.1f);
        Vector2 square = new Vector2(Dright, Dup);
        Dright=SquareToCircle(square).x;
        Dup = SquareToCircle(square).y;
        Dmag = Mathf.Sqrt((Dup * Dup) + (Dright * Dright));
        //Fixme:转向后的上下左右还是原来的
        Dvec = Dright *transform.right + Dup *transform.forward;
        run = Input.GetKey(keyA);
        bool newJump = Input.GetKey(keyB);
        if (newJump!=lastJump&&lastJump==true) {
            jump = true;
            //print("!!!!");
        }
        else {
            jump = false;
        }
        lastJump = newJump;

        bool newAttack = Input.GetKey(keyC);
        if (newAttack != lastAttack && lastAttack == true)
        {
            attack = true;
            //print("!!!!");
        }
        else
        {
            attack = false;
        }
        lastAttack = newAttack;

    }
    Vector2 SquareToCircle(Vector2 square) {//椭圆映射
        Vector2 circle;
        circle.x = square.x * Mathf.Sqrt(1-(square.y*square.y)/2);
        circle.y = square.y * Mathf.Sqrt(1-(square.x*square.x)/2);
        return circle;
    }


}
