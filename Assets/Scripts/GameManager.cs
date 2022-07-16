using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {

            instance = this;
        }
        else
        {

        }
    }

    public CameraManager cameraMng;
    public Character chara;
    public Partie currentPartie;
    public Place_Generator place;
}
