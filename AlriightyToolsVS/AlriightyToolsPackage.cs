using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Mono.Debugging.Client;
using Task = System.Threading.Tasks.Task;

namespace AlriightyToolsVS
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    /// 

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[Guid(PackageGuidString)]
    [ProvideProjectFactory(typeof(AlriightyFlavoredProjectFactory), "Alriighty.Project", null, "csproj", "csproj", null,
       LanguageVsTemplate = "CSharp", TemplateGroupIDsVsTemplate = "Alriighty")]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideOptionPage(typeof(AlriightyToolsGeneralOptions), "Alriighty Tools", "General", 0, 0, true)]
/*	[ProvideUIContextRule(UIContextHasCSProjectGuid, name: "Has CSharp Project",
		expression: "HasCSProject",
		termNames: new[] { "HasCSProject" },
		termValues: new[] { "SolutionHasProjectCapability:CSharp" })]*/
	public sealed class AlriightyToolsPackage : AsyncPackage
	{
		/// <summary>
		/// GodotPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "c7a2ebd8-63d8-4332-b696-67ca11f7f971";
		public const string AlriightyProjectGuid = "8F3E2DF0-C35C-4265-82FC-BEA011F4A7ED";//"477ACBA2-8465-4B8A-AFDA-600F571BCD88"; // Alriighty Engine ProjectTypeGuid

        private const string UIContextHasCSProjectGuid = "df4efbdd-f234-4d5c-a753-4b50e0837327";

		#region Package Members

		public static AlriightyToolsPackage Instance { get; private set; }

		public AlriightyToolsPackage()
		{
			Instance = this;
		}

		internal AlriightySolutionEventsListener SolutionEventsListener { get; private set; }
		internal AlriightyToolsGeneralOptions GeneralOptions => GetDialogPage(typeof(AlriightyToolsGeneralOptions)) as AlriightyToolsGeneralOptions;

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
		/// <param name="progress">A provider for progress updates.</param>
		/// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			
			// When initialized asynchronously, the current thread may be a background thread at this point.
			// Do any initialization that requires the UI thread after switching to the UI thread.
			await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
			await AttachEditorCommand.InitializeAsync(this);

            RegisterProjectFactory(new AlriightyFlavoredProjectFactory(this));

            SolutionEventsListener = new AlriightySolutionEventsListener(this);

		}

		public async Task ShowErrorMessageBoxAsync(string title, string message)
		{
			await JoinableTaskFactory.SwitchToMainThreadAsync();

			var uiShell = (IVsUIShell)await GetServiceAsync(typeof(SVsUIShell));

			if (uiShell == null)
				throw new ServiceUnavailableException(typeof(SVsUIShell));

			var clsID = Guid.Empty;
			Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
				0,
				ref clsID,
				title,
				message,
				string.Empty,
				0,
				OLEMSGBUTTON.OLEMSGBUTTON_OK,
				OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
				OLEMSGICON.OLEMSGICON_CRITICAL,
				0,
				pnResult: out _
			));
		}

		#endregion
	}
}
