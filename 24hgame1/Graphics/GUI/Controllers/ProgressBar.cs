using System;

namespace hgame1.Graphics.GUI.Controllers
{
	public class ProgressBar : GuiController<double>
	{

		public double Min {
			get;
			set;
		}

		public double Max {
			get;
			set;
		}

		public double Percentage {
			get {
				return (Value - Min) / (Max - Min);
			}
			set {
				Value = (value * (Max - Min) + Min);
			}
		}

		public ProgressBar ()
		{
			Min = 0;
			Max = 1.0;
		}
	}
}

