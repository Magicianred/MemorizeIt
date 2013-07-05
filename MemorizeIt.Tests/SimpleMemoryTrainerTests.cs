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
            target.PickQuestion();

            // assert
            Assert.That(target.CurrentQuestion.Question, Is.EqualTo("q"));
        }

        [Test]
        public void GetQuestion_When_Current_question_is_empty_storage_is_empty_Then_InvalidOperationException()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer();

            // act
            Assert.Throws<InvalidOperationException>(() => target.PickQuestion());
        }

        [Test]
        public void GetQuestion_When_Current_question_is_not_empty_Then_InvalidOperationException()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer("q", "a");
            target.PickQuestion();
            // act
            Assert.Throws<InvalidOperationException>(() => target.PickQuestion());
        }

        [Test]
        public void Validate_When_Answer_is_correct_Then_True_is_returned_record_is_marked_as_Success()
        {
            // arrange
            var storageMock = new Mock<IMemoryStorage>();
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer(storageMock.Object,"q", "a");
            target.PickQuestion();
            // act
            var result = target.Validate("a");

            // assert
            Assert.That(result, Is.EqualTo(true));
            storageMock.Verify(x => x.ItemSuccess(It.IsAny<Guid>()), Times.Once());
        }
        [Test]
        public void Validate_When_Answer_is_incorrect_Then_True_is_returned_record_is_marked_as_Success()
        {
            // arrange
            var storageMock = new Mock<IMemoryStorage>();
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer(storageMock.Object, "q", "a");
            target.PickQuestion();
            // act
            var result = target.Validate("c");

            // assert
            Assert.That(result, Is.EqualTo(false));
            storageMock.Verify(x => x.ItemFail(It.IsAny<Guid>()), Times.Once());
        }
        [Test]
        public void Validate_When_Current_question_is_empty_Then_InvalidOperationException()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer("q", "a");
            // act
            Assert.Throws<InvalidOperationException>(() => target.Validate("a"));
        }

        [Test]
        public void Clear_When_current_question_is_present_Then_current_question_is_reset()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer("q","a");
            target.PickQuestion();
            // act
            target.Clear();

            // assert
            Assert.That(target.CurrentQuestion, Is.EqualTo(null));
        }

        [Test]
        public void IsQuestionsAvalible_When_StorageIsEmpty_Then_False_is_returned()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer();

            // act
            var result = target.IsQuestionsAvalible();

            // assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsQuestionsAvalible_When_storage_is_not_empty_Then_True_is_returned()
        {
            // arrange
            SimpleMemoryTrainer target = CreateSimpleMemoryTrainer("q","q");

            // act
            var result =target.IsQuestionsAvalible();

            // assert
            Assert.That(result, Is.EqualTo(true));
        }

        private SimpleMemoryTrainer CreateSimpleMemoryTrainer()
        {
            return new SimpleMemoryTrainer(new Mock<IMemoryStorage>().Object, new Mock<IRandomizer>().Object);
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
