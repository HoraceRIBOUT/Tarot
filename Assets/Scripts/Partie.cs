using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Partie : MonoBehaviour
{
    public Color color1; // is the off
    public Color color2; //is the neutral
    public Color color3; //is the on

    public Gradient bg_Grad; //2 color but blackened
    [SerializeField] private float blackening = 0.4f;
    public Gradient bg2_Grad; //2 color but blackened
    [SerializeField] private float blackening2 = 0.8f;
    public Gradient bg_over_Grad; //Last color but blackneed (less)
    [SerializeField] private float blackening_less = 0.6f;
    public Gradient branchBG_Grad; //from off to on ->  bg color , one is off, other is on
    [SerializeField] private float branchBG_alpha = 0.1f;
    public Gradient branch_Grad; //from off to on -> 
    [SerializeField] private float branch_alpha = 0.9f;
    public Gradient starsBG_Grad; //one color only (not really off)
    public Gradient stars_Grad; //from off to on ->

    public bool setuptest = false;
    public void Update()
    {
        if (setuptest)
        {
            SetUp();
            setuptest = false;
        }
    }

    public void SetUp()
    {
        bg_Grad         = SetUp_Gradient(color1 * blackening        , color2 * blackening       , 1                 , 1                 );
        bg2_Grad        = SetUp_Gradient(color1 * blackening2       , color2 * blackening2      , 1                 , 1                 );
        bg_over_Grad    = SetUp_Gradient(color3 * blackening_less   , color3 * blackening_less  , 1                 , 1                 );

        branchBG_Grad   = SetUp_Gradient(color1                     , color2                    , branchBG_alpha    , branchBG_alpha    );
        branch_Grad     = SetUp_Gradient(color1                     , color3                    , branch_alpha      , branch_alpha      );

        starsBG_Grad    = SetUp_Gradient(color2                     , color2                    , 1                 , 1                 );
        stars_Grad      = SetUp_Gradient(color1                     , color3                    , 1                 , 1                 );
    }

    private static Gradient SetUp_Gradient(Color startCol, Color endCol, float startAlpha, float endAlpha)
    {
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startCol, 0.0f), new GradientColorKey(endCol, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(startAlpha, 0.0f), new GradientAlphaKey(endAlpha, 1.0f) }
        );
        return grad;
    }
}
