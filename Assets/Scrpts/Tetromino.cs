using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I,
    J,
    L,
    O,
    S,
    T,
    Z
}

[System.Serializable]
public struct TetrominoData
{
    public Vector2Int[] Cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public Tetromino tetromino;
    public Tile tile;

    public void Initialize()
    {
        Cells = Data.Cells[tetromino];
        wallKicks = Data.WallKicks[tetromino];
    }
}
