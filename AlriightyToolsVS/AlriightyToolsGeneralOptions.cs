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

	}
}
