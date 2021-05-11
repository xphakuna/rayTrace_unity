using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInfo
{
    public Ray m_ray;
    public int m_x;
    public int m_y;
    public int m_idx; // idx of texture2d
}

public class MyCamera 
{
    static MyCamera _instance;
    public static MyCamera instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MyCamera();
            }
            return _instance;
        }
    }

    Vector3 m_origin;
    Vector3 m_lower_left_corner;
    Vector3 m_horizontal;
    Vector3 m_vertical;
    Vector3 m_u, m_v, m_w;
    float m_lens_radius;

    List<RayInfo> m_allRay = new List<RayInfo>();

    Ray get_ray(int ix, int iy)
    {


        Vector3 dir = m_lower_left_corner + (float)(ix) / APP.s_width * m_horizontal
            + (float)(iy) / APP.s_height * m_vertical
            - m_origin;
        return new Ray(m_origin, dir.normalized);

        //Vector3 rd = m_lens_radius * Helper.random_in_unit_disk();
        //Vector3 offset = m_u * rd.x + m_v * rd.y;

        //Vector3 dir = m_lower_left_corner + (float)(ix) / APP.s_width * m_horizontal
        //    + (float)(iy) / APP.s_height * m_vertical
        //    -m_origin-offset;
        //return new Ray(m_origin, dir.normalized);
    }
    //
    public void Init(Vector3 lookFrom,
    Vector3 lookAt,
    Vector3 vup,
    float deg_vfov, // vertical field-of-view in degrees
    float aspect_ratio,
    float aperture,
     float focus_dist)
    {
        var theta = Mathf.Deg2Rad*deg_vfov;
        var h = Mathf.Tan(theta / 2);
        var viewport_height = 2.0f * h;
        var viewport_width = aspect_ratio * viewport_height;

        m_w = (lookFrom - lookAt).normalized;
        m_u = Vector3.Cross(vup, m_w).normalized;
        m_v = Vector3.Cross(m_w, m_u);
         
        m_origin = lookFrom;
        m_horizontal = focus_dist * viewport_width * m_u;
        m_vertical = focus_dist * viewport_height * m_v;
        m_lower_left_corner = m_origin - m_horizontal / 2 - m_vertical / 2 - focus_dist*m_w;

        m_lens_radius = aperture / 2;

        m_allRay.Clear();


        for (int iy = 0; iy < APP.s_height; iy++)
        {
            for (int ix = 0; ix < APP.s_width; ix++)
            {
                RayInfo info = new RayInfo();
                info.m_x = ix;
                info.m_y = iy;
                info.m_idx = iy * APP.s_width + ix;
                info.m_ray = get_ray(ix, iy);

                m_allRay.Add(info);
            }
        }

    }


    public List<RayInfo> GetAllRayInfo()
    {
        return m_allRay;
    }
}
