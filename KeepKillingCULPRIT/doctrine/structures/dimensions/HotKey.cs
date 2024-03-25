using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

using KeepKillingCULPRIT.doctrine.structures.enums;

namespace KeepKillingCULPRIT.doctrine.structures.dimensions
{
	public class Hotkey
	{
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr windowHandle, int id, KeyboardCode.Modifier modifier, KeyboardCode.Key key);
		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr windowHandle, int id);

		private const int windowMessageHotkey = 0x0312;
		private readonly Window invokerWindow;
		private readonly IntPtr windowHandle;
		private readonly HwndSource windowSource;
		private Dictionary<HotkeyAction, HotkeyRegister> register { get; set; }

		public Hotkey(Window invokerWindow)
		{
			try
			{
				this.invokerWindow = invokerWindow;

				this.windowHandle = new WindowInteropHelper(this.invokerWindow).Handle;
				this.windowSource = HwndSource.FromHwnd(this.windowHandle);
				this.windowSource.AddHook(this.windowHook);
				this.invokerWindow.Closing += this.HotkeyClosing;

				this.register = new Dictionary<HotkeyAction, HotkeyRegister>() { };
			}
			catch (Exception exception)
			{
				throw new Exception("Construction unsuccess.", exception);
			}
		}

		private IntPtr windowHook(IntPtr windowHandle, int message, IntPtr wordParam, IntPtr longParam, ref bool handled)
		{
			if (message == Hotkey.windowMessageHotkey)
			{
				foreach (HotkeyAction action in this.register.Keys)
				{
					if ((int)wordParam == (int)action)
					{
						this.register[action].invoke.Invoke();
					}
				}
			}
			return IntPtr.Zero;
		}

		public bool addRegister(HotkeyAction action, HotkeyCombination keyCombination, Action invoke)
		{
			bool success = false;

			try
			{
				HotkeyRegister hotkeyRegister = new HotkeyRegister { action = action, keyCombination =  keyCombination, invoke = invoke };
				if (Hotkey.RegisterHotKey(this.windowHandle, (int)action, keyCombination.modifier, keyCombination.key))
				{
					this.register.Add(action, hotkeyRegister);
				}
				else
				{
					throw new Exception("Registration unsuccess.");
				}
				success = true;
			}
			catch (Exception exception)
			{
				throw new Exception("Register did not add.", exception);
			}

			return success;
		}

		private void HotkeyClosing(object sender, EventArgs e)
		{
			try
			{
				this.windowSource.RemoveHook(this.windowHook);
				foreach (HotkeyAction action in this.register.Keys)
				{
					Hotkey.UnregisterHotKey(this.windowHandle, (int)action);
				}
			}
			catch (Exception exception)
			{
				throw new Exception("Unregistration unsuccess.", exception);
			}
		}
	}
}