using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Place_Branch : MonoBehaviour
{
    public Place_Stars start;
    public Place_Stars end;

    //public Card : barrage or meeting ? 
    //ressources ? deal with somebody else, just call when reach

    public float scale = 1f;
    public Vector2 direction = new Vector2(0, 0);
    public Transform rightDir = null;


    [Header("Visual")]
    public SpriteRenderer main = null;
    public SpriteRenderer cover = null;
    public List<SpriteRenderer> bg = new List<SpriteRenderer>();

    [Range(0,1)]
    public float discovery = 0;
    public bool disco_sideL = true;

    public Vector2 disco_data = new Vector2(-0.032f, -0.521f);

    // Start is called before the first frame update
    void Start()
    {
        if (start != null && end != null)
            SetUp();
        else if(Application.isPlaying)
        {
            Debug.LogError("This branch have no two start !", this.gameObject);
        }

        if (Application.isPlaying)
        {
            main.color = GameManager.instance.currentPartie.branch_Grad.Evaluate(0);
            cover.color = GameManager.instance.currentPartie.branch_Grad.Evaluate(1);
        }
    }

    public void SetUp()
    {
        this.transform.position = (start.Pos() + end.Pos()) / 2;
        this.transform.rotation = Quaternion.Euler(0, 0, ZRotationFromTwoVector(start.Pos(), end.Pos()) );
        scale = (start.Pos() - end.Pos()).magnitude - Place_Generator.GetScaler();
        this.transform.localScale = new Vector3(scale, 1, 1);
        direction = (rightDir.position - this.transform.position).normalized;
    }

    
    public void Update()
    {

        if (!Application.isPlaying)
        {
            if(start != null && end != null)
                SetUp();
            UpdateDiscovery(discovery, !disco_sideL);
        }
    }

    public void UpdateDiscovery(float newValue, bool right)
    {
        if (discovery > newValue)
            return;

        float correctSide = right ? -(disco_data.y) : disco_data.y;
        cover.transform.localPosition = new Vector3(Mathf.Lerp(correctSide, disco_data.x, newValue), 0, 0);
        cover.transform.localScale = new Vector3(newValue * 1f, 0.2f, 1);

        foreach (SpriteRenderer sR in bg)
        {
            sR.color = GameManager.instance.currentPartie.branchBG_Grad.Evaluate(newValue);
        }

        discovery = newValue;
        disco_sideL = !right;
    }

    public static float ZRotationFromTwoVector(Vector2 A, Vector2 B)
    {
        Vector3 currentWorldForwardDirection = Vector3.right;
        Vector3 direction = B - A;
        float angleDiff = Vector3.SignedAngle(currentWorldForwardDirection, direction.normalized, Vector3.forward);

        return angleDiff;
    }
    
}
