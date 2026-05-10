"use client";

import { useState, type CSSProperties } from "react";
import {
  complaintsApiBaseUrl,
  complaintsHeaders,
  complaintsRoutes,
  type ComplaintAudience,
  type ComplaintResponse,
} from "../api/complaintsApi";

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

export function ComplaintSubmitAction({ audience, schoolAccountId }: { audience: ComplaintAudience; schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const path = audience === "school" ? complaintsRoutes.school(schoolAccountId) : audience === "guardian" ? complaintsRoutes.guardian() : complaintsRoutes.student();
        await submitJson(
          path,
          schoolAccountId,
          actorFor(audience),
          {
            studentProfileId: String(form.get("studentProfileId")),
            categoryCode: String(form.get("categoryCode")),
            description: String(form.get("description")),
            requestedOutcome: String(form.get("requestedOutcome")),
            clientRequestId: String(form.get("clientRequestId")),
            submitterRole: audience === "school" ? "staff" : audience,
          },
          setState,
          "Complaint submitted and tracking reference returned by API.",
        );
      }}
    >
      <Field name="studentProfileId" label="Student profile" defaultValue="student-amina" />
      <Field name="categoryCode" label="Category" defaultValue="wellbeing" />
      <Field name="description" label="Description" defaultValue="Student needs confidential wellbeing follow-up." />
      <Field name="requestedOutcome" label="Requested outcome" defaultValue="Assign wellbeing owner and notify guardian with safe summary." />
      <Field name="clientRequestId" label="Request id" defaultValue={`complaint-${Date.now()}`} />
      <SubmitButton state={state} label="Submit complaint" />
      <ActionMessage state={state} />
    </form>
  );
}

export function ComplaintStatusAction({ schoolAccountId, complaints }: { schoolAccountId: string; complaints: ComplaintResponse[] }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const defaultComplaintId = complaints[0]?.complaintId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const complaintId = String(form.get("complaintId"));
        const action = String(form.get("action"));
        const path = action === "assign"
          ? complaintsRoutes.assign(schoolAccountId, complaintId)
          : action === "escalate"
            ? complaintsRoutes.escalate(schoolAccountId, complaintId)
            : complaintsRoutes.resolve(schoolAccountId, complaintId);

        await submitJson(
          path,
          schoolAccountId,
          "complaints-owner",
          {
            action,
            reason: String(form.get("reason")),
            actorId: "complaints-owner",
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          `Complaint ${action} action accepted by API.`,
        );
      }}
    >
      <label style={labelStyle}>
        Complaint
        <select name="complaintId" defaultValue={defaultComplaintId} style={inputStyle} required>
          {complaints.map((complaint) => <option key={complaint.complaintId} value={complaint.complaintId}>{complaint.trackingReference}</option>)}
        </select>
      </label>
      <label style={labelStyle}>
        Action
        <select name="action" defaultValue="assign" style={inputStyle} required>
          <option value="assign">Assign</option>
          <option value="escalate">Escalate</option>
          <option value="resolve">Resolve</option>
        </select>
      </label>
      <Field name="reason" label="Reason" defaultValue="Reviewed by complaint owner queue." />
      <Field name="clientRequestId" label="Request id" defaultValue={`complaint-action-${Date.now()}`} />
      <SubmitButton state={state} label="Record action" disabled={complaints.length === 0} />
      <ActionMessage state={state} />
    </form>
  );
}

export function ComplaintFeedbackAction({ schoolAccountId, complaints }: { schoolAccountId: string; complaints: ComplaintResponse[] }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const defaultComplaintId = complaints[0]?.complaintId ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          complaintsRoutes.guardianFeedback(String(form.get("complaintId"))),
          schoolAccountId,
          "guardian-demo",
          {
            action: "feedback",
            reason: String(form.get("reason")),
            actorId: "guardian-demo",
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Guardian feedback submitted to complaint workflow.",
        );
      }}
    >
      <label style={labelStyle}>
        Complaint
        <select name="complaintId" defaultValue={defaultComplaintId} style={inputStyle} required>
          {complaints.map((complaint) => <option key={complaint.complaintId} value={complaint.complaintId}>{complaint.trackingReference}</option>)}
        </select>
      </label>
      <Field name="reason" label="Feedback" defaultValue="The visible summary is accepted by the guardian." />
      <Field name="clientRequestId" label="Request id" defaultValue={`complaint-feedback-${Date.now()}`} />
      <SubmitButton state={state} label="Send feedback" disabled={complaints.length === 0} />
      <ActionMessage state={state} />
    </form>
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

function SubmitButton({ state, label, disabled = false }: { state: ActionState; label: string; disabled?: boolean }) {
  return <button type="submit" style={buttonStyle} disabled={state.status === "submitting" || disabled}>{label}</button>;
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
  const baseUrl = complaintsApiBaseUrl();
  if (!baseUrl) {
    setState({ status: "error", message: "Set NEXT_PUBLIC_API_BASE_URL to submit this complaint action to the SafeSchool API." });
    return;
  }

  setState({ status: "submitting", message: "Submitting..." });
  const response = await fetch(`${baseUrl}${path}`, {
    method: "POST",
    headers: complaintsHeaders(schoolAccountId, actorReference),
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Complaint API rejected the action." });
    return;
  }

  setState({ status: "success", message: successMessage });
}

function actorFor(audience: ComplaintAudience) {
  if (audience === "guardian") return "guardian-demo";
  if (audience === "student") return "student-demo";
  return "complaints-operator";
}
