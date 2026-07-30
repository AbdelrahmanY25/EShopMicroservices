namespace BuildingBlocks.Messaging.Events;

public record IntegrationEvent
{
	public Guid Id => Guid.CreateVersion7();
	public DateTime OccuredOn => DateTime.UtcNow;
	public string EventType => GetType().AssemblyQualifiedName;
}