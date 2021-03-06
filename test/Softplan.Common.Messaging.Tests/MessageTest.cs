using System;
using Xunit;
using Softplan.Common.Messaging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Softplan.Common.Messaging.UnitTest
{
    public class MessageTest
    {
        [Fact]
        public void AutoGeneratedOperationIdTest()
        {
            Message msg = new Message();
            Assert.NotEmpty(msg.OperationId);
            Assert.Equal(msg.OperationId, msg.ParentOperationId);
            Assert.Equal(msg.OperationId, msg.MainOperationId);
        }

        [Fact]
        public void InheritedOperationIdsTest()
        {
            var parentMsg = new Message
            {
                MainOperationId = "MainOperationId"
            };

            var msg = new Message(parentMsg);
            Assert.Equal("MainOperationId", msg.MainOperationId);
            Assert.Equal(parentMsg.OperationId, msg.ParentOperationId);
            Assert.NotEqual(parentMsg.OperationId, msg.OperationId);
            Assert.NotEmpty(msg.OperationId);
            Assert.NotEmpty(msg.ParentOperationId);
            Assert.NotEmpty(msg.MainOperationId);
        }

        [Fact]
        public void AssignBaseMessageDataTest()
        {
            var parentMsg = new Message
            {
                MainOperationId = "MainOperationId"
            };
            parentMsg.CustomParams.Add("test", "1");
            parentMsg.CustomParams.Add("test2", "2");

            var msg = new Message();
            msg.AssignBaseMessageData(parentMsg);

            Assert.Equal("MainOperationId", msg.MainOperationId);
            Assert.Equal(parentMsg.OperationId, msg.ParentOperationId);
            Assert.Equal(parentMsg.CustomParams, msg.CustomParams);
            Assert.NotEqual(parentMsg.OperationId, msg.OperationId);
            Assert.NotEmpty(msg.OperationId);
            Assert.NotEmpty(msg.ParentOperationId);
            Assert.NotEmpty(msg.MainOperationId);
        }

        [Fact]
        public void PropertiesTest()
        {
            var msg = new Message
            {
                Id = "msgId",
                Token = "msgToken",
                UserId = "msgUserId",
                ReplyQueue = "msgReplyQueue",
                ReplyTo = "msgReplyTo"
            };

            Assert.Equal("msgId", msg.Id);
            Assert.Equal("msgToken", msg.Token);
            Assert.Equal("msgUserId", msg.UserId);
            Assert.Equal("msgReplyQueue", msg.ReplyQueue);
            Assert.Equal("msgReplyTo", msg.ReplyTo);
        }

        [Fact]
        public void OldCustomParamsSerializationTest()
        {
            var msg = new Message();
            msg.CustomParams["test"] = "nice";
            msg.CustomParams["foo"] = "bar";

            var newMsg = JsonConvert.DeserializeObject<Message>(JsonConvert.SerializeObject(msg));

            Assert.Equal(new List<string> { "test=nice", "foo=bar" }, newMsg.OldCustomParams.Items);
            Assert.Equal(msg.CustomParams, newMsg.CustomParams);
        }
    }
}
