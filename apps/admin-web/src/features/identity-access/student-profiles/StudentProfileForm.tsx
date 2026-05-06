"use client";

import { useState } from "react";
import type { CreateStudentProfileInput } from "./studentProfilesApi";

type Props = {
  onSubmit?: (input: CreateStudentProfileInput) => void;
};

export function StudentProfileForm({ onSubmit }: Props) {
  const [legalName, setLegalName] = useState("");
  const [schoolStudentNumber, setSchoolStudentNumber] = useState("");

  return (
    <form
      onSubmit={(event) => {
        event.preventDefault();
        onSubmit?.({
          schoolStudentNumber,
          legalName,
          dateOfBirth: "2015-01-01",
          gradeLevel: "5",
          enrollmentStatus: "Enrolled",
          profileStatus: "Draft",
          reviewReason: "Initial profile review.",
          clientRequestId: crypto.randomUUID(),
        });
      }}
      style={{ display: "grid", gap: "12px", padding: "16px", border: "1px solid #d8dee8", borderRadius: "8px" }}
    >
      <label>
        <span style={{ display: "block", marginBottom: "6px" }}>Student number</span>
        <input value={schoolStudentNumber} onChange={(event) => setSchoolStudentNumber(event.target.value)} />
      </label>
      <label>
        <span style={{ display: "block", marginBottom: "6px" }}>Legal name</span>
        <input value={legalName} onChange={(event) => setLegalName(event.target.value)} />
      </label>
      <button type="submit">Create profile</button>
    </form>
  );
}
