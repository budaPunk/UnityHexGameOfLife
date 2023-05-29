using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Tilemap nextStateTilemap;
    [SerializeField] private Tilemap prevStateTilemap;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;
    [SerializeField] private Pattern pattern;
    [SerializeField] private int turnPerSecond = 1;

    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;

    private void Awake(){
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
    }
 
    private void Start() {
        SetPattern(pattern);
    }

    private void SetPattern(Pattern pattern) {
        Clear();

        for(int i = 0; i < pattern.cells.Length; i++) {
            Vector3Int aliveCellCoordinates = pattern.cells[i];
            prevStateTilemap.SetTile(aliveCellCoordinates, aliveTile);
            aliveCells.Add(aliveCellCoordinates);
        }
    }

    private void Clear() {
        aliveCells.Clear();
        cellsToCheck.Clear();
        prevStateTilemap.ClearAllTiles();
        nextStateTilemap.ClearAllTiles();
    }

    private void OnEnable() {
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate() {
        var interval = new WaitForSeconds(1f / turnPerSecond);
        while (enabled) {
            UpdateState();
            yield return interval;
        }
    }

    private void UpdateState() {
        cellsToCheck.Clear();

        foreach (Vector3Int cell in aliveCells) {
            cellsToCheck.Add(cell);
            Vector3Int[] neighbors = HexCoordinateConverter.GetOffsetNeighbors(cell);
            foreach (Vector3Int neighbor in neighbors) {
                cellsToCheck.Add(neighbor);
            }
        }

        foreach (Vector3Int cell in cellsToCheck) {
            int aliveNeighbors = CountNeighbors(cell);
            bool alive = isAlive(cell);
            if (aliveNeighbors < 2 || 2 < aliveNeighbors) {
                if (alive) {
                    nextStateTilemap.SetTile(cell, deadTile);
                    aliveCells.Remove(cell);
                }
            } else if (aliveNeighbors == 2) {
                if (alive) {
                    nextStateTilemap.SetTile(cell, aliveTile);
                } else {
                    nextStateTilemap.SetTile(cell, aliveTile);
                    aliveCells.Add(cell);
                }
            }
        }
        Tilemap temp = prevStateTilemap;
        prevStateTilemap = nextStateTilemap;
        nextStateTilemap = temp;
        nextStateTilemap.ClearAllTiles();
    }

    private int CountNeighbors(Vector3Int cell) {
        int aliveNeighbors = 0;
        Vector3Int[] neighbors = HexCoordinateConverter.GetOffsetNeighbors(cell);
        foreach (Vector3Int neighbor in neighbors) {
            if (isAlive(neighbor)) {
                aliveNeighbors++;
            }
        }
        return aliveNeighbors;
    }

    private bool isAlive(Vector3Int cell) {
        return prevStateTilemap.GetTile(cell) == aliveTile;
    }
}
