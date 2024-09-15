using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Dictionary<string, GameSceneManager> _gameSceneManagers = new();
    [SerializeField] private List<string> _gameSceneToStart = new();

    private void Start()
    {
        int _sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int _i = 1; _i < _sceneCount; _i++)
        {
            AsyncOperation _op = SceneManager.LoadSceneAsync(_i, LoadSceneMode.Additive);
            int _id = _i;
            _op.completed += (op) => AddGameSceneManager(_id);
        }
    }

    private void ActiveScene(string _sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
    }

    public void AddGameSceneManager(int _id)
    {
        Scene _scene = SceneManager.GetSceneAt(_id);
        _gameSceneManagers.Add(_scene.name, _scene.GetRootGameObjects()[0].GetComponent<GameSceneManager>());
        if(_gameSceneToStart.Contains(_scene.name)) _gameSceneManagers[_scene.name].StartScene();
    }

    public void LaunchFightGameMode(FightData _fightData)
    {
        _gameSceneManagers["Exploration"].StopScene();
        ActiveScene("Fight");
        FightSceneManager _manager = _gameSceneManagers["Fight"] as FightSceneManager;
        _manager.StartScene(_fightData);
    }

    internal void ExitFightMode()
    {
        _gameSceneManagers["Fight"].StopScene();
        ActiveScene("Exploration");
        _gameSceneManagers["Exploration"].StartScene();
    }
}