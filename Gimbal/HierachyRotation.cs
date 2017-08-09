using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierachyRotation : MonoBehaviour
{
    delegate void MultiRotate();
    MultiRotate multiRotate;

    public bool isYaw;
    public bool isRoll;
    public bool isPitch;

    [Range(-10.0f,10.0f)]
    public float speed;
    public Transform target;

    Vector3 targetEulerAnglesPre;
    Vector3 targetEulerAngles;
    Vector3 transferV;


    void Start ()
    {
        transferV = Vector3.zero;
        targetEulerAnglesPre = target.transform.eulerAngles;

        if (isYaw) multiRotate += Yaw;
        if (isRoll) multiRotate += Roll;
        if (isPitch) multiRotate += Pitch;       
	}
	
	void Update ()
    {
        targetEulerAngles = target.transform.eulerAngles;

        if (multiRotate != null)
        {
            multiRotate();
            transform.eulerAngles = transferV;
        }

        targetEulerAnglesPre = targetEulerAngles;
    }

    void Yaw()
    {
        transferV += new Vector3(0,0,targetEulerAngles.z);
    }

    void Roll()
    {
        transferV += new Vector3(0,targetEulerAngles.y, 0);
    }

    void Pitch()
    {
        transferV += new Vector3(targetEulerAngles.x, 0,0);
    }
}
