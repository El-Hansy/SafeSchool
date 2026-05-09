import type { GateSummary } from "./gatesApi";

export function GateForm({ gate }: { gate?: GateSummary }) {
  return (
    <form aria-label="Gate">
      <label>
        Gate code
        <input name="gateCode" defaultValue={gate?.gateCode ?? ""} />
      </label>
      <label>
        Display name
        <input name="displayName" defaultValue={gate?.displayName ?? ""} />
      </label>
      <label>
        Offline allowed
        <input name="offlineAllowed" type="checkbox" defaultChecked={gate?.offlineAllowed ?? true} />
      </label>
      <button type="submit">Save</button>
    </form>
  );
}

export function ScanPointForm() {
  return (
    <form aria-label="Scan point">
      <label>
        Device reference
        <input name="deviceReference" />
      </label>
      <button type="submit">Register</button>
    </form>
  );
}

