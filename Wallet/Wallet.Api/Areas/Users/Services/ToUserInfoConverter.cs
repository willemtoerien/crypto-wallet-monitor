using System;
using Newtonsoft.Json;
using Wallet.Api.Areas.Users.Models;

namespace Wallet.Api.Areas.Users.Services
{
    /// <summary>
    /// You don't want to send back the full user. Instead only certain properties of the User.
    /// </summary>
    public class ToUserInfoConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var user = new UserInfo((DataAccess.User)value);
            serializer.Serialize(writer, user);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => throw new NotImplementedException();

        public override bool CanConvert(Type objectType) => objectType == typeof(DataAccess.User);
    }
}
