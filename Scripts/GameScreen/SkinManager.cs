using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [Space]
    [Header("Current Outfits")]
    [SerializeField] SkinnedMeshRenderer CurrentHair;
    [SerializeField] SkinnedMeshRenderer CurrentTop;
    [SerializeField] SkinnedMeshRenderer CurrentBottom;
    [SerializeField] SkinnedMeshRenderer CurrentShoes;

    [Space]
    [Header("Additional Skins")]
	[SerializeField] Mesh[] Hairs;
	[SerializeField] Mesh[] Tops;
	[SerializeField] Mesh[] Bottoms;
	[SerializeField] Mesh[] Shoes;

	[SerializeField] private bool isRandomSkin;

	private void Awake()
	{
		if (isRandomSkin) { RandomSkinGenerate(); }
	}

	public void ChangeHair(int i)
    {
        CurrentHair.sharedMesh = Hairs[i];
    }

	public void ChangeTop(int i)
	{
		CurrentTop.sharedMesh = Tops[i];
	}

	public void ChangeBottom(int i)
	{
		CurrentBottom.sharedMesh = Bottoms[i];
	}

	public void ChangeShoes(int i)
	{
		CurrentShoes.sharedMesh = Shoes[i];
	}

	private void RandomSkinGenerate()
	{
		int hairIndex = Random.Range(0, Hairs.Length);
		int topIndex = Random.Range(0, Tops.Length);
		int bottomIndex = Random.Range(0, Bottoms.Length);
		int shoesIndex = Random.Range(0, Shoes.Length);

		CurrentHair.sharedMesh = Hairs[hairIndex];
		CurrentTop.sharedMesh = Tops[topIndex];
		CurrentBottom.sharedMesh = Bottoms[bottomIndex];
		CurrentShoes.sharedMesh = Shoes[shoesIndex];
	}
}
