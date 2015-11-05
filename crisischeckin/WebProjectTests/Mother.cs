using System.Collections.Specialized;
using System.Globalization;
using System.Web.Mvc;


namespace WebProjectTests
{
    /// <summary>
    /// Put pieces of useful reusable code here for unit testing.
    /// </summary>
    public static class Mother
    {


         public static class ControllerHelpers
         {
             public static void SetupControllerModelState<T>(T model, Controller controller)
             {
                 var modelBinder = new ModelBindingContext
                 {
                     ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType()),
                     ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), CultureInfo.InvariantCulture)
                 };

                 var binder = new DefaultModelBinder();
                 binder.BindModel(new ControllerContext(), modelBinder);
                 controller.ModelState.Clear();
                 controller.ModelState.Merge(modelBinder.ModelState);
             }
         }


    }
}