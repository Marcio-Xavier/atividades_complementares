using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FGameState
{
    public enum EState { Menu, Jogo, GameOver };
    private EState _currentEState = EState.Menu;
    private FState _currentState = null;

    [SerializeField]
    public bool bOnMenu = true;
    [SerializeField]
    public bool bIsDeath = false;


    //private FState[] states = new FMenu[5];

    public FGameState()
    {
        _currentEState = EState.Menu;
        _currentState = new FMenu(this);
    }

    public void OnUpdate()
    {
        //EState frameState = states[(int)_currentEState].OnUpdate();

        EState frameState = _currentState.OnUpdate();
        if (frameState != _currentEState)
        {
            _currentState.OnEnd();
            // Troca de estado
            _currentEState = frameState;

            switch (_currentEState)
            {
                case EState.Menu:
                    _currentState = new FMenu(this);
                    break;
                case EState.Jogo:
                    _currentState = new FGame(this);
                    break;
                case EState.GameOver:
                    _currentState = new FGameOver(this);
                    break;
                default:
                    break;
            }
            _currentState.OnBegin();
        }
    }
}

public abstract class FState
{
    protected FGameState.EState _nextState;
    protected FGameState _gameState;

    public FState(FGameState inGameState) { _gameState = inGameState; }

    public abstract void OnBegin();
    public abstract FGameState.EState OnUpdate();
    public abstract void OnEnd();
}

public class FMenu : FState
{

    

    public FMenu(FGameState inGameState) : base(inGameState) { }

    public override void OnBegin()
    {
        _nextState = FGameState.EState.Menu;
        SceneManager.LoadScene("Menu");
        //_deltaTime = 0.0f;
    }

    public override FGameState.EState OnUpdate()
    {        
        if(!_gameState.bOnMenu)
            _nextState = FGameState.EState.Jogo;
        // Defino qual estado estou...
        Debug.Log("Estou no Menu");

        return _nextState;
    }

    public override void OnEnd()
    {
        GameManager.GetInstance().SetMenu(true);
    }
}

public class FGame : FState
{

    public FGame(FGameState inGameState) : base(inGameState) { }

    public override void OnBegin()
    {
        _nextState = FGameState.EState.Jogo;
        SceneManager.LoadScene("Demo");
    }

    public override FGameState.EState OnUpdate()
    {
        if (_gameState.bIsDeath)
            _nextState = FGameState.EState.GameOver;


        Debug.Log("Estou no Jogo");
        return _nextState;
    }

    public override void OnEnd()
    {
        GameManager.GetInstance().SetDeath(false);
    }
}

public class FGameOver : FState
{

    private float _targetTime = 3.0f;
    private float _deltaTime = 0.0f;

    public FGameOver(FGameState inGameState) : base(inGameState) { }

    public override void OnBegin()
    {
        _deltaTime = 0.0f;
        _nextState = FGameState.EState.GameOver;
        SceneManager.LoadScene("GameOver");
    }

    public override FGameState.EState OnUpdate()
    {
        _deltaTime += Time.deltaTime;
        if (_deltaTime >= _targetTime)
            _nextState = FGameState.EState.Menu;


        Debug.Log("Estou no GameOver");
        return _nextState;
    }

    public override void OnEnd()
    {

    }
}






