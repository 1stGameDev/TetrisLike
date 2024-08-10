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
        this.Board.Clear(this);


        Fall(Time.deltaTime);

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

        //Now that we have moved (or not) the piece, it draws it in the board.
        this.Board.Set(this);
    }

    //Works the same as the movement but with the added feature of the delta.
    //delta controls how quicly it falls.
    private void Fall(float delta){
        timer += delta;
        if(timer >= fallTime){
            Vector3Int newpos = new Vector3Int(Position.x, Position.y - 1, Position.z);
            if(this.Board.IsValidPosition(this, newpos)){
                Position = newpos;
            }
            timer = 0.0f;
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
