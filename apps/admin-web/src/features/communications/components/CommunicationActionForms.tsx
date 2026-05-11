"use client";

import { useState, type CSSProperties } from "react";
import {
  communicationsRoutes,
} from "../api/communicationsApi";
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

export function SourceEventAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          communicationsRoutes.sourceEvents(schoolAccountId),
          schoolAccountId,
          {
            sourceModule: String(form.get("sourceModule")),
            sourceReference: String(form.get("sourceReference")),
            category: String(form.get("category")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Source event accepted and notification workflow started.",
        );
      }}
    >
      <Field name="sourceModule" label="Source module" defaultValue="attendance-access" />
      <Field name="sourceReference" label="Source reference" defaultValue="entry-exit-2026-05-10" />
      <Field name="category" label="Category" defaultValue="attendance-entry" />
      <Field name="clientRequestId" label="Request id" defaultValue={`source-${Date.now()}`} />
      <SubmitButton state={state} label="Accept source event" />
      <ActionMessage state={state} />
    </form>
  );
}

export function DirectMessageAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          communicationsRoutes.conversations(schoolAccountId),
          schoolAccountId,
          {
            subject: String(form.get("subject")),
            body: String(form.get("body")),
            recipientScope: String(form.get("recipientScope")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Direct message submitted to communication API.",
        );
      }}
    >
      <Field name="subject" label="Subject" defaultValue="Pickup update" />
      <Field name="body" label="Body" defaultValue="Amina is ready for pickup at the front office." />
      <Field name="recipientScope" label="Recipient scope" defaultValue="guardian:student-amina" />
      <Field name="clientRequestId" label="Request id" defaultValue={`message-${Date.now()}`} />
      <SubmitButton state={state} label="Send message" />
      <ActionMessage state={state} />
    </form>
  );
}

export function BroadcastAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          communicationsRoutes.broadcasts(schoolAccountId),
          schoolAccountId,
          {
            title: String(form.get("title")),
            body: String(form.get("body")),
            audienceRule: String(form.get("audienceRule")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Broadcast submitted to communication API.",
        );
      }}
    >
      <Field name="title" label="Title" defaultValue="Early dismissal" />
      <Field name="body" label="Body" defaultValue="Grade 4 will dismiss at 12:30 today." />
      <Field name="audienceRule" label="Audience rule" defaultValue="grade:4" />
      <Field name="clientRequestId" label="Request id" defaultValue={`broadcast-${Date.now()}`} />
      <SubmitButton state={state} label="Publish broadcast" />
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
  payload: object,
  setState: (state: ActionState) => void,
  successMessage: string,
) {
  setState({ status: "submitting", message: "Submitting..." });
  const response = await postSafeSchoolJson(path, schoolAccountId, "communications-operator", payload);

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Communication API rejected the action." });
    return;
  }

  setState({ status: "success", message: successMessage });
}
