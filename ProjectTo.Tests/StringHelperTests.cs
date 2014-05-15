using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectTo.Tests
{
    [TestClass]
    public class StringHelperTests
    {
        /// <summary>
        /// PERSON => []
        /// </summary>
        [TestMethod]
        public void SplitToWordGroups_NoUnderscore_AllUpper_Empty()
        {
            var result = StringHelper.SplitToWordGroups("PERSON").ToArray();
            Assert.AreEqual(0, result.Length);
        }

        /// <summary>
        /// person => []
        /// </summary>
        [TestMethod]
        public void SplitToWordGroups_NoUnderscore_AllLower_Empty()
        {
            var result = StringHelper.SplitToWordGroups("person").ToArray();
            Assert.AreEqual(0, result.Length);
        }

        /// <summary>
        /// Person => []
        /// </summary>
        [TestMethod]
        public void SplitToWordGroups_NoUnderscore_OneWord_Empty()
        {
            var result = StringHelper.SplitToWordGroups("Person").ToArray();
            Assert.AreEqual(0, result.Length);
        }

        /// <summary>
        /// PERSON_NAME => [PERSON, NAME]
        /// </summary>
        [TestMethod]
        public void SplitToWordGroups_Underscore2Words_OK()
        {
            var result = StringHelper.SplitToWordGroups("PERSON_NAME").ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("PERSON", result[0][0]);
            Assert.AreEqual("NAME", result[0][1]);
        }

        /// <summary>
        /// PersonName => [Person, Name]
        /// </summary>
        [TestMethod]
        public void SplitToWordGroups_CamelCase2Words_OK()
        {
            var result = StringHelper.SplitToWordGroups("PersonName").ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("Person", result[0][0]);
            Assert.AreEqual("Name", result[0][1]);
        }
    }
}
