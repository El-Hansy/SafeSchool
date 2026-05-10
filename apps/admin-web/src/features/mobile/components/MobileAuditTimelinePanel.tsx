import { mobileSupportEvents } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function MobileAuditTimelinePanel() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Audit timeline</h2>
      {mobileSupportEvents.map((event) => (
        <p key={event.id}>{event.result}: {event.reason}</p>
      ))}
    </section>
  );
}
