using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public Map map;
    public Vector2 MatrixPosition { get; set; }
    public virtual bool IsWalkable { get; set; }
    public virtual bool IsOccupied { get; set; }

    public void SetMap(Map _map)
    {
        map = _map;
    }
}

public class Node
{
    public MapTile tile;
    public int fCost;
    public int gCost;
    public int hCost;
    public Node parentNode;

    public Node(MapTile _tile, int _gCost, int _hCost, Node _parentNode = null)
    {
        tile = _tile;
        gCost = _gCost;
        hCost = _hCost;
        fCost = _gCost + _hCost;
        parentNode = _parentNode;
    }
}

public class Map : MonoBehaviour
{
    public Vector2 matrixPosition;
    public Vector2 mapSize;
    public List<MapTile> mapTiles = new();
    public List<MapTile> GetTileOfYPosition(int _y)
    {
        return mapTiles.FindAll(_t => _t.MatrixPosition.y == _y).OrderBy(_t => _t.MatrixPosition.x).ToList();
    }
}

public static class MapSizeData
{
    public static int SIZE = 14;
}

public class ConcatenatorMapList
{
    public List<MapTile> ConcatenateMaps(List<Map> _mapList, MapTile _currentTile, MapTile _targetTile)
    {
        List<MapTile> _concatenatedMap = new();
        List<Map> _orderedMapList = OrderByMatrixPosition(_mapList);
        Vector2 _mapSize = GetMapDiff(_currentTile, _targetTile) * MapSizeData.SIZE;

        // We take the range of tile lists from each map and add them to the concatenated list
        for (int _y = 0; _y < _mapSize.y; _y++)
        {
            for (int _index = 0; _index < _orderedMapList.Count; _index++)
            {
                if (_orderedMapList[_index].matrixPosition.y == _y)
                {
                    List<MapTile> _tiles = _orderedMapList[_index].GetTileOfYPosition(_y);
                    _concatenatedMap.AddRange(_tiles);
                }
            }
        }

        // Set the matrix position of each tile in the concatenated map
        for (int _i = 0; _i < _concatenatedMap.Count; _i++)
        {
            _concatenatedMap[_i].MatrixPosition = new Vector2(_i % (int)_mapSize.x, _i / (int)_mapSize.x);
        }

        return _concatenatedMap;
    }

    private List<Map> OrderByMatrixPosition(List<Map> _mapList)
    {
        return _mapList.OrderBy(_m => _m.matrixPosition.y).ThenBy(_m => _m.matrixPosition.x).ToList();
    }

    private Vector2 GetMapDiff(MapTile _currentTile, MapTile _targetTile)
    {
        Vector2 _mapDiff = new Vector2();
        _mapDiff.x = Math.Abs((int)_targetTile.map.matrixPosition.x - (int)_currentTile.map.matrixPosition.x);
        _mapDiff.y = Math.Abs((int)_targetTile.map.matrixPosition.y - (int)_currentTile.map.matrixPosition.y);
        return _mapDiff;
    }
}

public static class AStar
{
    private static List<Node> openList = new();
    private static HashSet<MapTile> closedList = new();

    public static List<MapTile> FindPath(List<MapTile> _tiles, MapTile _startTile, MapTile _goalTile)
    {
        Debug.Log(_tiles.Count);
        openList.Clear();
        closedList.Clear();

        Node _startNode = new(_startTile, 0, CalculateHeuristic(_startTile, _goalTile));
        openList.Add(_startNode);

        while (openList.Count > 0)
        {
            // Sélectionner le Node avec le plus petit fCost
            Node _currentNode = GetLowestFCostNode(openList);
            if (_currentNode.tile == _goalTile)
            {
                // On a trouvé le chemin, on le retrace
                return RetracePath(_currentNode);
            }

            openList.Remove(_currentNode);
            closedList.Add(_currentNode.tile);

            // Explorer les voisins de la tuile actuelle
            foreach (MapTile _neighborTile in GetNeighborTiles(_currentNode.tile, _tiles))
            {
                if (closedList.Contains(_neighborTile)) continue;

                int _tentativeGCost = _currentNode.gCost + CalculateDistanceCost(_currentNode.tile, _neighborTile);
                Node _neighborNode = new(_neighborTile, _tentativeGCost, CalculateHeuristic(_neighborTile, _goalTile), _currentNode);

                // Si un meilleur chemin est trouvé ou si le voisin n'est pas dans la liste ouverte
                if (_tentativeGCost < _neighborNode.gCost || !openList.Exists(n => n.tile == _neighborTile))
                {
                    _neighborNode.gCost = _tentativeGCost;
                    _neighborNode.fCost = _neighborNode.gCost + _neighborNode.hCost;
                    _neighborNode.parentNode = _currentNode;

                    // Ajouter à la liste ouverte si le voisin n'y est pas déjà
                    if (!openList.Exists(n => n.tile == _neighborTile))
                    {
                        openList.Add(_neighborNode);
                    }
                }
            }
        }

        return null; // Aucun chemin trouvé
    }

