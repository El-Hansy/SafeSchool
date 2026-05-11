"use client";

import { useState, type CSSProperties } from "react";
import { postSafeSchoolJson } from "../../common/apiProxyClient";
import { requestRoutes, type RequestAudience, type RequestResponse } from "../api/requestsApi";

type ActionState = { status: "idle" | "submitting" | "success" | "error"; message: string };

const formStyle = { display: "grid", gap: 10, gridTemplateColumns: "repeat(auto-fit, minmax(190px, 1fr))", alignItems: "end" } satisfies CSSProperties;
const labelStyle = { display: "grid", gap: 6, color: "#344054", fontWeight: 700 } satisfies CSSProperties;
const inputStyle = { minHeight: 38, border: "1px solid #cfd7e3", borderRadius: 6, padding: "7px 8px", font: "inherit" } satisfies CSSProperties;
const buttonStyle = { minHeight: 40, border: "1px solid #1d4ed8", borderRadius: 6, background: "#1d4ed8", color: "#fff", fontWeight: 800, cursor: "pointer" } satisfies CSSProperties;

export function RequestSubmitAction({ audience, schoolAccountId }: { audience: RequestAudience; schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const path = audience === "guardian" ? requestRoutes.guardian() : audience === "student" ? requestRoutes.student() : requestRoutes.school(schoolAccountId);
        await submitJson(path, schoolAccountId, actorFor(audience), {
          studentProfileId: String(form.get("studentProfileId")),
          requestType: String(form.get("requestType")),
          reason: String(form.get("reason")),
          requestedOutcome: String(form.get("requestedOutcome")),
          startsAt: String(form.get("startsAt")),
          endsAt: String(form.get("endsAt")),
          clientRequestId: String(form.get("clientRequestId")),
          submitterRole: audience === "school" ? "staff" : audience,
        }, setState, "Request submitted to approval workflow.");
      }}
    >
      <Field name="studentProfileId" label="Student" defaultValue="student-amina" />
      <Field name="requestType" label="Type" defaultValue="outing" />
      <Field name="reason" label="Reason" defaultValue="Library activity permission requested by guardian." />
      <Field name="requestedOutcome" label="Requested outcome" defaultValue="Approve outing and notify guardian." />
      <Field name="startsAt" label="Starts at" defaultValue="2026-05-12T08:00:00Z" />
      <Field name="endsAt" label="Ends at" defaultValue="2026-05-12T10:00:00Z" />
      <Field name="clientRequestId" label="Request id" defaultValue={`request-${Date.now()}`} />
      <SubmitButton state={state} label="Submit request" />
      <ActionMessage state={state} />
    </form>
  );
}

export function RequestApprovalAction({ schoolAccountId, requests }: { schoolAccountId: string; requests: RequestResponse[] }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const defaultRequestId = requests[0]?.requestId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const requestId = String(form.get("requestId"));
        const action = String(form.get("action"));
        const path = action === "approve" ? requestRoutes.approve(schoolAccountId, requestId) : action === "reject" ? requestRoutes.reject(schoolAccountId, requestId) : requestRoutes.cancel(schoolAccountId, requestId);
        await submitJson(path, schoolAccountId, "request-approver", {
          action,
          reason: String(form.get("reason")),
          actorId: "request-approver",
          clientRequestId: String(form.get("clientRequestId")),
        }, setState, `Request ${action} action accepted.`);
      }}
    >
      <label style={labelStyle}>
        Request
        <select name="requestId" defaultValue={defaultRequestId} style={inputStyle} required>
          {requests.map((request) => <option key={request.requestId} value={request.requestId}>{request.trackingReference}</option>)}
        </select>
      </label>
      <label style={labelStyle}>
        Action
        <select name="action" defaultValue="approve" style={inputStyle} required>
          <option value="approve">Approve</option>
          <option value="reject">Reject</option>
          <option value="cancel">Cancel</option>
        </select>
      </label>
      <Field name="reason" label="Reason" defaultValue="Reviewed against school approval policy." />
      <Field name="clientRequestId" label="Action id" defaultValue={`request-action-${Date.now()}`} />
      <SubmitButton state={state} label="Record decision" disabled={requests.length === 0} />
      <ActionMessage state={state} />
    </form>
  );
}

function Field({ name, label, defaultValue }: { name: string; label: string; defaultValue: string }) {
  return <label style={labelStyle}>{label}<input name={name} defaultValue={defaultValue} style={inputStyle} required /></label>;
}

function SubmitButton({ state, label, disabled = false }: { state: ActionState; label: string; disabled?: boolean }) {
  return <button type="submit" style={buttonStyle} disabled={state.status === "submitting" || disabled}>{label}</button>;
}

function ActionMessage({ state }: { state: ActionState }) {
  if (state.status === "idle") return null;
  return <p style={{ gridColumn: "1 / -1", margin: 0, color: state.status === "error" ? "#b42318" : "#166534", fontWeight: 800 }}>{state.message}</p>;
}

async function submitJson(path: string, schoolAccountId: string, actorReference: string, payload: object, setState: (state: ActionState) => void, successMessage: string) {
  setState({ status: "submitting", message: "Submitting..." });
  const response = await postSafeSchoolJson(path, schoolAccountId, actorReference, payload);
  if (!response.ok) {
    const body = await response.json().catch(() => null);
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Requests API rejected the action." });
    return;
  }
  setState({ status: "success", message: successMessage });
}

function actorFor(audience: RequestAudience) {
  if (audience === "guardian") return "guardian-demo";
  if (audience === "student") return "student-demo";
  return "request-operator";
}

