using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("宝石预制体")]
    public Gemstone gemstone;
    [Header("行数")]
    public int rowNum = 7; // 宝石行数
    [Header("列数")]
    public int colNum = 10; // 宝石列数
    [Header("连击提示")]
    public Text multiHit;
    
    private List<List<Gemstone>> _gemstoneGrid; // 所有宝石
    private List<Gemstone> _matchesGemstones;   // 匹配的宝石

    private Gemstone _currGemstone; // 当前被选中的宝石
    
    
    private void Start()
    {
        _gemstoneGrid = new(rowNum);
        _matchesGemstones = new();
        for (int i = 0; i < rowNum; i++)
        {
            List<Gemstone> temp = new(colNum);
            for (int j = 0; j < colNum; j++)
            {
                Gemstone g = GenGemstone(i, j);
                temp.Add(g);
            }
            _gemstoneGrid.Add(temp);
        }
    }

    // 生成宝石
    private Gemstone GenGemstone(int rowIdx, int colIdx)
    {
        Gemstone g = Instantiate(gemstone, transform, true);
        g.GetComponent<Gemstone>().RandomCreateGemstoneBg();
        g.GetComponent<Gemstone>().UpdatePosition(rowIdx, colIdx);
        return g;
    }
    
    // 鼠标选中某块宝石
    public void Select(Gemstone g)
    {
        if (_currGemstone == null)
        {
            _currGemstone = g;
            _currGemstone.isSelected = true;
            return;
        }

        if (Mathf.Abs(_currGemstone.rowIdx - g.rowIdx) + Mathf.Abs(_currGemstone.colIdx - g.colIdx) == 1)
            StartCoroutine(ExchangeAndMatches(_currGemstone, g));
        _currGemstone.isSelected = false;
        _currGemstone = null;
    }

    // 生成所对应行号和列号的宝石
    private void SetGemstone(int rowIdx, int colIdx, Gemstone g)
    {
        _gemstoneGrid[rowIdx][colIdx] = g;
    }

    // 交换宝石数据
    private void Exchange(Gemstone g1, Gemstone g2)
    {
        SetGemstone(g1.rowIdx, g1.colIdx, g2);
        SetGemstone(g2.rowIdx, g2.colIdx, g1);
        // 交换行号与列号
        (g1.rowIdx, g2.rowIdx) = (g2.rowIdx, g1.rowIdx);
        (g1.colIdx, g2.colIdx) = (g2.colIdx, g1.colIdx);
        // 移动
        g1.TweenToPosition(g1.rowIdx, g1.colIdx);
        g2.TweenToPosition(g2.rowIdx, g2.colIdx);
    }

    // 交换宝石并检测匹配消除
    private IEnumerator ExchangeAndMatches(Gemstone curr, Gemstone next)
    {
        Exchange(curr, next);
        yield return new WaitForSeconds(0.5f);
        if(CheckHorizontalMatches() || CheckVerticalMatches()) 
            RemoveMatches();
        else
            Exchange(curr, next);
    }

    private Gemstone GetGemstone(int rowIdx, int colIdx) => _gemstoneGrid[rowIdx][colIdx];

    // 检测水平方向是否匹配
    private bool CheckHorizontalMatches()
    {
        bool isMatches = false;
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum - 2; j++)
            {
                Gemstone gemstone1 = GetGemstone(i, j);
                Gemstone gemstone2 = GetGemstone(i, j + 1);
                Gemstone gemstone3 = GetGemstone(i, j + 2);
                if (gemstone1.gemstoneType == gemstone2.gemstoneType && gemstone1.gemstoneType == gemstone3.gemstoneType)
                {
                    //print($"行：有匹配的宝石了 {i},{j}");
                    AddMatches(gemstone1);
                    AddMatches(gemstone2);
                    AddMatches(gemstone3);
                    isMatches = true;
                }
            }
        }
        return isMatches;
    }

    // 检测垂直方向是否匹配
    private bool CheckVerticalMatches()
    {
        bool isMatches = false;
        for (int i = 0; i < colNum; i++)
        {
            for (int j = 0; j < rowNum - 2; j++)
            {
                Gemstone gemstone1 = GetGemstone(j, i);
                Gemstone gemstone2 = GetGemstone(j + 1, i);
                Gemstone gemstone3 = GetGemstone(j + 2, i);
                if (gemstone1.gemstoneType == gemstone2.gemstoneType && gemstone1.gemstoneType == gemstone3.gemstoneType)
                {
                    //print($"列：有匹配的宝石了 {j},{i}");
                    AddMatches(gemstone1);
                    AddMatches(gemstone2);
                    AddMatches(gemstone3);
                    isMatches = true;
                }
            }
        }
        return isMatches;
    }

    // 保存符合消除条件的宝石
    private void AddMatches(Gemstone g)
    {
        if (_matchesGemstones == null)
            _matchesGemstones = new();
        int idx = _matchesGemstones.IndexOf(g);
        if(idx == -1)
            _matchesGemstones.Add(g);
    }

    // 删除/生成宝石
    private void RemoveGemstone(Gemstone g)
    {
        g.Dispose();
        // 删除宝石后生成新的宝石
        for (int i = g.rowIdx + 1; i < rowNum; i++)
        {
            Gemstone tempGemstone = GetGemstone(i, g.colIdx);
            tempGemstone.rowIdx--;
            SetGemstone(tempGemstone.rowIdx, tempGemstone.colIdx, tempGemstone);
            
            tempGemstone.TweenToPosition(tempGemstone.rowIdx, tempGemstone.colIdx);
        }

        Gemstone newGemstone = GenGemstone(rowNum, g.colIdx);
        newGemstone.rowIdx--;
        SetGemstone(newGemstone.rowIdx, newGemstone.colIdx, newGemstone);
        
        newGemstone.TweenToPosition(newGemstone.rowIdx, newGemstone.colIdx);
    }

    // 删除所有匹配的宝石
    private void RemoveMatches()
    {
        if (_matchesGemstones.Count == 0)
            return;
        
        foreach (Gemstone matchGemstone in _matchesGemstones)
            RemoveGemstone(matchGemstone);
        
        _matchesGemstones.Clear();
        StartCoroutine(WaitForCheckMatchesAgain());
    }

    // 连续检测匹配消除
    private IEnumerator WaitForCheckMatchesAgain()
    {
        yield return new WaitForSeconds(0.5f);
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {
            RemoveMatches();
            multiHit.text = "连击";
            yield return new WaitForSeconds(3f);
            multiHit.text = "";
        }
    }
}
