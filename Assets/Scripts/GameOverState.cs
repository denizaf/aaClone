using UnityEngine;

public class GameOverState : IGameState
{
    private GameManager _gameManager;

    public GameOverState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public void Enter()
    {
        Debug.Log("Entering Game Over State.");
        _gameManager.DisplayGameOverUI();
    }

    public void Update()
    {
        if (_gameManager.CheckRestartCondition())
        {
            _gameManager.RestartLevel();
            _gameManager.ChangeState(new PlayingState(_gameManager));
        }else if (_gameManager.CheckBackToMenuCondition())
        {
            _gameManager.ChangeState(new MenuState(_gameManager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Game Over State.");
        _gameManager.HideGameOverUI();
    }
}
