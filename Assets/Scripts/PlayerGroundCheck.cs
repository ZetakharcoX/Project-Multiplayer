using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController pc;

    void Awake()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == pc.gameObject)
            return;
        pc.SetGroundedState(true);
    }
    
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == pc.gameObject)
            return;
        pc.SetGroundedState(true);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == pc.gameObject)
            return;
        pc.SetGroundedState(false);   
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject == pc.gameObject)
            return;
        pc.SetGroundedState(true);
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject == pc.gameObject)
            return;
        pc.SetGroundedState(true);
    }

    void OnCollisionExit(Collision other)
    {
        if(other.gameObject == pc.gameObject)
            return;
        pc.SetGroundedState(false);
    }
}
