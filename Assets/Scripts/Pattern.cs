using UnityEngine;

[CreateAssetMenu(menuName = "Game of Life/Pattern")]
public class Pattern : ScriptableObject
{
    public Vector3Int[] cells;
    public Vector3Int GetCenter()
    {
        if (cells == null || cells.Length == 0) {
            return Vector3Int.zero;
        }
        Vector3Int min = Vector3Int.zero;
        Vector3Int max = Vector3Int.zero;

        for (int i = 0; i < cells.Length; i++) {
            Vector3Int cell = cells[i];
            min.x = Mathf.Min(min.x, cell.x);
            min.y = Mathf.Min(min.y, cell.y);
            max.x = Mathf.Max(max.x, cell.x);
            max.y = Mathf.Max(max.y, cell.y);
        }

        return (min + max) / 2;
    }
    
}
