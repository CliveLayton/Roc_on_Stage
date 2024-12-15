using UnityEngine;

public class LoadNewScenes : MonoBehaviour
{
    public void LoadNewScene(int levelNumber)
    {
        switch (levelNumber)
        {
            case 1:
                GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level1SceneName);
                break;
            case 2:
                GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level2SceneName);
                break;
            case 3:
                GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level3SceneName);
                break;
            case 4:
                GameStateManager.instance.LoadNewGameplayScene(GameStateManager.level4SceneName);
                break;
        }
    }
}
