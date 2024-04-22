using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace AlriightyToolsVS
{
	public class AlriightyToolsGeneralOptions : DialogPage
	{

		[Category("Debugging")]
		[DisplayName("Connection Port")]
		[Description("The port that Alriighty Editor is expected to use for the debugger. This value should match the Debugger Port value in the Alriighty Editor project settings")]
		public int ConnectionPort { get; set; } = 2550;

		[Category("Debugging")]
		[DisplayName("Maximum Connection Attempts")]
		[Description("Determines how many connection attempts Alriighty Tools can make if it fails to attach to Alriighty Editor (0 means inifite attempts. Default: 1)")]
		public int MaxConnectionAttempts { get; set; } = 1;

        [Category("Debugging")]
        [DisplayName("Always Use Configured Executable")]
        [Description("When disabled, Visual Studio will attempt to get the Alriighty Editor executable path from a running Godot editor instance")]
        public bool AlwaysUseConfiguredExecutable { get; set; } = false;

        [Category("Debugging")]
        [DisplayName("AlriightyEditor Executable Path")]
        [Description("Path to the Alriighty Editor executable to use when launching the application for debugging")]
        public string AlriightyEditorExecutablePath { get; set; } = "";

        [Category("Debugging")]
        [DisplayName("Debugger Listen Timeout")]
        [Description("Time in milliseconds after which the debugging session will end if no debugger is connected")]
        public int DebuggerListenTimeout { get; set; } = 10000;

        /*[Category("Code Completion")]
        [DisplayName("Provide Node Path Completions")]
        [Description("Whether to provide code completion for node paths when the Alriighty Editor is connected")]
        public bool ProvideNodePathCompletions { get; set; } = true;

        [Category("Code Completion")]
        [DisplayName("Provide Input Action Completions")]
        [Description("Whether to provide code completion for input actions when the Alriighty Editor is connected")]
        public bool ProvideInputActionCompletions { get; set; } = true;

        [Category("Code Completion")]
        [DisplayName("Provide Resource Path Completions")]
        [Description("Whether to provide code completion for resource paths when the Alriighty Editor is connected")]
        public bool ProvideResourcePathCompletions { get; set; } = true;

        [Category("Code Completion")]
        [DisplayName("Provide Scene Path Completions")]
        [Description("Whether to provide code completion for scene paths when the Alriighty Editor is connected")]
        public bool ProvideScenePathCompletions { get; set; } = true;

        [Category("Code Completion")]
        [DisplayName("Provide Signal Name Completions")]
        [Description("Whether to provide code completion for signal names when the Alriighty Editor is connected")]
        public bool ProvideSignalNameCompletions { get; set; } = true;*/

    }
}
