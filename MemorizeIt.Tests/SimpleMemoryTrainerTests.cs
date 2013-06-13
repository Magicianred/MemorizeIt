using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemorizeIt.MemoryStorage;
using MemorizeIt.MemoryTrainers;
using MemorizeIt.Model;
using Moq;
using NUnit.Framework;

namespace MemorizeIt.Tests
{
    [TestFixture]
    public class SimpleMemoryTrainerTests
    {
        [Test]
        public void GetQuestion_When_Current_question_is_empty_Then_Qestion_text_is_returned()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer("q", "a");

            // act
            var question = target.GetQuestion();

            // assert
            Assert.That(question, Is.EqualTo("q"));
        }

        [Test]
        public void GetQuestion_When_Current_question_is_not_empty_Then_InvalidOperationException()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer("q", "a");
            target.GetQuestion();
            // act
            Assert.Throws<InvalidOperationException>(() => target.GetQuestion());
        }

        [Test]
        public void Validate_When_Answer_is_correct_Then_True_is_returned_record_is_marked_as_Success()
        {
            // arrange
            var storageMock = new Mock<IMemoryStorage>();
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer(storageMock.Object,"q", "a");
            target.GetQuestion();
            // act
            var result = target.Validate("a");

            // assert
            Assert.That(result, Is.EqualTo(true));
            storageMock.Verify(x => x.Success(It.IsAny<Guid>()), Times.Once());
        }
        [Test]
        public void Validate_When_Answer_is_incorrect_Then_True_is_returned_record_is_marked_as_Success()
        {
            // arrange
            var storageMock = new Mock<IMemoryStorage>();
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer(storageMock.Object, "q", "a");
            target.GetQuestion();
            // act
            var result = target.Validate("c");

            // assert
            Assert.That(result, Is.EqualTo(false));
            storageMock.Verify(x => x.Fail(It.IsAny<Guid>()), Times.Once());
        }
        [Test]
        public void Validate_When_Current_question_is_empty_Then_InvalidOperationException()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer("q", "a");
            // act
            Assert.Throws<InvalidOperationException>(() => target.Validate("a"));
        }
        private SimpleMemoryTrainer CreateSimpleMemoryTrainer(string q, string a)
        {
            return CreateSimpleMemoryTrainer(new Mock<IMemoryStorage>().Object, q, a);
        }

        private SimpleMemoryTrainer CreateSimpleMemoryTrainer(IMemoryStorage storage,string q, string a)
        {
            var randomizer = new Mock<IRandomizer>();
            randomizer.Setup(x => x.GetRandomUnsuccessItem())
                      .Returns(new QuestionAndAnswer(Guid.NewGuid(), q, a));
            return new SimpleMemoryTrainer(storage, randomizer.Object);
        }
    }
}
