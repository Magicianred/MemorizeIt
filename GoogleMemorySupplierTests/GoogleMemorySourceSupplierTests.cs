using GoogleMemorySupplier;
using NUnit.Framework;

namespace GoogleMemorySupplierTests
{
    [TestFixture]
    public class GoogleMemorySourceSupplierTests
    {
        [Test]
        public void Download_When_test_Then_test()
        {
            // arrange
            GoogleMemorySourceSupplier target = CreateGoogleMemorySourceSupplier();

            // act
            target.Download();

            // assert
            
        }

        private GoogleMemorySourceSupplier CreateGoogleMemorySourceSupplier()
        {
            return new GoogleMemorySourceSupplier("MemorizeIt", "memorize.it.test@gmail.com", "MemorizeIt");
        }
    }
}
