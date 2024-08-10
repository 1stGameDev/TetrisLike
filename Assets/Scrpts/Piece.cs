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
    public float fallTime = 1.0f;
    private float timer = 0.0f;
    private bool canRight = true;
    private bool canLeft = true;
    private bool canDown = true;
    private bool canHardDown = true;
    private int lockCount = 0;
    private bool isLocked = false;

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
    }


    void Update(){
        //Before moving the piece in any way, it clears it from the board. This way the piece won't "colide" with itself.
        if(!isLocked){
            this.Board.Clear(this);


            int fall = Fall(Time.deltaTime);
            if(fall == 0){
                lockCount++;
            }
            else if(fall == 1){
                lockCount--;
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

            if(lockCount >= 2){
                isLocked = true;
            }
        }
    }

    //Works the same as the movement but with the added feature of the delta.
    //delta controls how quicly it falls.
    private int Fall(float delta){
        timer += delta;
        if(timer >= fallTime){
            bool ret = Down();
            timer = 0.0f;
            if(ret){
                return 1;
            }
            else{
                return 0;
            }
        }
        else{
            return 2;
        }
    }


    private void HardDown(){
        while(Down()){

        }
    }
    private bool Down(){
        Vector3Int newpos = new Vector3Int(Position.x, Position.y - 1, Position.z);
            if(this.Board.IsValidPosition(this, newpos)){
                Position = newpos;
                return true;
            }
            else{
                return false;
            }
    }

    private void Right(){
        //Checks if the piece would collide with the limits, bottom or another piece. If it doesn't, it moves.
        Vector3Int newpos = new Vector3Int(Position.x + 1, Position.y, Position.z);
        if(this.Board.IsValidPosition(this, newpos)){
            Position = newpos;
        }
    }

    private void Left(){
        Vector3Int newpos = new Vector3Int(Position.x - 1, Position.y, Position.z);
        if(this.Board.IsValidPosition(this, newpos)){
            Position = newpos;
        }
    }
}
