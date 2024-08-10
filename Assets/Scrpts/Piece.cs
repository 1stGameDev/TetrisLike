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
        timer += Time.deltaTime;
        if(timer >= fallTime){
            Position = new Vector3Int(Position.x, Position.y - 1, Position.z);
            timer = 0.0f;
        }
    }


    //I don't know why, but it doesn't work with the new input system
    public void Right(InputAction.CallbackContext context){
        Debug.Log("OnRight");
        Position = new Vector3Int(Position.x + 1, Position.y, Position.z);
    }

    public void Left(InputAction.CallbackContext context){
        Debug.Log("OnLeft");
        Position = new Vector3Int(Position.x - 1, Position.y, Position.z);
    }
}
