using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    private Rigidbody[] _ragdollRigidBodies;
    void Awake()
    {
        _ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisableRagdoll()
    {
        foreach(var rigidbody in _ragdollRigidBodies)
        { 
            rigidbody.isKinematic = true;
        }
    }
    public void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidBodies)
        {
            rigidbody.isKinematic = false;
        }
    }
}
