using MatrixNavigation;
using System.Collections.Generic;
using UnityEngine;

public class NodeAStar
{
    public FightMapTile Tile { get; private set; } = null;
    //Cout supplementaire dû à la case (Ex : Si boue qui augmente le cout etc)
    private int g;
    //Cout restant pour atteindre le but 
    private int h;
    // Cout total entre ce node et le node but par rapport au cout restant et supplémentaire
    public int f { get; private set; }
    public NodeAStar NodeParent { get; private set; }

    public NodeAStar(FightMapTile _tile, int _g, int _h, NodeAStar _nodeParent = null)
    {
        Tile = _tile;
        this.g = _g;
        this.h = _h;
        this.f = _h + _g;
        NodeParent = _nodeParent;
    }
}
public class TestAStar : MonoBehaviour
{
    private static readonly List<NodeAStar> openPath = new();
    private static readonly List<FightMapTile> closedPath = new();

    private static readonly List<NodeAStar> finishPath = new();
    private static readonly Dictionary<Direction, Vector2> directions = MatrixDirection.matrixDirections;

    internal static List<FightMapTile> SearchPath(FightMapTile _currentTile, FightMapTile _goalTile)
    {
        finishPath.Clear();
        openPath.Clear();
        closedPath.Clear();
        List<FightMapTile> _path = new();

        NodeAStar _startNode = new(_currentTile, 0, GetDistance(_currentTile, _goalTile), null);
        openPath.Add(_startNode);

        while (openPath.Count > 0)
        {
            List<NodeAStar> _localPath = new(openPath);
            foreach (NodeAStar _currentNode in _localPath)
            {
                BrowseNeighboorTiles(_currentNode, _goalTile);
            }
        }
        NodeAStar _nodeLowCost = GetLowCostNode();
        _path = GetPathTile(_nodeLowCost);
        _path.Remove(_currentTile);
        return _path;
    }
    internal static void BrowseNeighboorTiles(NodeAStar _node, FightMapTile _goalTile)
    {
        foreach (KeyValuePair<Direction, Vector2> direction in directions)
        {
            NodeAStar _nodeAdded = AddNode(_node, GetNeighboorTile(_node.Tile, direction.Value), _goalTile);
            if (_nodeAdded != null)
            {
                if (_nodeAdded.Tile == _goalTile)
                {
                    finishPath.Add(_nodeAdded);
                }
            }
        }
        openPath.Remove(_node);
    }

    internal static NodeAStar AddNode(NodeAStar _parentNode, FightMapTile _tile, FightMapTile _goalTile)
    {
        NodeAStar _newNode = null;
        if (_tile != null)
        {
            if (closedPath.Contains(_tile) && _tile != _goalTile)
                return null;

            if (_tile.IsWalkable && !_tile.IsOccupied)
            {
                _newNode = new(_tile, 0, GetDistance(_tile, _goalTile), _parentNode);
                closedPath.Add(_tile);
                openPath.Add(_newNode);
            }
        }
        return _newNode;
    }

    internal static FightMapTile GetNeighboorTile(FightMapTile _tile, Vector2 _direction)
    {
        FightMapTile _neighboorTile = MapManager.I.GetTileByMatrixPosition(_tile.map, _tile.MatrixPositionLocalTemporary + _direction);
        return _neighboorTile;
    }

    internal static NodeAStar GetLowCostNode()
    {
        NodeAStar _lowCostNode = finishPath[0];
        if (finishPath.Count > 0)
        {
            int _minimalCost = GetNodeCost(finishPath[0]);
            foreach (NodeAStar _node in finishPath)
            {
                int _costCurrentNode = GetNodeCost(_node);
                if (_minimalCost > _costCurrentNode)
                {
                    _minimalCost = _costCurrentNode;
                    _lowCostNode = _node;
                }
            }
        }
        return _lowCostNode;
    }

    internal static int GetNodeCost(NodeAStar _node)
    {
        int _totalCost = 0;
        NodeAStar _currentNode = _node;
        while (_currentNode != null)
        {
            _totalCost += _currentNode.f;
            _currentNode = _currentNode.NodeParent;
        }
        return _totalCost;
    }
    internal static List<FightMapTile> GetPathTile(NodeAStar _node)
    {
        List<FightMapTile> _path = new();
        NodeAStar _currentNode = _node;
        while (_currentNode != null)
        {
            _path.Add(_currentNode.Tile);
            _currentNode = _currentNode.NodeParent;
        }
        return _path;
    }
    internal static int GetDistance(FightMapTile _currentTile, FightMapTile _goalTile)
    {
        return (int)Mathf.Abs(_currentTile.MatrixPositionLocalTemporary.x - _goalTile.MatrixPositionLocalTemporary.x) + (int)Mathf.Abs(_currentTile.MatrixPositionLocalTemporary.y - _goalTile.MatrixPositionLocalTemporary.y);
    }
}
