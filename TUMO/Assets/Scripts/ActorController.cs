using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;
    public  float walkSpeed = 1.5f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 5.0f;
    public float rollVelocity = 1.0f;
    //public float jabVelocity = 3.0f;

    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;//冲量


    private bool lockPlanar = false;
    //private float rotateSpeed = 2f;
    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //速度暴增用插值
        float targetRunMulti = ((pi.run) ? 2.0f : 1.0f);
        anim.SetFloat("forward",pi.Dmag*Mathf.Lerp(anim.GetFloat("forward"),targetRunMulti,0.5f) );
        if (rigid.velocity.magnitude>1.0f) {
            anim.SetTrigger("roll");
        }

        if (pi.jump==true) {
            anim.SetTrigger("jump");
        }
        
        if (pi.Dmag>0.1f) {
            model.transform.forward = Vector3.Slerp(model.transform.forward,pi.Dvec,0.3f);
            
        }
        if (lockPlanar==false)
        {         
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
        }

    }
    private void FixedUpdate()
    {
        //rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.velocity =new Vector3(planarVec.x,rigid.velocity.y,planarVec.z)+thrustVec;
        thrustVec = Vector3.zero;
    }
///
///message processing block
///
    public void OnJumpEnter() {
        thrustVec = new Vector3(0, jumpVelocity, 0);
        pi.inputEnable = false;
        lockPlanar = true;
        
    }
    //public void OnJumpExit()
    //{
    //    pi.inputEnable = true;
    //    lockPlanar = false;
    //}
    public void IsGround() {
   
        anim.SetBool("isGround",true);
    }
    public void IsNotGround() {
        
        anim.SetBool("isGround", false);
    }
    public void OnGroundEnter() {
        pi.inputEnable = true;
        lockPlanar = false;
    }
    public void OnFallEnter() {
        pi.inputEnable = false;
        lockPlanar = true;
    }
    public void OnRollEnter() {
        thrustVec = new Vector3(0, rollVelocity, 0);
        pi.inputEnable = false;
        lockPlanar = true;
    }
    public void OnJabEnter() {

        pi.inputEnable = false;
        lockPlanar = true;
    }
    public void OnJabUpdate() {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity") ;
    }
}
