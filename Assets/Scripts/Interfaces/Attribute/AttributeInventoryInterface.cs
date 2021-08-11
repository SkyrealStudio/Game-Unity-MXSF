using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Attribute
{
    /// <summary>
    /// Basic attribute of character I/O
    /// </summary>
    interface IAttributeInventory
    {
        /// <summary>
        /// Add an attribute to the target character.
        /// </summary>
        /// <param name="tarChar">Target character.</param>
        /// <param name="attr">The attribute.</param>
        /// <returns>If succeeded.</returns>
        bool AddAttribute(string tarChar, string attr);
        /// <summary>
        /// Remove an attribute from the target character.
        /// </summary>
        /// <param name="tarChar">Target character.</param>
        /// <param name="attr">The Attribute.</param>
        /// <returns>If succeeded.</returns>
        bool RemoveAttribute(string tarChar, string attr);
        /// <summary>
        /// Detect whether the target character contains the attribute.
        /// </summary>
        /// <param name="tarChar">The target character.</param>
        /// <param name="attr">The attribute.</param>
        /// <returns>Whether contains.</returns>
        bool Contains(string tarChar, string attr);
        /// <summary>
        /// Get all the attributes of the target character.
        /// </summary>
        /// <param name="tarChar">The target character.</param>
        /// <returns>The attributes.</returns>
        string[] ListAttribute(string tarChar);
        /// <summary>
        /// Get all the characters who contain the attribute.
        /// </summary>
        /// <param name="attr">The attribute.</param>
        /// <returns>The characters.</returns>
        string[] SearchAttribute(string attr);
    }
}
