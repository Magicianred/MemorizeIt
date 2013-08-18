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
    public class PublicGoogleMemorySourceSupplierTests
    {
        [Test]
        public void Download_KeyIsCorrect_MemoryTable_is_returned()
        {

            //arrange

            PublicGoogleMemorySourceSupplier target = CreatePublicGoogleMemorySourceSupplier();

            //act

            var result = target.Download("Done");

            //assert

        }

        [Test]
        public void GetSourcesList_ListOf2_is_returned()
        {

            //arrange
            var correctResults = new string[] {"InProcess", "Atlas", "Done", "New"};
            PublicGoogleMemorySourceSupplier target = CreatePublicGoogleMemorySourceSupplier();

            //act

            var result = target.GetSourcesList();

            //assert
            foreach (var correctResult in correctResults)
            {
                    Assert.True(result.Contains(correctResult), string.Format("{0} is missing", correctResult));
            }
        }

        private PublicGoogleMemorySourceSupplier CreatePublicGoogleMemorySourceSupplier()
        {
            return new PublicGoogleMemorySourceSupplier("0AhBI8qe9tkQYdEo3aTBEN21wQ0ptN0owdlJRSW5NNGc", "memorize.it.test@gmail.com", "MemorizeIt");
        }
    }
}
