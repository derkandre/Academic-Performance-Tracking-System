using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Academic_Performance_Tracking_System.Utilities
{    
    public static class VisualChildren
    {
        /** 
        -----------------------------------------------------------------------------------       
        Adapted from "WPF - Find Child by Type" by Paul Keating (2013).
        
        Retrieved from http://www.journeyintocode.com/2013/03/wpf-find-child-by-type.html
        on 2025-01-03 (ISO8601)

        Archived URL here: 
        https://web.archive.org/web/20250103041149/http://www.journeyintocode.com/2013/03/wpf-find-child-by-type.html

        The following Get<T> method finds all the child elements of a specific element like
        a GroupBox containing TextBoxes and Buttons as its children.
        -----------------------------------------------------------------------------------       
        */

        public static List<T> Get<T>(this Visual referenceVisual) where T : Visual
        {
            List<T> children = new List<T>();
            int childCount = VisualTreeHelper.GetChildrenCount(referenceVisual);

            for (int i = 0; i < childCount; i++)
            {
                if (VisualTreeHelper.GetChild(referenceVisual, i) is Visual child)
                {
                    if (child is T t) children.Add(t);
                    List<T> temp = Get<T>(child);
                    children.AddRange(temp);
                }
            }

            return children;
        }
    }
}
