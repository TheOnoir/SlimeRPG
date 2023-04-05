using UnityEngine;
using TMPro;

public class PlayerUpgrade : MonoBehaviour
{
    [Header("Stats")]

    [SerializeField] private int damageLevel;
    [SerializeField] private TextMeshProUGUI damageLevelText;

    [Space]

    [SerializeField] private int maxHealthLevel;
    [SerializeField] private TextMeshProUGUI maxHealthLevelText;

    [Space]

    [SerializeField] private int attackSpeedLevel;
    [SerializeField] private TextMeshProUGUI attackSpeedLevelText;

    [Header("Coins")]

    [SerializeField] private int coins = 100;
    [SerializeField] private int coinsForKill = 10;
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Enemy Stats Level")]

    [SerializeField] private int enemyLevel = 0;

    public float enemyDamage = 20f;
    public float enemyHealth = 100f;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        coinsText.text = coins.ToString();
    }

    public void BoostEnemyStats()
    {
        enemyLevel++;
        enemyDamage += 25;
        enemyHealth += 200;
    }

    public void CoinForKill()
    {
        coins += coinsForKill;
        coinsText.text = coins.ToString();
    }

    public void UpgradeDamageLevel()
    {
        int UpgradePrice = 5;

        if (coins >= UpgradePrice)
        {
            coins -= UpgradePrice;
            coinsText.text = coins.ToString();

            int intermediateDamageLevel = 1;
            damageLevel += intermediateDamageLevel;
            damageLevelText.text = damageLevel.ToString();

            int intermediateDamage = 25;
            playerController.UpgradeDamage(intermediateDamage);
        }
    }

    public void UpgradeHealthLevel()
    {
        int UpgradePrice = 5;

        if (coins >= UpgradePrice)
        {
            coins -= UpgradePrice;
            coinsText.text = coins.ToString();

            int intermediateHealthLevel = 1;
            maxHealthLevel += intermediateHealthLevel;
            maxHealthLevelText.text = maxHealthLevel.ToString();

            int intermediateHealth = 200;
            playerController.UpgradeHealth(intermediateHealth);
        }
    }

    public void UpgradeAttackSpeedLevel()
    {
        int UpgradePrice = 5;

        if (coins >= UpgradePrice)
        {
            coins -= UpgradePrice;
            coinsText.text = coins.ToString();

            int intermediateAttackSpeedLevel = 1;
            attackSpeedLevel += intermediateAttackSpeedLevel;
            attackSpeedLevelText.text = attackSpeedLevel.ToString();

            float intermediateAttackSpeed = 0.1f;
            playerController.UpgradeattackSpeed(intermediateAttackSpeed);
        }
    }
}
