using UnityEngine;

public enum PiecesType { I, O, L, T, J, S, Z };

public static class PieceShapes
{
    public static Vector2[] GetShape(PiecesType type)
    {
        switch (type)
        {
            case PiecesType.I:
                return new Vector2[] {
                    new Vector2(-1, 0), new Vector2(0, 0),
                    new Vector2(1, 0), new Vector2(2, 0)
                };
            case PiecesType.O:
                return new Vector2[] {
                    new Vector2(0, 0), new Vector2(1, 0),
                    new Vector2(0, 1), new Vector2(1, 1)
                };
            case PiecesType.L:
                return new Vector2[] {
                    new Vector2(-1, 0), new Vector2(0, 0),
                    new Vector2(1, 0), new Vector2(1, 1)
                };
            case PiecesType.J:
                return new Vector2[] {
                    new Vector2(-1, 1), new Vector2(-1, 0),
                    new Vector2(0, 0), new Vector2(1, 0)
                };
            case PiecesType.T:
                return new Vector2[] {
                    new Vector2(-1, 0), new Vector2(0, 0),
                    new Vector2(1, 0), new Vector2(0, 1)
                };
            case PiecesType.S:
                return new Vector2[] {
                    new Vector2(0, 0), new Vector2(1, 0),
                    new Vector2(-1, 1), new Vector2(0, 1)
                };
            case PiecesType.Z:
                return new Vector2[] {
                    new Vector2(-1, 0), new Vector2(0, 0),
                    new Vector2(0, 1), new Vector2(1, 1)
                };
            default:
                return new Vector2[0];
        }
    }

    public static PiecesType GetRandomPieceType()
{
    PiecesType[] values = (PiecesType[])System.Enum.GetValues(typeof(PiecesType));
    int index = Random.Range(0, values.Length);
    return values[index];
}

}

