using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvancedInspector
{
    /// <summary>
    /// Defines an attribute that override a node "expandability". 
    /// A node control if its expendable by itself, unless force not to be. 
    /// </summary>
    public interface IExpandable
    {
        /// <summary>
        /// Default true.
        /// If false, the node won't have an expander control.
        /// </summary>
        bool IsExpandable(object[] instances, object[] values);

        /// <summary>
        /// The default expanded state of the node.
        /// If true, the node is "expanded" by default.
        /// It doesn't prevent someone from closing it.
        /// </summary>
        bool IsExpanded(object[] instances, object[] values);
    }
}
