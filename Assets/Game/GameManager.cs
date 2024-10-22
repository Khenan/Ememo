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
    private Dictionary<Player, PlayerController> playerControllers = new();
    public Dictionary<Player, PlayerController> PlayerControllers => playerControllers;

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
        GetAllPlayers();
        InitPlayer(PhotonNetwork.LocalPlayer);
        int _sceneCount = allGameSceneToLoad.Count;
        for (int _i = 0; _i < _sceneCount; _i++)
        {
            AsyncOperation _op = SceneManager.LoadSceneAsync(allGameSceneToLoad[_i], LoadSceneMode.Additive);
            string _sceneName = allGameSceneToLoad[_i];
            _op.completed += (op) => AddGameSceneManager(_sceneName);
        }
    }


    internal PlayerController GetLocalPlayerController()
    {
        return playerControllerLocal;
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
    public void GoToFight(List<PlayerController> _playerControllers, FightData _fightData)
    {
        ExplorationManager.I.ClearAllGarbage();
        gameSceneManagers["Exploration"].StopScene();
        ActiveScene("Fight");
        FightManager.I.EnterFight(_playerControllers, _fightData);
    }

    internal void ExitFightMode(List<PlayerController> _playerControllers)
    {
        gameSceneManagers["Fight"].StopScene();
        ActiveScene("Exploration");
        ExplorationSceneManager _manager = gameSceneManagers["Exploration"] as ExplorationSceneManager;
        _manager.StartScene();
        ExplorationManager.I.SetAllPlayersOnExploration(_playerControllers);
    }
    #endregion

    private void InitPlayer(Player _player)
    {
        if (PhotonNetwork.LocalPlayer == _player)
        {
            CreateLocalPlayerController(_player);
        }
        CreatePlayerCharacter(_player);
    }

    private void GetAllPlayers()
    {
        PlayerController[] _playerControllers = FindObjectsOfType<PlayerController>();
        foreach (PlayerController _playerController in _playerControllers)
        {
            if (_playerController.Player != null) playerControllers.Add(_playerController.Player, _playerController);
        }
    }

    private PlayerController GetPlayerController(Player player)
    {
        if (playerControllers.ContainsKey(player)) return playerControllers[player];
        return null;
    }

    private void CreateLocalPlayerController(Player _player)
    {
        if (playerControllerPrefab == null) Debug.LogError("PlayerControllerPrefab is null");
        if (_player.IsLocal)
        {
            int _x = PlayerPrefs.GetInt("PlayerXWorldPosition", 0);
            int _z = PlayerPrefs.GetInt("PlayerYWorldPosition", 0);
            playerControllerLocal = PhotonNetwork.Instantiate(playerControllerPrefab.name, new Vector3(_x, 0, _z), Quaternion.identity).GetComponent<PlayerController>();
            playerControllerLocal.name = PhotonNetwork.NickName;
            playerControllerLocal.SetPlayer(_player);
            playerControllers.Add(_player, playerControllerLocal);
        }
    }

    private void CreatePlayerCharacter(Player _player)
    {
        PlayerController _playerController = GetPlayerController(_player);
        if (_playerController != null) _playerController.InitCharacter();
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
        InitPlayer(_newPlayer);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    internal List<PlayerController> GetPlayers()
    {
        List<PlayerController> _players = new();
        foreach (PlayerController _playerController in playerControllers.Values)
        {
            _players.Add(_playerController);
        }
        return _players;
    }
    #endregion

}