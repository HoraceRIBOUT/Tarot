using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Generator : MonoBehaviour
{
    public List<GameObject> listPrefab = new List<GameObject>();
    public List<GameObject> listPrefab2 = new List<GameObject>();
    public List<Sprite> listSprite = new List<Sprite>();
    public List<SpriteRenderer> listCreated = new List<SpriteRenderer>();

    public Transform cameraPos;

    public bool setuptest = false;

    [Header("Value")]
    public float densityPerScreen = 6;
    public float screenSize = 20f;//if objct too far
    public float littleOffset = 1.2f;
    [Range(0,1)]
    public float alpha = 0.3f;
    public Vector2 rand_Pos = new Vector2(-5f, 5f);
    public Vector2 rand_Scale = new Vector2(10f,20f);
    public Vector2 rand_Ratio = new Vector2(0.8f, 1.6f);

    [Header("Little Zone")]
    public float lil_densityPerScreen = 6;
    public float lil_screenSize = 20f;
    [Range(0, 1)]
    public float lil_alpha = 0.3f;
    public Vector2 lil_rand_Pos = new Vector2(-5f, 5f);
    public Vector2 lil_rand_Scale = new Vector2(10f, 20f);
    public Vector2 lil_rand_Ratio = new Vector2(0.8f, 1.6f);


    // Start is called before the first frame update
    void Start()
    {
        cameraPos = GameManager.instance.cameraMng.transform;

        SetUp();
    }

    public void SetUp()
    {
        //Camera color
        if (GameManager.instance != null)
            GameManager.instance.cameraMng.ChangeBG(GameManager.instance.currentPartie.bg_Grad.Evaluate(1));
        else
            FindObjectOfType<CameraManager>().ChangeBG(FindObjectOfType<Partie>().bg_Grad.Evaluate(1));

        //Clean previous
        foreach(SpriteRenderer gO in listCreated)
        {
            if (Application.isPlaying)
                Destroy(gO.gameObject);
            else
                DestroyImmediate(gO.gameObject);
        }
        listCreated.Clear();

        //Creation
        for (int i = 0; i < densityPerScreen; i++)
        {
            SpriteRenderer sR = (Instantiate(
                listPrefab[Random.Range(0, listPrefab.Count)],
                this.transform)
                ).GetComponent<SpriteRenderer>();

            ReplaceRandom(sR, cameraPos.position +
                new Vector3(
                    Random.Range(-(screenSize + rand_Pos.x), (screenSize + rand_Pos.x)),
                    Random.Range(-(screenSize + rand_Pos.y), (screenSize + rand_Pos.y)), 0)
                    );

            listCreated.Add(sR);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (setuptest)
        {
            SetUp();
            setuptest = false;
        }

        TestDistance();
    }

    public void TestDistance()
    {
        foreach (SpriteRenderer gO in listCreated)
        {
            Vector3 direction = (gO.transform.position - cameraPos.position);
            if (direction.magnitude > screenSize * littleOffset)
            {
                ReplaceRandom(gO, cameraPos.position - (direction * 0.9f) + new Vector3(Random.Range(-rand_Pos.x, rand_Pos.x), Random.Range(-rand_Pos.y, rand_Pos.y), 0));
            }
        }
    }

    public void ReplaceRandom(SpriteRenderer sR, Vector3 position)
    {
        sR.transform.position = position;
        sR.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180f, 180f));
        sR.sprite = listSprite[(int)Random.Range(0, listSprite.Count)];
        sR.color = 
            ((Random.Range(0,100)>50)? GameManager.instance.currentPartie.bg_Grad : GameManager.instance.currentPartie.bg2_Grad)
            .Evaluate(Random.Range(0f, 1f)) - Color.black * (1-alpha);

        Vector3 scale = Vector3.one * Random.Range(rand_Scale.x, rand_Scale.y);
        scale.y = scale.x * Random.Range(rand_Ratio.x, rand_Ratio.y);
        sR.transform.localScale = scale;
    }



    public void CreateNewZone(Vector3 newCenter)
    {

        //Creation
        for (int i = 0; i < lil_densityPerScreen; i++)
        {
            SpriteRenderer sR = (Instantiate(
                listPrefab2[Random.Range(0, listPrefab2.Count)],
                this.transform)
                ).GetComponent<SpriteRenderer>();

            ReplaceRandom_LittleZone(sR, newCenter +
                new Vector3(
                    Random.Range(-(lil_screenSize + lil_rand_Pos.x), (lil_screenSize + lil_rand_Pos.x)),
                    Random.Range(-(lil_screenSize + lil_rand_Pos.y), (lil_screenSize + lil_rand_Pos.y)), 0)
                    );

            listCreated.Add(sR);
        }
        
    }

    public void ReplaceRandom_LittleZone(SpriteRenderer sR, Vector3 position)
    {
        sR.transform.position = position;
        sR.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180f, 180f));
        sR.color =
            ((Random.Range(0, 100) > 60) ? GameManager.instance.currentPartie.bg_over_Grad : GameManager.instance.currentPartie.bg2_Grad)
            .Evaluate(Random.Range(0f, 1f)) - Color.black * (1 - lil_alpha);

        Vector3 scale = Vector3.one * Random.Range(lil_rand_Scale.x, lil_rand_Scale.y);
        scale.y = scale.x * Random.Range(lil_rand_Ratio.x, lil_rand_Ratio.y);
        sR.transform.localScale = scale;
    }
}
