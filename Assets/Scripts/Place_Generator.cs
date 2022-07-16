using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Generator : MonoBehaviour
{
    public List<Place_Branch> allBranches;
    public List<Place_Stars> allStars;

    public Place_Stars startStar = null;

    [Header("Generator")]
    public Vector2 spaceBetween = new Vector2(1, 3);


    [Header("Data")]
    public float scaler = 1;

    // Start is called before the first frame update
    void Start()
    {
        //GenerateAPlace();
    }

    public void GenerateAPlace()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Getter
    public static float GetScaler()
    {
        if (GameManager.instance == null)
            GameManager.instance = FindObjectOfType<GameManager>();

        return GameManager.instance.place.scaler;
    }
    #endregion
}
