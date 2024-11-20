using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// AiSensor s�n�f�, AI'n�n etraf�ndaki nesneleri alg�lamak ve takip etmek i�in kullan�l�r
// Edit�rde de �al��mas�n� sa�lar
[ExecuteInEditMode]
public class AiSensor : MonoBehaviour
{

    public float distance = 10f;
    public float angle = 30f;
    public float height = 1f;
    public Color meshColor = Color.red;
    // Alg�lama s�kl���
    public int scanFrequency = 30;
    // Alg�lanacak katmanlar
    public LayerMask layers;

    Collider[] colliders = new Collider[50];
    int count;
    // Tarama aral���
    float scanInterval;
    float scanTimer;

    Mesh mesh;
    // Alg�lanan nesneler listesi
    public List<GameObject> Objects
    {
        get
        {// Olmayan nesneleri kald�r ve listeyi d�nd�r
            objects.RemoveAll(obj => !obj);
            return objects;
        }
    }
    // Alg�lanan nesneler listesi
    private List<GameObject> objects = new List<GameObject>();
    // G�r�nmez katmanlar
    public LayerMask occlusionLayers;

    void Start()
    {
        // Tarama aral���n� hesapla
        scanInterval = 1f / scanFrequency;

    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();// Alg�lama yap
        }
    }
    private void Scan()
    {
        // Belirtilen mesafede kollide olan nesneleri al
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);
        objects.Clear();
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            // Objenin g�r�� a��s�nda olup olmad���n� kontrol et
            if (IsInsight(obj))
            {
                objects.Add(obj);
            }
        }
    }
    // Bir nesnenin g�r�� a��s�nda olup olmad���n� kontrol eder
    public bool IsInsight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction =dest - origin;
        if(direction.y<0 || direction.y > height)
        {
            return false;
        }
        direction.y = 0;
        float deltaAngle =Vector3.Angle(direction , transform.forward);
        if(deltaAngle > angle)
        {
            return false;
        }
        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }
        return true;
    }
    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) +2 +2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft =Quaternion.Euler(0,-angle,0)*Vector3.forward*distance;
        Vector3 bottomRight =Quaternion.Euler(0,angle,0)*Vector3.forward*distance;


        Vector3 topCenter = bottomCenter + Vector3.up *height ;
        Vector3 topRight = bottomRight + Vector3.up *height ;
        Vector3 topLeft = bottomLeft + Vector3.up *height ;

        int vert = 0;
        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;


        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // rigth side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;


        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        // Segmentleri d�ng� ile dola�arak ��genleri ve verticeleri olu�tur
        float currentAngle =-angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
    
             bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
             bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;


             topRight = bottomRight + Vector3.up * height;
             topLeft = bottomLeft + Vector3.up * height;


            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;


            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
            // bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }


        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1f / scanFrequency;
    }
    private void OnDrawGizmos()
    {
        if(mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh , transform.position ,transform.rotation);
        }
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0;i< count; ++i)
        {
            Gizmos.DrawSphere(colliders[i].transform.position + new Vector3(0f,1f,1f), .8f);
        }
        Gizmos.color = Color.green;
        foreach(var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position + new Vector3(0f, 1f, 1f), .8f);
        }
    }
}
