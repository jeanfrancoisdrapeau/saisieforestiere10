using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Desktop.AddIns;

namespace SaisieForestiere10
{
    public class SF10_cmbLayers : ESRI.ArcGIS.Desktop.AddIns.ComboBox
    {
        private static SF10_cmbLayers s_comboBox;

        public SF10_cmbLayers()
        {
            s_comboBox = this;
        }

        internal static SF10_cmbLayers GetSelectionComboBox()
        {
            return s_comboBox;
        }

        internal void AddItem(string itemName, IFeatureLayer layer)
        {
            int cookie = s_comboBox.Add(itemName, layer);
        }

        internal void ClearAll()
        {
            s_comboBox.Clear();
        }

        protected override void OnUpdate()
        {
            this.Enabled = true;
        }

        protected override void OnSelChange(int cookie)
        {
            if (cookie == -1)
                return;

            foreach (ComboBox.Item item in this.items)
            {
                if (cookie == item.Cookie)
                {
                    SF10_extMain.m_LayerName = item.Tag as IFeatureLayer;
                }
            }
        }
    }

}
