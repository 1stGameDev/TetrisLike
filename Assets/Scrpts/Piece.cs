using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public Vector3Int Position; //{ get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    



    //For moving the pice:
    private float moveTime = 0.1f;
    private float moveTimer;
    public float fallTime;
    private float fallTimer;
    private float lockTime;
    public float lockLimit;
    private bool isLocked;

    //For rotations
    private int rotationIndex;

    public void Initialize(Board board, Vector3Int position, TetrominoData d)
    {
        Board = board;
        Position = position;
        data = d;

        Cells ??= new Vector3Int[data.Cells.Length];

        for (int i = 0; i < data.Cells.Length; i++)
        {
            Cells[i] = (Vector3Int)data.Cells[i];
        }

        moveTimer = 0.0f;
        fallTimer = 0.0f;
        lockTime = 0.0f;
        lockLimit = 0.5f;

        rotationIndex = 0;

    }


    void Update(){
        //Before moving the piece in any way, it clears it from the board. This way the piece won't "colide" with itself.
        if(!isLocked){
            this.Board.Clear(this);

            //If the time since the last Fall is greater than fallTime, it falls.
            fallTimer += Time.deltaTime;
            lockTime += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if(fallTimer >= fallTime)
            {
                Fall();
            }

            if(moveTimer >= moveTime){
                CheckMovements();
            }            
            
            if(Input.GetKeyDown(KeyCode.Space)){
                HardDown();
            }

            if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow)){
                Rotate(1);
            }

            //Now that we have moved (or not) the piece, it draws it in the board.
            this.Board.Set(this);
        }
    }

    private void CheckMovements(){

        if(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow)){
            //Same as with the pause menu.
            //It checks if it has moved in the same key-press.
            Move(new Vector2Int(1,0));
        }
        if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow)){
            Move(new Vector2Int(-1, 0));
        }
        if(Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)){
            Move(new Vector2Int(0, -1));
        }
        
        
            
    }
    
    private void Fall(){
        fallTimer = 0.0f;
        Down();
        //If the time since the last movement is greater than lockLimit, it lock the piece.
        //lockTime gets setted to 0 everytime the piece moves.
        if(lockTime >= lockLimit){
            Lock();
        }
    }

    private void HardDown(){
        while(Down()){

        }
        Lock();
    }
    
    private bool Down(){
        Vector3Int newpos = new Vector3Int(Position.x, Position.y - 1, Position.z);
            if(this.Board.IsValidPosition(this, newpos)){
                Position = newpos;
                fallTimer = 0.0f;
                lockTime = 0.0f;
                return true;
            }
        return false;
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.Position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = Board.IsValidPosition(this, newPosition);

        // Only save the movement if the new position is valid
        if (valid)
        {
            Position = newPosition;
            lockTime = 0f; // reset
            moveTimer = 0.0f;
        }

        return valid;
    }
    private void Lock(){
        this.Board.Set(this);
        this.Board.CheckLine();
        this.Board.SpawnPiece();
    }

    private void Rotate(int direction){

        int originalRotation = this.rotationIndex;
        this.rotationIndex += Wrap(this.rotationIndex+direction, 0, 4);

        ApplyRotationMatrix(direction);

        if(!TestWallKicks(this.rotationIndex, direction)){
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }

        
    }

    private void ApplyRotationMatrix(int direction){

        for(int i = 0; i < this.data.Cells.Length; i++){
            Vector3 cell = this.Cells[i];
            int x, y;

            switch(this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1]*direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3]*direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1]*direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3]*direction));
                    break;
            }
            this.Cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection){
        int wallKickIndex = rotationIndex*2;

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++){
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

             if (Move(translation)) {
                return true;
            }
        }

        return false;
    }
    
    private int GetWallKickIndex(int rotationIndex, int rotationDirection){
        int wallKickIndex = rotationIndex*2;

        if (rotationDirection < 0){
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max){
        if(input<min){
            return max - (min-input)%(max-min);
        }
        else{
            return min + (input-min) %(max-min);
        }
    }

}
