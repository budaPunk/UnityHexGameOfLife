using UnityEngine;

public class HexCoordinateConverter {
    // 0: right, 1: up-right, 2: up-left, 3: left, 4: down-left, 5: down-right
    public static Vector3Int[] cubeDirections = {
        new Vector3Int(1, 0, -1), new Vector3Int(0, 1, -1), new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 0, 1), new Vector3Int(0, -1, 1), new Vector3Int(1, -1, 0)
    };

    public static Vector3Int cube2offset (Vector3Int cubeCoordinates) {
        if (cubeCoordinates == null) {
            Debug.LogError("Invalid cube coordinates: " + cubeCoordinates);
        } else if (cubeCoordinates.x + cubeCoordinates.y + cubeCoordinates.z != 0) {
            Debug.LogError("Invalid cube coordinates: " + cubeCoordinates);
        }
        int col = cubeCoordinates.x + (cubeCoordinates.y - (cubeCoordinates.y & 1)) / 2;
        int row = cubeCoordinates.y;
        return new Vector3Int(col, row, 0);
    }

    public static Vector3Int offset2cube (Vector3Int offsetCoordinates) {
        if (offsetCoordinates == null) {
            Debug.LogError("Invalid offset coordinates: " + offsetCoordinates);
        }
        int x = offsetCoordinates.x - (offsetCoordinates.y - (offsetCoordinates.y & 1)) / 2;
        int y = offsetCoordinates.y;
        int z = -(x + y);
        return new Vector3Int(x, y, z);
    }

    public static Vector3Int[] GetCubeNeighbors (Vector3Int cubeCoordinates) {
        Vector3Int[] cubeNeighbors = new Vector3Int[6];
        for (int i = 0; i < 6; i++) {
            cubeNeighbors[i] = cubeCoordinates + cubeDirections[i];
        }
        return cubeNeighbors;
    }

    public static Vector3Int[] GetOffsetNeighbors (Vector3Int offsetCoordinates) {
        Vector3Int[] offsetNeighbors = new Vector3Int[6];
        for (int i = 0; i < 6; i++) {
            offsetNeighbors[i] = cube2offset(offset2cube(offsetCoordinates) + cubeDirections[i]);
        }
        return offsetNeighbors;
    }

}
