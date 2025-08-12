using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour {
    public Button rollButton;
    public TextMeshProUGUI resultText;
    public GameObject[] tetrominoPrefabs; // Predefined Tetris shapes

    public void RollDice() {
        int roll = Random.Range(1, 7);
        resultText.text = roll.ToString();
        SpawnTetromino(roll);
    }

    void SpawnTetromino(int diceValue) {
        // Spawn Tetromino based on dice roll
        Instantiate(tetrominoPrefabs[diceValue % tetrominoPrefabs.Length], transform.position, Quaternion.identity);
    }
}