"use client";

import { useState, type CSSProperties } from "react";
import {
  documentsRoutes,
  type CertificateResponse,
  type DocumentResponse,
} from "../api/documentStorageApi";
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

export function DocumentUploadAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          documentsRoutes.documents(schoolAccountId),
          schoolAccountId,
          "document-admin",
          {
            title: String(form.get("title")),
            categoryCode: String(form.get("categoryCode")),
            subjectReference: String(form.get("subjectReference")),
            sourceModule: String(form.get("sourceModule")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Document metadata submitted to document API.",
        );
      }}
    >
      <Field name="title" label="Title" defaultValue="Amina consent form" />
      <Field name="categoryCode" label="Category" defaultValue="guardian-consent" />
      <Field name="subjectReference" label="Subject" defaultValue="student-amina" />
      <Field name="sourceModule" label="Source module" defaultValue="identity-access" />
      <Field name="clientRequestId" label="Request id" defaultValue={`document-${Date.now()}`} />
      <SubmitButton state={state} label="Upload metadata" />
      <ActionMessage state={state} />
    </form>
  );
}

export function DocumentHoldExportAction({ schoolAccountId, documents }: { schoolAccountId: string; documents: DocumentResponse[] }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const defaultDocument = documents[0]?.reference ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        const documentId = String(form.get("documentId"));
        const action = String(form.get("action"));
        await submitJson(
          action === "hold" ? documentsRoutes.documentHold(schoolAccountId, documentId) : documentsRoutes.documentExport(schoolAccountId, documentId),
          schoolAccountId,
          "document-reviewer",
          {
            action,
            reason: String(form.get("reason")),
            actorId: "document-reviewer",
          },
          setState,
          `Document ${action} action accepted by API.`,
        );
      }}
    >
      <SelectField name="documentId" label="Document" options={documents.map((document) => [document.reference, document.reference] as const)} defaultValue={defaultDocument} />
      <SelectField name="action" label="Action" options={[["Legal hold", "hold"], ["Controlled export", "export"]]} />
      <Field name="reason" label="Reason" defaultValue="Authorized reviewer request with audit evidence." />
      <SubmitButton state={state} label="Record document action" disabled={documents.length === 0} />
      <ActionMessage state={state} />
    </form>
  );
}

export function CertificateIssueAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          documentsRoutes.certificates(schoolAccountId),
          schoolAccountId,
          "certificate-issuer",
          {
            certificateType: String(form.get("certificateType")),
            subjectReference: String(form.get("subjectReference")),
            sourceReference: String(form.get("sourceReference")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Certificate issue request submitted to API.",
        );
      }}
    >
      <Field name="certificateType" label="Certificate type" defaultValue="attendance-letter" />
      <Field name="subjectReference" label="Subject" defaultValue="student-amina" />
      <Field name="sourceReference" label="Source reference" defaultValue="attendance-2026-05" />
      <Field name="clientRequestId" label="Request id" defaultValue={`certificate-${Date.now()}`} />
      <SubmitButton state={state} label="Issue certificate" />
      <ActionMessage state={state} />
    </form>
  );
}

export function CertificateVerifyAction({ schoolAccountId, certificates }: { schoolAccountId: string; certificates: CertificateResponse[] }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });
  const defaultCertificate = certificates[0]?.reference ?? "";

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          documentsRoutes.certificateVerify(schoolAccountId, String(form.get("certificateId"))),
          schoolAccountId,
          "certificate-verifier",
          {},
          setState,
          "Certificate verification returned by API.",
        );
      }}
    >
      <SelectField name="certificateId" label="Certificate" options={certificates.map((certificate) => [certificate.reference, certificate.reference] as const)} defaultValue={defaultCertificate} />
      <SubmitButton state={state} label="Verify certificate" disabled={certificates.length === 0} />
      <ActionMessage state={state} />
    </form>
  );
}

export function SearchSubmitAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          documentsRoutes.search(schoolAccountId),
          schoolAccountId,
          "search-reviewer",
          {
            text: String(form.get("text")),
            scope: String(form.get("scope")),
            page: 1,
            pageSize: 25,
          },
          setState,
          "Search query submitted with access revalidation.",
        );
      }}
    >
      <Field name="text" label="Search text" defaultValue="Amina attendance certificate" />
      <Field name="scope" label="Scope" defaultValue="school" />
      <SubmitButton state={state} label="Search" />
      <ActionMessage state={state} />
    </form>
  );
}

function SelectField({ name, label, options, defaultValue }: { name: string; label: string; options: ReadonlyArray<readonly [string, string]>; defaultValue?: string }) {
  return (
    <label style={labelStyle}>
      {label}
      <select name={name} defaultValue={defaultValue ?? options[0]?.[1] ?? ""} style={inputStyle} required>
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
  setState({ status: "submitting", message: "Submitting..." });
  const response = await postSafeSchoolJson(path, schoolAccountId, actorReference, payload);

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Documents API rejected the action." });
    return;
  }

  setState({ status: "success", message: successMessage });
}
