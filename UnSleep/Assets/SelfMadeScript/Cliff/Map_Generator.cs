using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map_Generator : MonoBehaviour //처음 타일맵 생성할때 설정을 해주는 스크립트(미완인 부분이 있음)
{
    // Start is called before the first frame update
    public Tilemap tiles;
    public Tile t;
    Tile[,] Tarray = new Tile[5,5];
    void Start() //시작함과 동시에 지뢰가 깔린 타일을 빨간색으로 표시해준다.
    {
        tiles.SetTileFlags(new Vector3Int(1,1,0), TileFlags.None);
        tiles.SetColor(new Vector3Int(1, 1, 0), (Color.red));
    }
}
