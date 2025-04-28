using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CanvasGroup panelWin;
    [SerializeField] CanvasGroup panelLose;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    private void OnEnable()
    {
        PlaneController.OnPlayerDeadth += Lose;
        PlaneController.OnPlayerGetPointsForWin += Win;
    }

    private void OnDisable()
    {
        PlaneController.OnPlayerDeadth -= Lose;
        PlaneController.OnPlayerGetPointsForWin -= Win;
    }

    void Win()
    {
        Time.timeScale = 0;
        panelWin.alpha = 1;
        panelWin.interactable = true;
        panelWin.blocksRaycasts = true;
    }

    void Lose()
    {
        Time.timeScale = 0;
        panelLose.alpha = 1;
        panelLose.interactable = true;
        panelLose.blocksRaycasts = true;
    }


}
