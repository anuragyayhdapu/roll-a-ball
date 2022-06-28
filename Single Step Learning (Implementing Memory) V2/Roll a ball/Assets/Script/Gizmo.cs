using UnityEngine;
using UnityEditor;
using System.Collections;

public class Gizmo : MonoBehaviour
{
    public static LineRenderer linerenderer;
    public static float length, posx, posz, d_length;
    public static int n1, n2, n3;

    void Start()
    {
        n1 = n2 = n3 = 0;

        length = 0.2f;
        d_length = 0.1f;
        posx = 10.0f;
        posz = -3.0f;
    }

    void Update()
    {
    }

    public static void minus(float posx, float posz)
    {
        minus(posx, posz, Color.gray);
    }

    public static void minus(float posx, float posz, Color color)
    {
        n2 += 1;
        linerenderer = new GameObject("m" + n2).AddComponent<LineRenderer>();
        linerenderer.material = new Material(Shader.Find("Particles/Additive"));
        DontDestroyOnLoad(linerenderer);
        linerenderer.enabled = true;
        linerenderer.useWorldSpace = true;
        linerenderer.SetWidth(0.03f, 0.03f);
        linerenderer.SetColors(color, color);

        linerenderer.SetPosition(0, new Vector3(8.0f + posx - (length / 2), 0, -4.5f + posz));
        linerenderer.SetPosition(1, new Vector3(8.0f + posx + (length / 2), 0, -4.5f + posz));
    }

    public static void plus(float posx, float posz)
    {
        n1 += 1;
        //minus(posx, posz, Color.cyan);  
		minus(posx, posz, Color.red); 	// red for exact force
        linerenderer = new GameObject("p" + n1).AddComponent<LineRenderer>();
        linerenderer.material = new Material(Shader.Find("Particles/Additive"));
        DontDestroyOnLoad(linerenderer);
        linerenderer.enabled = true;
        linerenderer.useWorldSpace = true;
        linerenderer.SetWidth(0.03f, 0.03f);
        linerenderer.SetColors(Color.red, Color.red);

        linerenderer.SetPosition(0, new Vector3(8.0f + posx, 0, -4.5f + posz - (length / 2)));
        linerenderer.SetPosition(1, new Vector3(8.0f + posx, 0, -4.5f + posz + (length / 2)));

    }

    public static void dash(float posx, float posz)
    {
        n3 += 1;
        linerenderer = new GameObject("m" + n3).AddComponent<LineRenderer>();
        linerenderer.material = new Material(Shader.Find("Particles/Additive"));
        DontDestroyOnLoad(linerenderer);
        linerenderer.enabled = true;
        linerenderer.useWorldSpace = true;
        linerenderer.SetWidth(0.03f, 0.03f);
        linerenderer.SetColors(Color.cyan, Color.cyan);

        linerenderer.SetPosition(0, new Vector3(8.0f + posx - (d_length / 2), 0, -4.5f + posz));
        linerenderer.SetPosition(1, new Vector3(8.0f + posx + (d_length / 2), 0, -4.5f + posz));
    }



}
