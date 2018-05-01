using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RochadeController {

    public bool Left { get; private set; }
    public bool Right { get; private set; }

    public RochadeController()
    {
        Left = true;
        Right = true;
    }

    public void OnKingsMove()
    {
        Left = false;
        Right = false;
    }

    public void OnLeftRooksMove()
    {
        Left = false;
    }

    public void OnRightRooksMove()
    {
        Right = false;
    }

}
