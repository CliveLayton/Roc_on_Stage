using System;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    #region Variables

    public enum NextScene
    {
        Forest,
        Town,
        Castle,
        Boss
    }

    public NextScene scene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (scene)
            {
                case NextScene.Forest:
                    GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level1SceneName);
                    MusicManager.Instance.PlayMusic(MusicManager.Instance.forestMusic, 0.1f);
                    break;
                case NextScene.Town:
                    GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level2SceneName);
                    MusicManager.Instance.PlayMusic(MusicManager.Instance.townMusic, 0.1f);
                    break;
                case NextScene.Castle:
                    GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level3SceneName);
                    MusicManager.Instance.PlayMusic(MusicManager.Instance.castleMusic, 0.1f);
                    break;
                case NextScene.Boss:
                    GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level4SceneName);
                    MusicManager.Instance.PlayMusic(MusicManager.Instance.castleMusic, 0.1f);
                    break;
            }
        }
    }

    public void SwitchScene(string sceneToLoad)
    {
        GameStateManager.instance.LoadNewGameplayScene(sceneToLoad);
    }

    #endregion
}
