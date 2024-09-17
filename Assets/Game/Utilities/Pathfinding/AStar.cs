using System;
using System.Collections.Generic;
using UnityEngine;

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

public static class AStar
{
    public static List<MapTile> FindPath(List<MapTile> _tiles, MapTile _startTile, MapTile _goalTile, bool _canDiagonal = false)
    {
        List<Node> _openList = new();
        HashSet<MapTile> _closedList = new();

        Node _startNode = new(_startTile, 0, CalculateHeuristic(_startTile, _goalTile));
        _openList.Add(_startNode);

        while (_openList.Count > 0)
        {
            // Sélectionner le Node avec le plus petit fCost
            Node _currentNode = GetLowestFCostNode(_openList);
            if (_currentNode.tile == _goalTile)
            {
                // On a trouvé le chemin, on le retrace
                return RetracePath(_currentNode);
            }

            _openList.Remove(_currentNode);
            _closedList.Add(_currentNode.tile);

            // Explorer les voisins de la tuile actuelle
            foreach (MapTile _neighborTile in GetNeighborTiles(_currentNode.tile, _tiles, _canDiagonal))
            {
                if (_closedList.Contains(_neighborTile)) continue;

                int _tentativeGCost = _currentNode.gCost + CalculateDistanceCost(_currentNode.tile, _neighborTile);
                Node _neighborNode = new(_neighborTile, _tentativeGCost, CalculateHeuristic(_neighborTile, _goalTile), _currentNode);

                // Si un meilleur chemin est trouvé ou si le voisin n'est pas dans la liste ouverte
                if (_tentativeGCost < _neighborNode.gCost || !_openList.Exists(n => n.tile == _neighborTile))
                {
                    _neighborNode.gCost = _tentativeGCost;
                    _neighborNode.fCost = _neighborNode.gCost + _neighborNode.hCost;
                    _neighborNode.parentNode = _currentNode;

                    // Ajouter à la liste ouverte si le voisin n'y est pas déjà
                    if (!_openList.Exists(n => n.tile == _neighborTile))
                    {
                        _openList.Add(_neighborNode);
                    }
                }
            }
        }

        return null; // Aucun chemin trouvé
    }

    private static int CalculateHeuristic(MapTile _tileA, MapTile _tileB)
    {
        int _dx = Math.Abs(_tileA.MatrixPositionLocalTemporary.x - _tileB.MatrixPositionLocalTemporary.x);
        int _dy = Math.Abs(_tileA.MatrixPositionLocalTemporary.y - _tileB.MatrixPositionLocalTemporary.y);
        return _dx + _dy;
    }

    private static int CalculateDistanceCost(MapTile _fromTile, MapTile _toTile)
    {
        int dstX = Mathf.Abs(_fromTile.MatrixPositionLocalTemporary.x - _toTile.MatrixPositionLocalTemporary.x);
        int dstY = Mathf.Abs(_fromTile.MatrixPositionLocalTemporary.y - _toTile.MatrixPositionLocalTemporary.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
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
    private static List<MapTile> GetNeighborTiles(MapTile _currentTile, List<MapTile> _tiles, bool _canDiagonal = false)
    {
        List<MapTile> _neighbors = new();

        bool _right = AddNeighborIfValid(_currentTile, 1, 0, _neighbors, _tiles);   // Droite
        bool _left = AddNeighborIfValid(_currentTile, -1, 0, _neighbors, _tiles);  // Gauche
        bool _top = AddNeighborIfValid(_currentTile, 0, 1, _neighbors, _tiles);   // Haut
        bool _bottom = AddNeighborIfValid(_currentTile, 0, -1, _neighbors, _tiles);  // Bas

        if (_canDiagonal)
        {
            if (_right && _top) AddNeighborIfValid(_currentTile, 1, 1, _neighbors, _tiles); // Haut Droite
            if (_right && _bottom) AddNeighborIfValid(_currentTile, 1, -1, _neighbors, _tiles); // Bas Droite
            if (_left && _top) AddNeighborIfValid(_currentTile, -1, 1, _neighbors, _tiles); // Haut Gauche
            if (_left && _bottom) AddNeighborIfValid(_currentTile, -1, -1, _neighbors, _tiles); // Bas Gauche
        }
        return _neighbors;
    }

    // Ajoute une tuile voisine si elle est valide
    private static bool AddNeighborIfValid(MapTile _currentTile, int _offsetX, int _offsetY, List<MapTile> _neighbors, List<MapTile> _tiles)
    {
        bool _valid = false;
        Vector2 _neighborPos = new Vector2(
            _currentTile.MatrixPositionLocalTemporary.x + _offsetX,
            _currentTile.MatrixPositionLocalTemporary.y + _offsetY
        );
        MapTile _neighborTile = GetTileByMatrixPosition(_tiles, _neighborPos);
        if (_neighborTile != null && _neighborTile.IsWalkable && !_neighborTile.IsOccupied)
        {
            _neighbors.Add(_neighborTile);
            _valid = true;
        }
        return _valid;
    }

    private static MapTile GetTileByMatrixPosition(List<MapTile> _tiles, Vector2 _mPos)
    {
        return _tiles.Find(_t => _t.MatrixPositionLocalTemporary == _mPos);
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