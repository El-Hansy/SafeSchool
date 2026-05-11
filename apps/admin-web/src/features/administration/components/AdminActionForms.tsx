"use client";

import { useState, type CSSProperties } from "react";
import { adminRoutes } from "../api/adminApi";
import { postSafeSchoolJson } from "../../common/apiProxyClient";

type ActionState = {
  status: "idle" | "submitting" | "success" | "error";
  message: string;
};

const formStyle = {
  display: "grid",
  gap: "10px",
  gridTemplateColumns: "repeat(auto-fit, minmax(190px, 1fr))",
  alignItems: "end",
} satisfies CSSProperties;

const labelStyle = {
  display: "grid",
  gap: "6px",
  color: "#344054",
  fontWeight: 700,
} satisfies CSSProperties;

const inputStyle = {
  minHeight: "38px",
  border: "1px solid #cfd7e3",
  borderRadius: "6px",
  padding: "7px 8px",
  font: "inherit",
} satisfies CSSProperties;

const buttonStyle = {
  minHeight: "40px",
  border: "1px solid #1d4ed8",
  borderRadius: "6px",
  background: "#1d4ed8",
  color: "#ffffff",
  fontWeight: 800,
  cursor: "pointer",
} satisfies CSSProperties;

export function TenantConfigurationAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          adminRoutes.configuration(schoolAccountId),
          schoolAccountId,
          "tenant-admin",
          {
            capabilityKey: String(form.get("capabilityKey")),
            enabled: String(form.get("enabled")) === "true",
            reason: String(form.get("reason")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Tenant feature configuration submitted with audit evidence.",
        );
      }}
    >
      <Field name="capabilityKey" label="Capability" defaultValue="guardian.mobile.access" />
      <SelectField name="enabled" label="State" options={[["Enabled", "true"], ["Disabled", "false"]]} />
      <Field name="reason" label="Reason" defaultValue="Enable guardian role for pilot demo." />
      <Field name="clientRequestId" label="Request id" defaultValue={`admin-config-${Date.now()}`} />
      <SubmitButton state={state} label="Apply configuration" />
      <ActionMessage state={state} />
    </form>
  );
}

export function AuditExportAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          adminRoutes.auditExport(schoolAccountId),
          schoolAccountId,
          "audit-reviewer",
          {
            scope: String(form.get("scope")),
            reason: String(form.get("reason")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Audit export prepared by the administration API.",
        );
      }}
    >
      <Field name="scope" label="Scope" defaultValue="documents-search-admin-observability" />
      <Field name="reason" label="Reason" defaultValue="Sales demo evidence pack." />
      <Field name="clientRequestId" label="Request id" defaultValue={`audit-export-${Date.now()}`} />
      <SubmitButton state={state} label="Prepare audit export" />
      <ActionMessage state={state} />
    </form>
  );
}

export function OpenIncidentAction({ schoolAccountId, alertId }: { schoolAccountId: string; alertId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          adminRoutes.alertIncident(schoolAccountId, String(form.get("alertId"))),
          schoolAccountId,
          "platform-admin",
          {},
          setState,
          "Incident opened from alert threshold.",
        );
      }}
    >
      <Field name="alertId" label="Alert id" defaultValue={alertId} />
      <SubmitButton state={state} label="Open incident" />
      <ActionMessage state={state} />
    </form>
  );
}

function SelectField({ name, label, options }: { name: string; label: string; options: ReadonlyArray<readonly [string, string]> }) {
  return (
    <label style={labelStyle}>
      {label}
      <select name={name} defaultValue={options[0]?.[1] ?? ""} style={inputStyle} required>
        {options.map(([labelText, value]) => <option key={`${name}-${value}`} value={value}>{labelText}</option>)}
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

function SubmitButton({ state, label }: { state: ActionState; label: string }) {
  return <button type="submit" style={buttonStyle} disabled={state.status === "submitting"}>{label}</button>;
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
  actorReference: string,
  payload: object,
  setState: (state: ActionState) => void,
  successMessage: string,
) {
  setState({ status: "submitting", message: "Submitting..." });
  const response = await postSafeSchoolJson(path, schoolAccountId, actorReference, payload);

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Administration API rejected the action." });
    return;
  }

  setState({ status: "success", message: successMessage });
}
