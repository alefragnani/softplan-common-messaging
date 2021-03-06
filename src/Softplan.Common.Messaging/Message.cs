using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Softplan.Common.Messaging.Abstractions;

namespace Softplan.Common.Messaging
{
    public class Message : IMessage
    {
        public Message(IMessage parentMessage = null)
        {
            this.OperationId = Guid.NewGuid().ToString();
            this.ParentOperationId = OperationId;
            this.MainOperationId = OperationId;
            this.CustomParams = new Dictionary<string, string>();
            this.OldCustomParams = new LegacyCustomParams();
            this.Headers = new Dictionary<string, object>();

            if (parentMessage != null)
            {
                this.AssignBaseMessageData(parentMessage);
            }
        }

        public string Id { get; set; }
        public IDictionary<string, object> Headers { get; set; }
        public string OperationId { get; set; }
        public string ParentOperationId { get; set; }
        public string MainOperationId { get; set; }
        [JsonProperty("CustomParams")]
        public LegacyCustomParams OldCustomParams { get; set; }
        [JsonIgnore]
        public IDictionary<string, string> CustomParams { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string ReplyQueue { get; set; }
        public string ReplyTo { get; set; }

        public void AssignBaseMessageData(IMessage baseMessage)
        {
            this.MainOperationId = baseMessage.MainOperationId;
            this.ParentOperationId = baseMessage.OperationId;
            this.ReplyQueue = baseMessage.ReplyQueue;
            foreach (var p in baseMessage.CustomParams)
            {
                this.CustomParams.Add(p.Key, p.Value);
            }
        }

        [OnSerializing()]
        private void OnSerializing(StreamingContext context)
        {
            OldCustomParams.FromDictionary(CustomParams);
        }

        [OnDeserialized()]
        private void OnDeSerialized(StreamingContext context)
        {
            OldCustomParams.ToDictionary(CustomParams);
        }
    }
}
