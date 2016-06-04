using UnityEngine;
using System.Collections;

public class StartingSpace : MonoBehaviour
{

    public static StartingSpace ss;

    public bool HasPiece { get; set; }

    void Awake()
    {
        ss = this;
        HasPiece = false;
    }
}
