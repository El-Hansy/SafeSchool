"use client";

import { useState } from "react";

export function GuardianForm({ onSubmit }: { onSubmit?: (displayName: string) => void }) {
  const [displayName, setDisplayName] = useState("");

  return (
    <form
      onSubmit={(event) => {
        event.preventDefault();
        onSubmit?.(displayName);
      }}
      style={{ display: "flex", gap: "10px", alignItems: "end" }}
    >
      <label>
        <span style={{ display: "block", marginBottom: "6px" }}>Guardian name</span>
        <input value={displayName} onChange={(event) => setDisplayName(event.target.value)} />
      </label>
      <button type="submit">Create guardian</button>
    </form>
  );
}
