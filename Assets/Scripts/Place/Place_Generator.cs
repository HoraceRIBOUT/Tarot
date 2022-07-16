using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Generator : MonoBehaviour
{
    public GameObject mainStar;
    public GameObject normalStar;
    public GameObject branch;

    public List<Place_Branch> allBranches;
    public List<Place_Stars> allStars;

    public Place_Stars startStar = null;

    [Header("Generator")]
    public Vector2 spaceBetween = new Vector2(2, 4);


    [Header("Data")]
    public float scaler = 1;

    // Start is called before the first frame update
    void Start()
    {
        //GenerateAPlace();
    }

    public void GenerateAPlace()
    {
        //Spawn the center : add 3 to 4 branch. Each have a general direction  (+/- a random) and will spawn next star far away.
        //The further your are without shift, the bigger the random
        //Then, each star will get 1 or 2 branch, and sometime 3 sometime 0 (not 0 if youare juste next to a big star, nor if a direct link from the start)
        //When reach a 0 or 3, decide if it's a big star or not (card star !!!)
        //At least X stars put, and max Y stars !
        //once finish, random for X+Y star and see if they are already link OR if there distance are low, then, you can link them !

        //then, finish, make the chara on this startStar and it's done !
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
