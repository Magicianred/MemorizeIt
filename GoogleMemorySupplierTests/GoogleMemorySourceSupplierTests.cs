using System.Linq;
using GoogleMemorySupplier;
using NUnit.Framework;

namespace GoogleMemorySupplierTests
{
    [TestFixture]
    public class GoogleMemorySourceSupplierTests
    {
        [Test]
        public void Download_When_sheet_is_absent_Then_result_is_null()
        {
            // arrange
            GoogleMemorySourceSupplier target = CreateGoogleMemorySourceSupplier();

            // act
            var result = target.Download("dddd");

            // assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void Download_sheet_is_present_result_is_not_empty()
        {

            //arrange

            GoogleMemorySourceSupplier target = CreateGoogleMemorySourceSupplier();

            //act

            var result = target.Download("S1");

            //assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetSourcesList_WorksheetIsPresented_List_of_spreadsheets_is_returned()
        {

            //arrange

            GoogleMemorySourceSupplier target = CreateGoogleMemorySourceSupplier();

            //act

            var retval = target.GetSourcesList();

            //assert
            Assert.That(retval.Count(), Is.EqualTo(2));
            Assert.AreEqual(retval.First(),"S1");
            Assert.AreEqual(retval.Last(), "S2");
            

        }

        private GoogleMemorySourceSupplier CreateGoogleMemorySourceSupplier()
        {
            return new GoogleMemorySourceSupplier("memorize.it.test@gmail.com", "MemorizeIt", "MemorizeItTests");
        }
    }
}
