"use client";

import { useState, type CSSProperties } from "react";
import { postSafeSchoolJson } from "../../common/apiProxyClient";
import { medicalRoutes, type MedicalResponse } from "../api/medicalApi";

type ActionState = { status: "idle" | "submitting" | "success" | "error"; message: string };

const formStyle = { display: "grid", gap: 10, gridTemplateColumns: "repeat(auto-fit, minmax(190px, 1fr))", alignItems: "end" } satisfies CSSProperties;
const labelStyle = { display: "grid", gap: 6, color: "#344054", fontWeight: 700 } satisfies CSSProperties;
const inputStyle = { minHeight: 38, border: "1px solid #cfd7e3", borderRadius: 6, padding: "7px 8px", font: "inherit" } satisfies CSSProperties;
const buttonStyle = { minHeight: 40, border: "1px solid #1d4ed8", borderRadius: 6, background: "#1d4ed8", color: "#fff", fontWeight: 800, cursor: "pointer" } satisfies CSSProperties;

export function MedicalProfileAction({ schoolAccountId, guardian = false }: { schoolAccountId: string; guardian?: boolean }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  return (
    <form style={formStyle} onSubmit={async (event) => {
      event.preventDefault();
      const form = new FormData(event.currentTarget);
      await submitJson(guardian ? medicalRoutes.guardianUpdates() : medicalRoutes.records(schoolAccountId), schoolAccountId, guardian ? "guardian-demo" : "school-nurse", {
        studentProfileId: String(form.get("studentProfileId")),
        summary: String(form.get("summary")),
        restrictedDetail: String(form.get("restrictedDetail")),
        severity: String(form.get("severity")),
        clientRequestId: String(form.get("clientRequestId")),
      }, setState, guardian ? "Guardian medical update submitted for nurse review." : "Medical profile update recorded.");
    }}>
      <Field name="studentProfileId" label="Student" defaultValue="student-amina" />
      <Field name="summary" label="Summary" defaultValue="Allergy plan reviewed and visible to guardian." />
      <Field name="restrictedDetail" label="Restricted detail" defaultValue="Clinic-only note retained with privacy controls." />
      <Field name="severity" label="Severity" defaultValue="Routine" />
      <Field name="clientRequestId" label="Request id" defaultValue={`medical-${Date.now()}`} />
      <SubmitButton state={state} label={guardian ? "Submit update" : "Save profile"} />
      <ActionMessage state={state} />
    </form>
  );
}

export function EmergencyAccessAction({ schoolAccountId, breakGlass = false }: { schoolAccountId: string; breakGlass?: boolean }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  return (
    <form style={formStyle} onSubmit={async (event) => {
      event.preventDefault();
      const form = new FormData(event.currentTarget);
      await submitJson(breakGlass ? medicalRoutes.breakGlass(schoolAccountId) : medicalRoutes.emergencyAccess(schoolAccountId), schoolAccountId, breakGlass ? "emergency-authorized-staff" : "school-nurse", {
        studentProfileId: String(form.get("studentProfileId")),
        reason: String(form.get("reason")),
        actorId: breakGlass ? "emergency-authorized-staff" : "school-nurse",
        clientRequestId: String(form.get("clientRequestId")),
      }, setState, breakGlass ? "Break-glass emergency access opened and routed to review." : "Emergency access opened for minimum necessary medical details.");
    }}>
      <Field name="studentProfileId" label="Student" defaultValue="student-omar" />
      <Field name="reason" label="Reason" defaultValue="Immediate clinic response requires critical summary." />
      <Field name="clientRequestId" label="Access id" defaultValue={`emergency-${Date.now()}`} />
      <SubmitButton state={state} label={breakGlass ? "Break glass" : "Open emergency access"} />
      <ActionMessage state={state} />
    </form>
  );
}

export function MedicalIncidentAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  return (
    <form style={formStyle} onSubmit={async (event) => {
      event.preventDefault();
      const form = new FormData(event.currentTarget);
      await submitJson(medicalRoutes.incidents(schoolAccountId), schoolAccountId, "school-nurse", {
        studentProfileId: String(form.get("studentProfileId")),
        severity: String(form.get("severity")),
        observation: String(form.get("observation")),
        careAction: String(form.get("careAction")),
        clientRequestId: String(form.get("clientRequestId")),
      }, setState, "Medical incident logged with care action evidence.");
    }}>
      <Field name="studentProfileId" label="Student" defaultValue="student-amina" />
      <Field name="severity" label="Severity" defaultValue="High" />
      <Field name="observation" label="Observation" defaultValue="Student reported allergy symptoms after lunch." />
      <Field name="careAction" label="Care action" defaultValue="Moved to clinic and guardian contact started." />
      <Field name="clientRequestId" label="Incident id" defaultValue={`incident-${Date.now()}`} />
      <SubmitButton state={state} label="Log incident" />
      <ActionMessage state={state} />
    </form>
  );
}

export function MedicalReviewAction({ schoolAccountId, records }: { schoolAccountId: string; records: MedicalResponse[] }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const defaultRecordId = records[0]?.medicalRecordId ?? "";
  return (
    <form style={formStyle} onSubmit={async (event) => {
      event.preventDefault();
      const form = new FormData(event.currentTarget);
      await submitJson(medicalRoutes.resolveReview(schoolAccountId, String(form.get("recordId"))), schoolAccountId, "medical-reviewer", {
        action: "resolve",
        reason: String(form.get("reason")),
        actorId: "medical-reviewer",
        clientRequestId: String(form.get("clientRequestId")),
      }, setState, "Medical review resolved with evidence preserved.");
    }}>
      <label style={labelStyle}>Record<select name="recordId" defaultValue={defaultRecordId} style={inputStyle} required>{records.map((record) => <option key={record.medicalRecordId} value={record.medicalRecordId}>{record.recordReference}</option>)}</select></label>
      <Field name="reason" label="Reason" defaultValue="Reviewed by nurse and ready for history." />
      <Field name="clientRequestId" label="Review id" defaultValue={`medical-review-${Date.now()}`} />
      <SubmitButton state={state} label="Resolve review" disabled={records.length === 0} />
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
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Medical API rejected the action." });
    return;
  }
  setState({ status: "success", message: successMessage });
}

