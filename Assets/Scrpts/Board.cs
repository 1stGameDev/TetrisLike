using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    public Piece CurrentPiece { get; private set; }

    public Vector3Int spawnPosition;
    public TetrominoData[] tetrominos;

    private void Awake()
    {
        Tilemap = GetComponentInChildren<Tilemap>();
        CurrentPiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominos.Length; i++)
        {
            tetrominos[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
        Set(CurrentPiece);
    }

    private void SpawnPiece()
    {
        int random = Random.Range(0, tetrominos.Length);
        TetrominoData tetromino = tetrominos[random];

        CurrentPiece.Initialize(this, spawnPosition, tetromino);
        Set(CurrentPiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            Tilemap.SetTile(tilePosition, piece.Data.tile);
        }
    }
}
