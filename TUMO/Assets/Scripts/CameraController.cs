using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public IUserInput pi;
    public float horizontalSpeed = 50.0f;
    public float veticalSpeed = 40.0f;
    public float cameraDampValue = 0.5f;
    public Image lockDot;
    public bool lockState;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerx;
    private GameObject model;
    private GameObject camera;

    private Vector3 cameraDampVelocity;

    [SerializeField]
    private LockTarget lockTarget;

    private void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerx = 20;
        model = playerHandle.GetComponent<ActorController>().model;
        camera = Camera.main.gameObject;
        lockDot.enabled = false;
        lockState = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);
            tempEulerx -= pi.Jup * veticalSpeed * Time.fixedDeltaTime;
            tempEulerx = Mathf.Clamp(tempEulerx, -30, 30);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerx, 0, 0);
            model.transform.eulerAngles = tempModelEuler;
        }
        else {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);

        }
        //camera.transform.position = Vector3.Lerp(camera.transform.position,transform.position,0.2f);
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        //camera.transform.eulerAngles = transform.eulerAngles;
        camera.transform.LookAt(cameraHandle.transform);

    }
    public void LockUnlock() {
        //print("LockUnlock");
        //try to lock
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, LayerMask.GetMask("Enemy"));
        if (cols.Length == 0)
        {
            lockTarget = null;
            lockDot.enabled = false;
        }
        else {
            foreach (var col in cols)
            {
                if (lockTarget!=null&&lockTarget.obj == col.gameObject) {
                    lockTarget = null;
                    lockDot.enabled = false;
                    lockState = false;
                    break;
                }
                lockTarget = new LockTarget(col.gameObject,col.bounds.extents.y) ;
                //lockDot.transform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position);
                lockDot.enabled = true;
                lockState = true;
                break;
            }
        }


    }
    private void Update()
    {
        if (lockTarget!=null) {
            //print(lockTarget.halfHeight);
            lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position+new Vector3(0,lockTarget.halfHeight,0));
            if (Vector3.Distance(model.transform.position,lockTarget.obj.transform.position)>10.0f) {
                lockTarget = null;
                lockDot.enabled = false;
                lockState = false;
            }
        }
    }
    private class LockTarget {
        public GameObject obj;
        public float halfHeight;
        public LockTarget(GameObject _obj,float _halfHeight) {
            obj = _obj;
            halfHeight = _halfHeight;
        }
    }

}
