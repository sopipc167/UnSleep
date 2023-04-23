using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFactory
{
    GameObject block;
    GameObject yBomb;
    GameObject bBomb;
    GameObject rBomb;
    GameObject wBomb;
    GameObject Magma;
    BlockFactory(GameObject block,GameObject[] bomb,GameObject Magma)
    {
        this.block = block;
        this.yBomb = bomb[0];
        this.bBomb = bomb[1];
        this.rBomb = bomb[2];
        this.wBomb = bomb[3];
        this.Magma = Magma;
    }
    static void BlockCreate(int[][] map)
    {
        for(int i = 0; i < 16; i++)
        {
            for(int j = 0; j < 12; j++)
            {
                
            }
        }
    }
}