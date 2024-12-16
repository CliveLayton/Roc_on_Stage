using System;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    #region Variables

    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameStateManager.instance.LoadNewGameplayScene(sceneName);
        }
    }

    public void SwitchScene(string sceneToLoad)
    {
        GameStateManager.instance.LoadNewGameplayScene(sceneToLoad);
    }

    #endregion
}
