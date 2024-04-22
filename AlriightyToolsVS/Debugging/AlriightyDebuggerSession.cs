using System;
using System.ComponentModel.Design;
using System.IO;

using Mono.Debugging.Client;
using Mono.Debugging.Soft;
using Mono.Debugger.Soft;

namespace AlriightyToolsVS.Debugging
{
	internal class AlriightyDebuggerSession : SoftDebuggerSession
	{
		private bool m_IsAttached;

		protected override void OnRun(DebuggerStartInfo startInfo)
		{
			var alriightyStartInfo = startInfo as AlriightyStartInfo;

			switch (alriightyStartInfo.SessionType)
			{
			case AlriightySessionType.PlayInEditor:
				break;
			case AlriightySessionType.AttachEditorDebugger:
			{
				m_IsAttached = true;
				base.OnRun(alriightyStartInfo);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException(alriightyStartInfo.SessionType.ToString());
			}
		}

		protected override void OnConnectionError(Exception ex)
		{
			// The session was manually terminated
			if (HasExited)
			{
				base.OnConnectionError(ex);
				return;
			}

			if (ex is VMDisconnectedException || ex is IOException)
			{
				HasExited = true;
				base.OnConnectionError(ex);
				return;
			}

			string message = "An error occured when trying to attach to Alriighty Editor. Please make sure that Alriighty Editor is running and that it's up-to-date.";
			message += Environment.NewLine;
			message += string.Format("Message: {0}", ex != null ? ex.Message : "No error message provided.");

			if (ex != null)
			{
				message += Environment.NewLine;
				message += string.Format("Source: {0}", ex.Source);
				message += Environment.NewLine;
				message += string.Format("Stack Trace: {0}", ex.StackTrace);

				if (ex.InnerException != null)
				{
					message += Environment.NewLine;
					message += string.Format("Inner Exception: {0}", ex.InnerException.ToString());
				}
			}
			
			_ = AlriightyToolsPackage.Instance.ShowErrorMessageBoxAsync("Connection Error", message);
			base.OnConnectionError(ex);
		}

		protected override void OnExit()
		{
			if (m_IsAttached)
			{
				m_IsAttached = false;
				base.OnDetach();
			}
			else
			{
				base.OnExit();
			}
		}
	}
}
