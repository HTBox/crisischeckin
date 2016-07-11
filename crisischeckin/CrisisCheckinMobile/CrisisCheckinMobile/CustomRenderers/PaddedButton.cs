using Xamarin.Forms;

namespace CrisisCheckinMobile.CustomRenderers
{
    class PaddedButton : Button
    {
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            var result = base.OnSizeRequest(widthConstraint, heightConstraint);

            return new SizeRequest(new Size(result.Request.Width + 20, result.Request.Height));
        }
    }
}