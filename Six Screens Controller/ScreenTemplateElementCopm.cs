using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Six_Screens_Controller
{
    class ScreenTemplateElementCopm : IComparer<ScreenTemplateElement>
    {
        public int Compare([AllowNull] ScreenTemplateElement x, [AllowNull] ScreenTemplateElement y)
        {
            if (x.ScreenNumber > y.ScreenNumber)
                return 1;
            else if (x.ScreenNumber < y.ScreenNumber)
                return -1;
            else
                return 0;
        }
    }
}
