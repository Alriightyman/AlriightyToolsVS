using EnvDTE;
using Mono.Debugging.Soft;
using Mono.Debugging.VisualStudio;

namespace AlriightyToolsVS.Debugging
{
    public enum AlriightySessionType
    {
        PlayInEditor = 0,
        AttachEditorDebugger
    }

    internal class AlriightyStartInfo : StartInfo
    {
        public readonly AlriightySessionType SessionType;
        public string StartArguments;
        public AlriightyStartInfo(SoftDebuggerStartArgs args, DebuggingOptions options, Project startupProject, AlriightySessionType sessionType)
            : base(args, options, startupProject)
        {
            SessionType = sessionType;
        }
    }
}
