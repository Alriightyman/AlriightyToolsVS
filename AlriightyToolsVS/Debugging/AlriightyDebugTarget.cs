using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlriightyToolsVS.Debugging
{
    public class AlriightyDebugTarget
    {
        private const string DebugTargetsGuidStr = "4E50788E-B023-4F77-AFE9-797603876907"; // TODO: Regenerate - used in AlriightyToolsPackage.vsct
        public static readonly Guid DebugTargetsGuid = new Guid(DebugTargetsGuidStr);

        public Guid Guid { get; }

        public uint Id { get; }

        public string Name { get; }

        public ExecutionType ExecutionType { get; }

        public AlriightyDebugTarget(ExecutionType executionType, string name)
        {
            Guid = DebugTargetsGuid;
            Id = 0x8192 + (uint)executionType;
            ExecutionType = executionType;
            Name = name;
        }
    }

    public enum ExecutionType : uint
    {
        PlayInEditor = 0,
        Launch,
        Attach
    }
}
