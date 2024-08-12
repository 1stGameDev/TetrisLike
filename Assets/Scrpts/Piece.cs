using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public Vector3Int Position; //{ get; private set; }
    public TetrominoData Data { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    



    //For moving the pice:
    public float fallTime;
    private float fallTimer;
    private bool canRight;
    private bool canLeft;
    private bool canDown;
    private bool canHardDown;
    private float lockTime;
    public float lockLimit;
    private bool isLocked;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        Board = board;
        Position = position;
        Data = data;

        Cells ??= new Vector3Int[data.Cells.Length];

        for (int i = 0; i < data.Cells.Length; i++)
        {
            Cells[i] = (Vector3Int)data.Cells[i];
        }

        fallTimer = 0.0f;
        canRight = true;
        canLeft = true;
        canDown = true;
        canHardDown = true;
        isLocked = false;
        lockTime = 0.0f;
        lockLimit = 0.5f;


    }


    void Update(){
        //Before moving the piece in any way, it clears it from the board. This way the piece won't "colide" with itself.
        if(!isLocked){
            this.Board.Clear(this);

            //If the time since the last Fall is greater than fallTime, it falls.
            fallTimer += Time.deltaTime;
            lockTime += Time.deltaTime;
            if(fallTimer >= fallTime)
            {
                Fall();
            }

            //Second try to move the pieces sideways (without using Unity's new Input system)
            if(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow)){
                //Same as with the pause menu.
                //It checks if it has moved in the same key-press.
                if(canRight){
                    Right();
                }
                canRight = false;
            }
            else{
                canRight = true;
            }
            if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow)){
                if(canLeft){
                    Left();
                }
                canLeft = false;
            }
            else{
                canLeft = true;
            }
            if(Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)){
                if(canDown){
                    Down();
                }
                canDown = false;            
            }
            else{
                canDown = true;
            }
            if(Input.GetKey(KeyCode.Space)){
                if(canHardDown){
                    HardDown();
                }
                canHardDown = false;
            }
            else{
                canHardDown = true;
            }

            
            
            //Now that we have moved (or not) the piece, it draws it in the board.
            this.Board.Set(this);
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

    private void Right(){
        //Checks if the piece would collide with the limits, bottom or another piece. If it doesn't, it moves.
        Vector3Int newpos = new Vector3Int(Position.x + 1, Position.y, Position.z);
        if(this.Board.IsValidPosition(this, newpos)){
            Position = newpos;
            lockTime = 0.0f;
        }
    }

    private void Left(){
        Vector3Int newpos = new Vector3Int(Position.x - 1, Position.y, Position.z);
        if(this.Board.IsValidPosition(this, newpos)){
            Position = newpos;
            lockTime = 0.0f;
        }
    }

    private void Lock(){
        this.Board.Set(this);
        this.Board.CheckLine();
        this.Board.SpawnPiece();
    }
}
