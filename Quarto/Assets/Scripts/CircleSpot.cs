using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum PieceType : byte
{
    P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, PE
}

//[System.Serializable]
//public class Piece
//{
//    public Piece(PieceType p)
//    {
//        pType = p;
//    }
//    public PieceType pType;
//}
public class CircleSpot : MonoBehaviour
{

    public Vector2 position;

    //public PieceType currentPiece;
    public bool HasPiece { get; set; }
    public PieceType pType = PieceType.PE; // { get; set; }
    public void OnMouseDown()
    {
        if (HasPiece || !GameController.controller.YourTurn) return;
        if (StartingSpace.ss.HasPiece)
        {
            GameController.controller.CurrentGamePiece.gameObject.transform.position = transform.position;
            GameController.controller.CurrentGamePiece.Placed = true;
            StartingSpace.ss.HasPiece = false;
            pType = GameController.controller.CurrentGamePiece.pType;
            HasPiece = true;
        }
    }
    public void OnVirtualMouseDown()
    {
        if (HasPiece) return;
        if (StartingSpace.ss.HasPiece)
        {
            GameController.controller.CurrentGamePiece.gameObject.transform.position = transform.position;
            GameController.controller.CurrentGamePiece.Placed = true;
            StartingSpace.ss.HasPiece = false;
            pType = GameController.controller.CurrentGamePiece.pType;

            HasPiece = true;
        }
    }

    //PieceType tempPieceType;

    //public void OnVirtualMouseDownTest()
    //{
    //    tempPieceType = pType;
    //    pType = GameController.controller.CurrentGamePiece.pType;
    //    HasPiece = true;
    //}

    //public void UndoTest()
    //{
    //    pType = tempPieceType;
    //    HasPiece = false;
    //}
}
