using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumHelper
{

    public static List<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

}
