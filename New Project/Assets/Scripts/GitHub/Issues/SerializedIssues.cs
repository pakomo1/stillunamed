using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public struct SerializedIssues : IEquatable<SerializedIssues>, INetworkSerializable
{
    /// <summary>
    /// The internal Id for this issue (not the issue number)
    /// </summary>
    public int Id;

    /// <summary>
    /// GraphQL Node Id
    /// </summary>
    public FixedString128Bytes NodeId;

    /// <summary>
    /// The URL for this issue.
    /// </summary>
    public FixedString128Bytes Url;

    /// <summary>
    /// The URL for the HTML view of this issue.
    /// </summary>
    public FixedString128Bytes HtmlUrl;

    /// <summary>
    /// The Comments URL of this issue.
    /// </summary>
    public FixedString128Bytes CommentsUrl;

    /// <summary>
    /// The Events URL of this issue.
    /// </summary>
    public FixedString128Bytes EventsUrl;

    /// <summary>
    /// The issue number.
    /// </summary>
    public int Number;

    /// <summary>
    /// Whether the issue is open or closed.
    /// </summary>
    public FixedString128Bytes State;

    /// <summary>
    /// Title of the issue
    /// </summary>
    public FixedString128Bytes Title;

    /// <summary>
    /// Details about the issue.
    /// </summary>
    public FixedString128Bytes Body;

    /// <summary>
    /// The Unix timestamp for when the issue was closed, if it was closed.
    /// </summary>
    public long ClosedAtTicks;

    /// <summary>
    /// The Unix timestamp for when the issue was created.
    /// </summary>
    public long CreatedAtTicks;

    /// <summary>
    /// The Unix timestamp for when the issue was last updated.
    /// </summary>
    public long UpdatedAtTicks;

    /// <summary>
    /// If the issue is locked or not.
    /// </summary>
    public bool Locked;
    /// <summary>
    /// The number of comments on the issue.
    /// </summary>
    public int Comments;

    // Converters for DateTimeOffset properties
    public DateTimeOffset? ClosedAt
    {
        get => ClosedAtTicks == 0 ? (DateTimeOffset?)null : DateTimeOffset.FromUnixTimeSeconds(ClosedAtTicks);
        set => ClosedAtTicks = value.HasValue ? value.Value.ToUnixTimeSeconds() : 0;
    }

    public DateTimeOffset CreatedAt
    {
        get => DateTimeOffset.FromUnixTimeSeconds(CreatedAtTicks);
        set => CreatedAtTicks = value.ToUnixTimeSeconds();
    }

    public DateTimeOffset? UpdatedAt
    {
        get => UpdatedAtTicks == 0 ? (DateTimeOffset?)null : DateTimeOffset.FromUnixTimeSeconds(UpdatedAtTicks);
        set => UpdatedAtTicks = value.HasValue ? value.Value.ToUnixTimeSeconds() : 0;
    }

    public bool Equals(SerializedIssues other)
    {
        return (Id, NodeId, Url, HtmlUrl, CommentsUrl, EventsUrl, Number, State, Title, Body, ClosedAtTicks, CreatedAtTicks, UpdatedAtTicks, Locked) ==
               (other.Id, other.NodeId, other.Url, other.HtmlUrl, other.CommentsUrl, other.EventsUrl, other.Number, other.State, other.Title, other.Body, other.ClosedAtTicks, other.CreatedAtTicks, other.UpdatedAtTicks, other.Locked);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref NodeId);
        serializer.SerializeValue(ref Url);
        serializer.SerializeValue(ref HtmlUrl);
        serializer.SerializeValue(ref CommentsUrl);
        serializer.SerializeValue(ref EventsUrl);
        serializer.SerializeValue(ref Number);
        serializer.SerializeValue(ref State);
        serializer.SerializeValue(ref Title);
        serializer.SerializeValue(ref Body);
        serializer.SerializeValue(ref ClosedAtTicks);
        serializer.SerializeValue(ref CreatedAtTicks);
        serializer.SerializeValue(ref UpdatedAtTicks);
        serializer.SerializeValue(ref Locked);
        serializer.SerializeValue(ref Comments);
    }
}
