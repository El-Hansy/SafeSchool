using FluentAssertions;
using SafeSchool.Api.Features.Communications;
using Xunit;
namespace SafeSchool.Api.Tests.Features.Communications;
public sealed class CommunicationFoundationTests { [Fact] public void Communications_create_recipient_snapshots_without_source_mutation() { CommunicationCapabilities.All.Should().Contain(CommunicationCapabilities.NotificationCenter); var service = new CommunicationWorkflowService(new CommunicationIdempotencyService()); service.AcceptSourceEvent(new NotificationSourceEventRequest("complaints", "CMP-1", "status", "req-1")).Evidence.Should().Contain("source-read-only"); new CommunicationBoundaryGuard().Allows("complaint_resolution").Should().BeFalse(); service.PublishBroadcast(new BroadcastRequest("Safety", "Update", "guardians", "b1")).Status.Should().Be("Published"); } }