    private static int CalculateHeuristic(MapTile _tileA, MapTile _tileB)
    {
        int _dx = Math.Abs((int)_tileA.MatrixPosition.x - (int)_tileB.MatrixPosition.x);
        int _dy = Math.Abs((int)_tileA.MatrixPosition.y - (int)_tileB.MatrixPosition.y);
        return _dx + _dy;
    }

    private static int CalculateDistanceCost(MapTile _fromTile, MapTile _toTile)
    {
        return 1; // C'est 1 car les tuiles sont adjacentes et qu'on ne fait pas de déplacement en diagonale
    }

    // Sélectionner le Node avec le plus petit fCost dans la liste ouverte
    private static Node GetLowestFCostNode(List<Node> _nodes)
    {
        Node _lowestFCostNode = _nodes[0];
        for (int i = 1; i < _nodes.Count; i++)
        {
            if (_nodes[i].fCost < _lowestFCostNode.fCost)
            {
                _lowestFCostNode = _nodes[i];
            }
        }
        return _lowestFCostNode;
    }

    // Récupère les voisins adjacents (haut, bas, gauche, droite)
    private static List<MapTile> GetNeighborTiles(MapTile _currentTile, List<MapTile> _tiles)
    {
        List<MapTile> _neighbors = new();

        AddNeighborIfValid(_currentTile, 1, 0, _neighbors, _tiles);   // Droite
        AddNeighborIfValid(_currentTile, -1, 0, _neighbors, _tiles);  // Gauche
        AddNeighborIfValid(_currentTile, 0, 1, _neighbors, _tiles);   // Haut
        AddNeighborIfValid(_currentTile, 0, -1, _neighbors, _tiles);  // Bas

        return _neighbors;
    }

    // Ajoute une tuile voisine si elle est valide
    private static void AddNeighborIfValid(MapTile _currentTile, int _offsetX, int _offsetY, List<MapTile> _neighbors, List<MapTile> _tiles)
    {
        Vector2 _neighborPos = new Vector2(
            _currentTile.MatrixPosition.x + _offsetX,
            _currentTile.MatrixPosition.y + _offsetY
        );
        MapTile _neighborTile = GetTileByMatrixPosition(_tiles, _neighborPos);
        if (_neighborTile != null && _neighborTile.IsWalkable && !_neighborTile.IsOccupied)
        {
            _neighbors.Add(_neighborTile);
        }
    }

    private static MapTile GetTileByMatrixPosition(List<MapTile> _tiles, Vector2 _mPos)
    {
        return _tiles.Find(_t => _t.MatrixPosition == _mPos);
    }

    // Retrace le chemin en partant du Node final jusqu'au Node de départ
    private static List<MapTile> RetracePath(Node _endNode)
    {
        List<MapTile> _path = new();
        Node _currentNode = _endNode;

        // Remonter jusqu'au Node de départ en utilisant les parents
        while (_currentNode.parentNode != null)
        {
            _path.Add(_currentNode.tile);
            _currentNode = _currentNode.parentNode;
        }

        _path.Reverse(); // On renverse le chemin pour obtenir le bon ordre
        return _path;
    }
}
