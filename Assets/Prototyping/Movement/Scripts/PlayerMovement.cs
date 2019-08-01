using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform playerModel;

    [Header("Settings")]
    public bool joystick = true;

    [Header("Parameters")]
    public float xySpeed = 18;
    public float lookSpeed = 340;
    public float forwardSpeed = 6;
    public float invertUpDown = 1;
    public bool sideRoll = false;

    [Header("Public References")]
    public Transform aimTarget;
    //public CinemachineDollyCart dolly;
    //public Transform cameraParent;
    public float yMin = 0;
    public float yMax = 1;
    public float xMin = 0;
    public float xMax = 1;
    public Animator sideWindAnim;

    void Start()
    {
        playerModel = transform.GetChild(0);
    }


    void Update()
    {
        float h = joystick ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
        float v = joystick ? Input.GetAxis("Vertical") : Input.GetAxis("Mouse Y");
        LocalMove(h, v*invertUpDown, xySpeed);
        RotationLook(h, v*invertUpDown, lookSpeed);
        HorizontalLean(playerModel, h, 80, .1f);
  


        //sideRoll
        if (Input.GetKeyDown(KeyCode.Q) && !sideRoll)
        {
            sideRoll = true;
            sideWindAnim.SetBool("leanLeft", true);
            SideWind(playerModel, -1, 1f);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            sideRoll = false;
            sideWindAnim.SetBool("leanLeft", false);
            SideWind(playerModel, 0,  1f);
        }

        if (Input.GetKeyDown(KeyCode.E) && !sideRoll)
        {
            sideRoll = true;
            sideWindAnim.SetBool("leanRight", true);
            SideWind(playerModel, 1, 1f);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            sideRoll = false;
            sideWindAnim.SetBool("leanRight", false);
            SideWind(playerModel, 0, 1f);
        }
        //sideRoll END

    }

    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void RotationLook(float h, float v, float speed)
    {
        aimTarget.parent.position = Vector3.zero;
        aimTarget.localPosition = new Vector3(h, v, 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * speed * Time.deltaTime);
    }

    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    void SideWind(Transform target, int dir, float lerpTime)
    {
       
        //Vector3 targetEulerAngels = target.localEulerAngles;
        //target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -dir * 90, lerpTime));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTarget.position, .5f);
        Gizmos.DrawSphere(aimTarget.position, .15f);
    }
}
