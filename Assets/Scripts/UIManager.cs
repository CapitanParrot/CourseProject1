using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject HealthPanel;

    public Image FullHeart;
    public Image HalfHeart;
    public Image HollowHeart;

    public static UIManager Instance;

    public GameObject BossHealthBar;

    private Slider bossHealthSlider;

    public TextMeshProUGUI bossNameText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        Instance = this;
    }

    public void DrawBossHealth(string bossName, int health)
    {
        BossHealthBar.SetActive(true);
        bossHealthSlider = BossHealthBar.GetComponent<Slider>();
        bossHealthSlider.maxValue = health;
        bossHealthSlider.value = health;
        bossNameText.text = bossName;
    }
    public void ChangeBossHealth(int newHealth)
    {
        bossHealthSlider.value = newHealth;
    }

    public void DeactivateBossHealth()
    {
        BossHealthBar.SetActive(false);
    }

    public void DrawHealth(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < HealthPanel.transform.childCount; i++)
        {
            Destroy(HealthPanel.transform.GetChild(i).gameObject);
        }

        int fullHeartsCount = currentHealth / 2;
        int halfHeartsCount = currentHealth % 2;
        int hollowHeartsCount = (maxHealth - currentHealth) / 2;

        for (int i = 0; i < fullHeartsCount; i++)
        {
            Instantiate(FullHeart, HealthPanel.transform);
        }
        for (int i = 0; i < halfHeartsCount; i++)
        {
            Instantiate(HalfHeart, HealthPanel.transform);
        }
        for (int i = 0; i < hollowHeartsCount; i++)
        {
            Instantiate(HollowHeart, HealthPanel.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
