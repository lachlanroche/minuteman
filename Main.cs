
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MinuteMan
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// If you have defined a view, add it here:
			// window.AddSubview (navigationController.View);
			
			app.StatusBarStyle = UIStatusBarStyle.Default;
			
			backdrop.Image = UIImage.FromFile("images/background.png");
			
			// startButton.TouchDown += new EventHandler( StartMeeting );

			var font = UIFont.FromName("Helvetica", 60.0f );
			
			var scrollFrame = scrollView.Frame;
			scrollFrame.Width = 2 * scrollFrame.Width;
			scrollView.ContentSize = scrollFrame.Size;
			
			costLabel = new UILabel{ TextColor = UIColor.White, BackgroundColor = UIColor.Clear, TextAlignment = UITextAlignment.Center, Font = font };
			var frame = scrollView.Frame;
			var location = new PointF{ Y = 0, X = 0 };
			frame.Location = location;
			costLabel.Frame = frame;
			scrollView.AddSubview( costLabel );
			
			timeLabel = new UILabel{ TextColor = UIColor.White, BackgroundColor = UIColor.Clear, TextAlignment = UITextAlignment.Center, Font = font };
			frame = scrollView.Frame;
			location = new PointF{ Y = 0, X = frame.Width };
			frame.Location = location;
			timeLabel.Frame = frame;
			scrollView.AddSubview( timeLabel );	
			
			scrollView.Scrolled += ScrollViewScrolled;
			pager.ValueChanged += PagerScrolled;
			
			// make sure the splash is seen
			Thread.Sleep( new TimeSpan( 0, 0, 1 ));
			
			
			StartMeeting(null, null); // TODO: not auto-start
			
			NSTimer.CreateScheduledTimer( 0.05, this, new Selector("UpdateTimer"), null, true );
			
			window.MakeKeyAndVisible();
			app.StatusBarStyle = UIStatusBarStyle.BlackOpaque;
			
			return true;
		}
		
		void ScrollViewScrolled( object sender, EventArgs e )
		{
			var current = (int) Math.Floor( scrollView.ContentOffset.X / scrollView.Frame.Width );
			
			pager.CurrentPage = current;
		}
		
		void PagerScrolled( object sender, EventArgs e )
		{
			var current = scrollView.ContentSize.Width / scrollView.Frame.Width;
			if (current == pager.CurrentPage) {
				return;
			}
					
			var offset = new PointF{ Y = 0, X = pager.CurrentPage * scrollView.Frame.Width };
			scrollView.SetContentOffset( offset, true );
		}
		
		UILabel timeLabel;
		UILabel costLabel;

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
		
		DateTime start_t = DateTime.MinValue;
		int cost = 120;
		int people = 2;
		bool is_running;
		
		void StartMeeting( object sender, EventArgs e )
		{
			is_running = !is_running;
			// startButton.SetTitle( is_running ? "Stop" : "Start", UIControlState.Normal );
			if (is_running) {
				start_t = DateTime.Now; //.AddMinutes( -125 );
			}
		}
		
		void Update()
		{
			TimeSpan t = DateTime.Now - start_t;
			string format = "{0:00}:{1:00}:{2:00}.{3:0}";
			/*
			if (t.Hours == 0 && t.Minutes == 0) {
				format = "{2:0}.{3:0}";
			} else if (t.Hours == 0) {
				format = "{1:0}:{2:00}.{3:0}";
			}
			*/
			if (t.Hours == 0) {
				format = "{1:0}:{2:00}.{3:0}";
			}
			timeLabel.Text = string.Format( format, t.Hours, t.Minutes, t.Seconds, t.Milliseconds/100 );
			costLabel.Text = string.Format("${0:0.00}", cost * t.TotalHours * people );
		}
			   
		[Export("UpdateTimer")]
		public void UpdateTimer()
		{
			if (is_running) {
				Update();
			}
		}
	}
}
