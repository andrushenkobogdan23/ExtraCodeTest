using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Shared.Common.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class AggregateBaseException : Exception
    {
        protected const string ID = "Id";
        string _innerError = null;
        string _msg = null;

        public AggregateBaseException(string msgTemplate, string field, object value, string code, Exception inner) : base(string.Format(msgTemplate, field, value, code), inner)
        {
            Code = code;
            Field = field;
            Value = value;
        }

        public AggregateBaseException(string msgTemplate, string field, object value, string code) : this(msgTemplate, field, value, code, null)
        { }

        public AggregateBaseException(string msgTemplate, object value) : this(msgTemplate, ID, value, null, null)
        { }

        [JsonProperty]
        public new string Message
        {
            get
            {
                return _msg ?? (_msg = base.Message);
            }
            set
            {
                _msg = value;
            }
        }

        [JsonProperty]
        public string Code { get;  set; }

        [JsonProperty]
        public string InnerError
        {
            get
            {
                return InnerException == null ? _innerError : InnerException.Message;
            }
            set
            {
                _innerError = value;
            }
        }

        [JsonProperty]
        public string Field { get; set; }

        [JsonProperty]
        public object Value { get; set; }

        public object ToJson()
        {
            var obj = JObject.FromObject(this);
            return obj;
        }

    }
}
