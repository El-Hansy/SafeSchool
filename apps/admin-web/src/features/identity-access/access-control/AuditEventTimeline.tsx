import type { AuditEvent } from "./accessControlApi";

export function AuditEventTimeline({ events }: { events: AuditEvent[] }) {
  return (
    <ol style={{ display: "grid", gap: "10px", paddingLeft: "22px" }}>
      {events.map((event) => (
        <li key={event.auditEventId}>
          <strong>{event.eventType}</strong>
          <div style={{ color: "#536179" }}>
            {event.actorReference} on {event.subjectType} {event.subjectReference}: {event.reason}
          </div>
        </li>
      ))}
    </ol>
  );
}
