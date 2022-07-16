using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Place_Branch currentBranch = null;
    [Range(0,1)]
    public float progression = 0;
    public float progression_delay = 0;
    public float progression_delayValue = 10f;
    public Place_Stars currentStar = null;
    public Place_Stars lastStar = null;


    [Header("Value for deplacement")]
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //For now, here
        if (currentStar == null)
        {
            currentStar = GameManager.instance.place.startStar;
            currentStar.discover =0.99f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovementManagement();

    }

    private void MovementManagement()
    {

        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        //        Debug.Log("direction = "+ direction);
        if (currentBranch == null && currentStar != null)
        {
            int indexRes = -1;
            float maxDot = -1;
            for (int i = 0; i < currentStar.allMyBranches.Count; i++)
            {
                Place_Branch br = (Place_Branch)currentStar.allMyBranches[i];
                float dot = Vector2.Dot(br.direction, direction);
                dot *= (br.start == currentStar ? 1 : -1);
                if(dot > 0 && dot > maxDot)
                {
                    maxDot = dot;
                    indexRes = i;
                }
            }
            if (indexRes == -1)
            {
                UpdatePosition();
                return; //don't move
            }

            currentBranch = currentStar.allMyBranches[indexRes];
            lastStar = currentStar;
            //how to define progress ? from 1 or from 0 ???
            if(currentBranch.start == lastStar)
            {
                progression =     Time.deltaTime * maxDot * speed / currentBranch.scale;
                progression_delay = 0;
            }
            else
            {
                progression = 1 - Time.deltaTime * maxDot * speed / currentBranch.scale;
                progression_delay = 1;
            }
            Debug.Log("set up branches " +indexRes + " with a dot at "+maxDot);
            currentStar = null;
        }
        else if (currentBranch != null)
        {
            float dot = Vector2.Dot(currentBranch.direction, direction);//dot between direction and branch.direction

            Debug.DrawRay(this.transform.position, direction, Color.red);
            Debug.DrawRay(this.transform.position, currentBranch.direction, Color.blue);
            Debug.DrawRay(this.transform.position, Vector3.up * dot * 2, Color.red + Color.blue);

            progression += Time.deltaTime * (dot) * speed / currentBranch.scale;
        }
        else
        {
            Debug.LogError("why ?");
        }






        if (progression > 1)
        {
            currentStar = currentBranch.end;
            currentStar.GetDiscover( currentBranch.direction, currentStar.discover == 0);
            if (currentStar.discover == 0)
            {
                currentBranch.UpdateDiscovery(1, false);
            }
            progression = 0; progression_delay = 0;
            currentBranch = null;
        }
        if (progression < 0)
        {
            currentStar = currentBranch.start;
            currentStar.GetDiscover(-currentBranch.direction, currentStar.discover == 0);
            if (currentStar.discover == 0)
            {
                currentBranch.UpdateDiscovery(1, true);
            }
            progression = 0;            progression_delay = 0;
            currentBranch = null;
        }
        
        if (currentBranch != null)
        {
            if(currentBranch.start == lastStar)
                currentBranch.UpdateDiscovery(    progression, false);
            else
                currentBranch.UpdateDiscovery(1 - progression, true );
        }



        UpdatePosition();
    }

    public void UpdatePosition()
    {
        progression_delay = Mathf.Lerp(progression_delay, progression, Time.deltaTime * progression_delayValue);

        if (currentBranch == null && currentStar != null)
        {
            this.transform.position = currentStar.transform.position;
        }
        else if (currentBranch != null)
        {
            this.transform.position = Vector3.Lerp(currentBranch.start.Pos(), currentBranch.end.Pos(), progression_delay);
        }
        else
        {

        }
    }
}
