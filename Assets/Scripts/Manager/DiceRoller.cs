using UnityEngine;
using System.Collections;
public class DiceRoller : MonoBehaviour
{
    public int minValue = 1;
    public int maxValue = 6;
    public float rollDuration = 1f;
    public TMPro.TextMeshProUGUI resultText;
    
    private int result;
    private bool isRolling;
    
    public bool HasResult => !isRolling;
    
    public void RollDice()
    {
        StartCoroutine(RollDiceCoroutine());
    }
    
    private IEnumerator RollDiceCoroutine()
    {
        isRolling = true;
        float endTime = Time.time + rollDuration;
        
        // Visual rolling effect
        while (Time.time < endTime)
        {
            result = Random.Range(minValue, maxValue + 1);
            resultText.text = result.ToString();
            yield return null;
        }
        
        isRolling = false;
    }
    
    public int GetResult() => result;
    
    public void ResetDice()
    {
        result = 0;
    }
}