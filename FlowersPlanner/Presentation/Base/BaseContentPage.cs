using System;
using ReactiveUI.XamForms;
using Xamarin.Forms.Internals;

namespace FlowersPlanner.Presentation.Base
{
    public class BaseContentPage<TViewModel> : ReactiveContentPage<TViewModel> where TViewModel : class
    {
        private double _width;
        private double _height;

        public BaseContentPage()
        {
            _width = this.Width;
            _height = this.Height;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            var oldWidth = _width;
            const double sizenotallocated = -1;

            base.OnSizeAllocated(width, height);
            if (Equals(_width, width) && Equals(_height, height)) return;

            _width = width;
            _height = height;

            // ignore if the previous height was size unallocated
            if (Equals(oldWidth, sizenotallocated)) return;

            // Has the device been rotated ?
            if (!Equals(width, oldWidth))
            {
                var orientation = (width < height) ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
                OnOrientationChanged(width, height, orientation);
            }
        }

        protected virtual void OnOrientationChanged(double width, double height, DeviceOrientation orientation) { }
    }
}
