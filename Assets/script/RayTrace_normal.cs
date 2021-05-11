using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class RayTrace 
{

    Color ray_color_normal(Ray r)
    {
        Hit_record rec = new Hit_record();
        if (World.instance.Hit(r, 0, Mathf.Infinity, ref rec))
        {
            var vcolor = 0.5f * (rec.m_normal + new Vector3(1, 1, 1));
            return new Color(vcolor.x, vcolor.y, vcolor.z);
        }
        var unit_direction = r.direction;
        var t = 0.5f * (unit_direction.y + 1.0f);
        var vcolor2 = (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
        return new Color(vcolor2.x, vcolor2.y, vcolor2.z);
    }


    

    public void Calc_normal()
    {
        prepare("cfg_normal");

        var allRayInfo = MyCamera.instance.GetAllRayInfo();

        var tex2d = APP.instance.GetTexture();
        var colors = tex2d.GetPixels();

        Hit_record rec = new Hit_record();

        foreach (var info in allRayInfo)
        {
            colors[info.m_idx] = ray_color_normal(info.m_ray);
           
        }
        //
        tex2d.SetPixels(colors);
        tex2d.Apply();
    }
    
}
