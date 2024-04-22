using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using AlriightyToolsVS.Debugging;

namespace AlriightyToolsVS
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid(AlriightyToolsPackage.AlriightyProjectGuid)]
    internal class AlriightyFlavoredProject : FlavoredProjectBase, IVsProjectFlavorCfgProvider
    {
        private IVsProjectFlavorCfgProvider _innerFlavorConfig;
        private AlriightyToolsPackage _package;

        public AlriightyFlavoredProject(AlriightyToolsPackage package)
        {
            _package = package;
        }

        public int CreateProjectFlavorCfg(IVsCfg pBaseProjectCfg, out IVsProjectFlavorCfg ppFlavorCfg)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            ppFlavorCfg = null;

            if (_innerFlavorConfig != null)
            {
                GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out var project);

                _innerFlavorConfig.CreateProjectFlavorCfg(pBaseProjectCfg, out IVsProjectFlavorCfg cfg);
                ppFlavorCfg = new AlriightyDebuggableProjectCfg(cfg, project as EnvDTE.Project);
            }

            return ppFlavorCfg != null ? VSConstants.S_OK : VSConstants.E_FAIL;
        }

        protected override void SetInnerProject(IntPtr innerIUnknown)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            object inner = Marshal.GetObjectForIUnknown(innerIUnknown);
            _innerFlavorConfig = inner as IVsProjectFlavorCfgProvider;

            if (serviceProvider == null)
                serviceProvider = _package;

            base.SetInnerProject(innerIUnknown);
        }
    }
}
