using UnityEngine;

public class MenuState : IGameState
{
    private GameManager gameManager;

    public MenuState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Entering Menu State");
        int currentLevel = LevelManager.Instance.GetCurrentLevelIndex() + 1;
        UIManager.Instance.UpdateLevelButton(currentLevel);
        
        UIManager.Instance.ShowStartPanel();
        UIManager.Instance.HideGameOverPanel();
        UIManager.Instance.HideLevelCompletedPanel();
    }

    public void Update()
    {
        if (UIManager.Instance.IsPlayButtonPressed())
        {
            gameManager.ChangeState(new PlayingState(gameManager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Menu State");
        UIManager.Instance.HideStartPanel();
    }
}
