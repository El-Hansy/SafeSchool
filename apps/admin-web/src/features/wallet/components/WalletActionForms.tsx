"use client";

import { useMemo, useState } from "react";
import {
  type WalletResponse,
  walletRoutes,
} from "../api/client";
import { postSafeSchoolJson } from "../../common/apiProxyClient";

type ActionState = {
  status: "idle" | "submitting" | "success" | "error";
  message: string;
};

type WalletActionProps = {
  schoolAccountId: string;
  wallets: WalletResponse[];
};

const formStyle = {
  display: "grid",
  gap: "10px",
  gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))",
  alignItems: "end",
} satisfies React.CSSProperties;

const labelStyle = {
  display: "grid",
  gap: "6px",
  color: "#344054",
  fontWeight: 700,
} satisfies React.CSSProperties;

const inputStyle = {
  minHeight: "36px",
  border: "1px solid #cfd7e3",
  borderRadius: "6px",
  padding: "6px 8px",
  font: "inherit",
} satisfies React.CSSProperties;

const buttonStyle = {
  minHeight: "38px",
  border: "1px solid #1d4ed8",
  borderRadius: "6px",
  background: "#1d4ed8",
  color: "#ffffff",
  fontWeight: 800,
  cursor: "pointer",
} satisfies React.CSSProperties;

export function CashierTopUpAction({ schoolAccountId, wallets }: WalletActionProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const defaultWalletId = wallets[0]?.walletId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          walletRoutes.cashierTopUp(schoolAccountId),
          schoolAccountId,
          {
            walletId: String(form.get("walletId")),
            clientRequestId: String(form.get("clientRequestId")),
            amountMinor: Number(form.get("amountMinor")),
            currencyCode: "SAR",
            cashierReference: String(form.get("cashierReference")),
            reason: "cashier top-up",
            actorReference: "cashier-demo",
          },
          setState,
          "Cashier top-up accepted by wallet API.",
        );
      }}
    >
      <WalletSelect wallets={wallets} defaultWalletId={defaultWalletId} />
      <Field name="amountMinor" label="Amount halalas" defaultValue="2500" />
      <Field name="cashierReference" label="Cashier reference" defaultValue="cash-receipt-1" />
      <Field name="clientRequestId" label="Request id" defaultValue={`cash-${Date.now()}`} />
      <button type="submit" style={buttonStyle} disabled={state.status === "submitting" || wallets.length === 0}>Credit wallet</button>
      <ActionMessage state={state} />
    </form>
  );
}

export function CanteenPurchaseAction({ schoolAccountId, wallets }: WalletActionProps) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const defaultWalletId = wallets[0]?.walletId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          walletRoutes.posPurchases(schoolAccountId),
          schoolAccountId,
          {
            walletId: String(form.get("walletId")),
            merchantId: String(form.get("merchantId")),
            posTerminalId: String(form.get("posTerminalId")),
            credentialReference: String(form.get("credentialReference")),
            clientPurchaseId: String(form.get("clientPurchaseId")),
            amountMinor: Number(form.get("amountMinor")),
            currencyCode: "SAR",
            itemCategoryCode: String(form.get("itemCategoryCode")),
            itemSummary: String(form.get("itemSummary")),
            operatorReference: "canteen-operator",
            deviceReference: "pos-web",
          },
          setState,
          "Purchase decision returned by wallet API.",
        );
      }}
    >
      <WalletSelect wallets={wallets} defaultWalletId={defaultWalletId} />
      <Field name="merchantId" label="Merchant id" defaultValue="77777777-7777-4777-8777-777777777777" />
      <Field name="posTerminalId" label="Terminal id" defaultValue="88888888-8888-4888-8888-888888888888" />
      <Field name="credentialReference" label="Credential" defaultValue="NFC-AMINA-001" />
      <Field name="amountMinor" label="Amount halalas" defaultValue="850" />
      <Field name="itemCategoryCode" label="Category" defaultValue="meal" />
      <Field name="itemSummary" label="Item" defaultValue="Lunch meal" />
      <Field name="clientPurchaseId" label="Purchase id" defaultValue={`pos-${Date.now()}`} />
      <button type="submit" style={buttonStyle} disabled={state.status === "submitting" || wallets.length === 0}>Charge wallet</button>
      <ActionMessage state={state} />
    </form>
  );
}

function WalletSelect({ wallets, defaultWalletId }: { wallets: WalletResponse[]; defaultWalletId: string }) {
  const options = useMemo(() => wallets.map((wallet) => ({
    label: `${wallet.studentProfileId} - ${wallet.walletCode}`,
    value: wallet.walletId,
  })), [wallets]);

  return (
    <label style={labelStyle}>
      Wallet
      <select name="walletId" defaultValue={defaultWalletId} style={inputStyle} required>
        {options.map((option) => <option key={option.value} value={option.value}>{option.label}</option>)}
      </select>
    </label>
  );
}

function Field({ name, label, defaultValue }: { name: string; label: string; defaultValue: string }) {
  return (
    <label style={labelStyle}>
      {label}
      <input name={name} defaultValue={defaultValue} style={inputStyle} required />
    </label>
  );
}

function ActionMessage({ state }: { state: ActionState }) {
  if (state.status === "idle") return null;

  return (
    <p style={{ gridColumn: "1 / -1", margin: 0, color: state.status === "error" ? "#b42318" : "#166534", fontWeight: 800 }}>
      {state.message}
    </p>
  );
}

async function submitJson(
  path: string,
  schoolAccountId: string,
  payload: object,
  setState: (state: ActionState) => void,
  successMessage: string,
) {
  setState({ status: "submitting", message: "Submitting..." });
  const response = await postSafeSchoolJson(path, schoolAccountId, "finance-admin", payload);

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Wallet API rejected the action." });
    return;
  }

  setState({ status: "success", message: successMessage });
}
