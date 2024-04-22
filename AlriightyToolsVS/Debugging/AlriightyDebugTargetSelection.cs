using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlriightyToolsVS.Debugging
{
    public class AlriightyDebugTargetSelection : IVsProjectCfgDebugTargetSelection
    {
        private static readonly List<AlriightyDebugTarget> DebugTargets = new List<AlriightyDebugTarget>()
        {
            // For now, I only support Attaching to the editor
            //new AlriightyDebugTarget(ExecutionType.PlayInEditor, "Play in Editor"),
            //new AlriightyDebugTarget(ExecutionType.Launch, "Launch Editor"),
            new AlriightyDebugTarget(ExecutionType.Attach, "Attach to Alriighty Editor")
        };

        public static readonly AlriightyDebugTargetSelection Instance = new AlriightyDebugTargetSelection();

        private IVsDebugTargetSelectionService _debugTargetSelectionService;

        public AlriightyDebugTarget CurrentDebugTarget { get; private set; } = DebugTargets.First();

        public void GetCurrentDebugTarget(out Guid pguidDebugTargetType,
            out uint pDebugTargetTypeId, out string pbstrCurrentDebugTarget)
        {
            pguidDebugTargetType = CurrentDebugTarget.Guid;
            pDebugTargetTypeId = CurrentDebugTarget.Id;
            pbstrCurrentDebugTarget = CurrentDebugTarget.Name;
        }

        public Array GetDebugTargetListOfType(Guid guidDebugTargetType, uint debugTargetTypeId)
        {
            return DebugTargets
                .Where(t => t.Guid == guidDebugTargetType && t.Id == debugTargetTypeId)
                .Select(t => t.Name).ToArray();
        }

        public bool HasDebugTargets(
            IVsDebugTargetSelectionService pDebugTargetSelectionService, out Array pbstrSupportedTargetCommandIDs)
        {
            _debugTargetSelectionService = pDebugTargetSelectionService;
            pbstrSupportedTargetCommandIDs = DebugTargets
                .Select(t => $"{t.Guid}:{t.Id}").ToArray();
            return true;
        }

        public void SetCurrentDebugTarget(Guid guidDebugTargetType,
            uint debugTargetTypeId, string bstrCurrentDebugTarget)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            CurrentDebugTarget = DebugTargets
                .First(t => t.Guid == guidDebugTargetType && t.Id == debugTargetTypeId);
            _debugTargetSelectionService?.UpdateDebugTargets();
        }
    }
}
