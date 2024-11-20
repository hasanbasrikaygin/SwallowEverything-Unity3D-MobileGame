using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// MeshSockets s�n�f�, oyun nesnelerine ba�lanacak mesh soketlerini y�netir
public class MeshSockets : MonoBehaviour
{
    // Mesh soketlerinin kimlikleri
    public enum SocketId
    {
        Spine,

    }
    // Mesh soketlerini ve onlar�n kimliklerini i�eren bir s�zl�k
    Dictionary<SocketId, MeshSocket> socketMap = new Dictionary<SocketId, MeshSocket>();
    void Start()
    {
        // Bu nesnenin alt�ndaki t�m MeshSocket bile�enlerini al
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach (MeshSocket socket in sockets)
        {
            // Soketin kimli�ini ve kendisini s�zl��e ekle
            socketMap[socket.socketId] = socket;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Bir nesneyi belirli bir mesh soketine ba�lamak i�in kullan�lan fonksiyon
    public void Attach(Transform objectTransform , SocketId socketId)
    {
        // Belirtilen kimli�e sahip mesh soketine ba�lanan nesneyi aktar
        socketMap[socketId].Attach(objectTransform);
    }
}
