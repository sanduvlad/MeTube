using FluentAssertions;
using MessageQueue.Models;
using MessageQueue.Storage;
using System;
using Xunit;

namespace MessageQueue.Tests
{
    public class QueueManagerShould
    {

        [Fact]
        public void Store_A_Given_Message()
        {
            var mq = new QueueManager();

            Guid messageId = mq.AddMessage(new Message()
            {
                Data = "Test Data",
                Name = "Test Name"
            });

            var receivedMessage = mq.GetMessage(messageId);

            receivedMessage.Data.Should().Be("Test Data");
            receivedMessage.Name.Should().Be("Test Name");
        }

        [Fact]
        public void Not_Delete_A_Message_When_GettingIt()
        {
            var mq = new QueueManager();

            Guid messageId = mq.AddMessage(new Message()
            {
                Data = "Test Data",
                Name = "Test Name"
            });

            var receivedFirst = mq.GetMessage(messageId);
            var receivedSecond = mq.GetMessage(messageId);

            receivedFirst.Data.Should().Be("Test Data");
            receivedFirst.Name.Should().Be("Test Name");
            receivedFirst.Should().BeSameAs(receivedSecond);
        }

        [Fact]
        public void Delete_A_Message_Once_Popped()
        {
            var mq = new QueueManager();

            Guid messageId = mq.AddMessage(new Message()
            {
                Data = "Test Data",
                Name = "Test Name"
            });

            var receivedFirst = mq.PopMessage(messageId);
            var receivedSecond = mq.PopMessage(messageId);

            receivedFirst.Data.Should().Be("Test Data");
            receivedFirst.Name.Should().Be("Test Name");
            receivedSecond.Should().BeNull();
        }

        [Fact]
        public void Delete_A_Message_On_Remove()
        {
            var mq = new QueueManager();

            Guid messageId = mq.AddMessage(new Message()
            {
                Data = "Test Data",
                Name = "Test Name"
            });

            var removedStatus = mq.RemoveMessage(messageId);
            var receivedMessage = mq.GetMessage(messageId);

            receivedMessage.Should().BeNull();
            removedStatus.Should().BeTrue();
        }
    }
}
