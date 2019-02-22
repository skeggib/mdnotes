using System;

namespace MdNotes.Tests
{
    public static class Utils
    {
        /// <summary>
        /// Generates an unique string.
        /// </summary>
        /// <returns>The generated string.</returns>
        public static string GetUniqueString() => DateTime.Now.ToOADate().ToString();

        public static string BaseUri => "http://localhost:3000/";
    }
}