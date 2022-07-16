using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Stars : MonoBehaviour
{
    public List<Place_Branch> allMyBranches;

    public Vector3 mainPos;

    [Header("Discovery")]
    public SpriteRenderer mainStar;
    public SpriteRenderer layerOne;
    public SpriteRenderer layerTwo;
    public float discoverForce = 0.6f;
    public float normalForce = 0.4f;
    public float discover = 0;
    public Vector2 offsetTarget = new Vector2(1,1);
    public Vector2 offsetTarget_delay = new Vector2(0,0);
    public float offsetTargetValue = 10f;
    public AnimationCurve amplifier = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Lil mov'")]
    public float lilMov_intensity;
    public Vector2 lilMov_Speed = new Vector2(2, 3);
    public Vector2 lilMov_Amplitude = new Vector2(0.5f, 0.7f);

    public void Start()
    {
        mainPos = Pos();
        transform.Rotate(0, 0, Random.Range(-180, 180));


        mainStar.color = GameManager.instance.currentPartie.starsBG_Grad.Evaluate(discover);
        layerOne.color = GameManager.instance.currentPartie.stars_Grad.Evaluate(discover);
        layerTwo.color = GameManager.instance.currentPartie.stars_Grad.Evaluate(discover);
    }


    public void Update()
    {

        float offsetMagn = offsetTarget.magnitude;
        if (offsetMagn > 0.01f)
        {
            offsetTarget -= offsetTarget.normalized * Time.deltaTime * amplifier.Evaluate(Mathf.Clamp01(offsetMagn));
        }
        else
        {
            offsetTarget = Vector2.zero;
            offsetTarget_delay = Vector2.zero;
        }
        offsetTarget_delay = Vector2.Lerp(offsetTarget_delay, offsetTarget, offsetTargetValue * Time.deltaTime);

        ALittleMovement();

        ColorUpdate();
    }


    public void ALittleMovement()
    {
        this.transform.position = mainPos
            + Vector3.up * Mathf.PerlinNoise(mainPos.x + (Time.time * lilMov_Speed.x), mainPos.y + (Time.time * lilMov_Speed.y)) * lilMov_Amplitude.x
            + Vector3.right * Mathf.PerlinNoise(mainPos.y + (Time.time * lilMov_Speed.x), mainPos.x + (Time.time * lilMov_Speed.y)) * lilMov_Amplitude.y
            + (Vector3)offsetTarget_delay
            ;

        foreach(Place_Branch br in allMyBranches)
        {
            br.SetUp();
        }
    }

    public void GetDiscover(Vector2 direction, bool firstTime)
    {
        offsetTarget = direction.normalized * (firstTime ? discoverForce : normalForce);
        if (!firstTime)
            return;
        if (discover != 0)
            return;


        discover = 0.001f;
    }

    public void ColorUpdate()
    {
        if (discover > 0 && discover < 1)
        {
            discover += Time.deltaTime * 0.5f;
            if (discover > 1)
                discover = 1;

            mainStar.color = GameManager.instance.currentPartie.starsBG_Grad.Evaluate(discover);
            layerOne.color = GameManager.instance.currentPartie.stars_Grad.Evaluate(discover);
            layerTwo.color = GameManager.instance.currentPartie.stars_Grad.Evaluate(discover);
        }
    }

    public Vector3 Pos()
    {
        return transform.position;
    }


}
