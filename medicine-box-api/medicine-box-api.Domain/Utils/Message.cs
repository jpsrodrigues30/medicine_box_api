using System;

namespace medicine_box_api.Domain.Utils;
public abstract class Message
{
    public string MessageType { get; protected set; }

    public Guid AggregateId { get; protected set; }

    protected Message()
    {
        MessageType = GetType().Name;
    }
}