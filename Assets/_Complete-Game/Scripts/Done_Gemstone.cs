using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Done_Gemstone : MonoBehaviour {
    public float xOffset = -4.5f;//宝石的x间距
    public float yOffset = -2.0f;//宝石的y间距  
    public int rowIndex = 0;
    public int columIndex = 0;
    public GameObject[] gemstoneBgs;//宝石数组  
    public int gemstoneType;//宝石类型  
    private GameObject gemstoneBg;
    private SpriteRenderer spriteRenderer;
    private Done_GameController gameController;


    // Use this for initialization
    void Start ()
    {
        gameController = GameObject.Find("GameController").GetComponent<Done_GameController>();
        spriteRenderer = gemstoneBg.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

  

    /// <summary>
    /// 随机的宝石类型  
    /// </summary>
    public void RandomCreateGemstoneBg()
    {
        if (gemstoneBg != null)
            return;
        gemstoneType = Random.Range(0, gemstoneBgs.Length);//从宝石数组中随机选择一种宝石
        gemstoneBg = Instantiate(gemstoneBgs[gemstoneType]) as GameObject;//实例化随机的宝石
        gemstoneBg.transform.parent = this.transform;
    }

    /// <summary>
    /// 获取宝石的位置  
    /// </summary>
    /// <param name="_rowIndex"></param>
    /// <param name="_columIndex"></param>
    public void UpdatePosition(int _rowIndex, int _columIndex)
    {
        rowIndex = _rowIndex;
        columIndex = _columIndex;
        this.transform.position = new Vector3(columIndex + xOffset, rowIndex + yOffset, 0);//控制生成宝石的位置
    }

    public bool isSelected
    {
        set
        {
            if (value)
            {
                spriteRenderer.color = Color.red;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    public void OnMouseDown()
    {
        gameController.Select(this);
    }

    /// <summary>
    /// 调用iTween插件实现宝石滑动效果  
    /// </summary>
    /// <param name="_rowIndex"></param>
    /// <param name="_columIndex"></param>
    public void TweenToPostion(int _rowIndex, int _columIndex)
    {
        rowIndex = _rowIndex;
        columIndex = _columIndex;
        //在Unity 6.0中，下面这行代码编译不通过，所以使用DoTween插件重写了
        //iTween.MoveTo(this.gameObject, iTween.Hash("x", columIndex + xOffset, "y", rowIndex + yOffset, "time", 0.5f));
        transform.DOMove(new Vector3(columIndex + xOffset, rowIndex + yOffset, transform.position.z), 0.5f);

    }

    public void Dispose()
    {
        Destroy(this.gameObject);
        Destroy(gemstoneBg.gameObject);
        gameController = null;
    }
}
