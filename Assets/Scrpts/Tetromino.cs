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

    public Tetromino tetromino;
    public Tile tile;

    public void Initialize()
    {
        Cells = Data.Cells[tetromino];
    }
}
