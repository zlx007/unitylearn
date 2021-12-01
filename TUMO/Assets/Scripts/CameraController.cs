using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed = 50.0f;
    public float veticalSpeed = 40.0f;
    public float cameraDampValue=0.5f;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerx;
    private GameObject model;
    private GameObject camera;

    private Vector3 cameraDampVelocity;

    private void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerx = 20;
        model = playerHandle.GetComponent<ActorController>().model;
        camera = Camera.main.gameObject;
    }
    private void FixedUpdate()
    {
        Vector3 tempModelEuler = model.transform.eulerAngles;

        playerHandle.transform.Rotate(Vector3.up,pi.Jright*horizontalSpeed*Time.fixedDeltaTime);
        tempEulerx -= pi.Jup * veticalSpeed * Time.fixedDeltaTime;
        tempEulerx = Mathf.Clamp(tempEulerx,-30,30);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerx,0,0);
        model.transform.eulerAngles = tempModelEuler;
        //camera.transform.position = Vector3.Lerp(camera.transform.position,transform.position,0.2f);
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position,transform.position,ref cameraDampVelocity,cameraDampValue);
        camera.transform.eulerAngles = transform.eulerAngles;
    }
}
