using Newtonsoft.Json;
using System;
using Xunit.Abstractions;

namespace Test.Data.AdoNet
{
    /// <summary>  
    /// Provides factory methods for creating instances of <see cref="SerializableMemberData{T}"/>.  
    /// </summary>  
    public static class SerializableMemberData
    {
        /// <summary>  
        /// Creates a new instance of <see cref="SerializableMemberData{T}"/> with the specified member data.  
        /// </summary>  
        /// <typeparam name="T">The type of the member data.</typeparam>  
        /// <param name="memberData">The member data to serialize.</param>  
        /// <returns>A new instance of <see cref="SerializableMemberData{T}"/>.</returns>  
        public static SerializableMemberData<T> Create<T>(T memberData) where T : new()
        {
            return new SerializableMemberData<T>(memberData);
        }
    }

    /// <summary>  
    /// Represents serializable member data for use with xUnit tests.  
    /// </summary>  
    /// <typeparam name="T">The type of the member data.</typeparam>  
    public class SerializableMemberData<T> : IXunitSerializable where T : new()
    {
        /// <summary>  
        /// Gets the member data.  
        /// </summary>  
        public T MemberData { get; private set; }

        /// <summary>  
        /// Initializes a new instance of the <see cref="SerializableMemberData{T}"/> class with default member data.  
        /// </summary>  
        public SerializableMemberData()
        {
            MemberData = new T();
        }

        /// <summary>  
        /// Initializes a new instance of the <see cref="SerializableMemberData{T}"/> class with the specified member data.  
        /// </summary>  
        /// <param name="memberData">The member data to serialize.</param>  
        public SerializableMemberData(T memberData)
        {
            MemberData = memberData;
        }

        /// <summary>  
        /// Deserializes the member data from the provided xUnit serialization info.  
        /// </summary>  
        /// <param name="info">The serialization info containing the serialized data.</param>  
        /// <exception cref="InvalidOperationException">Thrown if the deserialization fails.</exception>  
        public void Deserialize(IXunitSerializationInfo info)
        {
            if (info == null) throw new InvalidOperationException();

            var data = info.GetValue<string>("memberData");
            if (data == null) throw new InvalidOperationException();

            var objectData = JsonConvert.DeserializeObject<T>(data);
            if (objectData == null) throw new InvalidOperationException();

            MemberData = objectData;
        }

        /// <summary>  
        /// Serializes the member data into the provided xUnit serialization info.  
        /// </summary>  
        /// <param name="info">The serialization info to populate with serialized data.</param>  
        public void Serialize(IXunitSerializationInfo info)
        {
            var json = JsonConvert.SerializeObject(MemberData);
            info.AddValue("memberData", json);
        }

        /// <summary>  
        /// Returns a string representation of the member data.  
        /// </summary>  
        /// <returns>A string representation of the member data.</returns>  
        public override string ToString()
        {
            return MemberData?.ToString() ?? "";
        }
    }
}
