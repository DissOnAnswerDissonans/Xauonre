using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Core;

namespace MiniXauonre.Graphics
{
    class MapView : Panel
    {
        private readonly MapPainter painter;
        private bool dragInProgress;
		private Point dragStart;
		private PointF dragStartCenter;
		private PointF mouseLogicalPos;
		//private int zoomScale;
	    
	    //public bool FitToWindow { get; set; }
	    
		public MapView(MapPainter painter) : this()
		{
			this.painter = painter;
		}		 

		public MapView()
		{
			ZoomScale = 3;
		}
			    
		public PointF MouseLogicalPos => mouseLogicalPos;

		public PointF CenterLogicalPos { get; set; }  
		public int ZoomScale { get; set; }

		protected override void InitLayout()
		{
			base.InitLayout();
			ResizeRedraw = true;
			DoubleBuffered = true;
		}   

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
		}
			    

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Right)
			{
				dragInProgress = true;
				dragStart = e.Location;
				dragStartCenter = CenterLogicalPos;
			}
			else if (e.Button == MouseButtons.Left)
			{
				//painter.OnMouseDown(Point.Truncate(mouseLogicalPos));
			}
		}	
	    
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			dragInProgress = false;
            //painter.OnMouseUp();
            Invalidate();
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			mouseLogicalPos = ToLogical(e.Location);
			if (dragInProgress)
			{
				var loc = e.Location;
				var dx = (loc.X - dragStart.X);
				var dy = (loc.Y - dragStart.Y);
				CenterLogicalPos = new PointF(dragStartCenter.X - dx, dragStartCenter.Y - dy);
				Invalidate();
			}
		}
		
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
            if (e.Delta < 0 && ZoomScale < 3)
            {
                ZoomScale++;
                CenterLogicalPos = new PointF(CenterLogicalPos.X / 2, CenterLogicalPos.Y / 2);
            }
            if (e.Delta > 0 && ZoomScale > 0)
            {
                ZoomScale--;
                CenterLogicalPos = new PointF(CenterLogicalPos.X * 2, CenterLogicalPos.Y * 2);
            }
			Invalidate();
		}
		    
		private PointF ToLogical(Point p)
		{
			var shift = GetShift();
			return new PointF(
				(p.X - shift.X) * (float)Math.Pow(2, ZoomScale),
				(p.Y - shift.Y) * (float)Math.Pow(2, ZoomScale));
		}
	    
		private PointF GetShift()
		{
			return new PointF(
				ClientSize.Width / 2f - CenterLogicalPos.X,
				ClientSize.Height / 2f - CenterLogicalPos.Y);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
            e.Graphics.Clear(Color.Black);
			if (painter == null) return;
			var sceneSize = painter.DrawSize;

			var shift = GetShift();
			e.Graphics.ResetTransform();
			e.Graphics.TranslateTransform(shift.X, shift.Y);
            painter.Paint(e.Graphics, ZoomScale);
        }
    }
}