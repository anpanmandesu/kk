using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kyakuseki : MonoBehaviour
{
    /// <summary>aaa</summary>
    public GameObject seki;
    public GameObject miniseki;
    public GameObject Circle;
    public GameObject yokoseki;
    public Transform Butai;
    
    //初期座標
    private float xx = -14f;
    private float yy = 11f;
    private float xxx = -6.82f;
    private float yyy = 14.5f;
    //
    private float fx = 0.14f;
    private float fy = -2.5f;
    private float habax = 0.5f;
    private float habay = -0.7f;
    private float yokohabax = 0.64f;
    private float FX, FY, FXX, FYY;
    [SerializeField]
    private int nunber;
    //たまに広くなる道の幅
    private float a;
    private float b = 0f;
    void Start()
    {
        //縦の客席とエージェント
        /*for (int i = -1; i < 8; i++)
        {
            a = 0;
            if (i == 2 || i == 5)
            {
                b += -0.5f;
            }
            for (int j = 0; j < 68; j++)
            {

                if (j == 3 || j == 8 || j == 28 || j == 48)
                {
                    a += 0.5f;
                }
                if (i == -1 || i == 7)
                {
                    if (j > 2)
                    {
                        if (i == -1)
                        {
                            FY = yy - (fy / 2 + habay + fy / 6) + b;
                        }
                        else
                        {
                            FY = yy + (i - 1) * (fy + habay) + fy / 2 + habay + fy / 6 + b;
                        }
                        FX = xx + j * 0.5f + a;
                        seisei(miniseki, FX, FY);
                    }
                }
                else
                {
                    FY = yy + i * (fy + habay) + b;
                    FX = xx + j * habax + a;
                    seisei(seki, FX, FY);
                }



                if (i == -1 || i == 7)
                {

                    if (j > 2)
                    {
                        //4にする
                        for (int k = 0; k < 4; k++)
                        {
                            seisei(Circle, FX + 0.5f * fx + 0.1f, FY + 0.2f * 1.5f - 0.2f * k);
                        }
                    }
                }
                //12にする
                else
                {
                    for (int k = 0; k < 12; k++)
                    {
                        seisei(Circle, FX + 0.5f * fx + 0.1f, FY + 0.2f * 5.5f - 0.2f * k);
                    }
                }
            }
        }*/

        //横の客席とエージェント
        /*a = 0;
        for(int k = 0; k < 2; k++) {
            {
                for (int i = 0; i < 6; i++)
                {
                    if (i == 2 || i == 4)
                    {
                        a += 0.22f;
                    }
                    for (int j = 0; j < 6; j++)
                    {
                        FX = xxx + i * (4.5f + yokohabax) + a;
                        FY = yyy + j * habax;
                        //seisei(yokoseki,FX ,FY );
                        //22にする
                        for (int l = 0; l < nunber; l++)
                        {
                            if (k == 0)
                            {
                                FXX = FX + 0.2f * -10.5f + 0.2f * l;
                                FYY = FY - fx / 2 - 0.1f;
                                tansaku.instance.keirokeisan(FXX , FYY , 17.5f, 19f);
                                seisei(Circle,FXX, FYY);
                            }
                            else
                            {
                                FXX = FX + 0.2f * -10.5f + 0.2f * l;
                                FYY = FY + fx / 2 + 0.1f;
                                tansaku.instance.keirokeisan(FXX , FYY , 17.5f, 19f);
                                seisei(Circle,FXX ,FYY );
                            }
                        }
                    }
                }
                a = 0f;
                yyy -= 30f;
            }
        }
        */
    }
    

    void Update()
    {

    }
    //席や座席を生成する
    void seisei(GameObject OBJ, float X, float Y)
    {
        GameObject obj = Instantiate(OBJ, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(Butai);
        obj.transform.position = new Vector3(X, Y, 0);
    }
}