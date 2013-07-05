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
            GoogleMemorySourceSupplier target = CreateGoogleMemorySourceSupplier("dddd");

            // act
            var result = target.Download();

            // assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void Download_sheet_is_present_result_is_not_empty()
        {

            //arrange

            GoogleMemorySourceSupplier target = CreateGoogleMemorySourceSupplier();

            //act

            var result = target.Download();

            //assert
            Assert.IsNotNull(result);
        }

        private GoogleMemorySourceSupplier CreateGoogleMemorySourceSupplier(string sheetName="S1")
        {
            return new GoogleMemorySourceSupplier(sheetName, "memorize.it.test@gmail.com", "MemorizeIt");
        }
    }
}
