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
        
        _currGemstone.isSelected = false;
        _currGemstone = null;
    }
}
