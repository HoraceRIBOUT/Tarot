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
            currentStar.discover = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovementManagement();

    }

    private void MovementManagement()
    {

        Vector2 direction = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

//        Debug.Log("direction = "+ direction);
        if(direction.x > 0)
        {
            if(currentBranch == null && currentStar != null)
            {
                currentBranch = currentStar.allMyBranches[1];
                lastStar = currentStar;
                progression = Time.deltaTime * speed / currentBranch.scale; progression_delay = 0;
                Debug.Log("set up branches 0");
                currentStar = null;
            }
            else if( currentBranch != null)
            {
                progression += Time.deltaTime * speed / currentBranch.scale;
            }
            else
            {
                Debug.LogError("why ?");
            }
        }
        if (direction.x < 0)
        {
            if (currentBranch == null && currentStar != null)
            {
                currentBranch = currentStar.allMyBranches[0];
                lastStar = currentStar;
                progression = 1 - Time.deltaTime * speed / currentBranch.scale; progression_delay = 1;
                Debug.Log("set up branches 1");
                currentStar = null;

            }
            else if (currentBranch != null)
            {
                progression -= Time.deltaTime * speed / currentBranch.scale;
            }
            else
            {
                Debug.LogError("why ?");
            }
        }

        if (progression > 1)
        {
            currentStar = currentBranch.end;
            currentStar.GetDiscover(-currentBranch.direction, currentStar.discover == 0);
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
            currentStar.GetDiscover( currentBranch.direction, currentStar.discover == 0);
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
