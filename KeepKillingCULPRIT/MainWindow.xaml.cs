﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using KeepKillingCULPRIT.doctrine.job.stacker;
using KeepKillingCULPRIT.doctrine.structures.dimensions;
using KeepKillingCULPRIT.doctrine.structures.enums;
using KeepKillingCULPRIT.doctrine.structures.vectors;
using KeepKillingCULPRIT.doctrine.threads.grasses;
using KeepKillingCULPRIT.doctrine.transactions.derivatives;

namespace KeepKillingCULPRIT
{
	public partial class MainWindow : Window
	{
		#region window

		private Storyboard topBarStoryboard { get; set; }
		private Storyboard bottomBarStoryboard { get; set; }
		private Storyboard panelBarStoryboard { get; set; }
		private Storyboard actionSetStoryboard { get; set; }
		private Storyboard panelWordforestStoryboard { get; set; }
		private HostRestclient hostRestclient { get; set; }

		#endregion

		#region topBar

		private readonly DateTime appStartedAt;
		private DispatcherTimer timerTimeElapsed { get; set; }

		#endregion

		#region bottomBar

		private List<StackPanel> panels { get; set; }
		private StackPanel panelCurrent { get; set; }
		private List<StackPanel> actions { get; set; }

		#endregion

		#region panelBar

		private ulong killerKillCountCurrent;
		private DispatcherTimer timerKillerKillAutomatic { get; set; }
		private DispatcherTimer timerWordforestRefreshAutomatic { get; set; }

		#endregion

		public MainWindow()
		{
			this.InitializeComponent();

			// Construct values
			this.appStartedAt = DateTime.Now;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			try
			{
				// Feature invasive
				this.invasiveHotkeys();
				this.invasiveStoryboards();

				// Register parse
				this.parseTimers();

				// Value origin
				this.originCovers();

				// Trial bypass
				this.bypassCovers();

				// Maneuver pool
				this.poolHostRestclient();

				//
				this.wordforestRefreshWordsAsync();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message + Environment.NewLine + "Application shutting down.");
				Application.Current.Shutdown();
			}
		}

		private void invasiveHotkeys()
		{
			Hotkey hotkey = new Hotkey(this);
			hotkey.addRegister(HotkeyAction.Action1, new HotkeyCombination { modifier = KeyboardCode.Modifier.Alt, key = KeyboardCode.Key.S }, this.topBarToggleButtonSummeryToggle);
			hotkey.addRegister(HotkeyAction.Action91, new HotkeyCombination { modifier = KeyboardCode.Modifier.Control, key = KeyboardCode.Key.Left }, this.slidePanelToLeft);
			hotkey.addRegister(HotkeyAction.Action92, new HotkeyCombination { modifier = KeyboardCode.Modifier.Control, key = KeyboardCode.Key.Right }, this.slidePanelToRight);
			hotkey.addRegister(HotkeyAction.Action11, new HotkeyCombination { modifier = KeyboardCode.Modifier.Alt, key = KeyboardCode.Key.Space }, this.killerKill);
			hotkey.addRegister(HotkeyAction.Action12, new HotkeyCombination { modifier = KeyboardCode.Modifier.Alt, key = KeyboardCode.Key.A }, this.actionKillerToggleButtonKillAutomaticToggle);
			hotkey.addRegister(HotkeyAction.Action13, new HotkeyCombination { modifier = KeyboardCode.Modifier.Control, key = KeyboardCode.Key.Up }, this.wordforestRefreshWordsAsync);
			hotkey.addRegister(HotkeyAction.Action14, new HotkeyCombination { modifier = KeyboardCode.Modifier.Alt, key = KeyboardCode.Key.W }, this.actionWordforestToggleButtonRefreshAutomaticToggle);
		}

		private void invasiveStoryboards()
		{
			this.topBarStoryboard = (Storyboard)this.FindResource("topBarStoryboard");
			this.bottomBarStoryboard = (Storyboard)this.FindResource("bottomBarStoryboard");
			this.panelBarStoryboard = (Storyboard)this.FindResource("panelBarStoryboard");
			this.actionSetStoryboard = (Storyboard)this.FindResource("actionSetStoryboard");
			this.panelWordforestStoryboard = (Storyboard)this.FindResource("panelWordforestStoryboard");
		}

