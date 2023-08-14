using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAiming : MonoBehaviour
{
    public Transform target;
    public bool isAimed = false;
    
    private float speed = 3f;
    private float singleStep;
    private float deltaDegree;
    private Transform modelRoot;

    void Start() 
    {
        modelRoot = this.transform.parent.parent;
        singleStep = speed * Time.deltaTime;
    }

    void Update()
    {
        if(target == null)
        {
            isAimed = false;
            deltaDegree = Vector3.Angle(transform.forward, modelRoot.transform.forward);
            if (deltaDegree > .5f)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, modelRoot.transform.forward, singleStep, 0.0f);
                Debug.DrawRay(transform.position, newDirection * 100, Color.yellow);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        else
        {
            Vector3 targetDirection = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
            Debug.DrawRay(transform.position, targetDirection * 100, Color.green);

            deltaDegree = Vector3.Angle(transform.forward, targetDirection);
            if (deltaDegree < 1f)
            {
                isAimed = true;
            }
            else 
            {   
                isAimed = false;
            }

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            Debug.DrawRay(transform.position, newDirection * 1000, Color.yellow);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        Debug.DrawRay(this.transform.position, modelRoot.transform.forward * 100, Color.white);
    }
}

