using UnityEngine;

public class LevelCompletedState : IGameState
{
    private GameManager _gameManager;

    public LevelCompletedState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public void Enter()
    {
        Debug.Log("Entering Level Completed State.");
        _gameManager.DisplayLevelCompletedUI();
    }
    
    public void Update()
    {
        if (_gameManager.CheckNextLevelCondition())
        {
            _gameManager.LoadNextLevel();
            _gameManager.ChangeState(new PlayingState(_gameManager));
        }else if (_gameManager.CheckBackToMenuCondition())
        {
            _gameManager.ChangeState(new MenuState(_gameManager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Level Completed State.");
        _gameManager.HideLevelCompletedUI();
    }
}
