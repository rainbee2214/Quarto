using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum Attr
{
    Light = 1,
    Tall = 2,
    Striped = 4,
    Star = 8
}
public class GameController : MonoBehaviour
{
    public Text messageText;
    public static GameController controller;
    public GamePiece CurrentGamePiece { get; set; }

    public bool YourTurn; // { get; set; }
    CircleSpot[][] gameRows;

    bool hasWon = false;
    bool isChoosingCircleSpot;

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
        gameRows = new CircleSpot[4][];
        for (int i = 0; i < 4; i++)
        {
            gameRows[i] = new CircleSpot[4];
            for (int k = 0; k < 4; k++)
            {
                //Debug.Log(gameRows[i].ToString());
                gameRows[i][k] = GameObject.Find("CircleSpot" + (i + 1) + (k + 1)).GetComponent<CircleSpot>();
            }
        }

    }

    public byte piece = (byte)PieceType.P11;

    void Start()
    {
        if (GameSetup.instance != null) Debug.Log(GameSetup.instance.NumberOfPlayers + "Player");
        //allGamePieces = new List<Piece>();
        foreach (PieceType pt in Enum.GetValues(typeof(PieceType)))
        {
            byte p = (byte)pt;

        }
        StartCoroutine(StartGame());

    }

    void Update()
    {
        if (hasWon)
        {
            hasWon = false;
            Debug.Log("Verify the winner!");
            StartCoroutine(WonGame());
        }

        messageText.text = YourTurn ? (isChoosingCircleSpot ? " Place the piece on the board." : "Choose a piece for the AI to place") : "AI turn";
    }

    int mainScene = 0;
    public IEnumerator WonGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //do something for winning
    }

    IEnumerator StartGame()
    {
        Debug.Log("Starting game");
        YourTurn = (UnityEngine.Random.Range(0, 11) % 2 == 0) ? true : false;

        Debug.Log("Starting turn: " + (YourTurn ? "Your turn" : "Computers turn"));

        if (YourTurn)
        {
            //prompt to choose piece - after piece is selected
            while (!StartingSpace.ss.HasPiece)
            {
                yield return null;
            }
            YourTurn = false;
        }
        else
        {
            //choose a starting piece for the player at random
            GameObject[] g = GameObject.FindGameObjectsWithTag("GamePiece");
            g[UnityEngine.Random.Range(0, g.Length)].GetComponent<GamePiece>().OnVirtualMouseDown();
            YourTurn = true;
        }

        //Debug.Log("Regular turns are starting");

        int turnCount = 1;
        bool foundMatch = false;
        GameObject[] gamePieces = GameObject.FindGameObjectsWithTag("GamePiece");
        while (!foundMatch)
        {
            if (YourTurn)
            {
                Debug.Log("Your turn - choose a spot on the board");
                //wait for you to place piece on board
                while (StartingSpace.ss.HasPiece)
                {
                    isChoosingCircleSpot = true;
                    yield return null;
                }
                foundMatch = CheckBoardForMatches(gameRows);
                hasWon = foundMatch;
                //Debug.Log("Your turn - choose a new piece");
                //wait for you to pick a piece for computer
                while (!StartingSpace.ss.HasPiece)
                {
                    isChoosingCircleSpot = false;
                    yield return null;
                }
                YourTurn = false;
            }
            else
            {
                //pick a spot on the board
                //look for a winning solution - through all remaining places, if no place, pick random
                //CircleSpot[][] testRow = gameRows;
                //for(int i = 0; i < 4; i++)
                //{
                //    for (int k = 0; k < 4; k++)
                //    {
                //        if (gameRows[i][k].HasPiece) continue;
                //        testRow = gameRows;
                //        testRow[i][k].OnVirtualMouseDownTest();
                //        Debug.Log("Testing " + CheckBoardForMatches(testRow));
                //        testRow[i][k].UndoTest();
                //    }
                //}
                //Debug.Log("Computers turn - picking a spot to place piece");
                //random for now
                CircleSpot cSpot = gameRows[UnityEngine.Random.Range(0, 4)][UnityEngine.Random.Range(0, 4)];
                while (cSpot.HasPiece)
                {
                    //pick a new piece
                    cSpot = gameRows[UnityEngine.Random.Range(0, 4)][UnityEngine.Random.Range(0, 4)];
                    Debug.Log(cSpot.HasPiece);
                    yield return null;
                }
                yield return new WaitForSeconds(0.25f);
                cSpot.OnVirtualMouseDown();

                //Debug.Log("Computers turn - choosing a new piece");
                //Don't choose a piece that will cause a match (only matters if any row has more than 3 pieces)
                GamePiece gp = gamePieces[UnityEngine.Random.Range(0, gamePieces.Length)].GetComponent<GamePiece>();
                yield return new WaitForSeconds(0.25f);
                while (gp.Placed)
                {
                    gp = gamePieces[UnityEngine.Random.Range(0, gamePieces.Length)].GetComponent<GamePiece>();
                    yield return null;
                }
                gp.OnVirtualMouseDown();
                YourTurn = true;

            }
            turnCount++;

            //choose who starts 
            //- if your turn
            //pick a starting piece - your turn = false
            //computer starts regular turn: choose where to place piece, and choose another piece to place, yourturn = true
            //you do your turn 
            //check for wins
            //loop until game over
            //- computer turn
            foundMatch = CheckBoardForMatches(gameRows);
            hasWon = foundMatch;
            yield return null;
        }
        Debug.Log("Someone has won!");

    }
    bool CheckBoardForMatches(CircleSpot[][] rows)
    {
        // 8 Rows to check - 4 vertical, 4 horizontal
        for (int i = 0; i < 4; i++)
        {
            if (HasMatchingType(rows[i])) return true;
        }
        for (int i = 0; i < 4; i++)
        {
            if (HasMatchingType(rows[0][i], rows[1][i], rows[2][i], rows[3][i])) return true;
        }
        return false;
    }

    bool HasMatchingType(CircleSpot r1, CircleSpot r2, CircleSpot r3, CircleSpot r4)
    {
        CircleSpot[] row = new CircleSpot[4] { r1, r2, r3, r4 };
        return HasMatchingType(row);
    }

    bool HasMatchingType(CircleSpot[] row)
    {
        if (row[0].GetComponent<CircleSpot>().pType == PieceType.PE || row[1].GetComponent<CircleSpot>().pType == PieceType.PE || row[2].GetComponent<CircleSpot>().pType == PieceType.PE || row[3].GetComponent<CircleSpot>().pType == PieceType.PE) return false;
        bool[] r1 = GetBits(row[0].GetComponent<CircleSpot>().pType);
        bool[] r2 = GetBits(row[1].GetComponent<CircleSpot>().pType);
        bool[] r3 = GetBits(row[2].GetComponent<CircleSpot>().pType);
        bool[] r4 = GetBits(row[3].GetComponent<CircleSpot>().pType);

        if (r1[0] && r2[0] && r3[0] && r4[0]) return true;
        if (r1[1] && r2[1] && r3[1] && r4[1]) return true;
        if (r1[2] && r2[2] && r3[2] && r4[2]) return true;
        if (r1[3] && r2[3] && r3[3] && r4[3]) return true;

        if (!(r1[0] || r2[0] || r3[0] || r4[0])) return true;
        if (!(r1[1] || r2[1] || r3[1] || r4[1])) return true;
        if (!(r1[2] || r2[2] || r3[2] || r4[2])) return true;
        if (!(r1[3] || r2[3] || r3[3] || r4[3])) return true;

        return false;
    }

    public bool[] GetBits(PieceType pType)
    {
        switch (pType)
        {
            default:
            case PieceType.P0: return new bool[4] { false, false, false, false };
            case PieceType.P1: return new bool[4] { false, false, false, true };
            case PieceType.P2: return new bool[4] { false, false, true, false };
            case PieceType.P3: return new bool[4] { false, false, true, true };
            case PieceType.P4: return new bool[4] { false, true, false, false };
            case PieceType.P5: return new bool[4] { false, true, false, true };
            case PieceType.P6: return new bool[4] { false, true, true, false };
            case PieceType.P7: return new bool[4] { false, true, true, true };
            case PieceType.P8: return new bool[4] { true, false, false, false };
            case PieceType.P9: return new bool[4] { true, false, false, true };
            case PieceType.P10: return new bool[4] { true, false, true, false };
            case PieceType.P11: return new bool[4] { true, false, true, true };
            case PieceType.P12: return new bool[4] { true, true, false, false };
            case PieceType.P13: return new bool[4] { true, true, false, true };
            case PieceType.P14: return new bool[4] { true, true, true, false };
            case PieceType.P15: return new bool[4] { true, true, true, true };
        }
    }
}
