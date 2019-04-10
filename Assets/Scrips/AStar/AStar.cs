using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

    public int GridLong = 20;
    public int GridWide = 20;

    private int Start_X = 5;
    private int Start_Y = 5;
    private int End_X = 19;
    private int End_Y = 15;
    private int STEP = 10;
    private int OBLIQUE = 14;
    private bool IsFindPath;
    GridBase[,] GradAttr;
    List<GridBase> OpenList = new List<GridBase>();
    List<GridBase> CloseList = new List<GridBase>();
    void Start()
    { 
        GradAttr = new GridBase[GridLong, GridWide];
        UpdateData();
    }

    void Update()
    {

    }

    private void UpdateData()
    {
        IsFindPath = false;
        OpenList.Clear();
        CloseList.Clear();

        for (int i = 0; i < GridLong; i++)
        {
            for (int j = 0; j < GridWide; j++)
            {
                //int  G = Mathf.Abs(Start_X - i)+Mathf.Abs(Start_Y-j);
                //int H = Mathf.Abs(End_X - i) + Mathf.Abs(End_Y - j);

                GradAttr[i, j] = new GridBase(i, j);
                if (i == 9 && j == 9)
                {
                    GradAttr[i, j].CanMove = false;
                }
                //GradAttr[i, j].CalcF();
            }
        }
        GradAttr[Start_X, Start_Y].SetColor(Color.white);
        GradAttr[End_X, End_Y].SetColor(Color.blue);
        OpenList.Add(GradAttr[Start_X, Start_Y]);
        StartFind(GradAttr[Start_X, Start_Y]);
        Debug.LogError("sÊÇ·ñÑ°µ½Â·¾¶" + IsFindPath);


        //ShowPath(GradAttr[End_X, End_Y]);

    }
    void StartFind(GridBase nowGrid)
    {
        if (nowGrid.Equals(GradAttr[End_X, End_Y]))
        {
            IsFindPath = true;
            return;
        }
        if (!OpenList.Contains(nowGrid) && !CloseList.Contains(nowGrid))
        {
            
        }
        else if (OpenList.Contains(nowGrid))
        {

        }
        else if (CloseList.Contains(nowGrid))
        {

        }
        OpenList.Remove(nowGrid);

    }
    //List<GridBase> GetRoundGrid(GridBase tmpGrid)
    //{

    //}

}
