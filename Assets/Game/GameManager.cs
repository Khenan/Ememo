using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonPunCallbacks<GameManager>
{
    private Dictionary<string, GameSceneManager> gameSceneManagers = new();
    [SerializeField] private List<string> loadedGameSceneToStart = new();
    [SerializeField] private List<string> allGameSceneToLoad = new();

    [SerializeField] private PlayerController playerControllerPrefab;
    private PlayerController playerControllerLocal;
    public PlayerController PlayerControllerLocal => playerControllerLocal;
    private List<PlayerController> playerControllers = new();
    public List<PlayerController> PlayerControllers => playerControllers;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        LaunchGame();
    }

    #region GameManager Methods
    public void LaunchGame()
    {
        int _x = PlayerPrefs.GetInt("PlayerXWorldPosition", 0);
        int _z = PlayerPrefs.GetInt("PlayerYWorldPosition", 0);
        if (playerControllerPrefab != null) playerControllerLocal = PhotonNetwork.Instantiate(playerControllerPrefab.name, new Vector3(_x, 0, _z), Quaternion.identity).GetComponent<PlayerController>();
        Debug.Log("PlayerControllerLocal: " + playerControllerLocal.name, playerControllerLocal);
        playerControllerLocal.name = PhotonNetwork.NickName;
        int _sceneCount = allGameSceneToLoad.Count;
        for (int _i = 0; _i < _sceneCount; _i++)
        {
            AsyncOperation _op = SceneManager.LoadSceneAsync(allGameSceneToLoad[_i], LoadSceneMode.Additive);
            string _sceneName = allGameSceneToLoad[_i];
            _op.completed += (op) => AddGameSceneManager(_sceneName);
        }
    }

    private void ActiveScene(string _sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
    }

    public void AddGameSceneManager(string _name)
    {
        Scene _scene = SceneManager.GetSceneByName(_name);
        gameSceneManagers.Add(_scene.name, _scene.GetRootGameObjects()[0].GetComponent<GameSceneManager>());
        if (loadedGameSceneToStart.Contains(_scene.name)) gameSceneManagers[_scene.name].StartScene();
    }

    #region Fight
    public void GoToFight(FightData _fightData)
    {
        ExplorationManager.I.ClearAllGarbage();
        gameSceneManagers["Exploration"].StopScene();
        ActiveScene("Fight");
        FightSceneManager _manager = gameSceneManagers["Fight"] as FightSceneManager;
        _manager.StartScene(_fightData);
    }

    internal void ExitFightMode()
    {
        gameSceneManagers["Fight"].StopScene();
        ActiveScene("Exploration");
        ExplorationSceneManager _manager = gameSceneManagers["Exploration"] as ExplorationSceneManager;
        _manager.StartScene();
    }
    #endregion

    public void SetPlayerController(PlayerController _playerController)
    {
        playerControllerLocal = _playerController;
    }

    internal void AddPlayerController(PlayerController _playerController)
    {
        if (_playerController != null)
        {
            playerControllers.Add(_playerController);
            // _playerController.Character.SetCharacterName(_playerController.PlayerName);
        }
    }
    #endregion

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Player _newPlayer)
    {
        Debug.Log("A new player has joined the room");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MasterClient is loading the game");
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

}