using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APP : MonoBehaviour
{
    static APP _instance;
    public static APP instance { get { return _instance; } }

    bool m_isOrigin = false;

    Texture2D m_tex2d;
    public static int s_width = 1334/2;
    public static int s_height = 750/2;

    public Transform m_trObjs;

    Transform m_trCross;


    private void Awake()
    {
        _instance = this;

        m_trCross = GameObject.Find("Canvas/cross").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        initWorld();
        calc();
    }

    public Vector2Int m_debugPoint;
    public bool m_isDrag;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            m_isDrag = true;
        }
        if (m_isDrag)
        {
            if (Input.GetMouseButton(1))
            {
                m_trCross.position = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1))
            {
                m_isDrag = false;
            }
        }

        var rect = m_trCross.GetComponent<RectTransform>();

        Vector2Int v2 = new Vector2Int();
        v2.x = (int)(rect.anchoredPosition.x) + Screen.width / 2;
        v2.x = (int)(v2.x * (float)(APP.s_width) / Screen.width);
        v2.x = Mathf.Clamp(v2.x, 0, APP.s_width);

        v2.y = (int)(rect.anchoredPosition.y) + Screen.height / 2;
        v2.y = (int)(v2.y * (float)(APP.s_height) / Screen.height);
        v2.y = Mathf.Clamp(v2.y, 0, APP.s_height);

        m_debugPoint = v2;

        m_trCross.Find("Text").GetComponent<Text>().text = string.Format("drag right mouse\n({0}, {1})", 
            v2.x, v2.y);

    }

    void calc_fourColor()
    {
        var colors = m_tex2d.GetPixels();
        Color tl = Color.green;
        Color tr = Color.yellow;
        Color bl = Color.blue;
        Color br = Color.red;
        for (int iy = 0; iy < s_height; iy++)
        {
            int lineBegin = iy * s_width;
            Color left = Color.Lerp(tl, bl, 1 - (float)(iy) / s_height);
            Color right = Color.Lerp(tr, br, 1 - (float)(iy) / s_height);
            for (int ix = 0; ix < s_width; ix++)
            {
                colors[lineBegin + ix] = Color.Lerp(left, right, (float)(ix) / s_width);
            }
        }


        m_tex2d.SetPixels(colors);
        m_tex2d.Apply();
    }

    void calc()
    {
        
        if (m_tex2d == null)
        {
            m_tex2d = new Texture2D(s_width, s_height, TextureFormat.ARGB32, false);
        }

        calc_fourColor();
       
        
    }

    public Texture2D GetTexture()
    {
        return m_tex2d;
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (m_isOrigin)
        {
            Graphics.Blit(src, dst);
            return;
        }

        if (m_tex2d != null)
        {
            Graphics.Blit(m_tex2d, dst);
        }

        
    }

    void initWorld()
    {
        World world = World.instance;
        world.Clear();

        var colliders = m_trObjs.GetComponentsInChildren<SphereCollider>();
        foreach(var c in colliders)
        {
            var scale = c.transform.lossyScale.z;
            Sphere s = new Sphere(c.transform.position, c.radius* scale);

            world.Add(s);
        }
    }

    class BtnInfo
    {
        public string m_name;
        public int m_row;
        public System.Action m_action;
    }

    List<BtnInfo> m_allBtnInfo = new List<BtnInfo>();

    void OnGUI()
    {
        if (m_allBtnInfo.Count == 0)
        {
            BtnInfo info = null;
            // row 1
            info = new BtnInfo();
            info.m_name = "origin";
            info.m_row = 1;
            info.m_action = onClick_origin;
            m_allBtnInfo.Add(info);
            // row 2
            info = new BtnInfo();
            info.m_name = "normal";
            info.m_row = 2;
            info.m_action = onClick_normal;
            m_allBtnInfo.Add(info);

            info = new BtnInfo();
            info.m_name = "diffuse";
            info.m_row = 2;
            info.m_action = onClick_diffuse;
            m_allBtnInfo.Add(info);

            info = new BtnInfo();
            info.m_name = "metal";
            info.m_row = 2;
            info.m_action = onClick_metal;
            m_allBtnInfo.Add(info);

            info = new BtnInfo();
            info.m_name = "dielectric";
            info.m_row = 2;
            info.m_action = onClick_dielectric;
            m_allBtnInfo.Add(info);
            // row 3
            info = new BtnInfo();
            info.m_name = "normal";
            info.m_row = 3;
            info.m_action = onClick_normal;
            m_allBtnInfo.Add(info);

        }


        int BTN_W = 80;
        int BTN_H = 20;
        int BTN_BIG_W = 100;
        int BTN_BIG_H = 30;
        int START_X = 50;
        int START_Y = 50;

        int row1_idx = -1;
        int row2_idx = -1;
        int row3_idx = -1;

        for (int i=0; i<m_allBtnInfo.Count; i++)
        {
            var info = m_allBtnInfo[i];
            int xmin=0, ymin = 0;
            if (info.m_row==1)
            {
                row1_idx++;
                ymin = START_Y + 0 * BTN_BIG_H;
                xmin = START_X + row1_idx * BTN_BIG_W;
            } else if (info.m_row == 2) {
                row2_idx++;
                ymin = START_Y + 1 * BTN_BIG_H;
                xmin = START_X + row2_idx * BTN_BIG_W;
            } else if (info.m_row == 3)
            {
                row3_idx++;
                ymin = START_Y + 2 * BTN_BIG_H;
                xmin = START_X + row3_idx * BTN_BIG_W;
            }

            if (GUI.Button(new Rect(xmin, ymin, BTN_W, BTN_H), info.m_name))
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                info.m_action.Invoke();

                sw.Stop();
                Debug.LogError(info.m_name+" using time:" + ((float)(sw.ElapsedMilliseconds) / 1000));
            }

            m_quick = GUI.Toggle(new Rect(10, 10, BTN_W, BTN_H), m_quick, "quick");
        }


        

    }

    public bool m_quick = true;

    void onClick_origin()
    {
        m_isOrigin = true;
    }

    void onClick_normal()
    {
        m_isOrigin = false;

        RayTrace.instance.Calc_normal();
    }

    void onClick_diffuse()
    {
        m_isOrigin = false;

        RayTrace.instance.Calc_diffuse(m_quick?1:100);
    }

    void onClick_metal()
    {
        m_isOrigin = false;

        RayTrace.instance.Calc_metal(m_quick ? 1 : 100);
    }

    void onClick_dielectric()
    {
        m_isOrigin = false;

        RayTrace.instance.Calc_dielectric(m_quick ? 1 : 100);
    }

}
