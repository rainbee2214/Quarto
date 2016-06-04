using UnityEngine;
using System.Collections;

public class ConvertToGamePiece : MonoBehaviour
{

    public PieceType pieceType = PieceType.PE;

    //reference to ui game piece

    public void ConvertToGame()
    {
        //Move a game piece into the waiting circle
        //turn off this ui piece
        TurnUIPieceOff();
    }

    void TurnUIPieceOff()
    {

    }
}
