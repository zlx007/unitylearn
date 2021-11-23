using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("---key settings--")]
    public string keyUp = "w";
    public string keyDown="s";
    public string keyLeft="a";
    public string keyRight="d";
    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;
    [Header("---output singals--")]
    public float Dup;
    public float Dright;
    public float Dmag;//勾股定理中的c
    public Vector3 Dvec;//方向
    [Header("---others settings--")]
    public bool inputEnable = true;
    //press singals
    public bool run;
    //trigger once singals
    public bool jump;
    private bool lastJump;
    //double singals
        
     private float targetDup;
     private float targetDright;
     private float velocityDup;
     private float velocityDright;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetDup = (Input.GetKey(keyUp)?1.0f:0 )- (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        if (inputEnable == false)
        {
            targetDright = 0;
            targetDup = 0;
        }
        Dup = Mathf.SmoothDamp(Dup,targetDup,ref velocityDup,0.1f);
        Dright = Mathf.SmoothDamp(Dright,targetDright,ref velocityDright,0.1f);
        Vector2 tempDAxis = SqualToCircle(new Vector2(Dup,Dright));//椭圆映射
        float Dup2 = tempDAxis.x;
        float Dright2 = tempDAxis.y;

        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec= Dright2 * transform.right + Dup2 * transform.forward;
        run = Input.GetKey(keyA);
        bool newJump = Input.GetKey(keyB);
        jump = newJump;
        if (newJump != lastJump && newJump == true)
        {
            jump = true;
            //播放跳跃动画
            //print("jump sucess");
        }
        else {
            jump = false;
        }
        lastJump = newJump;
    }
    private Vector2 SqualToCircle(Vector2 input) {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1-(input.x*input.x)/2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        return output;
    }
}
