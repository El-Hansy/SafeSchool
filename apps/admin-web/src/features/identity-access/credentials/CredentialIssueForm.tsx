"use client";

import { useState } from "react";

export function CredentialIssueForm({ onIssue }: { onIssue?: (cardReference: string) => void }) {
  const [cardReference, setCardReference] = useState("");

  return (
    <form
      onSubmit={(event) => {
        event.preventDefault();
        onIssue?.(cardReference);
      }}
      style={{ display: "flex", gap: "10px", alignItems: "end" }}
    >
      <label>
        <span style={{ display: "block", marginBottom: "6px" }}>Card reference</span>
        <input value={cardReference} onChange={(event) => setCardReference(event.target.value)} />
      </label>
      <button type="submit">Issue NFC</button>
    </form>
  );
}
