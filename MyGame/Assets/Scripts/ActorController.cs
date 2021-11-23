using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;
    public float runModifier = 2.7f;
    public float jumpVelocit = 4.0f;
    
    private bool lockPlanar=false;
    public float walkSpeed = 1.4f;
    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    public Vector3 planarVec;
    public Vector3 thrustVec;
    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f), 0.5f));//a^2+b^2=c^2
        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }
        if (pi.Dmag > 0.1f) {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }
        if (lockPlanar==false) {
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runModifier : 1.0f);//玩家想要移动的方向
        }
        
    }
    private void FixedUpdate()
    {
        // rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z)+thrustVec;
        thrustVec = Vector3.zero;
    }
        ///
        ///Message processing block
        ///
    public void OnJumpEnter() {
        //print("OnJumpEnter");
        pi.inputEnable = false;
        lockPlanar = true;
        thrustVec = new Vector3(0,jumpVelocit,0);
    }
    //public void OnJumpExit(){
    //    //print("OnJumpExit");
    //    pi.inputEnable = true;
    //    lockPlanar = false;
    //}
    public void OnGroundEnter() {
        pi.inputEnable = true;
        lockPlanar = false;
    }
    public void IsGround() {
        //print("在地上");
        anim.SetBool("isGround",true);
    }
    public void IsNotGround() {
        //print("不在地上");
        anim.SetBool("isGround", false);
    }
    public void OnFallEnter() {
        pi.inputEnable = false;
        lockPlanar = true;
    }
}
