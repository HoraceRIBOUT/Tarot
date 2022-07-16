using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Generator : MonoBehaviour
{
    public GameObject mainStar;
    public GameObject normalStar;
    public GameObject normalBranch;

    public List<Place_Branch> allBranches;
    public List<Place_Stars> allStars;

    public Place_Stars startStar = null;

    [Header("Generator")]
    public Vector2 spaceBetween = new Vector2(2, 4);
    public Vector2 minMaxStars = new Vector2(15, 30);
    public Vector2 minMaxAmplitudeWeirdo = new Vector2(0.2f, 0.6f);
    public Vector2 minMaxShiftWhenSplit2 = new Vector2(0.3f, 0.45f);
    public Vector2 minMaxShiftWhenSplit3 = new Vector2(0.4f, 0.5f);

    [Header("Data")]
    public float scaler = 1;

    [Space]
    [Space]
    public bool generate_test = false;

    // Start is called before the first frame update
    void Start()
    {
        //GenerateAPlace();
    }

    public void GenerateAPlace()
    {
        foreach(Place_Branch br in allBranches)
        {
            Destroy(br.gameObject);
        }
        allBranches.Clear();

        foreach (Place_Stars st in allStars)
        {
            Destroy(st.gameObject);
        }
        allStars.Clear();
        
        int numberStarsTotal = (int)Random.Range(minMaxStars.x, minMaxStars.y);

        startStar = Instantiate(mainStar, this.transform).GetComponent<Place_Stars>();
        startStar.transform.position = Vector3.zero;
        allStars.Add(startStar);

        int numberBranch = (int)Random.Range(3f, 5f);
        float startDirection = Random.Range(-1f, 1f);

        for (int i = 0; i < numberBranch; i++)
        {
            //add a stars (direction)
            Place_Stars star = Instantiate(normalStar, this.transform).GetComponent<Place_Stars>();
            allStars.Add(star);
            float angle = startDirection + (2f / numberBranch) * i;
            angle += Random.Range(-minMaxAmplitudeWeirdo.y, minMaxAmplitudeWeirdo.y);
            star.mainDir = new Vector3(Mathf.Cos(angle * Mathf.PI), Mathf.Sin(angle * Mathf.PI), 0);
            //Debug.DrawRay(startStar.transform.position, star.mainDir, (i == 0 ? Color.red : i == 1 ? Color.blue : i == 2 ? Color.green : Color.white), 0.5f);
            star.transform.position = startStar.transform.position + (Vector3)star.mainDir * Random.Range(spaceBetween.x, spaceBetween.y);
            //Keep going on star, recursive
            GenerateAPlace_AddStar(star, angle, true, 0);

            Place_Branch branch = Instantiate(normalBranch, this.transform).GetComponent<Place_Branch>();
            MutualBranching(branch, startStar, star);
            allBranches.Add(branch);
        }

        //If not enough stars, spawn some more !!!
        //For now

        //once finish, random for X+Y star and see if they are already link OR if there distance are low, then, you can link them !
        //Do this later , ok ?

        //then, finish, make the chara on this startStar and it's done !
        GameManager.instance.chara.currentStar = startStar;
    }

    public void GenerateAPlace_AddStar(Place_Stars fatherStar, float angle, bool directAncestorOfTheStart, int stepFromLastShift)
    {
        //so, this is the direction : 
        //The further your are without shift, the bigger the random
        float shift = Mathf.Lerp(minMaxAmplitudeWeirdo.x, minMaxAmplitudeWeirdo.y, Mathf.Clamp01(stepFromLastShift * 0.35f));

        //Then, each star will get 1 or 2 branch, and sometime 3 sometime 0 (not 0 if youare juste next to a big star, nor if a direct link from the start)
        float rand = Random.Range(0f,100f);
        int numberBranch = 2;
        if ((numberBranch < 20 || numberBranch > 90) && !directAncestorOfTheStart)
        {
            numberBranch = rand < 50f ? 0 : 3;
        }
        else 
        {
            numberBranch = rand < 50f ? 1 : 2;
        }
         
        for (int i = 0; i < numberBranch; i++)
        {
            Place_Stars star = Instantiate(normalStar, this.transform).GetComponent<Place_Stars>();
            allStars.Add(star);
            float value= GetAngleFromShift(numberBranch, i);
            float newAngle = angle + value;
            //Each have a general direction  (+/- a random) and will spawn next star far away.
            newAngle += Random.Range(-shift, shift);
            Debug.Log("value = " + value);
            star.mainDir = new Vector3(Mathf.Cos(newAngle * Mathf.PI), Mathf.Sin(newAngle * Mathf.PI), 0);
            Debug.DrawRay(fatherStar.transform.position, star.mainDir, (i == 0 ? Color.red : i == 1 ? Color.blue : i == 2 ? Color.green : Color.white), 0.5f);
            star.transform.position = fatherStar.transform.position + (Vector3)star.mainDir * Random.Range(spaceBetween.x, spaceBetween.y);

            //Branch handling
            Place_Branch branch = Instantiate(normalBranch, this.transform).GetComponent<Place_Branch>();
            MutualBranching(branch, fatherStar, star);
            allBranches.Add(branch);

            //Max Y stars !
            if (allStars.Count > minMaxStars.y)
                return;
            //Keep going on star, recursive
            GenerateAPlace_AddStar(star, newAngle, false, numberBranch == 1 ? 0 : stepFromLastShift + 1);
        }

        
        //When reach a 0 or 3, decide if it's a big star or not (card star !!!)


        //GenerateAPlace_AddStar();

    }

    public float GetAngleFromShift(int numberBranch, int index)
    {
        if (numberBranch == 1)
            return 0;
        if (numberBranch == 2)
            return Random.Range(minMaxShiftWhenSplit2.x, minMaxShiftWhenSplit2.y) * (index == 0 ? -1 : 1);
        if (numberBranch == 3)
            return Random.Range(minMaxShiftWhenSplit3.x, minMaxShiftWhenSplit3.y) * (index - 1);
        return 0;
    }

    public static void MutualBranching(Place_Branch br, Place_Stars start, Place_Stars end)
    {
        br.Set(start, end);
        if(!start.allMyBranches.Contains(br))
            start.allMyBranches.Add(br);
        if (!end.allMyBranches.Contains(br))
            end.allMyBranches.Add(br);
    }

    // Update is called once per frame
    void Update()
    {

        if (generate_test)
        {
            GenerateAPlace();
            generate_test = false;
        }
        
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
