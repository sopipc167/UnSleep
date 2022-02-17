using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;

	public HexCell[] cells;
	public HexMesh hexMesh;

	void Awake () {
	    hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
	}
	void Start () {
		hexMesh.Triangulate(cells);
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = z * (HexMetrics.outerRadius * 1.5f);
		position.z = 0f;

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.position = position;
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
	}
}