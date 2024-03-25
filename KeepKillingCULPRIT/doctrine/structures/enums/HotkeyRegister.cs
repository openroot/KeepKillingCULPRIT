using System;

namespace KeepKillingCULPRIT.doctrine.structures.enums
{
	public struct HotkeyRegister
	{
		public HotkeyAction action { get; set; }
		public HotkeyCombination keyCombination { get; set; }
		public Action invoke { get; set; }
	}
}
