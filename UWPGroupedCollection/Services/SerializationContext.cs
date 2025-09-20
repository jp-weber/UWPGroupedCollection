using CommunityToolkit.Mvvm.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UWPGroupedCollection.Models;

namespace UWPGroupedCollection.Services
{
    [JsonSerializable(typeof(ContactsQueryResponse))]
    [JsonSerializable(typeof(IList<Contact>))]
    public partial class SerializationContext : JsonSerializerContext
    {
    }
}
