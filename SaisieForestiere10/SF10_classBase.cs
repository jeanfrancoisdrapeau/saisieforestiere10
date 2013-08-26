using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Desktop.AddIns;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace SaisieForestiere10
{
    class SF10_classBase
    {
        static public bool isTypePolygon(IFeatureLayer layer)
        {
            try
            {
                return (layer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon);
            }
            catch
            {
                return false;
            }
        }
    }
}
