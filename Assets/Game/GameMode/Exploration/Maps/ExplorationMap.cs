using System.Collections.Generic;
using UnityEngine;

public class ExplorationMap : Map
{
    [SerializeField] private int AreaId = 0;
    [SerializeField] private FightData fightDataPrefab = null;
    private List<FightData> currentFightDatas = new();
    public List<FightData> CurrentFightDatas => currentFightDatas;

    private void Start()
    {
        CreateFightDataOnRandomFreeTile();
    }
    private void CreateFightDataOnRandomFreeTile()
    {
        if(fightDataPrefab == null)
        {
            Debug.LogError("FightData prefab is null.");
            return;
        }

        List<ExplorationMapTile> _freeTiles = GetFreeTiles();
        if (_freeTiles.Count > 0)
        {
            ExplorationMapTile _tile = _freeTiles[Random.Range(0, _freeTiles.Count)];
            FightData _fightData = Instantiate(fightDataPrefab, _tile.transform.position, Quaternion.identity);
            _fightData.matrixPositionWorld = _tile.MatrixPositionWorld;
            _fightData.areaId = AreaId;
            _fightData.charactersToInstantiate = new();
            for (int _i = 0; _i < 3; _i++)
            {
                Character _character = CharacterDatabaseManager.I.GetCharacterById(Random.Range(0, CharacterDatabaseManager.I.characters.Count));
                _fightData.charactersToInstantiate.Add(new FightCharacter(1, _character));
            }
            currentFightDatas.Add(_fightData);
            ExplorationManager.I.AddGarbage(_fightData.gameObject);
        }
    }

    private List<ExplorationMapTile> GetFreeTiles()
    {
        List<ExplorationMapTile> _freeTiles = new();
        foreach (ExplorationMapTile _tile in mapTiles)
        {
            if (_tile.IsWalkable && _tile.characters.Count == 0) _freeTiles.Add(_tile);
        }
        return _freeTiles;
    }
}