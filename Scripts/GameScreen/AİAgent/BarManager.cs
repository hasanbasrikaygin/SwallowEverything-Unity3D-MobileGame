using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public Image damageBar;
    public Image bulletSpeedBar;
    public Image rangeBar;
    public Image fireRateBar;

    public void UpdateBars(string weaponName, float damage, float maxDamage, float bulletSpeed, float maxBulletSpeed, float range, float maxRange, float fireRate, float maxFireRate)
    {
        weaponNameText.text = weaponName;
        UpdateBar(damageBar, damage, maxDamage);
        UpdateBar(bulletSpeedBar, bulletSpeed, maxBulletSpeed);
        UpdateBar(rangeBar, range, maxRange);
        UpdateBar(fireRateBar, fireRate, maxFireRate);
    }

    void UpdateBar(Image bar, float currentValue, float maxValue)
    {
        bar.fillAmount = currentValue / maxValue;
    }
}
