using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace jira_capacity.Models;

public class JiraResponse<T>
{
    public int MaxResults { get; set; }
    public int StartAt { get; set; }
    public int Total { get; set; }
    public bool IsLast { get; set; }
    public List<T> Values { get; set; }
}

public class JiraIssuesResponse
{
    public int MaxResults { get; set; }
    public int StartAt { get; set; }
    public int Total { get; set; }
    public bool IsLast { get; set; }
    public List<JiraIssue> Issues { get; set; }
}

public class JiraIssue
{
    public int Id { get; set; }
    public string Key { get; set; }
    public IssueFields Fields { get; set; }
}

public class IssueFields
{
    public string Summary { get; set; }
    public Assignee Assignee { get; set; }
    
    [JsonProperty("customfield_13508")]
    [JsonConverter(typeof(NonNullDoubleConverter))]
    public double StoryPoints { get; set; }
}

public class Assignee
{
    public string DisplayName { get; set; }
    public string AccountId { get; set; }
    public string EmailAddress { get; set; }
}


public class NonNullDoubleConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(double) || objectType == typeof(double?);

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, 0);
            return;
        }
        var value = (long)untypedValue;
        serializer.Serialize(writer, value.ToString());
        return;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return 0d;
        }
        
        string? value = serializer.Deserialize<string>(reader);
        if (double.TryParse(value, out double l))
        {
            return l;
        }
        
        throw new Exception("Cannot unmarshal type double");
    }

    public static readonly NonNullDoubleConverter Singleton = new NonNullDoubleConverter();
}