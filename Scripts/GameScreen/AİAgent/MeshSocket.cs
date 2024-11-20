using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketId socketId;
    Transform attachPoint;
    void Start()
    {
        attachPoint = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attachPoint,false);
    }
}
