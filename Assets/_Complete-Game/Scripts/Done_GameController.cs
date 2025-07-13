using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Done_GameController : MonoBehaviour {

    public Done_Gemstone gemstone;
    public int rowNum = 7;//宝石列数  
    public int columNum = 10;//宝石行数  
    public ArrayList gemstoneList;//定义列表  
    private Done_Gemstone currentGemstone;
    private ArrayList matchesGemstone;

    // Use this for initialization
    void Start () {

        gemstoneList = new ArrayList();//新建列表  
        matchesGemstone = new ArrayList();
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            ArrayList temp = new ArrayList();
            for (int columIndex = 0; columIndex < columNum; columIndex++)
            {
                Done_Gemstone g = AddGemstone(rowIndex, columIndex);
                temp.Add(g);
            }
            gemstoneList.Add(temp);
        }
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {//开始检测匹配消除  
            RemoveMatches();
        }
    }
    public Done_Gemstone AddGemstone(int rowIndex, int columIndex)
    {
        Done_Gemstone g = Instantiate(gemstone) as Done_Gemstone;
        g.transform.parent = this.transform;//生成宝石为GameController子物体  
        g.GetComponent<Done_Gemstone>().RandomCreateGemstoneBg();
        g.GetComponent<Done_Gemstone>().UpdatePosition(rowIndex, columIndex);//传递宝石位置
        return g;
    }
    // Update is called once per frame
    void Update () {

    }


    /// <summary>
    /// 鼠标点选判定
    /// </summary>
    /// <param name="g"></param>
    public void Select(Done_Gemstone g)
    {
         
        if (currentGemstone == null)
        {
            currentGemstone = g;
            currentGemstone.isSelected = true;
            return;
        }
        else
        {
            if (Mathf.Abs(currentGemstone.rowIndex - g.rowIndex) + Mathf.Abs(currentGemstone.columIndex - g.columIndex) == 1)
            {
                StartCoroutine(ExangeAndMatches(currentGemstone, g));
            }
           
            currentGemstone.isSelected = false;
            currentGemstone = null;
        }
    }


    /// <summary>
    /// 实现宝石交换并且检测匹配消除
    /// </summary>
    /// <param name="g1"></param>
    /// <param name="g2"></param>
    /// <returns></returns>
    IEnumerator ExangeAndMatches(Done_Gemstone g1, Done_Gemstone g2)
    {
        Exchange(g1, g2);
        yield return new WaitForSeconds(0.5f);
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {
            RemoveMatches();
        }
        else
        {
            Exchange(g1, g2);//若不能消除，再次交换宝石
        }
    }
    

    /// <summary>
    /// 生成所对应行号和列号的宝石
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="columIndex"></param>
    /// <param name="g"></param>
    public void SetGemstone(int rowIndex, int columIndex, Done_Gemstone g)
    {
        ArrayList temp = gemstoneList[rowIndex] as ArrayList;
        temp[columIndex] = g;
    }

    /// <summary>
    /// 交换宝石数据
    /// </summary>
    /// <param name="g1"></param>
    /// <param name="g2"></param>
    public void Exchange(Done_Gemstone g1, Done_Gemstone g2)
    {
        
        SetGemstone(g1.rowIndex, g1.columIndex, g2);
        SetGemstone(g2.rowIndex, g2.columIndex, g1);
        //交换g1，g2的行号  
        int tempRowIndex;
        tempRowIndex = g1.rowIndex;
        g1.rowIndex = g2.rowIndex;
        g2.rowIndex = tempRowIndex;
        //交换g1，g2的列号  
        int tempColumIndex;
        tempColumIndex = g1.columIndex;
        g1.columIndex = g2.columIndex;
        g2.columIndex = tempColumIndex;


        g1.TweenToPostion(g1.rowIndex, g1.columIndex);
        g2.TweenToPostion(g2.rowIndex, g2.columIndex);
    }

    /// <summary>
    /// 通过行号和列号，获取对应位置的宝石
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="columIndex"></param>
    /// <returns></returns>
    public Done_Gemstone GetGemstone(int rowIndex, int columIndex)
    {
        ArrayList temp = gemstoneList[rowIndex] as ArrayList;
        Done_Gemstone g = temp[columIndex] as Done_Gemstone;
        return g;
    }

    /// <summary>
    /// 实现检测水平方向的匹配  
    /// </summary>
    /// <returns></returns>
    bool CheckHorizontalMatches()
    {
        bool isMatches = false;
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            for (int columIndex = 0; columIndex < columNum - 2; columIndex++)
            {
                if ((GetGemstone(rowIndex, columIndex).gemstoneType == GetGemstone(rowIndex, columIndex + 1).gemstoneType) && (GetGemstone(rowIndex, columIndex).gemstoneType == GetGemstone(rowIndex, columIndex + 2).gemstoneType))
                {
                    //Debug.Log ("发现行相同的宝石");  
                    AddMatches(GetGemstone(rowIndex, columIndex));
                    AddMatches(GetGemstone(rowIndex, columIndex + 1));
                    AddMatches(GetGemstone(rowIndex, columIndex + 2));
                    isMatches = true;
                }
            }
        }
        return isMatches;
    }

    /// <summary>
    /// 实现检测垂直方向的匹配
    /// </summary>
    /// <returns></returns>
    bool CheckVerticalMatches()
    {
        bool isMatches = false;
        for (int columIndex = 0; columIndex < columNum; columIndex++)
        {
            for (int rowIndex = 0; rowIndex < rowNum - 2; rowIndex++)
            {
                if ((GetGemstone(rowIndex, columIndex).gemstoneType == GetGemstone(rowIndex + 1, columIndex).gemstoneType) && (GetGemstone(rowIndex, columIndex).gemstoneType == GetGemstone(rowIndex + 2, columIndex).gemstoneType))
                {
                   //Debug.Log("发现列相同的宝石");  
                    AddMatches(GetGemstone(rowIndex, columIndex));
                    AddMatches(GetGemstone(rowIndex + 1, columIndex));
                    AddMatches(GetGemstone(rowIndex + 2, columIndex));
                    isMatches = true;
                }
            }
        }
        return isMatches;
    }
    /// <summary>
    /// 储存符合消除条件的数组
    /// </summary>
    /// <param name="g"></param>
    void AddMatches(Done_Gemstone g)
    {
        if (matchesGemstone == null)
            matchesGemstone = new ArrayList();
        int Index = matchesGemstone.IndexOf(g);//检测宝石是否已在数组当中  
        if (Index == -1)
        {
            matchesGemstone.Add(g);
        }
    }


    /// <summary>
    /// 删除/生成宝石  
    /// </summary>
    /// <param name="g"></param>
    void RemoveGemstone(Done_Gemstone g)
    {
        //Debug.Log("删除宝石");  
        g.Dispose();

        //删除宝石后在对应位置生成新的宝石
        for (int i = g.rowIndex + 1; i < rowNum; i++)
        {
            Done_Gemstone temGamestone = GetGemstone(i, g.columIndex);
            temGamestone.rowIndex--;
            SetGemstone(temGamestone.rowIndex, temGamestone.columIndex, temGamestone);

            temGamestone.TweenToPostion(temGamestone.rowIndex, temGamestone.columIndex);
        }
        Done_Gemstone newGemstone = AddGemstone(rowNum, g.columIndex);
        newGemstone.rowIndex--;
        SetGemstone(newGemstone.rowIndex, newGemstone.columIndex, newGemstone);

        newGemstone.TweenToPostion(newGemstone.rowIndex, newGemstone.columIndex);
    }

    /// <summary>
    /// 删除匹配的宝石
    /// </summary>
    void RemoveMatches()
    {
        for (int i = 0; i < matchesGemstone.Count; i++)
        {
            Done_Gemstone g = matchesGemstone[i] as Done_Gemstone;
            RemoveGemstone(g);
        }
        matchesGemstone = new ArrayList();
        StartCoroutine(WaitForCheckMatchesAgain());
    }

    /// <summary>
    /// 连续检测匹配消除
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForCheckMatchesAgain()
    {

        yield return new WaitForSeconds(0.5f);
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {
            RemoveMatches();
            GameObject.Find("Text").GetComponent<Text>().text = "连击";
            yield return new WaitForSeconds(3f);
            GameObject.Find("Text").GetComponent<Text>().text = "";
        }
    }
}
