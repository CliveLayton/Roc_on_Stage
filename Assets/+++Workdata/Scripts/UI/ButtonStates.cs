using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonStates : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    #region Variables

    [SerializeField] private GameObject blockImage;

    private Button button;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    
    /// <summary>
    /// plays button press sound
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        MusicManager.Instance.PlayUISFX(MusicManager.Instance.buttonPress);
        if (GameStateManager.instance.currentState == GameStateManager.GameState.InMainMenu)
        {
            StartCoroutine(SetInteractable());
        }
        button.onClick?.Invoke();
    }
    
    /// <summary>
    /// plays button hover sound
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        MusicManager.Instance.PlayUISFX(MusicManager.Instance.buttonHover);
    }

    #endregion

    #region ButtonStates Methods

    /// <summary>
    /// deactivate interactable of button and set it back active after 0.2 seconds
    /// </summary>
    private IEnumerator SetInteractable()
    {
        blockImage.SetActive(true);

        yield return new WaitForSeconds(1f);

        blockImage.SetActive(false);
    }

    #endregion
    
}
