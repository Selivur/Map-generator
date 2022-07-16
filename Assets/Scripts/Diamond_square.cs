using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond_square : MonoBehaviour
{
    public static int size = 257;
    public static float[,] heighmap = new float[size, size];
    public static float roughness = 2f;

    public static void Square(int lx, int ly, int rx, int ry)
    {
        int l = (rx - lx) / 2;

        float a = heighmap[lx, ly];              
        float b = heighmap[lx, ry];              
        float c = heighmap[rx, ry];              
        float d = heighmap[rx, ly];              
                                                 
        int cex = lx + l;
        int cey = ly + l;

        heighmap[cex, cey] = (a + b + c + d) / 4 ;
    }
    public static void Diamond(int tgx, int tgy, int l)
    {
        float a, b, c, d;

        if (tgy - l >= 0)
            a = heighmap[tgx, tgy - l];                        
        else                                                   
            a = heighmap[tgx, size - l];                       
                                                               
                                                               
        if (tgx - l >= 0)
            b = heighmap[tgx - l, tgy];
        else
            b = heighmap[size - l, tgy];


        if (tgy + l <= size - 1)
            c = heighmap[tgx, tgy + l];
        else
            c = heighmap[tgx, l];


        if (tgx + l <= size - 1)
            d = heighmap[tgx + l, tgy];
        else
            d = heighmap[l, tgy];

        heighmap[tgx, tgy] = (a + b + c + d) / 4 ;

    }
    public static void DiamondSquare(int lx, int ly, int rx, int ry)
    {
        int l = (rx - lx) / 2;
        if (l > 0)
        {
            Square(lx, ly, rx, ry);

            Diamond(lx, ly + l, l);
            Diamond(rx, ry - l, l);
            Diamond(rx - l, ry, l);
            Diamond(lx + l, ly, l);
        }
    }

    public static float[,] Generate()
    {
        heighmap[0, 0] = Random.Range(0f, 1f);
        heighmap[size - 1, 0] = Random.Range(0f, 1f);
        heighmap[size - 1, size - 1] = Random.Range(0f, 1f);
        heighmap[0, size - 1] = Random.Range(0f, 1f);

        for (int l = (size - 1) / 2; l > 0; l /= 2)
            for (int x = 0; x < size - 1; x += l)
                for (int y = 0; y < size - 1; y += l)
                    DiamondSquare(x, y, x + l, y + l);
        return heighmap;    
    }
}
