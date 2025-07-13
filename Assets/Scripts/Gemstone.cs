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

    public GameObject[] gemstones;
    // 宝石类型
    public int gemstoneType;

    private GameObject _gemstoneBg;

    private SpriteRenderer _spriteRenderer;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void RandomCreateGemstoneBg()
    {
        if (_gemstoneBg)
            return;
        gemstoneType = Random.Range(0, gemstones.Length);
        _gemstoneBg = Instantiate(gemstones[gemstoneType], transform, true);
    }

    public void UpdatePosition(int idxOfRow, int idxOfCol)
    {
        rowIdx = idxOfRow;
        colIdx = idxOfCol;
        //控制生成宝石的位置
        transform.position = new Vector3(colIdx + xOffset, rowIdx + yOffset, 0);
    }
}
