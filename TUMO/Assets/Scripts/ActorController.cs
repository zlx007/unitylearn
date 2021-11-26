using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;

    [SerializeField]
    private Animator anim;
    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
    }
    void Update()
    {
        anim.SetFloat("forward",pi.Dmag);
        model.transform.forward = pi.Dvec;
    }
}
