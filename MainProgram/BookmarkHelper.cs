using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Main_Program
{
    class BookmarkHelper
    {
        /// <summary>
        /// Bookmark navigator instance to access the bookmark from given <see cref="IWordDocument"/> object
        /// </summary>
        private readonly BookmarksNavigator _bookmarkNavigator;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="wordDocument">Word document to access bookmark</param>
        public BookmarkHelper(IWordDocument wordDocument)
        {
            _bookmarkNavigator = new BookmarksNavigator(wordDocument);
        }


        /// <summary>
        /// Replaces the contents of an existing <paramref name="bookmark"/> with <paramref name="text"/>
        /// </summary>
        /// <param name="bookmark"></param>
        /// <param name="text"></param>
        public BookmarkHelper ReplaceBookmarkWithText(string bookmark, string text)
        {
            // Moves the virtual cursor to the location before the end of the bookmark "Northwind"
            _bookmarkNavigator.MoveToBookmark(bookmark);


            // Replaces the contents of an existing bookmark with simple text
            _bookmarkNavigator.ReplaceBookmarkContent(text, true);

            // Returns this for Fluent API simulation
            return this;
        }

        /// <summary>
        /// Replaces the contents of an existing bookmark with text
        /// NOTE:
        ///      convention based method
        /// </summary>
        /// <param name="textBoxes"></param>
        public BookmarkHelper ReplaceBookmarkWithText(params TextBox[] textBoxes)
        {
            var args = textBoxes.Select(box => (box.Name.Replace("TextBox", ""), box.Text));
            foreach (var (bookmark, text) in args)
            {
                ReplaceBookmarkWithText(bookmark, text);
            }

            // Returns this for Fluent API simulation
            return this;
        }

        public BookmarkHelper MyMethod(string bookmark, string text)
        {
            _bookmarkNavigator.MoveToBookmark(bookmark);
            _bookmarkNavigator.ReplaceBookmarkContent(text, true);
            return this;
        }
    }
}
