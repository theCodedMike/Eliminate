using DG.Tweening;
using UnityEngine; 
using Random = UnityEngine.Random;

public class Gemstone : MonoBehaviour
{
    // 宝石的x轴起始位置
    public float xOffset = -5.5f;
    // 宝石的y轴起始位置
    public float yOffset = -2.0f;

    public int rowIdx;
    public int colIdx;
    public GameObject[] gemstoneBgs;
    // 宝石类型
    public int gemstoneType;
    
    private GameObject _gemstoneBg;
    private SpriteRenderer _spriteRenderer;
    private GameController _gameController;
    
    public bool isSelected
    {
        set => _spriteRenderer.color = value ? Color.red : Color.white;
    }
    
    
    private void Start()
    {
        _spriteRenderer = _gemstoneBg.GetComponent<SpriteRenderer>();
        _gameController = FindFirstObjectByType<GameController>();
    }

    // 随机生成宝石背景
    public void RandomCreateGemstoneBg()
    {
        if (_gemstoneBg)
            return;
        gemstoneType = Random.Range(0, gemstoneBgs.Length);
        _gemstoneBg = Instantiate(gemstoneBgs[gemstoneType], transform, true);
    }

    // 更新宝石的位置
    public void UpdatePosition(int idxOfRow, int idxOfCol)
    {
        rowIdx = idxOfRow;
        colIdx = idxOfCol;
        //控制生成宝石的位置
        transform.position = new Vector3(colIdx + xOffset, rowIdx + yOffset, 0);
    }

    // 点击宝石
    private void OnMouseDown()
    {
        _gameController.Select(this);
    }

    // 使用DoTween实现宝石滑动效果
    public void TweenToPosition(int idxOfRow, int idxOfCol)
    {
        rowIdx = idxOfRow;
        colIdx = idxOfCol;
        transform.DOMove(new Vector3(colIdx + xOffset, rowIdx + yOffset, 0), 0.5f);
    }
}
