using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Utilities
{
    public static class Common
    {
        public static bool IsNullOrDefault<T>(this T inObj)
        {
            if (inObj == null) return true;
            return EqualityComparer<T>.Default.Equals(inObj, default);
        }
    }
}
