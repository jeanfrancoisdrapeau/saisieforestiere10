using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Desktop.AddIns;
using ESRI.ArcGIS.Geodatabase;

namespace SaisieForestiere10
{
    public class SF10_btnRefreshLayersBombo : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public SF10_btnRefreshLayersBombo()
        {
        }

        protected override void OnClick()
        {
            SF10_cmbLayers selCombo = SF10_cmbLayers.GetSelectionComboBox();
            if (selCombo == null)
                return;
            
            selCombo.ClearAll();
            
            IFeatureLayer featureLayer;
            IFeatureSelection pFsel;
            ISelectionSet pSelectionSet;
            ICursor pICursor;

            bool validFeatureLayer;

            IMap map = ArcMap.Document.FocusMap;
            // Loop through the layers in the map and add the layer's name to the combo box.
            for (int i = 0; i < map.LayerCount; i++)
            {
                validFeatureLayer = true;

                if (map.get_Layer(i) is IFeatureSelection)
                {
                    featureLayer = map.get_Layer(i) as IFeatureLayer;
                    if (featureLayer == null)
                        validFeatureLayer = false;

                    if (!SF10_classBase.isTypePolygon(featureLayer))
                        validFeatureLayer = false;

                    pFsel = (IFeatureSelection)featureLayer;
                    pSelectionSet = pFsel.SelectionSet;
                    pSelectionSet.Search(null, true, out pICursor);

                    if (pICursor.Fields.FindField("nopeup") == -1)
                        validFeatureLayer = false;
                    if (pICursor.Fields.FindField("noacq_pee") == -1)
                        validFeatureLayer = false;
                    if (pICursor.Fields.FindField("saisie_ok") == -1)
                        validFeatureLayer = false;

                    if (validFeatureLayer)
                        selCombo.AddItem(featureLayer.Name, featureLayer);
                }
            }

            pICursor = null;
            pSelectionSet = null;
            pFsel = null;
            featureLayer = null;
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
