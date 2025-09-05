using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private TextMeshProUGUI playerNameText;
    private TextMeshProUGUI playerScoreText;

    TextMeshProUGUI gameStateText;
    private GameManager gameManager;
    private Player player;
    public List<Image> LifeImages = new List<Image>();
    public Sprite lifeSprite;

    public List<Image> ManaImages = new List<Image>();


    // Add this method to switch camera focus
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        playerNameText.text = player.PlayerName;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifeImages();
        UpdateScoreText();
    }

    private void UpdateLifeImages()
    {
        //Debug.Log("Updating Life Images"+ player.Lives);

        for (int i = 0; i < LifeImages.Count; i++)
        {
            LifeImages[i].color = player.IsAlive ? Color.white : Color.red;
            LifeImages[i].enabled = i < player.Lives;
            if (i < player.Lives)
            {
                LifeImages[i].sprite = lifeSprite;
                //LifeImages[i].sprite.fill= player.color;
                LifeImages[i].gameObject.SetActive(true);
            }
            else
            {
                LifeImages[i].gameObject.SetActive(false);
            }
        }
    }
    
    //Update Score Text
    private void UpdateScoreText()
    {
        playerScoreText.text = player.Score.ToString();
    }

    public void Initialize(Player player)
    {
        this.player = player;
        playerNameText = FindFirstObjectByType<TextMeshProUGUI>();
        playerScoreText = FindFirstObjectByType<TextMeshProUGUI>();
        gameStateText = FindFirstObjectByType<TextMeshProUGUI>();
        UpdateLifeImages();
        UpdateScoreText();           
    }
}
