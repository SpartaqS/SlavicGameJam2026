// Source - https://stackoverflow.com/a/972323
// Posted by JaredPar, modified by community. See post 'Timeline' for change history
// Retrieved 2026-08-07, License - CC BY-SA 4.0

using System;
using System.Collections.Generic;

public static class EnumUtil
{
    public static IEnumerable<T> GetValues<T>()
    {
        return (T[])Enum.GetValues(typeof(T));
    }
}
