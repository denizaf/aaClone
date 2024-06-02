using UnityEngine;

public class PlayingState : IGameState
{
    private GameManager _gameManager;

    public PlayingState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Entering Playing State.");
        _gameManager.EnablePinThrowing();
        _gameManager.ResetGameState();
        
        UIManager.Instance.HideStartPanel();
        UIManager.Instance.HideGameOverPanel();
        UIManager.Instance.HideLevelCompletedPanel();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            PinManager.Instance.ThrowPin();
        }
        
        if (_gameManager.CheckGameOverCondition())
        {
            _gameManager.ChangeState(new GameOverState(_gameManager));
        }

        if (_gameManager.CheckLevelCompletedCondition())
        {
            _gameManager.ChangeState(new LevelCompletedState(_gameManager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Playing State.");
        _gameManager.DisablePinThrowing();
    }
}
