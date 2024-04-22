using Microsoft.VisualStudio.Shell.Flavor;
using System;
using System.Runtime.InteropServices;

namespace AlriightyToolsVS
{
    [Guid(AlriightyToolsPackage.AlriightyProjectGuid)]
    public class AlriightyFlavoredProjectFactory : FlavoredProjectFactoryBase
    {
        private readonly AlriightyToolsPackage _package;

        public AlriightyFlavoredProjectFactory(AlriightyToolsPackage package)
        {
            _package = package;
        }

        protected override object PreCreateForOuter(IntPtr outerProjectIUnknown)
        {
            return new AlriightyFlavoredProject(_package);
        }
    }
}