		private void parseTimers()
		{
			this.timerTimeElapsedAsync();
			this.timerKillerKillAutomaticAsync();
			this.timerWordforestRefreshAutomaticAsync();
		}

		private void originCovers()
		{
			this.hostRestclient = (HostRestclient)new Distributor().getTribute(Tribute.HostRestclient);
			this.panels = new List<StackPanel>();
			foreach (StackPanel panel in this.panelSet.Children)
			{
				this.panels.Add(panel);
				this.bottomBarComboBoxPanelSwitcher.Items.Add(panel.Tag);
			}
			this.actions = new List<StackPanel>();
			foreach (StackPanel action in this.actionSet.Children)
			{
				this.actions.Add(action);
			}

			this.bottomBarComboBoxPanelSwitcher.SelectedIndex = 0;
			this.killerKillCountCurrent = 0;
			this.panelKillerTextBlockKillCountCurrent.Text = this.killerKillCountCurrent.ToString();
			this.topBarTextBlockKillCountCurrent.Text = this.panelKillerTextBlockKillCountCurrent.Text;
		}

		private bool bypassCovers()
		{
			bool success = true;
			return success;
		}

		private void poolHostRestclient()
		{
			RestclientConfiguration itsthisforthatRestclientConfiguration = new RestclientConfiguration("https://itsthisforthat.com");
			this.hostRestclient.addVoucher("itsthisforthat", "text", new Restclient(itsthisforthatRestclientConfiguration, HttpVerb.POST, "/api.php?text"));
			this.hostRestclient.addVoucher("itsthisforthat", "json", new Restclient(itsthisforthatRestclientConfiguration, HttpVerb.POST, "/api.php?json"));

			RestclientConfiguration techyapiRestclientConfiguration = new RestclientConfiguration("https://techy-api.vercel.app");
			this.hostRestclient.addVoucher("techyapi", "text", new Restclient(techyapiRestclientConfiguration, HttpVerb.GET, "/api/text"));

			RestclientConfiguration uselessfactsRestclientConfiguration = new RestclientConfiguration("https://uselessfacts.jsph.pl");
			this.hostRestclient.addVoucher("uselessfacts", "json", new Restclient(uselessfactsRestclientConfiguration, HttpVerb.GET, "/api/v2/facts/random"));
		}

		#region topBar

		private void topBarToggleButtonKeepAttopChecked(object sender, RoutedEventArgs e)
		{
			this.Topmost = true;
		}

		private void topBarToggleButtonKeepAttopUnchecked(object sender, RoutedEventArgs e)
		{
			this.Topmost = false;
		}

		private void topBarToggleButtonSummeryChecked(object sender, RoutedEventArgs e)
		{
			this.Height = 133;
			this.panelBar.Visibility = Visibility.Hidden;
		}

		private void topBarToggleButtonSummeryUnchecked(object sender, RoutedEventArgs e)
		{
			this.Height = 400;
			this.panelBar.Visibility = Visibility.Visible;
		}

		private void topBarToggleButtonSummeryToggle()
		{
			if (this.topBarToggleButtonSummery.IsChecked ?? false)
			{
				this.topBarToggleButtonSummery.IsChecked = false;
			}
			else if (!this.topBarToggleButtonSummery.IsChecked ?? false)
			{
				this.topBarToggleButtonSummery.IsChecked = true;
			}
		}

		private async void timerTimeElapsedAsync()
		{
			this.timerTimeElapsed = await this.timerTimeElapsedTaskAsync();
			this.timerTimeElapsedTick(null, null);
			this.timerTimeElapsed.Start();
		}

