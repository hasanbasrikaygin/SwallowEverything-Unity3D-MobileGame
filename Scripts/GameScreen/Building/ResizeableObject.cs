using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeableObject : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamageAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger("OnHit");
        }
    }
}
