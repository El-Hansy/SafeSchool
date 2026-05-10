"use client";

import { useState } from "react";
import { guardianWalletRoutes } from "../api/client";
import { walletApiBaseUrl, walletHeaders } from "../../wallet/api/client";

type Props = {
  walletId: string;
  studentProfileId: string;
};

export function GuardianTopUpAction({ walletId, studentProfileId }: Props) {
  const [message, setMessage] = useState("");

  return (
    <form
      style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))", gap: 10, alignItems: "end" }}
      onSubmit={async (event) => {
        event.preventDefault();
        const baseUrl = walletApiBaseUrl();
        if (!baseUrl) {
          setMessage("Set NEXT_PUBLIC_API_BASE_URL to submit top-ups to the SafeSchool API.");
          return;
        }

        const form = new FormData(event.currentTarget);
        const response = await fetch(`${baseUrl}${guardianWalletRoutes.topUps(studentProfileId)}`, {
          method: "POST",
          headers: walletHeaders("school-demo", "guardian-demo"),
          body: JSON.stringify({
            walletId,
            clientRequestId: String(form.get("clientRequestId")),
            amountMinor: Number(form.get("amountMinor")),
            currencyCode: "SAR",
            paymentProvider: "DemoPay",
            guardianActorId: "guardian-demo",
          }),
        });

        setMessage(response.ok ? "Top-up request submitted to wallet API." : "Wallet API rejected the top-up request.");
      }}
    >
      <label style={{ display: "grid", gap: 6, fontWeight: 800 }}>
        Amount halalas
        <input name="amountMinor" defaultValue="5000" required style={{ minHeight: 36, border: "1px solid #cfd7e3", borderRadius: 6, padding: "6px 8px" }} />
      </label>
      <label style={{ display: "grid", gap: 6, fontWeight: 800 }}>
        Request id
        <input name="clientRequestId" defaultValue={`guardian-${Date.now()}`} required style={{ minHeight: 36, border: "1px solid #cfd7e3", borderRadius: 6, padding: "6px 8px" }} />
      </label>
      <button type="submit" style={{ minHeight: 38, border: "1px solid #1d4ed8", borderRadius: 6, background: "#1d4ed8", color: "#ffffff", fontWeight: 800 }}>Add amount</button>
      {message ? <p style={{ gridColumn: "1 / -1", margin: 0, color: message.includes("rejected") ? "#b42318" : "#166534", fontWeight: 800 }}>{message}</p> : null}
    </form>
  );
}
