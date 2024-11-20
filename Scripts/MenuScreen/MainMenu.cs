using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    private Animator animator;
    int freezeDanceAnimation;
    int hipHopDanceAnimation;
    int hipHopAnimation;
    int idleAnimation;
    int randomAnimation;
    public bool isWalking = false;
    private int currentClothingIndex = 0;
    [SerializeField] Button outfitButton;

    [SerializeField] SkinnedMeshRenderer bodyRenderer;
    [SerializeField] SkinnedMeshRenderer legsRenderer;
    [SerializeField] SkinnedMeshRenderer feetRenderer;
    [SerializeField] SkinnedMeshRenderer headRenderer;

    public List<ClothingSet> bodyClothes;
    public List<ClothingSet> legsClothes;
    public List<ClothingSet> feetClothes;
    public List<ClothingSet> headClothes;
    //public GameObject[] clothingArray;
    void Awake()
    {
        animator = GetComponent<Animator>();
        freezeDanceAnimation = Animator.StringToHash("FreezeDance");
        hipHopDanceAnimation = Animator.StringToHash("HipHopDance");
        hipHopAnimation = Animator.StringToHash("HipHop");
        idleAnimation = Animator.StringToHash("Idle");
    }

    void Start()
    {
       // if(currentClothingIndex == null)currentClothingIndex = 0; else
            currentClothingIndex = PlayerPrefs.GetInt("SelectedClothingIndex", 0);
        UpdateClothing();
        randomAnimation = Random.Range(0, 3);
        switch (randomAnimation)
        {
            case 0:
                animator.SetBool(freezeDanceAnimation, true);
                break;
            case 1:
                animator.SetBool(hipHopDanceAnimation, true);
                break;
            case 2:
                animator.SetBool(hipHopAnimation, true);
                break;
        }
        //outfitButton.onClick.AddListener(ChangePosition);
    }

    void Update()
    {
        // Eðer herhangi bir dans animasyonu oynanýyorsa ve animasyon bitti ise
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            // Tüm dans animasyonlarýný kapalý duruma getir
            animator.SetBool(freezeDanceAnimation, false);
            animator.SetBool(hipHopDanceAnimation, false);
            animator.SetBool(hipHopAnimation, false);
            animator.SetBool(idleAnimation, true);
        }
       // StartCoroutine(Walk());
    }
    private void UpdateClothing()
    {

        ApplyClothing(bodyRenderer, bodyClothes[currentClothingIndex]);
        ApplyClothing(legsRenderer, legsClothes[currentClothingIndex]);
        ApplyClothing(feetRenderer, feetClothes[currentClothingIndex]);
        ApplyClothing(headRenderer, headClothes[currentClothingIndex]);
       // clothingArray[currentClothingIndex].SetActive(true);
    }

    private void ApplyClothing(SkinnedMeshRenderer renderer, ClothingSet clothingSet)
    {
        //Debug.Log($"Applying clothing: {clothingSet.name}");
        renderer.sharedMesh = clothingSet.mesh;

        //Debug.Log($"Material count: {clothingSet.materials.Length}");
        renderer.materials = clothingSet.materials;
    }
}
