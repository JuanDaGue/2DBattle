using UnityEngine;
using System.Collections;
using TMPro;

public class DiceRoller : MonoBehaviour
{
    [Header("Dice Settings")]
    [SerializeField] private int minValue = 1;
    [SerializeField] private int maxValue = 6;
    [SerializeField] private float rollDuration = 1f;
    [SerializeField] private TMPro.TextMeshProUGUI resultText;
    
    private int result;
    private bool isRolling;
    
    public bool HasResult => !isRolling;
    
    public void RollDice()
    {
        if (!isRolling)
        {
            StartCoroutine(RollDiceCoroutine());
        }
    }
    
    private IEnumerator RollDiceCoroutine()
    {
        isRolling = true;
        float endTime = Time.time + rollDuration;
        
        // Visual rolling effect
        while (Time.time < endTime)
        {
            result = Random.Range(minValue, maxValue + 1);
            UpdateUI(result);
            yield return null;
        }
        
        isRolling = false;
        Debug.Log($"Dice roll result: {result}");
    }
    
    public int GetResult() => result;
    
    public void ResetDice()
    {
        result = 0;
        UpdateUI(0);
    }
    
    public void UpdateUI(int value)
    {
        if (resultText != null)
        {
            resultText.text = value.ToString();
        }
    }
}