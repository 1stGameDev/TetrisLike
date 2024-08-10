using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    public Piece CurrentPiece { get; private set; }

    public Vector3Int spawnPosition;
    public TetrominoData[] tetrominos;

    private Vector2Int boardSize = new Vector2Int(10, 20);
    public RectInt Bounds{
        get
        {
            //Creates a rectangle starting from the bottom left corner of the board.
            //It uses boardSize to determine so.
            Vector2Int pos = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2);
            return new RectInt(pos, this.boardSize);
        }
    }

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

    public void SpawnPiece()
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

//It sets the cells where the piece is to Null (works as in inverted Set()).
     public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            Tilemap.SetTile(tilePosition, null);
        }
    }


    public bool IsValidPosition(Piece piece, Vector3Int newposition)
    {
        //For each block that forms the current pice:
        for (int i=0; i < piece.Cells.Length; i++){
            Vector3Int tilePosition = (Vector3Int)piece.Cells[i] + newposition;
            
            //If the new position of this block is out of the board determined by the bounds, the new position is not valid.
            if(!Bounds.Contains((Vector2Int)tilePosition)){
                return false;
            }
            //If there is already a block in the block's new position, the new position is not valid.
            if(this.Tilemap.HasTile(tilePosition)){
                return false;
            }
        }
        return true;
    }
}
