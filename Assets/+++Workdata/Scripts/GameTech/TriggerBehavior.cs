using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBehavior : MonoBehaviour
{
    #region Variables

    //bool for collider is active or not
    private bool setActive = true;
    public UnityEvent triggerEnterEvent;
    public UnityEvent triggerExitEvent;
    private BoxCollider col;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        col.enabled = setActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggerEnterEvent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggerExitEvent?.Invoke();
        }
    }

    public void CollectItemSound()
    {
        MusicManager.Instance.PlayInGameSFX(MusicManager.Instance.collectItem);
    }

    #endregion
}
