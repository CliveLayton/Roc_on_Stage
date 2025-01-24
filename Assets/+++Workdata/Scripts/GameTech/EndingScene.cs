using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EndingScene : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private SpriteRenderer playerVisual;
    [SerializeField] private Animator faboCage;
    [SerializeField] private Volume volume;
    [SerializeField] private float direction = 0.8f; // Controls the direction of the chromatic change and value
    [SerializeField] private float colorChangeInterval = 0.1f;
    private ChromaticAberration chromaticAberration;
    private LiftGammaGain liftGammaGain;
    private Animator UiAnim;
    private bool inQuestion = false;
    private float chromaticValue = 0.5f;
    private Color currentLiftColor = Color.gray;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        UiAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        //ensure the volume profile is unique and editable
        if (volume == null)
        {
            Debug.LogError("Volume is not assigned!");
            return;
        }
        
        //Get references to the volume effects
        if (volume.profile.TryGet(out ChromaticAberration chromaticEffect))
        {
            chromaticAberration = chromaticEffect;
        }
        
        if (volume.profile.TryGet(out LiftGammaGain liftGammaGainEffect))
        {
            liftGammaGain = liftGammaGainEffect;
        }
        
        //initialize default values
        chromaticAberration.intensity.Override(chromaticValue);
        liftGammaGain.lift.Override(new Vector4(currentLiftColor.r, currentLiftColor.g,currentLiftColor.b,0));
    }

    #endregion

    #region EndingScene Methods

    /// <summary>
    /// disable playerinput, start post processing effects, unlock cursor
    /// </summary>
    public void StartScene()
    {
        inQuestion = true;
        StartCoroutine(ActivateVolume());
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// count up npc Counter, trigger rescue animation
    /// </summary>
    public void RescueFabo()
    {
        GameStateManager.instance.npcCounter += 1;
        inQuestion = false;
        UiAnim.SetTrigger("Rescue");
    }

    /// <summary>
    /// trigger open cage animation
    /// </summary>
    public void OpenCage()
    {
        faboCage.SetTrigger("Open");
    }

    /// <summary>
    /// trigger stay in cage animation
    /// </summary>
    public void StayCage()
    {
        faboCage.SetTrigger("Stay");
    }

    /// <summary>
    /// trigger fadeout animation for the question
    /// </summary>
    public void LeaveFabo()
    {
        inQuestion = false;
        UiAnim.SetTrigger("FadeOut");
    }

    /// <summary>
    /// load the credit scene
    /// </summary>
    public void LoadCreditScene()
    {
        LoadSceneManager.instance.SwitchScene(GameStateManager.creditSceneName, false);
    }

    /// <summary>
    /// show loading screen
    /// </summary>
    public void ShowLoadingScreen()
    {
        LoadSceneManager.instance.ShowLoadingScreen();
    }

    /// <summary>
    /// hide loading screen
    /// </summary>
    public void HideLoadingScreen()
    {
        LoadSceneManager.instance.OpenLoadingScreen();
    }

    /// <summary>
    /// disable player
    /// </summary>
    public void DisablePlayer()
    {
        playerVisual.enabled = false;
    }

    /// <summary>
    /// change values of the post processing effects over time
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateVolume()
    {
        volume.enabled = true;
        float hue = 0f;

        while (inQuestion)
        {
            //Adjust the chromatic aberration value
            if (chromaticAberration != null)
            {
                chromaticValue += direction * Time.deltaTime;
                if (chromaticValue >= 1f || chromaticValue <= 0.5f)
                {
                    direction *= -1; //Reverse direction when hitting bounds
                }
                
                chromaticAberration.intensity.Override(chromaticValue);
            }
            
            //Rotate liftgammagain color hue while keeping saturation and value constant
            if (liftGammaGain != null)
            {
                float saturation = 0.8f;
                float value = 0.8f;
                hue = (hue + 0.01f) % 1f;

                currentLiftColor = Color.HSVToRGB(hue, saturation, value);
                liftGammaGain.lift.Override(new Vector4(currentLiftColor.r, currentLiftColor.g, currentLiftColor.b, 0));
            }

            yield return new WaitForSeconds(colorChangeInterval);
        }
        volume.enabled = false;
    }

    #endregion
}
