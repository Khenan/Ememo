using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonPunCallbacks<GameManager>
{
    private Dictionary<string, GameSceneManager> _gameSceneManagers = new();
    [SerializeField] private List<string> _loadedGameSceneToStart = new();
    [SerializeField] private List<string> _allGameSceneToLoad = new();

    [SerializeField] private PlayerController _playerControllerPrefab;
    private PlayerController _playerController;
    public PlayerController PlayerController => _playerController;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start() {
        LaunchGame();
    }

    #region GameManager Methods
    public void LaunchGame()
    {
        int _x = PlayerPrefs.GetInt("PlayerXWorldPosition", 0);
        int _z = PlayerPrefs.GetInt("PlayerYWorldPosition", 0);
        if(_playerControllerPrefab != null) _playerController = PhotonNetwork.Instantiate(_playerControllerPrefab.name, new Vector3(_x, 0, _z), Quaternion.identity).GetComponent<PlayerController>();
        int _sceneCount = _allGameSceneToLoad.Count;
        for (int _i = 0; _i < _sceneCount; _i++)
        {
            AsyncOperation _op = SceneManager.LoadSceneAsync(_allGameSceneToLoad[_i], LoadSceneMode.Additive);
            string _sceneName = _allGameSceneToLoad[_i];
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
        _gameSceneManagers.Add(_scene.name, _scene.GetRootGameObjects()[0].GetComponent<GameSceneManager>());
        if (_loadedGameSceneToStart.Contains(_scene.name)) _gameSceneManagers[_scene.name].StartScene();
    }

    #region Fight
    public void GoToFight(FightData _fightData)
    {
        ExplorationManager.I.ClearGarbage();
        _gameSceneManagers["Exploration"].StopScene();
        ActiveScene("Fight");
        FightSceneManager _manager = _gameSceneManagers["Fight"] as FightSceneManager;
        _manager.StartScene(_fightData);
    }

    internal void ExitFightMode()
    {
        _gameSceneManagers["Fight"].StopScene();
        ActiveScene("Exploration");
        ExplorationSceneManager _manager = _gameSceneManagers["Exploration"] as ExplorationSceneManager;
        _manager.StartScene();
    }
    #endregion

    public void SetPlayerController(PlayerController _playerController)
    {
        this._playerController = _playerController;
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
        if(PhotonNetwork.IsMasterClient)
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