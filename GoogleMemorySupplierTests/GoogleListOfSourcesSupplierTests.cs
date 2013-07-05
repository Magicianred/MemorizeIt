using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleMemorySupplier;
using NUnit.Framework;

namespace GoogleMemorySupplierTests
{
    [TestFixture]
    public class GoogleListOfSourcesSupplierTests
    {
        [Test]
        public void GetSourcesList_WorksheetIsPresented_List_of_spreadsheets_is_returned()
        {

            //arrange

            GoogleListOfSourcesSupplier target = CreateGoogleListOfSourcesSupplier();

            //act

            var retval = target.GetSourcesList();

            //assert
            Assert.That(retval.Count(), Is.EqualTo(2));
            Assert.That(retval.First(), Is.EqualTo("S1"));
            Assert.That(retval.Last(), Is.EqualTo("S2"));
            
        }
        private GoogleListOfSourcesSupplier CreateGoogleListOfSourcesSupplier()
        {
            return new GoogleListOfSourcesSupplier("memorize.it.test@gmail.com", "MemorizeIt");
        }
    }
}
