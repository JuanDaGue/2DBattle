using UnityEngine;

public class Tile : MonoBehaviour
{

    public int x;
    public int y;
    public WorldBoard board;
    
    public void Setup(int x_, int y_, WorldBoard board_)
    {
        x = x_;
        y = y_;
        board = board_;
    }

    public void OnMouseDown()
    {
        //board.TileDown(this);
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void OnMouseEnter()
    {
        //Board.TileOver(this);
        this.GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void OnMouseExit()
    {
        //Board.TileOut(this);
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void OnMouseUp()
    {
        //board.TileUp(this);
        this.GetComponent<SpriteRenderer>().color = Color.blue;
        
    }
}
