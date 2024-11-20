using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// MeshSockets sýnýfý, oyun nesnelerine baðlanacak mesh soketlerini yönetir
public class MeshSockets : MonoBehaviour
{
    // Mesh soketlerinin kimlikleri
    public enum SocketId
    {
        Spine,

    }
    // Mesh soketlerini ve onlarýn kimliklerini içeren bir sözlük
    Dictionary<SocketId, MeshSocket> socketMap = new Dictionary<SocketId, MeshSocket>();
    void Start()
    {
        // Bu nesnenin altýndaki tüm MeshSocket bileþenlerini al
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach (MeshSocket socket in sockets)
        {
            // Soketin kimliðini ve kendisini sözlüðe ekle
            socketMap[socket.socketId] = socket;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Bir nesneyi belirli bir mesh soketine baðlamak için kullanýlan fonksiyon
    public void Attach(Transform objectTransform , SocketId socketId)
    {
        // Belirtilen kimliðe sahip mesh soketine baðlanan nesneyi aktar
        socketMap[socketId].Attach(objectTransform);
    }
}
