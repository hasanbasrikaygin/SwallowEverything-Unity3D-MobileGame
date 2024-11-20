using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Insans kemiklerini tan�mlayan bir veri yap�s�
[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;// �nsan kemiklerinin Unity taraf�ndan tan�mlanan sabitlerle referans�
    public float weight = 1f; // Kemik a��rl���

}
public class WeaponIKEnemy : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public int iterations = 10;
    [Range(0,1)]
    public float weight = 1f;
    public bool isTargetActive = false;
    public float angleLimit = 90f;
    public float distanceLimit = 1.5f;
    public Transform headTransform; // Karakterin kafas�n� temsil eden transform
    public HumanBone[] humanBones;
    Transform[] boneTransform;
    public TextMeshProUGUI statusText; // Sa�l�k �ubu�undaki yaz�
    private string targetString = "(*&^%$#@!)"; // Doldurulacak hedef string
    private bool isStatusTextFilled = false; // Yaz�n�n doldurulup doldurulmad���n� kontrol eder
    public float headLookOffset = 10f; // Kafan�n yukar� bakmas� i�in offset (a�� cinsinden)

    void Start()
    {
       
        Animator animator = GetComponent<Animator>();
        boneTransform = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransform.Length; i++)
        {
            boneTransform[i] = animator.GetBoneTransform(humanBones[i].bone);
        }

    }
    // Hedefin konumunu belirler
    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position -aimTransform.position;// Hedefin y�n�
        Vector3 aimDirection = aimTransform.forward; // Silah�n d�nme y�n�

        float blendOut = 0f;
        float targetAngle = Vector3.Angle(targetDirection , -aimDirection);
        if(targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50f;
        }

        float targetDistance =targetDirection.magnitude;
        if(targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, -aimDirection, blendOut);
        return aimTransform.position + direction;
    }
    // Her frame'de hedefe do�ru d�n�� i�lemini ger�ekle�tirir
    void Update()
    {
        
        if (targetTransform == null) { return; }
        if(aimTransform == null) { return; }
        if (isTargetActive)
        {
        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < boneTransform.Length; j++)
            {
                Transform bone = boneTransform[j];
                float boneWeight = humanBones[j].weight * weight;
                AimAtTarget(bone, targetPosition, weight);
            }
           
        }
            if (headTransform != null && targetTransform != null)
            {

                Vector3 targetDirection = targetTransform.position - headTransform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // Kafan�n hedefe do�ru bakmas�n� sa�lar
                headTransform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x + headLookOffset, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
                //Debug.Log(headTransform.rotation);
            }
            if (!isStatusTextFilled)
            {
                FillStatusText();
                isStatusTextFilled = true; // Yaz�y� doldurduktan sonra tekrar �a�r�lmamas� i�in
            }
        }


    }
    // Bir kemik d�n���m�n� hedefe do�ru d�nd�r�r
    private void AimAtTarget(Transform bone, Vector3 targetPosition , float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(-aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);

        bone.rotation = blendedRotation * bone.rotation;
    }
    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }
    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
    public void FillStatusText()
    {
        
        
        StartCoroutine(FillTextCoroutine());
        
    }

    // Yaz�y� zamanla dolduran Coroutine
    IEnumerator FillTextCoroutine()
    {
        
        statusText.text = string.Empty;
        foreach (char c in targetString)
        {
            statusText.text += c;
            yield return new WaitForSeconds(0.2f); // Her karakter i�in bekleme s�resi

        }
        yield return new WaitForSeconds(3);
        statusText.text = string.Empty;
        yield return new WaitForSeconds(3);
        isStatusTextFilled = false; // Yaz�y� doldurduktan sonra tekrar �a�r�lmamas� i�in
    }
}
