using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("宝石预制体")]
    public Gemstone gemstone;

    public int rowNum = 7; // 宝石行数
    public int colNum = 10; // 宝石列数

    private List<List<Gemstone>> _gemstoneList; // 定义列表
    private List<Gemstone> _matchesGemstone;

    private Gemstone _currGemstone; // 当前被选中的宝石
    
    
    private void Start()
    {
        _gemstoneList = new(rowNum);
        _matchesGemstone = new();
        for (int i = 0; i < rowNum; i++)
        {
            List<Gemstone> temp = new(colNum);
            for (int j = 0; j < colNum; j++)
            {
                Gemstone g = GenGemstone(i, j);
                temp.Add(g);
            }
            _gemstoneList.Add(temp);
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
        _gemstoneList[rowIdx][colIdx] = g;
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
        Exchange(curr, next);
    }
}
