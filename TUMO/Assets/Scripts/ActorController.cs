using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public CameraController camcon;
    public IUserInput pi;
    public  float walkSpeed = 1.5f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 5.0f;
    public float rollVelocity = 1.0f;
    //public float jabVelocity = 3.0f;

    [Space(10)]
    [Header("===== Friction Settings======")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;


    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;//冲量
    private bool canAttack;
    private CapsuleCollider col;
    private float lerpTarget;
    private Vector3 deltaPos;


    private bool lockPlanar = false;
    private bool trackDirection = false;
    //private float rotateSpeed = 2f;
    void Awake()
    {
        //pi = GetComponent<IUserInput>();
        //解决切换输入设备无反应的方法
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if (input.enabled==true) {
                pi = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        if (pi.lockon) {
            camcon.LockUnlock();
        }
        if (camcon.lockState == false)
        {
            //速度暴增用插值
            float targetRunMulti = ((pi.run) ? 2.0f : 1.0f);
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), targetRunMulti, 0.5f));
            anim.SetFloat("right",0);
        }
        else {
            Vector3 localDvec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward",localDvec.z* ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDvec.x * ((pi.run) ? 2.0f : 1.0f));
        }
        anim.SetBool("defense",pi.defense);


        if (rigid.velocity.magnitude>1.0f) {
            anim.SetTrigger("roll");
        }

        if (pi.jump==true) {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        if (pi.attack == true &&canAttack &&anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer")).IsName("ground")) {//判断人物状态是否在ground状态
            anim.SetTrigger("attack");
        }
        if (camcon.lockState == false)
        {
            if (pi.Dmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);

            }
            if (lockPlanar == false)
            {
                planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }
        else {
            if (trackDirection == false)
            {
                model.transform.forward = transform.forward;
            }
            else {
                model.transform.forward = planarVec.normalized;
            }

            if (lockPlanar == false) {
                planarVec = pi.Dvec * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }


        }


    }
    private void FixedUpdate()
    {
        //rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.position += deltaPos;
        rigid.velocity =new Vector3(planarVec.x,rigid.velocity.y,planarVec.z)+thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }
///
///message processing block
///
    public void OnJumpEnter() {
        thrustVec = new Vector3(0, jumpVelocity, 0);
        pi.inputEnable = false;
        lockPlanar = true;
        trackDirection = true;
        
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
        canAttack = true;
        col.material = frictionOne;
        trackDirection = false;
    }
    public void OnGroundExit() {
        col.material = frictionZero;
    }
    public void OnFallEnter() {
        pi.inputEnable = false;
        lockPlanar = true;
        
    }
    public void OnRollEnter() {
        thrustVec = new Vector3(0, rollVelocity, 0);
        pi.inputEnable = false;
        lockPlanar = true;
        trackDirection = true;
    }
    public void OnJabEnter() {

        pi.inputEnable = false;
        lockPlanar = true;
    }
    public void OnJabUpdate() {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity") ;
    }
    public void OnAttack1hAEnter() {
        pi.inputEnable = false;
        //lockPlanar = true;
        lerpTarget = 1.0f;

    }
    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        //简化版在标记处1
        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack Layer"));
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.3f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), currentWeight);
    }
    public void OnAttackIdleEnter() {
        pi.inputEnable = true;
        //lockPlanar = false;
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
        lerpTarget = 0f;
    }
    public void OnAttackIdleUpdate() {
        //标记处1
       anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("Attack Layer")), lerpTarget, 0.3f));
    }
    public void OnUpdateRM(object _deltaPos) {
        if (anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Attack Layer")).IsName("attack1hC")) {
            deltaPos += (deltaPos+(Vector3)_deltaPos)/2;
        }
        

    }
}
