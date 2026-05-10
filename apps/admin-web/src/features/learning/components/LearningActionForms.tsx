"use client";

import { useState, type CSSProperties } from "react";
import { learningApiBaseUrl, learningHeaders, learningRoutes } from "../api/learningApi";

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

export function PublishContentAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.content(schoolAccountId),
          schoolAccountId,
          "teacher",
          {
            title: String(form.get("title")),
            groupReference: String(form.get("groupReference")),
            visibility: String(form.get("visibility")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Content published with visibility and audit evidence.",
        );
      }}
    >
      <Field name="title" label="Title" defaultValue="Solar System lesson" />
      <Field name="groupReference" label="Group" defaultValue="grade-5a" />
      <SelectField name="visibility" label="Visibility" options={[["Students and guardians", "student_guardian"], ["Students only", "student"]]} />
      <Field name="clientRequestId" label="Request id" defaultValue={`content-${Date.now()}`} />
      <SubmitButton state={state} label="Publish content" />
      <ActionMessage state={state} />
    </form>
  );
}

export function CreateAssignmentAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.assignments(schoolAccountId),
          schoolAccountId,
          "teacher",
          {
            title: String(form.get("title")),
            groupReference: String(form.get("groupReference")),
            dueDate: String(form.get("dueDate")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Assignment created and assigned to the learning group.",
        );
      }}
    >
      <Field name="title" label="Title" defaultValue="Planet worksheet" />
      <Field name="groupReference" label="Group" defaultValue="grade-5a" />
      <Field name="dueDate" label="Due date" defaultValue="2026-05-20" />
      <Field name="clientRequestId" label="Request id" defaultValue={`assignment-${Date.now()}`} />
      <SubmitButton state={state} label="Create assignment" />
      <ActionMessage state={state} />
    </form>
  );
}

export function SubmitAssignmentAction({ schoolAccountId, studentProfileId }: { schoolAccountId: string; studentProfileId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.submissions(schoolAccountId),
          schoolAccountId,
          "student",
          {
            assignmentReference: String(form.get("assignmentReference")),
            studentProfileId,
            submissionText: String(form.get("submissionText")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Assignment submission recorded for student and guardian visibility.",
        );
      }}
    >
      <Field name="assignmentReference" label="Assignment" defaultValue="assignment-planet-worksheet" />
      <Field name="submissionText" label="Submission" defaultValue="Completed worksheet attached." />
      <Field name="clientRequestId" label="Request id" defaultValue={`submission-${Date.now()}`} />
      <SubmitButton state={state} label="Submit assignment" />
      <ActionMessage state={state} />
    </form>
  );
}

export function StartQuizAttemptAction({ schoolAccountId, studentProfileId }: { schoolAccountId: string; studentProfileId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.quizAttempts(schoolAccountId),
          schoolAccountId,
          "student",
          {
            quizReference: String(form.get("quizReference")),
            studentProfileId,
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Quiz attempt scored with feedback controls.",
        );
      }}
    >
      <Field name="quizReference" label="Quiz" defaultValue="quiz-planets-001" />
      <Field name="clientRequestId" label="Request id" defaultValue={`quiz-${Date.now()}`} />
      <SubmitButton state={state} label="Start quiz attempt" />
      <ActionMessage state={state} />
    </form>
  );
}

export function PostStarSourceEventAction({ schoolAccountId, studentProfileId }: { schoolAccountId: string; studentProfileId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.starSourceEvents(schoolAccountId),
          schoolAccountId,
          "teacher",
          {
            studentProfileId,
            points: Number(form.get("points")),
            sourceReference: String(form.get("sourceReference")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Stars posted to append-only learning ledger.",
        );
      }}
    >
      <Field name="points" label="Points" defaultValue="5" />
      <Field name="sourceReference" label="Source" defaultValue="quiz-planets-001" />
      <Field name="clientRequestId" label="Request id" defaultValue={`stars-${Date.now()}`} />
      <SubmitButton state={state} label="Post stars" />
      <ActionMessage state={state} />
    </form>
  );
}

