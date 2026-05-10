import { mobileSupportEvents } from "../api/mobileApi";
import { mobileStyles } from "./MobileAdminLayout";

export function MobileDeviceSessionPanel() {
  return (
    <section style={mobileStyles.panel}>
      <h2>Device sessions</h2>
      {mobileSupportEvents.map((event) => (
        <p key={event.id}>{event.user} on {event.device} using {event.version}</p>
      ))}
    </section>
  );
}
