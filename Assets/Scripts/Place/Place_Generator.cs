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

    public class Largeur{
        public Place_Stars star;
        public float angle;
        public bool ancestorOfTheStart;
        public int step;
        public Largeur(Place_Stars _star, float _angle, bool _ancestorOfTheStart, int _step)
        {
            star = _star;
            angle = _angle;
            ancestorOfTheStart = _ancestorOfTheStart;
            step = _step;
        }
    }
    public List<Largeur> toTreat;

    public Place_Stars startStar = null;

    [Header("Generator")]
    public Vector2 spaceBetween = new Vector2(2, 4);
    public Vector2 minMaxStars = new Vector2(15, 30);
    public float bigChance = 15f;
    public float zeroChance = 20f;
    public float threeChance = 5f;
    public Vector2 minMaxAmplitudeWeirdo = new Vector2(0.2f, 0.6f);
    public Vector2 minMaxShiftWhenSplit2 = new Vector2(0.3f, 0.45f);
    public Vector2 minMaxShiftWhenSplit3 = new Vector2(0.4f, 0.5f);
    public float shiftGain = 0.2f;

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
        toTreat = new List<Largeur>();

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
            toTreat.Add(new Largeur(star, angle, true, 0));

            Place_Branch branch = Instantiate(normalBranch, this.transform).GetComponent<Place_Branch>();
            MutualBranching(branch, startStar, star);
            allBranches.Add(branch);
        }
        for (int i = 0; i < toTreat.Count; i++)
        {
            Largeur larg = toTreat[i];
            GenerateAPlace_AddStar(larg.star, larg.angle, larg.ancestorOfTheStart, larg.step);
            if (allStars.Count > minMaxStars.y)
            {
                break;
            }
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
        float shift = Mathf.Lerp(minMaxAmplitudeWeirdo.x, minMaxAmplitudeWeirdo.y, Mathf.Clamp01(stepFromLastShift * shiftGain));

        //Then, each star will get 1 or 2 branch, and sometime 3 sometime 0 (not 0 if youare juste next to a big star, nor if a direct link from the start)
        float rand = Random.Range(0f,100f);
        int numberBranch = 2;
        if ((rand < zeroChance || rand > (100 - threeChance)) && !directAncestorOfTheStart) 
        {
            numberBranch = rand < 50f ? 0 : 3;
        }
        else 
        {
            numberBranch = rand < 50f ? 1 : 2;
        }

        //When reach a 0 or 3, decide if it's a big star or not (card star !!!)

        if (numberBranch == 3 || numberBranch == 0)
        {
            float bigRandom = Random.Range(0f,100f);
            if(bigRandom > bigChance)
            {
                //Became big !!!
                //Maybe a place ?
                //Maybe a beast !
            }
        }

        if (numberBranch == 3)
            Debug.LogWarning("numberBranch = " + numberBranch + " rand = "+ rand + "(" + rand +"< "+ zeroChance +" || "+ rand +" > "+(100 - threeChance) + ".");

        for (int i = 0; i < numberBranch; i++)
        {
            Place_Stars star = Instantiate(normalStar, this.transform).GetComponent<Place_Stars>();
            allStars.Add(star);
            float value = GetAngleFromShift(numberBranch, i);
            float newAngle = angle + value;
            //Each have a general direction  (+/- a random) and will spawn next star far away.
            newAngle += Random.Range(-shift, shift);
            Debug.Log("value = " + value);
            star.mainDir = new Vector3(Mathf.Cos(newAngle * Mathf.PI), Mathf.Sin(newAngle * Mathf.PI), 0);
            Debug.DrawRay(fatherStar.transform.position, star.mainDir, (i == 0 ? Color.red : i == 1 ? Color.blue : i == 2 ? Color.green : Color.white), 0.5f);
            star.transform.position = fatherStar.transform.position + (Vector3)star.mainDir * Random.Range(spaceBetween.x, spaceBetween.y);

            //Too close of over
            LinkToClosestStar(star);

            //Branch handling
            Place_Branch branch = Instantiate(normalBranch, this.transform).GetComponent<Place_Branch>();
            MutualBranching(branch, fatherStar, star);
            allBranches.Add(branch);



            //Max Y stars !
            if (allStars.Count > minMaxStars.y)
                return;

            toTreat.Add(new Largeur(star, newAngle, false, (numberBranch == 1 ? stepFromLastShift + 1 : 0)));
        }

    }

    public Place_Stars LinkToClosestStar(Place_Stars star)
    {
        float distMinSqr = spaceBetween.x * spaceBetween.x;
        float distTooCloseSqr = spaceBetween.y * spaceBetween.y;
        Place_Stars res = null;
        foreach(Place_Stars st in allStars)
        {
            Vector3 direction = (star.transform.position - st.transform.position);
            if (direction.sqrMagnitude < distTooCloseSqr)
            {
                //Make it a little further away
                star.transform.position += direction * 1f;

                if (AreVoisine(star,st))
                {
                    //Branch handling
                    Place_Branch branch = Instantiate(normalBranch, this.transform).GetComponent<Place_Branch>();
                    MutualBranching(branch, star, st);
                    allBranches.Add(branch);
                }

            }
            if (direction.sqrMagnitude < distMinSqr)
            {
                distMinSqr = direction.sqrMagnitude;
                res = st;


                if (AreVoisine(star, st))
                {
                    //Branch handling
                    Place_Branch branch = Instantiate(normalBranch, this.transform).GetComponent<Place_Branch>();
                    MutualBranching(branch, star, st);
                    allBranches.Add(branch);
                }
            }
        }
        return res;
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

    public static bool AreVoisine(Place_Stars start,Place_Stars end)
    {
        foreach(Place_Branch br in start.allMyBranches)
        {
            if (br.start == end || br.end == end)
                return false;
        }
        return true;
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