export function RedeemRewardAction({ schoolAccountId, studentProfileId }: { schoolAccountId: string; studentProfileId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.rewardRedemptions(schoolAccountId),
          schoolAccountId,
          "student",
          {
            studentProfileId,
            rewardReference: String(form.get("rewardReference")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Reward redemption reserved without wallet mutation.",
        );
      }}
    >
      <Field name="rewardReference" label="Reward" defaultValue="reward-library-pass" />
      <Field name="clientRequestId" label="Request id" defaultValue={`reward-${Date.now()}`} />
      <SubmitButton state={state} label="Redeem reward" />
      <ActionMessage state={state} />
    </form>
  );
}

export function LogBehaviorEventAction({ schoolAccountId, studentProfileId }: { schoolAccountId: string; studentProfileId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.behavior(schoolAccountId),
          schoolAccountId,
          "teacher",
          {
            studentProfileId,
            categoryCode: String(form.get("categoryCode")),
            summary: String(form.get("summary")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Behavior event recorded with audience visibility controls.",
        );
      }}
    >
      <Field name="categoryCode" label="Category" defaultValue="teamwork" />
      <Field name="summary" label="Summary" defaultValue="Collaborated well in science group." />
      <Field name="clientRequestId" label="Request id" defaultValue={`behavior-${Date.now()}`} />
      <SubmitButton state={state} label="Log behavior" />
      <ActionMessage state={state} />
    </form>
  );
}

export function ConfigureLearningRuleAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.configuration(schoolAccountId),
          schoolAccountId,
          "learning-admin",
          {
            ruleKey: String(form.get("ruleKey")),
            enabled: String(form.get("enabled")) === "true",
            reason: String(form.get("reason")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Learning rule configuration applied.",
        );
      }}
    >
      <Field name="ruleKey" label="Rule" defaultValue="guardian.behavior.summary" />
      <SelectField name="enabled" label="State" options={[["Enabled", "true"], ["Disabled", "false"]]} />
      <Field name="reason" label="Reason" defaultValue="Allow summary visibility for guardian demo." />
      <Field name="clientRequestId" label="Request id" defaultValue={`learning-rule-${Date.now()}`} />
      <SubmitButton state={state} label="Apply rule" />
      <ActionMessage state={state} />
    </form>
  );
}

export function CreateManualReviewAction({ schoolAccountId }: { schoolAccountId: string }) {
  const [state, setState] = useState<ActionState>({ status: "idle", message: "" });

  return (
    <form
      style={formStyle}
      onSubmit={async (event) => {
        event.preventDefault();
        const form = new FormData(event.currentTarget);
        await submitJson(
          learningRoutes.manualReviews(schoolAccountId),
          schoolAccountId,
          "learning-reviewer",
          {
            subjectReference: String(form.get("subjectReference")),
            reason: String(form.get("reason")),
            clientRequestId: String(form.get("clientRequestId")),
          },
          setState,
          "Manual learning review queued.",
        );
      }}
    >
      <Field name="subjectReference" label="Subject" defaultValue="assignment-planet-worksheet" />
      <Field name="reason" label="Reason" defaultValue="Manual scoring required for open response." />
      <Field name="clientRequestId" label="Request id" defaultValue={`learning-review-${Date.now()}`} />
      <SubmitButton state={state} label="Queue review" />
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
  const baseUrl = learningApiBaseUrl();
  if (!baseUrl) {
    setState({ status: "error", message: "Set NEXT_PUBLIC_API_BASE_URL to submit this learning action to the SafeSchool API." });
    return;
  }

  setState({ status: "submitting", message: "Submitting..." });
  const response = await fetch(`${baseUrl}${path}`, {
    method: "POST",
    headers: learningHeaders(schoolAccountId, actorReference),
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    const body = await response.json().catch(() => null);
    setState({ status: "error", message: body?.message ?? body?.[0]?.message ?? "Learning API rejected the action." });
    return;
  }

  setState({ status: "success", message: successMessage });
}
