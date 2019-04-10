using UnityEngine;
using System.Collections;

public class GridBase  {
    public GridBase LastGrid;
    int Grid_X = 0;
    int Grid_Y = 0;
    public int Value_G = 0;
    public int Value_H = 0;
    int Value_F = 0;
    public bool CanMove = true;
    public void CalcF()
    {
        this.Value_F = this.Value_G + this.Value_H;
    }
    GameObject obj;
    public GridBase(int X, int Y)
    {
        obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        this.Grid_X = X;
        this.Grid_Y = Y;
        obj.transform.position = new Vector3(Grid_X, Grid_Y, 0);
        obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    public void SetColor(Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }
    public bool Equals(GridBase other)
    {
        if (this.Grid_X == other.Grid_X && this.Grid_Y == other.Grid_Y)
        {
            return true;

        }
        return false;
    }

}
