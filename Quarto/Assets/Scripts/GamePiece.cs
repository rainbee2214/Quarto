using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class GamePiece : MonoBehaviour
{
    StartingSpace startingCircle;
    public Vector3 deltaPosition;

    public bool Placed { get; set; }
    public PieceType pType;// { get; set; }
    void Awake()
    {
        startingCircle = FindObjectOfType<StartingSpace>();
        pType = (PieceType)(byte)Convert.ToInt32(name.Substring(name.Length - 4, 4), 2);
    }

    public void OnMouseDown()
    {
        if (Placed || !GameController.controller.YourTurn) return;
        if (!StartingSpace.ss.HasPiece)
        {
            Debug.Log("I'm being touched!");
            transform.position = startingCircle.transform.position;
            GameController.controller.CurrentGamePiece = this;

            //GameObject newPiece = Instantiate(gameObject, startingCircle.transform.position + deltaPosition, Quaternion.identity) as GameObject;
            StartingSpace.ss.HasPiece = true;
        }
    }

    public void OnVirtualMouseDown()
    {
        if (Placed) return;
        if (!StartingSpace.ss.HasPiece)
        {
            Debug.Log("I'm being touched!");
            transform.position = startingCircle.transform.position;
            GameController.controller.CurrentGamePiece = this;

            //GameObject newPiece = Instantiate(gameObject, startingCircle.transform.position + deltaPosition, Quaternion.identity) as GameObject;
            StartingSpace.ss.HasPiece = true;
        }
    }
    //IEnumerator FadeColor()
    //{
    //    SpriteRenderer logo;

    //    Color startColor = logo.color;
    //    startColor.a = 0;
    //    Color endColor = startColor;

    //    float increment = 0.1f;
    //    float percentage = 0;

    //    //fade from full alpha to no alpha
    //    while(logo.color != endColor)
    //    {
    //        logo.color = Color.Lerp(startColor, endColor, percentage);
    //        percentage += increment;
    //        //yield return new WaitForSeconds(delay);
    //        yield return null;
    //    }

    //    percentage = 0;
    //    yield return new WaitForSeconds(1f);
    //    while (logo.color != endColor)
    //    {
    //        logo.color = Color.Lerp(endColor, startColor, percentage);
    //        percentage += increment;
    //        //yield return new WaitForSeconds(delay);
    //        yield return null;
    //    }
    //    yield return null;
    //}
}
