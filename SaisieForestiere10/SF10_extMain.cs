using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Desktop.AddIns;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Editor;

namespace SaisieForestiere10
{
    public class SF10_extMain : ESRI.ArcGIS.Desktop.AddIns.Extension
    {
        public static IApplication m_pApp;
        public static IMxDocument m_pDoc;
        public static IEditor m_pEditor;

        private static IEditEvents_Event m_editorEvents;

        public static IFeatureLayer m_LayerName;

        private static SF10_extMain s_extension;
        private static IDockableWindow s_dockWindow;

        protected override void OnStartup()
        {
            s_extension = this;

            ArcMap.Events.NewDocument += ArcMap_NewOpenDocument;
            ArcMap.Events.OpenDocument += ArcMap_NewOpenDocument;

            Initialize();
        }

        protected override void OnShutdown()
        {
            Uninitialize();

            ArcMap.Events.NewDocument -= ArcMap_NewOpenDocument;
            ArcMap.Events.OpenDocument -= ArcMap_NewOpenDocument;

            m_pApp = null;
            m_pDoc = null;
            s_extension = null;
            
            base.OnShutdown();
        }

        protected override bool OnSetState(ExtensionState state)
        {
            // Optionally check for a license here
            this.State = state;

            if (state == ExtensionState.Enabled)
                Initialize();
            else
                Uninitialize();

            return base.OnSetState(state);
        }

        protected override ExtensionState OnGetState()
        {
          return this.State;
        }

        private void ArcMap_NewOpenDocument()
        {
            Initialize();
        }

        private static SF10_extMain GetExtension()
        {
          if (s_extension == null)
          {
            // Call FindExtension to load this just-in-time extension.
            UID id = new UIDClass();
            id.Value = ThisAddIn.IDs.SF10_extMain;
            ArcMap.Application.FindExtensionByCLSID(id);
          }

          return s_extension;
        }

        internal static IDockableWindow GetDockWindow()
        {
            if (s_extension == null)
                GetExtension();

            // Only get/create the dockable window if they ask for it
            if (s_dockWindow == null)
            {
                // Use GetDockableWindow directly intead of FromID as we want the client IDockableWindow not the internal class
                UID dockWinID = new UIDClass();
                dockWinID.Value = ThisAddIn.IDs.SF10_dckFrmSaisie;
                s_dockWindow = ArcMap.DockableWindowManager.GetDockableWindow(dockWinID);
            }

            return s_dockWindow;
        }

        private void Initialize()
        {
            // If the extension hasn't been started yet or it's been turned off, bail
            if (s_extension == null || this.State != ExtensionState.Enabled)
                return;

            m_pApp = ArcMap.Application;
            m_pDoc = ArcMap.Document;

            UID editorUid = new UIDClass();
            editorUid.Value = "esriEditor.Editor";
            IEditor m_pEditor = m_pApp.FindExtensionByCLSID(editorUid) as IEditor;

            m_editorEvents = m_pEditor as IEditEvents_Event;
            IEditEvents_OnSelectionChangedEventHandler editEvents_OnSelectionChangedEventHandler = new IEditEvents_OnSelectionChangedEventHandler(this.Editor_OnSelectionChanged);
            m_editorEvents.OnSelectionChanged += editEvents_OnSelectionChangedEventHandler;
        }

        private void Uninitialize()
        {
            if (s_extension == null)
                return;

            UID editorUid = new UIDClass();
            editorUid.Value = "esriEditor.Editor";
            IEditor m_pEditor = m_pApp.FindExtensionByCLSID(editorUid) as IEditor;

            m_editorEvents = m_pEditor as IEditEvents_Event;
            IEditEvents_OnSelectionChangedEventHandler editEvents_OnSelectionChangedEventHandler = new IEditEvents_OnSelectionChangedEventHandler(this.Editor_OnSelectionChanged);
            m_editorEvents.OnSelectionChanged -= editEvents_OnSelectionChangedEventHandler;

            // Update UI
            SF10_cmbLayers selCombo = SF10_cmbLayers.GetSelectionComboBox();
            if (selCombo != null)
                selCombo.ClearAll();
        }

        internal static bool IsExtensionEnabled()
        {
            if (s_extension == null)
                GetExtension();

            if (s_extension == null)
                return false;

            return s_extension.State == ExtensionState.Enabled;
        }

        private void Editor_OnSelectionChanged()
        {
            SF10_dckFrmSaisie.AddinImpl winImp = AddIn.FromID<SF10_dckFrmSaisie.AddinImpl>(ThisAddIn.IDs.SF10_dckFrmSaisie);
            SF10_dckFrmSaisie dnrWindow = winImp.UI;

            dnrWindow.UpdateSaisieDockWin();
        }
    }
}