		private async Task<DispatcherTimer> timerTimeElapsedTaskAsync()
		{
			await Task.Delay(0);
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 1);
			timer.Tick += this.timerTimeElapsedTick;
			return timer;
		}

		private void timerTimeElapsedTick(object sender, EventArgs e)
		{
			this.topBarTextBlockTimeElapsed.Text = this.appStartedAt.ToLongTimeString() + " | " + DateTime.Now.ToLongTimeString() + " | " + DateTime.Now.Subtract(this.appStartedAt).ToString(@"hh\:mm\:ss");
		}

		#endregion

		#region bottomBar

		private void bottomBarComboBoxPanelSwitcherSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			foreach (StackPanel panel in this.panels)
			{
				if (this.bottomBarComboBoxPanelSwitcher.SelectedValue.ToString() == panel.Tag.ToString())
				{
					this.showPanel(panel);
					this.topBarStoryboard.Begin();
					this.bottomBarStoryboard.Begin();
					this.panelBarStoryboard.Begin();
				}
			}
		}

		private void showPanel(StackPanel panelToShow = null)
		{
			foreach (StackPanel panel in this.panels)
			{
				panel.Visibility = Visibility.Collapsed;
				if (panelToShow != null && panelToShow == panel)
				{
					this.panelCurrent = panel;
					this.panelCurrent.Visibility = Visibility.Visible;
				}
			}
			foreach (StackPanel action in this.actions)
			{
				action.Visibility = Visibility.Collapsed;
				if (panelToShow != null && panelToShow.Tag.ToString() == action.Tag.ToString())
				{
					action.Visibility = Visibility.Visible;
					this.actionSetStoryboard.Begin();
				}
			}
		}

		private void slidePanelToLeft()
		{
			int? indexCurrent = this.getPanelIndexCurrent();
			if (indexCurrent != null)
			{
				int indexNew = (int)indexCurrent - 1;
				if (indexNew == -1)
				{
					indexNew = this.panels.Count - 1;
				}
				this.bottomBarComboBoxPanelSwitcher.SelectedIndex = indexNew;
			}
		}

		private void slidePanelToRight()
		{
			int? indexCurrent = this.getPanelIndexCurrent();
			if (indexCurrent != null)
			{
				int indexNew = (int)indexCurrent + 1;
				if (indexNew == this.panels.Count)
				{
					indexNew = 0;
				}
				this.bottomBarComboBoxPanelSwitcher.SelectedIndex = indexNew;
			}
		}

		private int? getPanelIndexCurrent()
		{
			int indexCurrent = -1;
			bool indexFound = false;
			foreach (StackPanel panel in this.panels)
			{
				indexCurrent++;
				if (this.panelCurrent == panel)
				{
					indexFound = true;
					break;
				}
			}
			if (indexFound)
			{
				return indexCurrent;
			}
			return null;
		}

		#endregion

		#region panelBar

		#region panelKiller

		private void actionKillerButtonKillManualClick(object sender, RoutedEventArgs e)
		{
			this.killerKill();
		}

		private async void timerKillerKillAutomaticAsync()
		{
			this.timerKillerKillAutomatic = await this.timerKillerKillAutomaticTaskAsync();
		}

		private async Task<DispatcherTimer> timerKillerKillAutomaticTaskAsync()
		{
			await Task.Delay(0);
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
			timer.Tick += this.timerKillerKillAutomaticTick;
			return timer;
		}

		private void timerKillerKillAutomaticTick(object sender, EventArgs e)
		{
			this.killerKill();
		}

		private void actionKillerToggleButtonKillAutomaticChecked(object sender, RoutedEventArgs e)
		{
			this.timerKillerKillAutomatic.Start();
		}

		private void actionKillerToggleButtonKillAutomaticUnchecked(object sender, RoutedEventArgs e)
		{
			this.timerKillerKillAutomatic.Stop();
		}

		private void actionKillerToggleButtonKillAutomaticToggle()
		{
			if (this.actionKillerToggleButtonKillAutomatic.IsChecked ?? false)
			{
				this.actionKillerToggleButtonKillAutomatic.IsChecked = false;
			}
			else if (!this.actionKillerToggleButtonKillAutomatic.IsChecked ?? false)
			{
				this.actionKillerToggleButtonKillAutomatic.IsChecked = true;
			}
		}

		private void killerKill()
		{
			string killCountCurrentString = (++this.killerKillCountCurrent).ToString();
			this.panelKillerTextBlockKillCountCurrent.Text = killCountCurrentString;
			this.topBarTextBlockKillCountCurrent.Text = killCountCurrentString;
		}

		#endregion

		#region panelWordforest

		private void actionWordforestButtonRefreshManualClick(object sender, RoutedEventArgs e)
		{
			this.wordforestRefreshWordsAsync();
		}

		private async void timerWordforestRefreshAutomaticAsync()
		{
			this.timerWordforestRefreshAutomatic = await this.timerWordforestRefreshAutomaticTaskAsync();
		}

		private async Task<DispatcherTimer> timerWordforestRefreshAutomaticTaskAsync()
		{
			await Task.Delay(0);
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 0, 0, 3000);
			timer.Tick += this.timerWordforestRefreshAutomaticTick;
			return timer;
		}

		private void timerWordforestRefreshAutomaticTick(object sender, EventArgs e)
		{
			this.wordforestRefreshWordsAsync();
		}

		private void actionWordforestToggleButtonRefreshAutomaticChecked(object sender, RoutedEventArgs e)
		{
			this.timerWordforestRefreshAutomatic.Start();
		}

		private void actionWordforestToggleButtonRefreshAutomaticUnchecked(object sender, RoutedEventArgs e)
		{
			this.timerWordforestRefreshAutomatic.Stop();
		}

		private void actionWordforestToggleButtonRefreshAutomaticToggle()
		{
			if (this.actionWordforestToggleButtonRefreshAutomatic.IsChecked ?? false)
			{
				this.actionWordforestToggleButtonRefreshAutomatic.IsChecked = false;
			}
			else if (!this.actionWordforestToggleButtonRefreshAutomatic.IsChecked ?? false)
			{
				this.actionWordforestToggleButtonRefreshAutomatic.IsChecked = true;
			}
		}

		private async void wordforestRefreshWordsAsync()
		{
			await this.wordforestRefreshWordsFromInternetAsync();
		}

		private async Task wordforestRefreshWordsFromInternetAsync()
		{
			this.panelWordforestTextBlock1.Text = await this.hostRestclient.seriesCheckinAsync("itsthisforthat", "text");
			this.panelWordforestTextBlock2.Text = await this.hostRestclient.seriesCheckinAsync("techyapi", "text") + "." + Environment.NewLine + await this.hostRestclient.seriesCheckinAsync("techyapi", "text") + ".";
			this.panelWordforestTextBlock3.Text = await this.hostRestclient.seriesCheckinAsync("itsthisforthat", "text");
			//this.panelWordforestStoryboard.Begin();
		}

		#endregion

		#region panelWordpoc

		private void panelWordpocTextBox1TextChanged(object sender, TextChangedEventArgs e)
		{
			string wordpoc = ((TextBox)sender).Text;
			if (wordpoc.Length > 0)
			{
				int sum = 0;
				foreach (char c in wordpoc.ToCharArray())
				{
					switch (c) {
						case 'a': case 'A': sum += 3; break;
						case 'b': case 'B': sum += 2; break;
						case 'c': case 'C': sum += 3; break;
						case 'd': case 'D': sum += 3; break;
						case 'e': case 'E': sum += 3; break;
						case 'f': case 'F': sum += 5; break;
						case 'g': case 'G': sum += 3; break;
						case 'h': case 'H': sum += 4; break;
						case 'i': case 'I': sum += 3; break;
						case 'j': case 'J': sum += 5; break;
						case 'k': case 'K': sum += 5; break;
						case 'l': case 'L': sum += 7; break;
						case 'm': case 'M': sum += 5; break;
						case 'n': case 'N': sum += 5; break;
						case 'o': case 'O': sum += 5; break;
						case 'p': case 'P': sum += 5; break;
						case 'q': case 'Q': sum += 4; break;
						case 'r': case 'R': sum += 4; break;
						case 's': case 'S': sum += 5; break;
						case 't': case 'T': sum += 3; break;
						case 'u': case 'U': sum += 5; break;
						case 'v': case 'V': sum += 2; break;
						case 'w': case 'W': sum += 7; break;
						case 'x': case 'X': sum += 1; break;
						case 'y': case 'Y': sum += 6; break;
						case 'z': case 'Z': sum += 6; break;
					}
				}
				this.panelWordpocTextBlock1.Text = sum.ToString();
			}
			else
			{
				this.panelWordpocTextBlock1.Text = string.Empty;
			}
		}

		#endregion

		#region panelConfiguration

		#endregion

		#endregion

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
		}
	}
}