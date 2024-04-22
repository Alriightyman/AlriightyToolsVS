using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using EnvDTE;
using AlriightyToolsVS.Debugging;
//using AlriightyToolsVS.GodotMessaging;
/*using GodotTools.IdeMessaging;
using GodotTools.IdeMessaging.Requests;*/
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Windows.Forms;

namespace AlriightyToolsVS
{
	internal class AlriightySolutionEventsListener : SolutionEventsListener
	{

        private static readonly object RegisterLock = new object();
        private bool _registered;

        private string _alriightyProjectDir;

        private DebuggerEvents DebuggerEvents { get; set; }

        private IServiceContainer ServiceContainer => (IServiceContainer)ServiceProvider;

        //7public Client GodotMessagingClient { get; private set; }

        public AlriightySolutionEventsListener(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			Init();
		}

		public string SolutionDirectory
		{
			get
			{
				ThreadHelper.ThrowIfNotOnUIThread();
				Solution.GetSolutionInfo(out string solutionDirectory, out string solutionFile, out string userOptsFile);
				_ = solutionFile;
				_ = userOptsFile;
				return solutionDirectory;
			}
		}

        public override int OnBeforeCloseProject(IVsHierarchy hierarchy, int removed)
        {
            return 0;
        }

        private static IEnumerable<Guid> ParseProjectTypeGuids(string projectTypeGuids)
        {
            string[] strArray = projectTypeGuids.Split(';');
            var guidList = new List<Guid>(strArray.Length);

            foreach (string input in strArray)
            {
                if (Guid.TryParse(input, out var result))
                    guidList.Add(result);
            }

            return guidList.ToArray();
        }

        private static bool IsAlriightyProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return hierarchy is IVsAggregatableProject aggregatableProject &&
                   aggregatableProject.GetAggregateProjectTypeGuids(out string projectTypeGuids) == 0 &&
                   ParseProjectTypeGuids(projectTypeGuids)
                       .Any(g => g == typeof(AlriightyFlavoredProjectFactory).GUID);
        }

        public override int OnAfterOpenProject(IVsHierarchy hierarchy, int added)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!IsAlriightyProject(hierarchy))
                return 0;

            lock (RegisterLock)
            {
                if (_registered)
                    return 0;

                _alriightyProjectDir = SolutionDirectory;

                // TODO: This will be useful in the future when communicating to the editor
                /*DebuggerEvents = ServiceProvider.GetService<DTE>().Events.DebuggerEvents;
                DebuggerEvents.OnEnterDesignMode += DebuggerEvents_OnEnterDesignMode;

                GodotMessagingClient?.Dispose();
                GodotMessagingClient = new Client(identity: "VisualStudio",
                    _alriightyProjectDir, new MessageHandler(), GodotPackage.Instance.Logger);
                GodotMessagingClient.Connected += OnClientConnected;
                GodotMessagingClient.Start();

                ServiceContainer.AddService(typeof(Client), GodotMessagingClient);*/

                _registered = true;
            }

            return 0;
        }

        public override int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return 0;
        }

        public override int OnBeforeCloseSolution(object pUnkReserved)
        {
            lock (RegisterLock)
                _registered = false;
            Close();
            return 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            Close();
        }

        private void OnClientConnected()
        {
            var options = (AlriightyToolsGeneralOptions)AlriightyToolsPackage.Instance.GetDialogPage(typeof(AlriightyToolsGeneralOptions));

            // If the setting is not yet assigned any value, set it to the currently connected Alriighty editor path
           /* if (string.IsNullOrEmpty(options.AlriightyEditorExecutablePath))
            {
                string editorPath = GodotMessagingClient?.GodotEditorExecutablePath;
                if (!string.IsNullOrEmpty(editorPath) && File.Exists(editorPath))
                    options.AlriightyEditorExecutablePath = editorPath;
            }*/
        }

        private void DebuggerEvents_OnEnterDesignMode(dbgEventReason reason)
        {
           /* if (reason != dbgEventReason.dbgEventReasonStopDebugging)
                return;

            if (GodotMessagingClient == null || !GodotMessagingClient.IsConnected)
                return;

            var currentDebugTarget = AlriightyDebugTargetSelection.Instance.CurrentDebugTarget;*/

            /*if (currentDebugTarget != null && currentDebugTarget.ExecutionType == ExecutionType.PlayInEditor)
                _ = GodotMessagingClient.SendRequest<StopPlayResponse>(new StopPlayRequest());*/
        }

        private void Close()
        {
            //ThreadHelper.ThrowIfNotOnUIThread();
            /*if (GodotMessagingClient != null)
            {
                ServiceContainer.RemoveService(typeof(Client));
                GodotMessagingClient.Dispose();
                GodotMessagingClient = null;
            }*/

            /*if (DebuggerEvents != null)
            {
                DebuggerEvents.OnEnterDesignMode -= DebuggerEvents_OnEnterDesignMode;
                DebuggerEvents = null;
            }*/
        }
    }
}
