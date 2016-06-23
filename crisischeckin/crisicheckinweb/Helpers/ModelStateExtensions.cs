using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public static class ModelStateExtensions
{
    static public void RemoveErrorsExcept(this ModelStateDictionary modelState, params string[] keys)
    {
        var remove = modelState.Keys.Except(keys);
        foreach (string key in remove)
            modelState[key].Errors.Clear();
    }
}