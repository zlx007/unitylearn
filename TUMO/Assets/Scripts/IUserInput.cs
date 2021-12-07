using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("=====Output singals=====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    public float Jup;
    public float Jright;

    //pressing singnal
    public bool run;
    public bool defense;
    //trigger once singal
    public bool jump;
    protected bool lastJump;
    public bool attack;
    protected bool lastAttack;
    public bool lockon;
    //double singnal

    [Header("=====other======")]
    public bool inputEnable = true;

    protected float TargetDup;
    protected float TargetDright;
    protected float velocityDup;
    protected float velocityDright;
    protected Vector2 SquareToCircle(Vector2 square)
    {//Õ÷‘≤”≥…‰
        Vector2 circle;
        circle.x = square.x * Mathf.Sqrt(1 - (square.y * square.y) / 2);
        circle.y = square.y * Mathf.Sqrt(1 - (square.x * square.x) / 2);
        return circle;
    }
}
