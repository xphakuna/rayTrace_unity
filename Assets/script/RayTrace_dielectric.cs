using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class RayTrace 
{

    Color ray_color_dielectric(Ray r, int depth)
    {
        if (depth <= 0)
        {
            return Color.black;
        }

        Hit_record rec = new Hit_record();
        if (World.instance.Hit(r, 0.001f, Mathf.Infinity, ref rec))
        {
            Ray scatterd = new Ray();
            Color attenuation = new Color();
            if (rec.m_mat.Scatter(r, rec, ref attenuation, ref scatterd))
            {
                var c1 = ray_color_dielectric(scatterd, depth - 1);
                return attenuation * c1;
            }
            else
            {
                return Color.black;
            }
        }

        var unit_direction = r.direction.normalized;
        var t = 0.5f * (unit_direction.y + 1.0f);
        var vcolor2 = (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
        return new Color(vcolor2.x, vcolor2.y, vcolor2.z);
    }

    public void Calc_dielectric(int CNT)
    {
        prepare("cfg_dielectric");

        int max_depth = 30;
        var allRayInfo = MyCamera.instance.GetAllRayInfo();

        var tex2d = APP.instance.GetTexture();
        var colors = tex2d.GetPixels();

        Hit_record rec = new Hit_record();

        foreach (var info in allRayInfo)
        {
            if (info.m_x == APP.instance.m_debugPoint.x && info.m_y == APP.instance.m_debugPoint.y)
            {
                // add break point here
                int i = 0;
                i++;
            }
            Vector3 cSum = Vector3.zero;
            CNT = Mathf.Max(1, CNT);
            for (int i = 0; i < CNT; i++)
            {
                var c = ray_color_dielectric(info.m_ray, max_depth);
                cSum.x += c.r;
                cSum.y += c.g;
                cSum.z += c.b;
            }
            cSum = cSum / CNT;

            // gamma
            colors[info.m_idx] = new Color(Mathf.Sqrt(cSum.x), Mathf.Sqrt(cSum.y), Mathf.Sqrt(cSum.z));

        }
        //
        tex2d.SetPixels(colors);
        tex2d.Apply();
    }
    // metal end
}
