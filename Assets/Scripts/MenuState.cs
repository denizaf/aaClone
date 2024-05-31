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
        UIManager.Instance.ShowStartPanel();
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
