using UnityEngine;
using UnityEngine.UI;

public class HeartBarUI : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite brokenHeart;

    private PlayerController player;

    #endregion

    #region HeartBarUI Methods

    /// <summary>
    /// set the heart images to the current health
    /// </summary>
    /// <param name="health"></param>
    public void UpdateHearts(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = brokenHeart;
            }
        }
    }

    #endregion
}
