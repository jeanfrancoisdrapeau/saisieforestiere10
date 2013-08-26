using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;

namespace SaisieForestiere10
{
    public class SF10_btnToggleDckFrmSaisie : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public SF10_btnToggleDckFrmSaisie()
        {
        }

        protected override void OnClick()
        {
            IDockableWindow dockWindow = SF10_extMain.GetDockWindow();

            if (dockWindow == null)
                return;

            dockWindow.Show(!dockWindow.IsVisible());
        }

        protected override void OnUpdate()
        {
        }
    }
}
