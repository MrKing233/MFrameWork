using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MFrameWork.Asset;
public class test2 :UIBase{

	// Use this for initialization
	void Start () {
        //UIMsg uimsg = new UIMsg((ushort)UIListenID.Start_Test);
        //uimsg.UIName = "test2";
        //SendMsg(uimsg);
        ABSceneManager abScene = new ABSceneManager("ScenesOne");
        abScene.ReadConfiger();


    }
    public void Init()
    {
        List<fank> list = new List<fank>();
        list.Add(new fank(3, 8));
        list.Add(new fank(5, 2));
        list.Add(new fank(7, 0));
        int Min = 0;

        for (int i = 0; i < list.Count-1; i++)
        {
            for (int j = i+1; j < list.Count-1; j++)
            {
                int tmpMin = list[j].a + list[j - 1].b + list[j + 1].b;
                if (tmpMin<Min)
                {
                    Min = tmpMin;
                }
            }
        }

    }

}
public class fank
{
    public int a;
    public int b;
    public fank (int _a,int _b)
    {
        a = _a;
        b = _b;
    }
}
