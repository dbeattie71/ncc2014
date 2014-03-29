using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using DemoApp.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace DemoApp.Touch
{
	public partial class SpeakerView : MvxViewController
	{
		public SpeakerView () : base ("SpeakerView", null)
		{
		}

		MvxImageViewLoader _speakerImageViewLoader;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_speakerImageViewLoader = new MvxImageViewLoader (() => SpeakerImageView);
			
			var set = this.CreateBindingSet<SpeakerView, SpeakerViewModel> ();
			set.Bind (_speakerImageViewLoader).To (s => s.Speaker.ImageUrl);
			set.Bind (NameLabel).To (s => s.Speaker.Name);
			set.Bind (BioLabel).To (s => s.Speaker.Bio);
			set.Apply ();
		}
	}
}

