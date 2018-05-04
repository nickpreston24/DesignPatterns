using System;
using System.Diagnostics.CodeAnalysis;

namespace Common
{
    public static partial class Extensions
    {
        /// 
        /// Raises an event thread-safely if the event has subscribers. 
        /// 
        /// The event handler to raise. 
        /// The object that sent this event. 
        ///args"> The event args. 
        [SuppressMessage("Microsoft.Design",
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This warning comes up when you use the word `Fire` in a method name. This method specifically raises events, and so does not need changing.")]
        public static void Fire<T>(this EventHandler handler, object sender, EventArgs args) => handler?.Invoke(sender, args);
        /// 
        /// Raises an event thread-safely if the event has subscribers. 
        /// 
        /// The type of EventArgs the event takes.
        /// The event handler to raise. 
        /// The object that sent this event. 
        /// The event args. 
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This warning comes up when you use the word `Fire` in a method name. This method specifically raises events, and so does not need changing.")]
        public static void Fire<T>(this EventHandler handler, object sender, T args) where T : EventArgs => handler?.Invoke(sender, args);
        /*For statics*/
        /// 
        /// Raises a static event thread-safely if the event has subscribers. 
        /// 
        /// The event handler to raise. 
        ///args"> The event args. 
        [SuppressMessage("Microsoft.Design",
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This warning comes up when you use the word `Fire` in a method name. This method specifically raises events, and so does not need changing.")]
        public static void Fire<T>(this EventHandler handler, EventArgs args) => handler.Fire(null, args);
        /// 
        /// Raises a static event thread-safely if the event has subscribers. 
        /// 
        /// The type of EventArgs the event takes.
        /// The event handler to raise. 
        /// The event args. 
        [SuppressMessage("Microsoft.Design",
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This warning comes up when you use the word `Fire` in a method name. This method specifically raises events, and so does not need changing.")]
        public static void Fire<T>(this EventHandler handler, T args) where T : EventArgs => handler.Fire(null, args);
    }
}
